namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    partial class DAOSelectorDlg
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbExisting = new System.Windows.Forms.Label();
            this.cbDAO = new System.Windows.Forms.ComboBox();
            this.lbCreate = new System.Windows.Forms.Label();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbNew = new System.Windows.Forms.RadioButton();
            this.rbSelect = new System.Windows.Forms.RadioButton();
            this.txtDAOName = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errors)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(404, 64);
            this.panel1.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DSLFactory.Candle.SystemModel.Properties.Resources.XMLWS;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblHeader
            // 
            this.lblHeader.Location = new System.Drawing.Point(87, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(314, 47);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "With this wizard, you can associate a DAO to the entity {0}";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbExisting);
            this.groupBox1.Controls.Add(this.cbDAO);
            this.groupBox1.Controls.Add(this.lbCreate);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Controls.Add(this.rbNew);
            this.groupBox1.Controls.Add(this.rbSelect);
            this.groupBox1.Controls.Add(this.txtDAOName);
            this.groupBox1.Location = new System.Drawing.Point(11, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 166);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Associated DAO ";
            // 
            // lbExisting
            // 
            this.lbExisting.AutoSize = true;
            this.lbExisting.Location = new System.Drawing.Point(38, 60);
            this.lbExisting.Name = "lbExisting";
            this.lbExisting.Size = new System.Drawing.Size(41, 13);
            this.lbExisting.TabIndex = 15;
            this.lbExisting.Text = "Name :";
            // 
            // cbDAO
            // 
            this.cbDAO.FormattingEnabled = true;
            this.cbDAO.Location = new System.Drawing.Point(85, 57);
            this.cbDAO.Name = "cbDAO";
            this.cbDAO.Size = new System.Drawing.Size(279, 21);
            this.cbDAO.TabIndex = 14;
            // 
            // lbCreate
            // 
            this.lbCreate.AutoSize = true;
            this.lbCreate.Enabled = false;
            this.lbCreate.Location = new System.Drawing.Point(38, 110);
            this.lbCreate.Name = "lbCreate";
            this.lbCreate.Size = new System.Drawing.Size(41, 13);
            this.lbCreate.TabIndex = 13;
            this.lbCreate.Text = "Name :";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(18, 133);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(77, 17);
            this.rbNone.TabIndex = 12;
            this.rbNone.Text = "Do nothing";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbNew
            // 
            this.rbNew.AutoSize = true;
            this.rbNew.Location = new System.Drawing.Point(18, 84);
            this.rbNew.Name = "rbNew";
            this.rbNew.Size = new System.Drawing.Size(114, 17);
            this.rbNew.TabIndex = 11;
            this.rbNew.Text = "Create a new DAO";
            this.rbNew.UseVisualStyleBackColor = true;
            // 
            // rbSelect
            // 
            this.rbSelect.AutoSize = true;
            this.rbSelect.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rbSelect.Checked = true;
            this.rbSelect.Location = new System.Drawing.Point(18, 33);
            this.rbSelect.Name = "rbSelect";
            this.rbSelect.Size = new System.Drawing.Size(134, 17);
            this.rbSelect.TabIndex = 10;
            this.rbSelect.TabStop = true;
            this.rbSelect.Text = "Select an existing DAO";
            this.rbSelect.UseVisualStyleBackColor = true;
            this.rbSelect.CheckedChanged += new System.EventHandler(this.rbSelect_CheckedChanged);
            // 
            // txtDAOName
            // 
            this.txtDAOName.Enabled = false;
            this.txtDAOName.Location = new System.Drawing.Point(85, 107);
            this.txtDAOName.Name = "txtDAOName";
            this.txtDAOName.Size = new System.Drawing.Size(279, 20);
            this.txtDAOName.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(236, 242);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(317, 242);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // errors
            // 
            this.errors.ContainerControl = this;
            // 
            // DAOSelectorDlg
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(404, 272);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DAOSelectorDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add DAO";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDAOName;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ErrorProvider errors;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbExisting;
        private System.Windows.Forms.ComboBox cbDAO;
        private System.Windows.Forms.Label lbCreate;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbNew;
        private System.Windows.Forms.RadioButton rbSelect;
    }
}