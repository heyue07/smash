﻿namespace smash.plugins
{
    partial class ProxySettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxySettingForm));
            groupBox1 = new GroupBox();
            showPassword = new CheckBox();
            inputPort = new TextBox();
            inputName = new TextBox();
            label4 = new Label();
            inputPassword = new TextBox();
            label3 = new Label();
            inputUserName = new TextBox();
            label2 = new Label();
            inputHost = new TextBox();
            label1 = new Label();
            btnSave = new Button();
            proxysView = new DataGridView();
            contextMenu = new ContextMenuStrip(components);
            mainManuUseProxy = new ToolStripMenuItem();
            mainMenuDelProxy = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)proxysView).BeginInit();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(showPassword);
            groupBox1.Controls.Add(inputPort);
            groupBox1.Controls.Add(inputName);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(inputPassword);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(inputUserName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(inputHost);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnSave);
            groupBox1.Location = new Point(14, 323);
            groupBox1.Margin = new Padding(6, 5, 6, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(6, 5, 6, 5);
            groupBox1.Size = new Size(903, 205);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "代理设置";
            // 
            // showPassword
            // 
            showPassword.AutoSize = true;
            showPassword.Location = new Point(742, 88);
            showPassword.Name = "showPassword";
            showPassword.Size = new Size(142, 35);
            showPassword.TabIndex = 20;
            showPassword.Text = "显示密码";
            showPassword.UseVisualStyleBackColor = true;
            showPassword.CheckedChanged += showPassword_CheckedChanged;
            // 
            // inputPort
            // 
            inputPort.Location = new Point(742, 34);
            inputPort.Name = "inputPort";
            inputPort.Size = new Size(130, 38);
            inputPort.TabIndex = 19;
            // 
            // inputName
            // 
            inputName.Location = new Point(80, 35);
            inputName.Margin = new Padding(6, 5, 6, 5);
            inputName.Name = "inputName";
            inputName.Size = new Size(289, 38);
            inputName.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(18, 38);
            label4.Margin = new Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new Size(62, 31);
            label4.TabIndex = 16;
            label4.Text = "名称";
            // 
            // inputPassword
            // 
            inputPassword.Location = new Point(460, 87);
            inputPassword.Margin = new Padding(6, 5, 6, 5);
            inputPassword.Name = "inputPassword";
            inputPassword.PasswordChar = '*';
            inputPassword.Size = new Size(266, 38);
            inputPassword.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(398, 90);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new Size(62, 31);
            label3.TabIndex = 14;
            label3.Text = "密码";
            // 
            // inputUserName
            // 
            inputUserName.Location = new Point(80, 86);
            inputUserName.Margin = new Padding(6, 5, 6, 5);
            inputUserName.Name = "inputUserName";
            inputUserName.Size = new Size(289, 38);
            inputUserName.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 89);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(62, 31);
            label2.TabIndex = 12;
            label2.Text = "账号";
            // 
            // inputHost
            // 
            inputHost.Location = new Point(460, 35);
            inputHost.Margin = new Padding(6, 5, 6, 5);
            inputHost.Name = "inputHost";
            inputHost.Size = new Size(266, 38);
            inputHost.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(398, 38);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(62, 31);
            label1.TabIndex = 10;
            label1.Text = "主机";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(393, 149);
            btnSave.Margin = new Padding(6, 5, 6, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 42);
            btnSave.TabIndex = 9;
            btnSave.Text = "保存更改";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // proxysView
            // 
            proxysView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            proxysView.Location = new Point(14, 15);
            proxysView.Name = "proxysView";
            proxysView.RowHeadersWidth = 82;
            proxysView.RowTemplate.Height = 40;
            proxysView.Size = new Size(904, 300);
            proxysView.TabIndex = 3;
            // 
            // contextMenu
            // 
            contextMenu.ImageScalingSize = new Size(32, 32);
            contextMenu.Items.AddRange(new ToolStripItem[] { mainManuUseProxy, mainMenuDelProxy });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(185, 80);
            // 
            // mainManuUseProxy
            // 
            mainManuUseProxy.Name = "mainManuUseProxy";
            mainManuUseProxy.Size = new Size(184, 38);
            mainManuUseProxy.Text = "添加新项";
            mainManuUseProxy.Click += MainManuUseProxy_Click;
            // 
            // mainMenuDelProxy
            // 
            mainMenuDelProxy.Name = "mainMenuDelProxy";
            mainMenuDelProxy.Size = new Size(184, 38);
            mainMenuDelProxy.Text = "删除选中";
            mainMenuDelProxy.Click += MainMenuDelProxy_Click;
            // 
            // ProxySettingForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(932, 543);
            Controls.Add(proxysView);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 5, 6, 5);
            Name = "ProxySettingForm";
            Text = "代理设置";
            FormClosing += OnFormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)proxysView).EndInit();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnSave;
        private TextBox inputPassword;
        private Label label3;
        private TextBox inputUserName;
        private Label label2;
        private TextBox inputHost;
        private Label label1;
        private DataGridView proxysView;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem mainManuUseProxy;
        private ToolStripMenuItem mainMenuDelProxy;
        private TextBox inputName;
        private Label label4;
        private TextBox inputPort;
        private CheckBox showPassword;
    }
}