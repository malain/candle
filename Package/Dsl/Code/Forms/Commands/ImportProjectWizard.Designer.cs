namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    partial class ImportProjectWizard
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
            System.Windows.Forms.GroupBox groupBox1;
            this.rbModels = new System.Windows.Forms.RadioButton();
            this.rbUI = new System.Windows.Forms.RadioButton();
            this.rbBLL = new System.Windows.Forms.RadioButton();
            this.rbPres = new System.Windows.Forms.RadioButton();
            this.rbDAL = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbInterface = new System.Windows.Forms.RadioButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.rbInterface);
            groupBox1.Controls.Add(this.rbModels);
            groupBox1.Controls.Add(this.rbUI);
            groupBox1.Controls.Add(this.rbBLL);
            groupBox1.Controls.Add(this.rbPres);
            groupBox1.Controls.Add(this.rbDAL);
            groupBox1.Location = new System.Drawing.Point(12, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(249, 167);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Layer Type";
            // 
            // rbModels
            // 
            this.rbModels.AutoSize = true;
            this.rbModels.Location = new System.Drawing.Point(32, 122);
            this.rbModels.Name = "rbModels";
            this.rbModels.Size = new System.Drawing.Size(88, 17);
            this.rbModels.TabIndex = 5;
            this.rbModels.TabStop = true;
            this.rbModels.Text = "Models Layer";
            this.rbModels.UseVisualStyleBackColor = true;
            // 
            // rbUI
            // 
            this.rbUI.AutoSize = true;
            this.rbUI.Location = new System.Drawing.Point(32, 53);
            this.rbUI.Name = "rbUI";
            this.rbUI.Size = new System.Drawing.Size(113, 17);
            this.rbUI.TabIndex = 2;
            this.rbUI.TabStop = true;
            this.rbUI.Text = "UI Workflow Layer";
            this.rbUI.UseVisualStyleBackColor = true;
            // 
            // rbBLL
            // 
            this.rbBLL.AutoSize = true;
            this.rbBLL.Location = new System.Drawing.Point(32, 76);
            this.rbBLL.Name = "rbBLL";
            this.rbBLL.Size = new System.Drawing.Size(96, 17);
            this.rbBLL.TabIndex = 3;
            this.rbBLL.TabStop = true;
            this.rbBLL.Text = "Business Layer";
            this.rbBLL.UseVisualStyleBackColor = true;
            // 
            // rbPres
            // 
            this.rbPres.AutoSize = true;
            this.rbPres.Location = new System.Drawing.Point(32, 30);
            this.rbPres.Name = "rbPres";
            this.rbPres.Size = new System.Drawing.Size(152, 17);
            this.rbPres.TabIndex = 1;
            this.rbPres.TabStop = true;
            this.rbPres.Text = "Generic presentation Layer";
            this.rbPres.UseVisualStyleBackColor = true;
            // 
            // rbDAL
            // 
            this.rbDAL.AutoSize = true;
            this.rbDAL.Location = new System.Drawing.Point(32, 99);
            this.rbDAL.Name = "rbDAL";
            this.rbDAL.Size = new System.Drawing.Size(115, 17);
            this.rbDAL.TabIndex = 4;
            this.rbDAL.TabStop = true;
            this.rbDAL.Text = "Data Access Layer";
            this.rbDAL.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 40);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(273, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select the target layer type :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 245);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 47);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(186, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Annuler";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(105, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(groupBox1);
            this.panel3.Location = new System.Drawing.Point(0, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(273, 208);
            this.panel3.TabIndex = 3;
            // 
            // rbInterface
            // 
            this.rbInterface.AutoSize = true;
            this.rbInterface.Location = new System.Drawing.Point(32, 145);
            this.rbInterface.Name = "rbInterface";
            this.rbInterface.Size = new System.Drawing.Size(96, 17);
            this.rbInterface.TabIndex = 6;
            this.rbInterface.TabStop = true;
            this.rbInterface.Text = "Interface Layer";
            this.rbInterface.UseVisualStyleBackColor = true;
            // 
            // ImportProjectWizard
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(273, 292);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "ImportProjectWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Importation";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbModels;
        private System.Windows.Forms.RadioButton rbDAL;
        private System.Windows.Forms.RadioButton rbBLL;
        private System.Windows.Forms.RadioButton rbUI;
        private System.Windows.Forms.RadioButton rbPres;
        private System.Windows.Forms.RadioButton rbInterface;
    }
}