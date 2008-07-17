namespace DSLFactory.Candle.SystemModel.Strategies
{
    partial class LanguageConfigurationControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label7;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProjectTemplate = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(21, 37);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(90, 13);
            label1.TabIndex = 1;
            label1.Text = "Language name :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(297, 37);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(276, 13);
            label3.TabIndex = 3;
            label3.Text = "This must be a valid language supported by Visual Studio";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(297, 77);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(219, 13);
            label4.TabIndex = 5;
            label4.Text = "Default extension for the generated code file.";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(354, 134);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(310, 13);
            label7.TabIndex = 11;
            label7.Text = "Default project template used for generated Visual Studio project";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(label7);
            this.groupBox1.Controls.Add(this.txtProjectTemplate);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(this.txtExtension);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.cbLanguage);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 330);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Language configuration";
            // 
            // txtProjectTemplate
            // 
            this.txtProjectTemplate.Location = new System.Drawing.Point(146, 131);
            this.txtProjectTemplate.Name = "txtProjectTemplate";
            this.txtProjectTemplate.Size = new System.Drawing.Size(202, 20);
            this.txtProjectTemplate.TabIndex = 10;
            this.txtProjectTemplate.TextChanged += new System.EventHandler(this.txtProjectTemplate_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Default project template";
            // 
            // txtExtension
            // 
            this.txtExtension.Location = new System.Drawing.Point(146, 74);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(145, 20);
            this.txtExtension.TabIndex = 4;
            this.txtExtension.TextChanged += new System.EventHandler(this.txtExtension_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Default extension";
            // 
            // cbLanguage
            // 
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Items.AddRange(new object[] {
            "CSharp",
            "VisualBasic"});
            this.cbLanguage.Location = new System.Drawing.Point(146, 34);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(145, 21);
            this.cbLanguage.TabIndex = 0;
            this.cbLanguage.TextChanged += new System.EventHandler(this.cbLanguage_TextChanged);
            // 
            // LanguageConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "LanguageConfigurationControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(732, 350);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProjectTemplate;
        private System.Windows.Forms.Label label8;

    }
}
