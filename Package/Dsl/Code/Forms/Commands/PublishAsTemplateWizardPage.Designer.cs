namespace DSLFactory.Candle.SystemModel.Commands
{
    partial class PublishAsTemplateWizardPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.txtStrategiesName = new System.Windows.Forms.TextBox();
            this.txtModelName = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(this.txtStrategiesName);
            groupBox1.Controls.Add(this.txtModelName);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(15, 14);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(639, 101);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Remote name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(334, 63);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(83, 13);
            label4.TabIndex = 3;
            label4.Text = "(blank to ignore)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(334, 34);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(83, 13);
            label3.TabIndex = 2;
            label3.Text = "(blank to ignore)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(21, 63);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(105, 13);
            label2.TabIndex = 0;
            label2.Text = "Strategies file name :";
            // 
            // txtStrategiesName
            // 
            this.txtStrategiesName.Location = new System.Drawing.Point(129, 60);
            this.txtStrategiesName.Name = "txtStrategiesName";
            this.txtStrategiesName.Size = new System.Drawing.Size(199, 20);
            this.txtStrategiesName.TabIndex = 2;
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(129, 31);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(199, 20);
            this.txtModelName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(21, 34);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(102, 13);
            label1.TabIndex = 0;
            label1.Text = "Template file name :";
            // 
            // PublishAsTemplateWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(groupBox1);
            this.HeaderText = "With this wizard, you can publish a model as a template on the repository. You ca" +
                "n also publish the associated strategies file.";
            this.Name = "PublishAsTemplateWizardPage";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtStrategiesName;
        private System.Windows.Forms.TextBox txtModelName;

    }
}
