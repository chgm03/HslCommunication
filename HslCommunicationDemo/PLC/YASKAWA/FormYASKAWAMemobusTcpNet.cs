﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet;
using System.Threading;
using HslCommunication;
using HslCommunication.Profinet.YASKAWA;
using System.Xml.Linq;
using HslCommunicationDemo.PLC.Common;
using HslCommunicationDemo.DemoControl;

namespace HslCommunicationDemo
{
	public partial class FormYASKAWAMemobusTcpNet : HslFormContent
	{
		public FormYASKAWAMemobusTcpNet( )
		{
			InitializeComponent( );
			// omronHostLink.LogNet = new HslCommunication.LogNet.LogNetSingle( "omron.log.txt" );
		}


		private MemobusTcpNet memobus = null;
		private AddressExampleControl addressExampleControl;
		private CodeExampleControl codeExampleControl;

		private void FormSiemens_Load( object sender, EventArgs e )
		{
			DemoUtils.SetDeviveIp( textBox_ip );
			comboBox1.DataSource = HslCommunication.BasicFramework.SoftBasic.GetEnumValues<HslCommunication.Core.DataFormat>( );
			comboBox1.SelectedItem = HslCommunication.Core.DataFormat.CDAB;

			Language( Program.Language );

			addressExampleControl = new AddressExampleControl( );
			addressExampleControl.SetAddressExample( HslCommunicationDemo.PLC.YASKAWA.Helper.GetMemobusAddress( ) );
			userControlReadWriteDevice1.AddSpecialFunctionTab( addressExampleControl, false, DeviceAddressExample.GetTitle( ) );

			codeExampleControl = new CodeExampleControl( );
			userControlReadWriteDevice1.AddSpecialFunctionTab( codeExampleControl, false, CodeExampleControl.GetTitle( ) );
			userControlReadWriteDevice1.SetEnable( false );
		}


		private void Language( int language )
		{
			if (language == 2)
			{
				Text = "YASKAWA Read PLC Demo";

				label1.Text = "Station:";
				button1.Text = "Open";
				button2.Text = "Close";
				label29.Text = "Ip:";
				label28.Text = "Port:";
			}
		}

		private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
		{

		}
		
		#region Connect And Close


		private void button1_Click( object sender, EventArgs e )
		{
			if (!int.TryParse( textBox19.Text, out int port ))
			{
				MessageBox.Show( DemoUtils.PortInputWrong );
				return;
			}

			if (!byte.TryParse( textBox_cpu_from.Text, out byte cpuFrom ))
			{
				MessageBox.Show( "PLC cpuFrom input wrong！" );
				return;
			}

			if (!byte.TryParse ( textBox_cpu_to.Text, out byte cpuTo ))
			{
				MessageBox.Show( "PLC cpuTo input wrong！" );
				return;
			}

			memobus?.ConnectClose( );
			memobus = new MemobusTcpNet( );
			memobus.IpAddress = textBox_ip.Text;
			memobus.Port = port;
			memobus.CpuFrom = cpuFrom;
			memobus.CpuTo = cpuTo;
			memobus.LogNet = LogNet;

			try
			{
				memobus.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)comboBox1.SelectedItem;

				OperateResult connect = memobus.ConnectServer( );
				if (connect.IsSuccess)
				{
					MessageBox.Show( StringResources.Language.ConnectedSuccess );

					button2.Enabled = true;
					button1.Enabled = false;
					userControlReadWriteDevice1.SetEnable( true );

					// 设置基本的读写信息
					userControlReadWriteDevice1.SetReadWriteNet( memobus, "100", true );
					// 设置批量读取
					userControlReadWriteDevice1.BatchRead.SetReadWriteNet( memobus, "100", string.Empty );
					userControlReadWriteDevice1.BatchRead.SetReadWordRandom( memobus.ReadRandom, "1;100;300   针对09功能码的扩展保持寄存器" );
					// 设置报文读取
					userControlReadWriteDevice1.MessageRead.SetReadSourceBytes( m => memobus.ReadFromCoreServer( m, hasResponseData: true, usePackAndUnpack: false ), string.Empty, string.Empty );

					// 设置代码示例
					codeExampleControl.SetCodeText( memobus, nameof( memobus.CpuFrom ), nameof( memobus.CpuTo ), "ByteTransform.DataFormat" );
				}
				else
				{
					MessageBox.Show( StringResources.Language.ConnectedFailed + connect.Message );
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
			memobus.ConnectClose( );
			button2.Enabled = false;
			button1.Enabled = true;
			userControlReadWriteDevice1.SetEnable( false );
		}
		
		#endregion

		public override void SaveXmlParameter( XElement element )
		{
			element.SetAttributeValue( DemoDeviceList.XmlIpAddress, textBox_ip.Text );
			element.SetAttributeValue( DemoDeviceList.XmlPort, textBox19.Text );
			element.SetAttributeValue( DemoDeviceList.XmlDataFormat, comboBox1.SelectedIndex );
			element.SetAttributeValue( "cpu_from", textBox_cpu_from.Text );
			element.SetAttributeValue( "cpu_to", textBox_cpu_to.Text );

			this.userControlReadWriteDevice1.GetDataTable( element );
		}

		public override void LoadXmlParameter( XElement element )
		{
			base.LoadXmlParameter( element );
			textBox_ip.Text = element.Attribute( DemoDeviceList.XmlIpAddress ).Value;
			textBox19.Text = element.Attribute( DemoDeviceList.XmlPort ).Value;
			comboBox1.SelectedIndex = int.Parse( element.Attribute( DemoDeviceList.XmlDataFormat ).Value );
			textBox_cpu_from.Text = element.Attribute( "cpu_from" ).Value;
			textBox_cpu_to.Text = element.Attribute( "cpu_to" ).Value;

			if (this.userControlReadWriteDevice1.LoadDataTable( element ) > 0)
				this.userControlReadWriteDevice1.SelectTabDataTable( );
		}

		private void userControlHead1_SaveConnectEvent_1( object sender, EventArgs e )
		{
			userControlHead1_SaveConnectEvent( sender, e );
		}


	}
}
