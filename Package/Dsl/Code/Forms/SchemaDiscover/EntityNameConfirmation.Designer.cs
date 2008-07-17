namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    partial class EntityNameConfirmation
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.txtRootName = new System.Windows.Forms.TextBox();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtEntityName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(293, 39);
            label1.TabIndex = 0;
            label1.Text = "You can change the name of the entity or skip the importation of this entity.";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.txtEntityName);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(this.txtRootName);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new System.Drawing.Point(15, 41);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(290, 94);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // txtRootName
            // 
            this.txtRootName.Location = new System.Drawing.Point(79, 21);
            this.txtRootName.Name = "txtRootName";
            this.txtRootName.Size = new System.Drawing.Size(197, 20);
            this.txtRootName.TabIndex = 1;
            this.txtRootName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRootName_KeyUp);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 24);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 13);
            label2.TabIndex = 0;
            label2.Text = "Root Name :";
            // 
            // btnSkip
            // 
            this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSkip.Location = new System.Drawing.Point(230, 141);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 2;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(149, 141);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtEntityName
            // 
            this.txtEntityName.Location = new System.Drawing.Point(79, 57);
            this.txtEntityName.Name = "txtEntityName";
            this.txtEntityName.Size = new System.Drawing.Size(197, 20);
            this.txtEntityName.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 60);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 13);
            label3.TabIndex = 2;
            label3.Text = "Entity name :";
            // 
            // EntityNameConfirmation
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnSkip;
            this.ClientSize = new System.Drawing.Size(317, 176);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(groupBox1);
            this.Controls.Add(label1);
            this.Name = "EntityNameConfirmation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity Name Confirmation";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRootName;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtEntityName;
    }
}