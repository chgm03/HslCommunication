﻿namespace HslCommunicationDemo
{
    partial class FormDLT645With1997OverTcp
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBox_enable_Fe = new System.Windows.Forms.CheckBox();
			this.textBox_op_code = new System.Windows.Forms.TextBox();
			this.label_op_code = new System.Windows.Forms.Label();
			this.textBox_password = new System.Windows.Forms.TextBox();
			this.label_password = new System.Windows.Forms.Label();
			this.textBox_port = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox_ip = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox_station = new System.Windows.Forms.TextBox();
			this.label_address = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.userControlReadWriteDevice1 = new HslCommunicationDemo.DemoControl.UserControlReadWriteDevice();
			this.userControlHead1 = new HslCommunicationDemo.DemoControl.UserControlHead();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.checkBox_enable_Fe);
			this.panel1.Controls.Add(this.textBox_op_code);
			this.panel1.Controls.Add(this.label_op_code);
			this.panel1.Controls.Add(this.textBox_password);
			this.panel1.Controls.Add(this.label_password);
			this.panel1.Controls.Add(this.textBox_port);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.textBox_ip);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.textBox_station);
			this.panel1.Controls.Add(this.label_address);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Location = new System.Drawing.Point(3, 36);
			this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(997, 62);
			this.panel1.TabIndex = 0;
			// 
			// checkBox_enable_Fe
			// 
			this.checkBox_enable_Fe.AutoSize = true;
			this.checkBox_enable_Fe.Location = new System.Drawing.Point(420, 6);
			this.checkBox_enable_Fe.Name = "checkBox_enable_Fe";
			this.checkBox_enable_Fe.Size = new System.Drawing.Size(130, 21);
			this.checkBox_enable_Fe.TabIndex = 39;
			this.checkBox_enable_Fe.Text = "FE FE FE FE head?";
			this.checkBox_enable_Fe.UseVisualStyleBackColor = true;
			// 
			// textBox_op_code
			// 
			this.textBox_op_code.Location = new System.Drawing.Point(330, 32);
			this.textBox_op_code.Name = "textBox_op_code";
			this.textBox_op_code.ReadOnly = true;
			this.textBox_op_code.Size = new System.Drawing.Size(159, 23);
			this.textBox_op_code.TabIndex = 38;
			// 
			// label_op_code
			// 
			this.label_op_code.AutoSize = true;
			this.label_op_code.Location = new System.Drawing.Point(244, 35);
			this.label_op_code.Name = "label_op_code";
			this.label_op_code.Size = new System.Drawing.Size(80, 17);
			this.label_op_code.TabIndex = 37;
			this.label_op_code.Text = "操作者代码：";
			// 
			// textBox_password
			// 
			this.textBox_password.Location = new System.Drawing.Point(58, 32);
			this.textBox_password.Name = "textBox_password";
			this.textBox_password.ReadOnly = true;
			this.textBox_password.Size = new System.Drawing.Size(180, 23);
			this.textBox_password.TabIndex = 36;
			// 
			// label_password
			// 
			this.label_password.AutoSize = true;
			this.label_password.Location = new System.Drawing.Point(8, 35);
			this.label_password.Name = "label_password";
			this.label_password.Size = new System.Drawing.Size(44, 17);
			this.label_password.TabIndex = 35;
			this.label_password.Text = "密码：";
			// 
			// textBox_port
			// 
			this.textBox_port.Location = new System.Drawing.Point(299, 4);
			this.textBox_port.Name = "textBox_port";
			this.textBox_port.Size = new System.Drawing.Size(105, 23);
			this.textBox_port.TabIndex = 11;
			this.textBox_port.Text = "502";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(245, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 17);
			this.label3.TabIndex = 10;
			this.label3.Text = "端口号：";
			// 
			// textBox_ip
			// 
			this.textBox_ip.Location = new System.Drawing.Point(62, 4);
			this.textBox_ip.Name = "textBox_ip";
			this.textBox_ip.Size = new System.Drawing.Size(176, 23);
			this.textBox_ip.TabIndex = 9;
			this.textBox_ip.Text = "127.0.0.1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 17);
			this.label1.TabIndex = 8;
			this.label1.Text = "Ip地址：";
			// 
			// textBox_station
			// 
			this.textBox_station.Location = new System.Drawing.Point(554, 32);
			this.textBox_station.Name = "textBox_station";
			this.textBox_station.Size = new System.Drawing.Size(190, 23);
			this.textBox_station.TabIndex = 7;
			this.textBox_station.Text = "1";
			// 
			// label_address
			// 
			this.label_address.AutoSize = true;
			this.label_address.Location = new System.Drawing.Point(506, 35);
			this.label_address.Name = "label_address";
			this.label_address.Size = new System.Drawing.Size(44, 17);
			this.label_address.TabIndex = 6;
			this.label_address.Text = "站号：";
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(875, 11);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(91, 28);
			this.button2.TabIndex = 5;
			this.button2.Text = "断开连接";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(778, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(91, 28);
			this.button1.TabIndex = 4;
			this.button1.Text = "连接";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.userControlReadWriteDevice1);
			this.panel2.Location = new System.Drawing.Point(3, 101);
			this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(997, 542);
			this.panel2.TabIndex = 1;
			// 
			// userControlReadWriteDevice1
			// 
			this.userControlReadWriteDevice1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlReadWriteDevice1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.userControlReadWriteDevice1.Location = new System.Drawing.Point(0, 0);
			this.userControlReadWriteDevice1.Name = "userControlReadWriteDevice1";
			this.userControlReadWriteDevice1.Size = new System.Drawing.Size(995, 540);
			this.userControlReadWriteDevice1.TabIndex = 0;
			// 
			// userControlHead1
			// 
			this.userControlHead1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.userControlHead1.Dock = System.Windows.Forms.DockStyle.Top;
			this.userControlHead1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.userControlHead1.HelpLink = "http://www.cnblogs.com/dathlin/p/8974215.html";
			this.userControlHead1.Location = new System.Drawing.Point(0, 0);
			this.userControlHead1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.userControlHead1.MinimumSize = new System.Drawing.Size(800, 32);
			this.userControlHead1.Name = "userControlHead1";
			this.userControlHead1.ProtocolInfo = "DLT 645 1997 Over Tcp";
			this.userControlHead1.Size = new System.Drawing.Size(1004, 32);
			this.userControlHead1.TabIndex = 2;
			this.userControlHead1.SaveConnectEvent += new System.EventHandler<System.EventArgs>(this.userControlHead1_SaveConnectEvent_1);
			// 
			// FormDLT645With1997OverTcp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1004, 645);
			this.Controls.Add(this.userControlHead1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FormDLT645With1997OverTcp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DLT645访问Demo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSiemens_FormClosing);
			this.Load += new System.EventHandler(this.FormSiemens_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_station;
        private System.Windows.Forms.Label label_address;
        private DemoControl.UserControlHead userControlHead1;
		private System.Windows.Forms.TextBox textBox_port;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox_ip;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox_op_code;
		private System.Windows.Forms.Label label_op_code;
		private System.Windows.Forms.TextBox textBox_password;
		private System.Windows.Forms.Label label_password;
		private System.Windows.Forms.CheckBox checkBox_enable_Fe;
		private DemoControl.UserControlReadWriteDevice userControlReadWriteDevice1;
	}
}