﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using HslCommunication;
using HslCommunicationDemo.HslDebug;

namespace HslCommunicationDemo
{
	public partial class FormTcpDebug : HslFormContent
	{
		#region Form

		public FormTcpDebug( )
		{
			InitializeComponent( );

			connectHandShake = new List<byte[]>( );
		}

		private void FormTcpDebug_Load( object sender, EventArgs e )
		{
			panel_main.Enabled = false;
			Language( Program.Language );
		}

		private void Language( int language )
		{
			if (language == 1)
			{
				Text                    = "TCP/IP调试助手";
				label1.Text             = "Ip地址：";
				label3.Text             = "端口号：";
				button1.Text            = "连接";
				button2.Text            = "关闭连接";
			}
			else
			{
				Text                      = "TCP/IP Debug Tools";
				label1.Text               = "Ip:";
				label3.Text               = "Port:";
				button1.Text              = "Connect";
				button2.Text              = "Disconnect";
				label9.Text               = "Local Port：";
				label10.Text              = "(If empty, automatically assigned)";
				button_edit_hand.Text     = "Edit handshake";
				label2.Text               = "Buffer Len:";
			}
		}

		private void button_edit_hand_Click( object sender, EventArgs e )
		{
			// 编辑握手包
			using (HslDebug.FormHandShake handShake = new HslDebug.FormHandShake( this.connectHandShake ))
			{
				if (handShake.ShowDialog( this ) == DialogResult.OK)
				{
					this.connectHandShake = handShake.HandShake;
					this.label4.Text = (Program.Language == 1 ? "握手包：" : "HandShanke: ") + handShake.HandShake?.Count;
				}
			}
		}

		#endregion

		#region Start Close

		private void button1_Click( object sender, EventArgs e )
		{
			// 连接服务器
			if (!int.TryParse( textBox_buffer_length.Text, out int bufferLength ))
			{
				MessageBox.Show( "Buffer length input wrong!" );
				return;
			}

			try
			{
				if (bufferLength != this.buffer.Length) this.buffer = new byte[bufferLength];

				socketDebugSession?.WorkSocket?.Close( );
				System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse( HslCommunication.Core.HslHelper.GetIpAddressFromInput( textBox_ip.Text ) );
				if (radioButton_tcp.Checked)
				{
					Socket socketCore = new Socket( iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
					// 如果需要绑定端口的话
					if (!string.IsNullOrEmpty( textBox_localPort.Text ))
					{
						OperateResult<int> localPort = DemoUtils.ParseInt( textBox_localPort.Text );
						if (!localPort.IsSuccess)
						{
							MessageBox.Show( "Local Port input failed: " + localPort.Message );
							return;
						}
						socketCore.Bind( new IPEndPoint( IPAddress.Any, localPort.Content ) );
					}
					connectSuccess = false;
					// 以下代码是5秒的超时
					new Thread( ( ) =>
					{
						Thread.Sleep( 5000 );
						if (!connectSuccess) socketCore?.Close( );
					} ).Start( );
					socketCore.Connect( iPAddress, int.Parse( textBox_port.Text ) );
					connectSuccess = true;

					socketDebugSession = new SocketDebugSession( socketCore );
					this.debugControl1.AddSessions( socketDebugSession );

					// 如果有握手的报文
					if (this.connectHandShake.Count > 0)
					{
						for (int i = 0; i < this.connectHandShake.Count; i++)
						{
							// 发送
							this.debugControl1.SendSourceData( socketDebugSession, this.connectHandShake[i] );
							// 接收
							int len = socketCore.Receive( buffer, 0, buffer.Length, SocketFlags.None );
							this.debugControl1.RenderSendReceiveContent( socketDebugSession, 0, buffer.SelectBegin( len ) );
						}
					}

					socketDebugSession.WorkSocket.BeginReceive( buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback( ReceiveCallBack ), socketCore );
				}
				else
				{
					// UDP的情况
					udpEndPoint = new IPEndPoint( iPAddress, int.Parse( textBox_port.Text ) );

					Socket udpSocket = new Socket( iPAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp );
					// 如果需要绑定端口的话
					if (!string.IsNullOrEmpty( textBox_localPort.Text ))
					{
						OperateResult<int> localPort = DemoUtils.ParseInt( textBox_localPort.Text );
						if (!localPort.IsSuccess)
						{
							MessageBox.Show( "Local Port input failed: " + localPort.Message );
							return;
						}
						udpSocket.Bind( new IPEndPoint( udpEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, localPort.Content ) );
					}
					udpSocket.SendTo( new byte[0], udpEndPoint );

					socketDebugSession = new SocketDebugSession( udpSocket, udpEndPoint );
					this.debugControl1.AddSessions( socketDebugSession );

					IsStarted = true;
					threadReceive = new System.Threading.Thread( new System.Threading.ThreadStart( ThreadReceiveUdpCycle ) ) { IsBackground = true };
					threadReceive.Start( );

					// 如果有握手的报文
					if (this.connectHandShake.Count > 0)
					{
						for (int i = 0; i < this.connectHandShake.Count; i++)
						{
							// 发送
							this.debugControl1.SendSourceData( socketDebugSession, this.connectHandShake[i] );

							Thread.Sleep( 100 );
							//EndPoint Remote = (EndPoint)new IPEndPoint( udpEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0 );
							// 接收
							//int len = socketDebugSession.WorkSocket.ReceiveFrom( buffer, 0, buffer.Length, SocketFlags.None, ref Remote );
							//this.debugControl1.RenderSendReceiveContent( socketDebugSession, 0, buffer.SelectBegin( len ) );
						}
					}


				}

				button1.Enabled = false;
				button2.Enabled = true;
				panel_main.Enabled = true;
				panel_tcp_udp.Enabled = false;
				//button_send.Enabled = true;

				MessageBox.Show( StringResources.Language.ConnectServerSuccess );
			}
			catch(Exception ex)
			{
				MessageBox.Show( StringResources.Language.ConnectedFailed + Environment.NewLine + ex.Message );
			}
		}


		private void button2_Click( object sender, EventArgs e )
		{
			IsStarted = false;
			button1.Enabled = true;
			button2.Enabled = false;
			panel_tcp_udp.Enabled = true;
			this.debugControl1.RemoveSessionsAll( );
			//button_send.Enabled = false;
		}

		#endregion

		/// <summary>
		/// 后台接收数据的线程
		/// </summary>
		protected virtual void ThreadReceiveUdpCycle( )
		{
			IPEndPoint sender = new IPEndPoint( udpEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0 );
			EndPoint Remote = (EndPoint)sender;
			while (IsStarted)
			{
				int length = 0;
				try
				{
					length = socketDebugSession.WorkSocket.ReceiveFrom( buffer, 0, buffer.Length, SocketFlags.None, ref Remote );
				

					if (length > 0) Invoke( new Action( ( ) =>
					{
						this.debugControl1.RenderSendReceiveContent( socketDebugSession, 0, buffer.SelectBegin( length ) );
					} ) );
				}
				catch (Exception ex)
				{
					Invoke( new Action( ( ) =>
					{
						this.debugControl1.RenderSendReceiveContent( socketDebugSession, 2, null, (Program.Language == 1 ? "服务器断开连接。原因：" : "DisConnect from remote, reason: ") + ex.Message );
						button1.Enabled = true;
						button2.Enabled = false;
						panel_tcp_udp.Enabled = true;
					} ) );
					break;
				}
			}
		}

		private void ReceiveCallBack( IAsyncResult ar )
		{
			try
			{
				int length = socketDebugSession.WorkSocket.EndReceive( ar );
				byte[] data = new byte[length];
				if (length > 0) Array.Copy( buffer, 0, data, 0, length );

				socketDebugSession.WorkSocket.BeginReceive( buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback( ReceiveCallBack ), socketDebugSession.WorkSocket );

				if (length == 0)
				{
					// 大概率远程关闭了连接
					Invoke( new Action( ( ) =>
					{
						this.debugControl1.RenderSendReceiveContent( socketDebugSession, 2, null, (Program.Language == 1 ? "远程关闭连接" : "Remote closes connection") );
						button1.Enabled     = true;
						button2.Enabled     = false;
						panel_tcp_udp.Enabled = true;
					} ) );
					return;
				}

				Invoke( new Action( ( ) =>
				{
					this.debugControl1.RenderSendReceiveContent( socketDebugSession, 0, data );
				} ) );
			}
			catch(ObjectDisposedException)
			{
				Invoke( new Action( ( ) =>
				{
					this.debugControl1.RenderSendReceiveContent( socketDebugSession, 2, null, (Program.Language == 1 ? "客户端关闭连接" : "Client closes connection") );
					button1.Enabled = true;
					button2.Enabled = false;
					panel_tcp_udp.Enabled = true;
				} ) );
			}
			catch(Exception ex)
			{
				Invoke( new Action( ( ) =>
				{
					this.debugControl1.RenderSendReceiveContent( socketDebugSession, 2, null, (Program.Language == 1 ? "服务器断开连接: " : "DisConnect from remote: ") + ex.Message );
					button1.Enabled = true;
					button2.Enabled = false;
					panel_tcp_udp.Enabled = true;
				} ) );
			}
		}


		#region Toledo Test

		// private float toledoWeight = 30f;
		//private void button6_Click( object sender, EventArgs e )
		//{
		//	if (button6.BackColor != Color.Green)
		//	{
		//		toledoThread = true;
		//		button6.BackColor = Color.Green;
		//		new System.Threading.Thread( new System.Threading.ThreadStart( ToledoTest ) ) { IsBackground = true }.Start( );
		//	}
		//	else
		//	{
		//		toledoThread = false;
		//		button6.BackColor = SystemColors.Control;
		//	}
		//}

		//private void ToledoTest( )
		//{
		//	while (toledoThread)
		//	{
		//		System.Threading.Thread.Sleep( 30 );

		//		byte[] send = "02 2C 30 20 20 20 33 38 36 32 20 20 20 30 30 30 0D".ToHexBytes( );
		//		toledoWeight += random.Next( 200 ) / 100f - 1;
		//		if (toledoWeight < 0) toledoWeight = 5f;
		//		if (toledoWeight > 100) toledoWeight = 95f;
		//		string tmp = toledoWeight.ToString( "F2" ).Replace( ".", "" ).PadLeft( 6, ' ' );
		//		Encoding.ASCII.GetBytes( tmp ).CopyTo( send, 4 );

		//		try
		//		{
		//			socketDebugSession.WorkSocket?.Send( send, 0, send.Length, SocketFlags.None );
		//		}
		//		catch (Exception ex)
		//		{
		//			HslCommunication.BasicFramework.SoftBasic.ShowExceptionMessage( ex );
		//			return;
		//		}
		//	}
		//}

		#endregion

		#region XML Save Load

		public override void SaveXmlParameter( XElement element )
		{
			element.SetAttributeValue( DemoDeviceList.XmlIpAddress, textBox_ip.Text );
			element.SetAttributeValue( DemoDeviceList.XmlPort, textBox_port.Text );
			element.SetAttributeValue( "Tcp", radioButton_tcp.Checked );
			element.SetAttributeValue( "Local", textBox_localPort.Text );
			element.SetAttributeValue( "BufferLength", textBox_buffer_length.Text );

			element.RemoveNodes( );
			if (this.connectHandShake?.Count > 0)
			{
				foreach (var item in this.connectHandShake)
				{
					XElement xml = new XElement( "HandShake" );
					xml.SetAttributeValue( "Message", item.ToHexString( ) );
					element.Add( xml );
				}
			}
			this.debugControl1.SaveXml( element );
		}

		public override void LoadXmlParameter( XElement element )
		{
			base.LoadXmlParameter( element );
			textBox_ip.Text   = GetXmlValue( element, DemoDeviceList.XmlIpAddress, "127.0.0.1", m => m );
			textBox_port.Text = GetXmlValue( element, DemoDeviceList.XmlPort,      "502",       m => m );
			bool tcp = GetXmlValue( element, "Tcp", true, bool.Parse );
			if (tcp)
				radioButton_tcp.Checked = true;
			else
				radioButton_udp.Checked = true;
			textBox_localPort.Text = GetXmlValue( element, "Local", "", m => m );
			textBox_buffer_length.Text = GetXmlValue( element, "BufferLength", "2048", m => m );

			this.debugControl1.LoadXml( element );
			this.connectHandShake.Clear( );
			foreach (var item in element.Elements( "HandShake" ))
			{
				XAttribute message = item.Attribute( "Message" );
				if (message != null)
				{
					this.connectHandShake.Add( message.Value.ToHexBytes( ) );
				}
			}
			this.label4.Text = (Program.Language == 1 ? "握手包：" : "HandShanke: ") + this.connectHandShake?.Count;
		}

		private void userControlHead1_SaveConnectEvent_1( object sender, EventArgs e )
		{
			userControlHead1_SaveConnectEvent( sender, e );
		}

		#endregion

		#region Private Member

		private SocketDebugSession socketDebugSession;
		private bool connectSuccess = false;
		private byte[] buffer = new byte[2048];
		private List<byte[]> connectHandShake;
		private EndPoint udpEndPoint = null;
		private System.Threading.Thread threadReceive;
		private bool IsStarted = false;
		private bool isBinary = true;

		#endregion

	}
}
