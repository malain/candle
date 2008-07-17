namespace DSLFactory.Candle.SystemModel.Strategies
{
    partial class NamingStrategyControl
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label1;
            this.txtNamingStrategyType = new System.Windows.Forms.TextBox();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.propGrid);
            splitContainer1.Size = new System.Drawing.Size(732, 350);
            splitContainer1.SplitterDistance = 322;
            splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(this.txtNamingStrategyType);
            groupBox1.Location = new System.Drawing.Point(15, 14);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(293, 275);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Naming strategy";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 29);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(37, 13);
            label1.TabIndex = 1;
            label1.Text = "Type :";
            // 
            // txtNamingStrategyType
            // 
            this.txtNamingStrategyType.Location = new System.Drawing.Point(10, 45);
            this.txtNamingStrategyType.Multiline = true;
            this.txtNamingStrategyType.Name = "txtNamingStrategyType";
            this.txtNamingStrategyType.Size = new System.Drawing.Size(277, 80);
            this.txtNamingStrategyType.TabIndex = 0;
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(406, 350);
            this.propGrid.TabIndex = 0;
            // 
            // NamingStrategyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.Name = "NamingStrategyControl";
            this.Size = new System.Drawing.Size(732, 350);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtNamingStrategyType;
        private System.Windows.Forms.PropertyGrid propGrid;
    }
}
