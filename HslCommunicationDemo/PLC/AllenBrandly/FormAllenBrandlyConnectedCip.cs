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
using HslCommunication.Profinet.AllenBradley;
using System.Xml.Linq;
using HslCommunicationDemo.PLC.AllenBrandly;
using HslCommunicationDemo.DemoControl;

namespace HslCommunicationDemo
{
	public partial class FormAllenBrandlyConnectedCip : HslFormContent
	{
		public FormAllenBrandlyConnectedCip( )
		{
			InitializeComponent( );
			allenBradleyNet = new AllenBradleyConnectedCipNet( "192.168.0.110" );
		}


		private AllenBradleyConnectedCipNet allenBradleyNet = null;
		private AllenBrandlyControl control;
		private AddressExampleControl addressExampleControl;
		private CodeExampleControl codeExampleControl;


		private void FormSiemens_Load( object sender, EventArgs e )
		{
			DemoUtils.SetDeviveIp( textBox_ip );
			Language( Program.Language );
			control = new AllenBrandlyControl( );
			this.userControlReadWriteDevice1.AddSpecialFunctionTab( control );


			addressExampleControl = new AddressExampleControl( );
			addressExampleControl.SetAddressExample( HslCommunicationDemo.PLC.AllenBrandly.Helper.GetCIPAddressExamples( ) );
			userControlReadWriteDevice1.AddSpecialFunctionTab( addressExampleControl, false, DeviceAddressExample.GetTitle( ) );

			codeExampleControl = new CodeExampleControl( );
			userControlReadWriteDevice1.AddSpecialFunctionTab( codeExampleControl, false, CodeExampleControl.GetTitle( ) );
			userControlReadWriteDevice1.SetEnable( false );
		}


		private void Language( int language )
		{
			if (language == 2)
			{
				Text = "AllenBradly ConnectCIP Read Demo";
				label1.Text = "Ip:";
				label3.Text = "Port:";
				button1.Text = "Connect";
				button2.Text = "Disconnect";
				label21.Text = "Address:";
				label22.Text = "plc tag name";
			}
		}

		private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
		{

		}

		#region Connect And Close


		private void button1_Click( object sender, EventArgs e )
		{
			if (!int.TryParse( textBox2.Text, out int port ))
			{
				MessageBox.Show( DemoUtils.PortInputWrong );
				return;
			}

			allenBradleyNet.IpAddress = textBox_ip.Text;
			allenBradleyNet.Port = port;

			allenBradleyNet.LogNet = LogNet;

			try
			{
				OperateResult connect = allenBradleyNet.ConnectServer( );
				if (connect.IsSuccess)
				{
					MessageBox.Show( HslCommunication.StringResources.Language.ConnectedSuccess );
					button2.Enabled = true;
					button1.Enabled = false;
					userControlReadWriteDevice1.SetEnable( true );

					// 设置子控件的读取能力
					userControlReadWriteDevice1.SetReadWriteNet( allenBradleyNet, "A1", true, 1 );
					// 设置批量读取
					userControlReadWriteDevice1.BatchRead.SetReadWriteNet( allenBradleyNet, "A1", string.Empty );
					userControlReadWriteDevice1.BatchRead.SetReadRandom( allenBradleyNet.Read, "A1;A2    Length input \"1;1\"" );
					// 设置报文读取
					userControlReadWriteDevice1.MessageRead.SetReadSourceBytes( m => allenBradleyNet.ReadFromCoreServer( m, true, false ), string.Empty, string.Empty );
					userControlReadWriteDevice1.MessageRead.SetReadSourceBytes( m => allenBradleyNet.ReadCipFromServer( m ), "CIP", "CIP Message, example: " );

					control.SetDevice( allenBradleyNet, "A1" );

					// 设置代码示例
					codeExampleControl.SetCodeText( allenBradleyNet );
				}
				else
				{
					MessageBox.Show( StringResources.Language.ConnectedFailed + connect.ToMessageShowString( ) );
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
			OperateResult close = allenBradleyNet.ConnectClose( );
			button2.Enabled = false;
			button1.Enabled = true;
			userControlReadWriteDevice1.SetEnable( false );
			//if (!close.IsSuccess)
			//{
			//	MessageBox.Show( "关闭失败！" + close.Message );
			//}
		}



		#endregion


		public override void SaveXmlParameter( XElement element )
		{
			element.SetAttributeValue( DemoDeviceList.XmlIpAddress, textBox_ip.Text );
			element.SetAttributeValue( DemoDeviceList.XmlPort, textBox2.Text );

			this.userControlReadWriteDevice1.GetDataTable( element );
		}

		public override void LoadXmlParameter( XElement element )
		{
			base.LoadXmlParameter( element );
			textBox_ip.Text = element.Attribute( DemoDeviceList.XmlIpAddress ).Value;
			textBox2.Text = element.Attribute( DemoDeviceList.XmlPort ).Value;


			if (this.userControlReadWriteDevice1.LoadDataTable( element ) > 0)
				this.userControlReadWriteDevice1.SelectTabDataTable( );
		}

		private void userControlHead1_SaveConnectEvent_1( object sender, EventArgs e )
		{
			userControlHead1_SaveConnectEvent( sender, e );
		}
	}
}
