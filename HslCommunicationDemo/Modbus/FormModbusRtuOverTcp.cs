﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet;
using HslCommunication;
using HslCommunication.ModBus;
using System.Threading;
using System.Xml.Linq;
using HslCommunicationDemo.DemoControl;
using HslCommunication.Serial;
using HslCommunicationDemo.Modbus;
using HslCommunication.Core.Net;

namespace HslCommunicationDemo
{
	public partial class FormModbusRtuOverTcp : HslFormContent
	{
		public FormModbusRtuOverTcp( )
		{
			InitializeComponent( );
		}


		private ModbusRtuOverTcp busRtuClient = null;
		private ModbusControl control;
		private AddressExampleControl addressExampleControl;
		private CodeExampleControl codeExampleControl;

		private void FormSiemens_Load( object sender, EventArgs e )
		{
			DemoUtils.SetDeviveIp( textBox_ip );
			comboBox1.SelectedIndex = 2;

			comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
			checkBox3.CheckedChanged += CheckBox3_CheckedChanged;

			Language( Program.Language );
			control = new ModbusControl( );
			this.userControlReadWriteDevice1.AddSpecialFunctionTab( control );

			addressExampleControl = new AddressExampleControl( );
			addressExampleControl.SetAddressExample( Helper.GetModbusAddressExamples( ) );
			userControlReadWriteDevice1.AddSpecialFunctionTab( addressExampleControl, false, DeviceAddressExample.GetTitle( ) );

			codeExampleControl = new CodeExampleControl( );
			userControlReadWriteDevice1.AddSpecialFunctionTab( codeExampleControl, false, CodeExampleControl.GetTitle( ) );
			userControlReadWriteDevice1.SetEnable( false );
		}


		private void Language( int language )
		{
			if (language == 2)
			{
				Text = "Modbus Rtu OverTcp Read Demo";

				label1.Text = "Ip:";
				label3.Text = "Port:";
				label21.Text = "station";
				checkBox1.Text = "address from 0";
				checkBox3.Text = "string reverse";
				button1.Text = "Connect";
				button2.Text = "Disconnect";
			}
		}

		private void ComboBox1_SelectedIndexChanged( object sender, EventArgs e )
		{
			if (busRtuClient != null)
			{
				switch (comboBox1.SelectedIndex)
				{
					case 0: busRtuClient.DataFormat = HslCommunication.Core.DataFormat.ABCD;break;
					case 1: busRtuClient.DataFormat = HslCommunication.Core.DataFormat.BADC; break;
					case 2: busRtuClient.DataFormat = HslCommunication.Core.DataFormat.CDAB; break;
					case 3: busRtuClient.DataFormat = HslCommunication.Core.DataFormat.DCBA; break;
					default:break;
				}
			}
		}

		private void CheckBox3_CheckedChanged( object sender, EventArgs e )
		{
			if (busRtuClient != null)
			{
				busRtuClient.IsStringReverse = checkBox3.Checked;
			}
		}
		

		private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
		{

		}
		

		#region Connect And Close

		private void button1_Click( object sender, EventArgs e )
		{
			// 连接
			if(!int.TryParse(textBox2.Text,out int port))
			{
				MessageBox.Show( DemoUtils.PortInputWrong );
				return;
			}


			if(!byte.TryParse(textBox15.Text,out byte station))
			{
				MessageBox.Show( "Station input is wrong！" );
				return;
			}

			if (!int.TryParse( textBox_connect_timeout.Text, out int connect_timeout ))
			{
				MessageBox.Show( "Connect timeout input is wrong！" );
				return;
			}

			busRtuClient?.ConnectClose( );
			busRtuClient = new ModbusRtuOverTcp( textBox_ip.Text, port, station );
			busRtuClient.ConnectTimeOut       = connect_timeout;
			busRtuClient.AddressStartWithZero = checkBox1.Checked;
			busRtuClient.LogNet               = LogNet;
			busRtuClient.SendBeforeHex        = textBox_lora_head.Text;

			ComboBox1_SelectedIndexChanged( null, new EventArgs( ) );  // 设置数据服务
			busRtuClient.IsStringReverse = checkBox3.Checked;

			try
			{
				OperateResult connect = busRtuClient.ConnectServer( );
				if (connect.IsSuccess)
				{
					MessageBox.Show( HslCommunication.StringResources.Language.ConnectedSuccess );
					button2.Enabled = true;
					button1.Enabled = false;
					userControlReadWriteDevice1.SetEnable( true );

					// 设置子控件的读取能力
					userControlReadWriteDevice1.SetReadWriteNet( busRtuClient, "100", false );
					// 设置批量读取
					userControlReadWriteDevice1.BatchRead.SetReadWriteNet( busRtuClient, "100", string.Empty );
					// 设置报文读取
					userControlReadWriteDevice1.MessageRead.SetReadSourceBytes( m => busRtuClient.ReadFromCoreServer( m, true, false ), string.Empty, string.Empty );
					userControlReadWriteDevice1.MessageRead.SetReadSourceBytes( m => busRtuClient.ReadFromCoreServer( SoftCRC16.CRC16( m ), true, false ), "No CRC", "with no crc16, example: 01 03 00 00 00 01" );

					control.SetDevice( busRtuClient, "100" );

					List<string> props = new List<string>( ) { nameof( busRtuClient.AddressStartWithZero ), nameof( busRtuClient.IsStringReverse ),
						nameof( busRtuClient.DataFormat ), nameof( busRtuClient.Station ) };
					if (!string.IsNullOrEmpty( busRtuClient.SendBeforeHex )) props.Add( nameof( busRtuClient.SendBeforeHex ) );
					// 设置示例代码
					codeExampleControl.SetCodeText( "modbus", busRtuClient, props.ToArray( ) );
				}
				else
				{
					MessageBox.Show( HslCommunication.StringResources.Language.ConnectedFailed + connect.Message );
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show( ex.Message );
			}
		}

		private void button2_Click( object sender, EventArgs e )
		{
			// 断开连接
			busRtuClient.ConnectClose( );
			button2.Enabled = false;
			button1.Enabled = true;
			userControlReadWriteDevice1.SetEnable( false );
		}
		
		#endregion

		public override void SaveXmlParameter( XElement element )
		{
			element.SetAttributeValue( DemoDeviceList.XmlIpAddress, textBox_ip.Text);
			element.SetAttributeValue( DemoDeviceList.XmlPort, textBox2.Text );
			element.SetAttributeValue( nameof( NetworkDoubleBase.ConnectTimeOut ), textBox_connect_timeout.Text );
			element.SetAttributeValue( nameof( NetworkDoubleBase.SendBeforeHex ), textBox_lora_head.Text );
			element.SetAttributeValue( DemoDeviceList.XmlStation, textBox15.Text );
			element.SetAttributeValue( DemoDeviceList.XmlAddressStartWithZero, checkBox1.Checked );
			element.SetAttributeValue( DemoDeviceList.XmlDataFormat, comboBox1.SelectedIndex );
			element.SetAttributeValue( DemoDeviceList.XmlStringReverse, checkBox3.Checked );


			this.userControlReadWriteDevice1.GetDataTable( element );
		}

		public override void LoadXmlParameter( XElement element )
		{
			base.LoadXmlParameter( element );

			textBox_ip.Text                = element.Attribute( DemoDeviceList.XmlIpAddress ).Value;
			textBox2.Text                = element.Attribute( DemoDeviceList.XmlPort ).Value;
			textBox15.Text               = element.Attribute( DemoDeviceList.XmlStation ).Value;
			checkBox1.Checked            = bool.Parse( element.Attribute( DemoDeviceList.XmlAddressStartWithZero ).Value );
			comboBox1.SelectedIndex      = int.Parse( element.Attribute( DemoDeviceList.XmlDataFormat ).Value );
			checkBox3.Checked            = bool.Parse( element.Attribute( DemoDeviceList.XmlStringReverse ).Value );
			textBox_connect_timeout.Text = GetXmlValue( element, nameof( NetworkDoubleBase.ConnectTimeOut ), "5000", m => m );
			textBox_lora_head.Text       = GetXmlValue( element, nameof( NetworkDoubleBase.SendBeforeHex ), "", m => m );

			if (this.userControlReadWriteDevice1.LoadDataTable( element ) > 0)
				this.userControlReadWriteDevice1.SelectTabDataTable( );
		}

		private void userControlHead1_SaveConnectEvent_1( object sender, EventArgs e )
		{
			userControlHead1_SaveConnectEvent( sender, e );
		}
	}
}
