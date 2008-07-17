namespace DSLFactory.Candle.SystemModel.Configuration
{
    partial class OptionsServersPageControl
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
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDefaultProxy = new System.Windows.Forms.CheckBox();
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.txtPassword );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.txtUser );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.chkDefaultProxy );
            this.groupBox1.Controls.Add( this.txtProxy );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Location = new System.Drawing.Point( 3, 3 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 382, 125 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connexion";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point( 102, 98 );
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size( 109, 20 );
            this.txtPassword.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 6, 104 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 53, 13 );
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point( 102, 72 );
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size( 109, 20 );
            this.txtUser.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 6, 75 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 29, 13 );
            this.label2.TabIndex = 3;
            this.label2.Text = "User";
            // 
            // chkDefaultProxy
            // 
            this.chkDefaultProxy.AutoSize = true;
            this.chkDefaultProxy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDefaultProxy.Location = new System.Drawing.Point( 9, 23 );
            this.chkDefaultProxy.Name = "chkDefaultProxy";
            this.chkDefaultProxy.Size = new System.Drawing.Size( 108, 17 );
            this.chkDefaultProxy.TabIndex = 2;
            this.chkDefaultProxy.Text = "Use default proxy";
            this.chkDefaultProxy.UseVisualStyleBackColor = true;
            this.chkDefaultProxy.CheckedChanged += new System.EventHandler( this.chkDefaultProxy_CheckedChanged );
            // 
            // txtProxy
            // 
            this.txtProxy.Location = new System.Drawing.Point( 102, 46 );
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.Size = new System.Drawing.Size( 182, 20 );
            this.txtProxy.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 6, 49 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 33, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Proxy";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add( this.lstServers );
            this.groupBox3.Controls.Add( this.txtServer );
            this.groupBox3.Controls.Add( this.label5 );
            this.groupBox3.Controls.Add( this.btnAddServer );
            this.groupBox3.Location = new System.Drawing.Point( 3, 134 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size( 382, 166 );
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Servers";
            // 
            // lstServers
            // 
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point( 42, 47 );
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size( 287, 108 );
            this.lstServers.TabIndex = 3;
            this.lstServers.KeyUp += new System.Windows.Forms.KeyEventHandler( this.lstServers_KeyUp );
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point( 42, 17 );
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size( 287, 20 );
            this.txtServer.TabIndex = 2;
            this.txtServer.Text = "http://";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 6, 19 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 26, 13 );
            this.label5.TabIndex = 1;
            this.label5.Text = "Url :";
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point( 335, 14 );
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size( 41, 23 );
            this.btnAddServer.TabIndex = 0;
            this.btnAddServer.Text = "Add";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler( this.btnAddServer_Click );
            // 
            // OptionsServersPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.groupBox3 );
            this.Controls.Add( this.groupBox1 );
            this.Name = "OptionsServersPageControl";
            this.Size = new System.Drawing.Size( 388, 303 );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout( false );
            this.groupBox3.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtProxy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstServers;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddServer;
        private System.Windows.Forms.CheckBox chkDefaultProxy;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label2;
    }
}
