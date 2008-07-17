namespace DSLFactory.Candle.SystemModel.Strategies
{
    partial class StrategiesForm
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
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label1;
            this.tabStrategies = new System.Windows.Forms.TabControl();
            this.tabGlobals = new System.Windows.Forms.TabPage();
            this.tabNaming = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tabLanguage = new System.Windows.Forms.TabPage();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            this.tabStrategies.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(749, 37);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(257, 13);
            label1.TabIndex = 0;
            label1.Text = "List of strategies availables for this element.";
            // 
            // tabStrategies
            // 
            this.tabStrategies.Controls.Add(this.tabGlobals);
            this.tabStrategies.Controls.Add(this.tabNaming);
            this.tabStrategies.Controls.Add(this.tabLanguage);
            this.tabStrategies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabStrategies.Location = new System.Drawing.Point(0, 37);
            this.tabStrategies.Name = "tabStrategies";
            this.tabStrategies.SelectedIndex = 0;
            this.tabStrategies.Size = new System.Drawing.Size(749, 374);
            this.tabStrategies.TabIndex = 2;
            // 
            // tabGlobals
            // 
            this.tabGlobals.Location = new System.Drawing.Point(4, 22);
            this.tabGlobals.Name = "tabGlobals";
            this.tabGlobals.Size = new System.Drawing.Size(741, 348);
            this.tabGlobals.TabIndex = 1;
            this.tabGlobals.Text = "Globals";
            this.tabGlobals.UseVisualStyleBackColor = true;
            // 
            // tabNaming
            // 
            this.tabNaming.Location = new System.Drawing.Point(4, 22);
            this.tabNaming.Name = "tabNaming";
            this.tabNaming.Size = new System.Drawing.Size(741, 348);
            this.tabNaming.TabIndex = 2;
            this.tabNaming.Text = "Naming Strategy";
            this.tabNaming.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(662, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(566, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 411);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(749, 48);
            this.pnlButtons.TabIndex = 1;
            // 
            // tabLanguage
            // 
            this.tabLanguage.Location = new System.Drawing.Point(4, 22);
            this.tabLanguage.Name = "tabLanguage";
            this.tabLanguage.Padding = new System.Windows.Forms.Padding(3);
            this.tabLanguage.Size = new System.Drawing.Size(741, 348);
            this.tabLanguage.TabIndex = 3;
            this.tabLanguage.Text = "Target language";
            this.tabLanguage.UseVisualStyleBackColor = true;
            // 
            // StrategiesForm
            // 
            this.ClientSize = new System.Drawing.Size(749, 459);
            this.Controls.Add(this.tabStrategies);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(panel1);
            this.Name = "StrategiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Strategies";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            this.tabStrategies.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabStrategies;
        private System.Windows.Forms.TabPage tabGlobals;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TabPage tabNaming;
        private System.Windows.Forms.TabPage tabLanguage;
    }
}