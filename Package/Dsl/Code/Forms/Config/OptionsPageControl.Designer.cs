namespace DSLFactory.Candle.SystemModel.Configuration
{
    partial class OptionsPageControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowseConfig = new System.Windows.Forms.Button();
            this.txtBaseDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRepositoryPath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numCacheDelai = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkRepository = new System.Windows.Forms.CheckBox();
            this.txtRepositoryUrl = new System.Windows.Forms.TextBox();
            this.btnBrowseRepository = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLicenseId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.btnDomainPathSelector = new System.Windows.Forms.Button();
            this.helpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkTrace = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCacheDelai)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 57);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 13);
            label3.TabIndex = 3;
            label3.Text = "Remote repository :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 16);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(95, 13);
            label4.TabIndex = 0;
            label4.Text = "Models repository :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowseConfig);
            this.groupBox1.Controls.Add(this.txtBaseDirectory);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration";
            // 
            // btnBrowseConfig
            // 
            this.btnBrowseConfig.Location = new System.Drawing.Point(335, 30);
            this.btnBrowseConfig.Name = "btnBrowseConfig";
            this.btnBrowseConfig.Size = new System.Drawing.Size(25, 23);
            this.btnBrowseConfig.TabIndex = 2;
            this.btnBrowseConfig.Text = "...";
            this.btnBrowseConfig.UseVisualStyleBackColor = true;
            this.btnBrowseConfig.Click += new System.EventHandler(this.btnBrowseConfig_Click);
            // 
            // txtBaseDirectory
            // 
            this.txtBaseDirectory.Location = new System.Drawing.Point(9, 32);
            this.txtBaseDirectory.Name = "txtBaseDirectory";
            this.txtBaseDirectory.Size = new System.Drawing.Size(320, 20);
            this.txtBaseDirectory.TabIndex = 1;
            this.helpToolTip.SetToolTip(this.txtBaseDirectory, "The base directory is used to store templates and strategies. ");
            this.txtBaseDirectory.Leave += new System.EventHandler(this.txtBaseDirectory_Leave);
            this.txtBaseDirectory.TextChanged += new System.EventHandler(this.txtBaseDirectory_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base directory :";
            // 
            // txtRepositoryPath
            // 
            this.txtRepositoryPath.Location = new System.Drawing.Point(9, 32);
            this.txtRepositoryPath.Name = "txtRepositoryPath";
            this.txtRepositoryPath.Size = new System.Drawing.Size(320, 20);
            this.txtRepositoryPath.TabIndex = 1;
            this.helpToolTip.SetToolTip(this.txtRepositoryPath, "Used to store all the local models");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numCacheDelai);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnTest);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chkRepository);
            this.groupBox2.Controls.Add(this.txtRepositoryUrl);
            this.groupBox2.Controls.Add(label3);
            this.groupBox2.Controls.Add(this.btnBrowseRepository);
            this.groupBox2.Controls.Add(this.txtRepositoryPath);
            this.groupBox2.Controls.Add(label4);
            this.groupBox2.Location = new System.Drawing.Point(3, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 135);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Repository";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "minutes (0 for no cache)";
            // 
            // numCacheDelai
            // 
            this.numCacheDelai.Location = new System.Drawing.Point(82, 105);
            this.numCacheDelai.Name = "numCacheDelai";
            this.numCacheDelai.Size = new System.Drawing.Size(46, 20);
            this.numCacheDelai.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Delai cache :";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(335, 76);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(41, 23);
            this.btnTest.TabIndex = 7;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Url :";
            // 
            // chkRepository
            // 
            this.chkRepository.AutoSize = true;
            this.chkRepository.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRepository.Location = new System.Drawing.Point(110, 56);
            this.chkRepository.Name = "chkRepository";
            this.chkRepository.Size = new System.Drawing.Size(65, 17);
            this.chkRepository.TabIndex = 5;
            this.chkRepository.Text = "Enabled";
            this.chkRepository.UseVisualStyleBackColor = true;
            this.chkRepository.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtRepositoryUrl
            // 
            this.txtRepositoryUrl.Location = new System.Drawing.Point(42, 79);
            this.txtRepositoryUrl.Name = "txtRepositoryUrl";
            this.txtRepositoryUrl.Size = new System.Drawing.Size(287, 20);
            this.txtRepositoryUrl.TabIndex = 4;
            // 
            // btnBrowseRepository
            // 
            this.btnBrowseRepository.Location = new System.Drawing.Point(335, 30);
            this.btnBrowseRepository.Name = "btnBrowseRepository";
            this.btnBrowseRepository.Size = new System.Drawing.Size(25, 23);
            this.btnBrowseRepository.TabIndex = 2;
            this.btnBrowseRepository.Text = "...";
            this.btnBrowseRepository.UseVisualStyleBackColor = true;
            this.btnBrowseRepository.Click += new System.EventHandler(this.btnBrowseRepository_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "License id :";
            this.label6.Visible = false;
            // 
            // txtLicenseId
            // 
            this.txtLicenseId.Location = new System.Drawing.Point(70, 273);
            this.txtLicenseId.Name = "txtLicenseId";
            this.txtLicenseId.Size = new System.Drawing.Size(262, 20);
            this.txtLicenseId.TabIndex = 8;
            this.txtLicenseId.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Domain :";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(70, 209);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(262, 20);
            this.txtDomain.TabIndex = 11;
            // 
            // btnDomainPathSelector
            // 
            this.btnDomainPathSelector.Location = new System.Drawing.Point(338, 209);
            this.btnDomainPathSelector.Name = "btnDomainPathSelector";
            this.btnDomainPathSelector.Size = new System.Drawing.Size(25, 23);
            this.btnDomainPathSelector.TabIndex = 12;
            this.btnDomainPathSelector.Text = "...";
            this.btnDomainPathSelector.UseVisualStyleBackColor = true;
            this.btnDomainPathSelector.Click += new System.EventHandler(this.btnDomainPathSelector_Click);
            // 
            // helpToolTip
            // 
            this.helpToolTip.ToolTipTitle = "Help";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTrace.Location = new System.Drawing.Point(110, 235);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(222, 17);
            this.chkTrace.TabIndex = 13;
            this.chkTrace.Text = "Enable generation trace in output window";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // OptionsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.btnDomainPathSelector);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtLicenseId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "OptionsPageControl";
            this.Size = new System.Drawing.Size(388, 303);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCacheDelai)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowseConfig;
        private System.Windows.Forms.TextBox txtBaseDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRepositoryPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseRepository;
        private System.Windows.Forms.TextBox txtRepositoryUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkRepository;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLicenseId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numCacheDelai;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Button btnDomainPathSelector;
        private System.Windows.Forms.ToolTip helpToolTip;
        private System.Windows.Forms.CheckBox chkTrace;
    }
}
