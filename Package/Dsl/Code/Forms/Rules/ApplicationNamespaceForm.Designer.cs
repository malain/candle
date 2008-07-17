namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    partial class ApplicationNamespaceForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.PictureBox pictureBox1;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label lblNamespace;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Panel panel4;
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtApplicationName = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtDomainPath = new System.Windows.Forms.TextBox();
            this.txtVersionRevision = new System.Windows.Forms.NumericUpDown();
            this.txtVersionBuild = new System.Windows.Forms.NumericUpDown();
            this.txtVersionMinor = new System.Windows.Forms.NumericUpDown();
            this.txtVersionMajor = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDomainPathSelector = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ckLibrary = new System.Windows.Forms.CheckBox();
            this.lbHelp = new System.Windows.Forms.TextBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label5 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            lblNamespace = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label6 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errors)).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionRevision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionBuild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionMinor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionMajor)).BeginInit();
            groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(pictureBox1);
            this.panel1.Controls.Add(label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 64);
            this.panel1.TabIndex = 6;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = global::DSLFactory.Candle.SystemModel.Properties.Resources.Logo;
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(64, 64);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label5.Location = new System.Drawing.Point(72, 20);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(199, 13);
            label5.TabIndex = 0;
            label5.Text = "Initialize the component\'s settings";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.ckLibrary);
            groupBox1.Controls.Add(this.txtApplicationName);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(lblNamespace);
            groupBox1.Controls.Add(this.txtNamespace);
            groupBox1.Location = new System.Drawing.Point(219, 70);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(548, 92);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Component name";
            // 
            // txtApplicationName
            // 
            this.txtApplicationName.Location = new System.Drawing.Point(125, 45);
            this.txtApplicationName.Name = "txtApplicationName";
            this.txtApplicationName.Size = new System.Drawing.Size(417, 20);
            this.txtApplicationName.TabIndex = 1;
            this.txtApplicationName.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(15, 48);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(41, 13);
            label1.TabIndex = 10;
            label1.Text = "Name :";
            // 
            // lblNamespace
            // 
            lblNamespace.AutoSize = true;
            lblNamespace.Location = new System.Drawing.Point(15, 22);
            lblNamespace.Name = "lblNamespace";
            lblNamespace.Size = new System.Drawing.Size(70, 13);
            lblNamespace.TabIndex = 9;
            lblNamespace.Text = "Namespace :";
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(125, 19);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(417, 20);
            this.txtNamespace.TabIndex = 0;
            this.txtNamespace.Text = "DSLFactory.Candle.";
            this.txtNamespace.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            this.txtNamespace.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNamespace_KeyUp);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(608, 12);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // errors
            // 
            this.errors.ContainerControl = this;
            // 
            // txtDomainPath
            // 
            this.txtDomainPath.Location = new System.Drawing.Point(126, 16);
            this.txtDomainPath.Name = "txtDomainPath";
            this.txtDomainPath.Size = new System.Drawing.Size(325, 20);
            this.txtDomainPath.TabIndex = 0;
            this.txtDomainPath.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.txtVersionRevision);
            groupBox2.Controls.Add(this.txtVersionBuild);
            groupBox2.Controls.Add(this.txtVersionMinor);
            groupBox2.Controls.Add(this.txtVersionMajor);
            groupBox2.Controls.Add(this.label2);
            groupBox2.Location = new System.Drawing.Point(219, 168);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(548, 53);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Version";
            // 
            // txtVersionRevision
            // 
            this.txtVersionRevision.Location = new System.Drawing.Point(285, 19);
            this.txtVersionRevision.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.txtVersionRevision.Name = "txtVersionRevision";
            this.txtVersionRevision.Size = new System.Drawing.Size(47, 20);
            this.txtVersionRevision.TabIndex = 3;
            this.txtVersionRevision.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // txtVersionBuild
            // 
            this.txtVersionBuild.Location = new System.Drawing.Point(232, 19);
            this.txtVersionBuild.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.txtVersionBuild.Name = "txtVersionBuild";
            this.txtVersionBuild.Size = new System.Drawing.Size(47, 20);
            this.txtVersionBuild.TabIndex = 2;
            this.txtVersionBuild.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // txtVersionMinor
            // 
            this.txtVersionMinor.Location = new System.Drawing.Point(179, 19);
            this.txtVersionMinor.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.txtVersionMinor.Name = "txtVersionMinor";
            this.txtVersionMinor.Size = new System.Drawing.Size(47, 20);
            this.txtVersionMinor.TabIndex = 1;
            this.txtVersionMinor.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // txtVersionMajor
            // 
            this.txtVersionMajor.Location = new System.Drawing.Point(126, 19);
            this.txtVersionMajor.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.txtVersionMajor.Name = "txtVersionMajor";
            this.txtVersionMajor.Size = new System.Drawing.Size(47, 20);
            this.txtVersionMajor.TabIndex = 0;
            this.txtVersionMajor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtVersionMajor.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Version :";
            // 
            // groupBox3
            // 
            groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            groupBox3.Controls.Add(this.btnDomainPathSelector);
            groupBox3.Controls.Add(this.txtDomainPath);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(this.txtDescription);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(this.txtURL);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new System.Drawing.Point(219, 227);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(548, 158);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "Informations";
            // 
            // btnDomainPathSelector
            // 
            this.btnDomainPathSelector.Location = new System.Drawing.Point(467, 14);
            this.btnDomainPathSelector.Name = "btnDomainPathSelector";
            this.btnDomainPathSelector.Size = new System.Drawing.Size(25, 23);
            this.btnDomainPathSelector.TabIndex = 6;
            this.btnDomainPathSelector.Text = "...";
            this.btnDomainPathSelector.UseVisualStyleBackColor = true;
            this.btnDomainPathSelector.Click += new System.EventHandler(this.btnDomainPathSelector_Click);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 19);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(74, 13);
            label6.TabIndex = 4;
            label6.Text = "Domain Path :";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(9, 86);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(533, 66);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 70);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(66, 13);
            label4.TabIndex = 2;
            label4.Text = "Description :";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(126, 43);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(325, 20);
            this.txtURL.TabIndex = 1;
            this.txtURL.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 46);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 13);
            label3.TabIndex = 0;
            label3.Text = "Project URL:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 393);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(776, 49);
            this.panel2.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.buttonOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(776, 47);
            this.panel3.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(689, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ckLibrary
            // 
            this.ckLibrary.AutoSize = true;
            this.ckLibrary.Location = new System.Drawing.Point(125, 69);
            this.ckLibrary.Name = "ckLibrary";
            this.ckLibrary.Size = new System.Drawing.Size(151, 17);
            this.ckLibrary.TabIndex = 2;
            this.ckLibrary.Text = "This component is a library";
            this.ckLibrary.UseVisualStyleBackColor = true;
            this.ckLibrary.Enter += new System.EventHandler(this.ShowHelpOnFocus);
            // 
            // panel4
            // 
            panel4.BackColor = System.Drawing.Color.White;
            panel4.Controls.Add(this.lbHelp);
            panel4.Dock = System.Windows.Forms.DockStyle.Left;
            panel4.Location = new System.Drawing.Point(0, 64);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(213, 329);
            panel4.TabIndex = 10;
            // 
            // lbHelp
            // 
            this.lbHelp.BackColor = System.Drawing.Color.White;
            this.lbHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHelp.ForeColor = System.Drawing.Color.Silver;
            this.lbHelp.Location = new System.Drawing.Point(0, 0);
            this.lbHelp.Multiline = true;
            this.lbHelp.Name = "lbHelp";
            this.lbHelp.ReadOnly = true;
            this.lbHelp.Size = new System.Drawing.Size(213, 329);
            this.lbHelp.TabIndex = 0;
            this.lbHelp.Text = "deded";
            // 
            // ApplicationNamespaceForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(776, 442);
            this.Controls.Add(panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ApplicationNamespaceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Component";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errors)).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionRevision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionBuild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionMinor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersionMajor)).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ErrorProvider errors;
        private System.Windows.Forms.TextBox txtApplicationName;
        private System.Windows.Forms.NumericUpDown txtVersionBuild;
        private System.Windows.Forms.NumericUpDown txtVersionMinor;
        private System.Windows.Forms.NumericUpDown txtVersionMajor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.NumericUpDown txtVersionRevision;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDomainPathSelector;
        private System.Windows.Forms.TextBox txtDomainPath;
        private System.Windows.Forms.CheckBox ckLibrary;
        private System.Windows.Forms.TextBox lbHelp;
    }
}