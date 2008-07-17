namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    partial class AssociationPropertiesSelectorForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgForeignKeys = new System.Windows.Forms.DataGridView();
            this.ColForeignKey = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColPrimaryKey = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblTarget = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgForeignKeys)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 64);
            this.panel1.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DSLFactory.Candle.SystemModel.Properties.Resources.Logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(72, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(251, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Select the foreign keys in the source entity";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(388, 12);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 367);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(572, 49);
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
            this.panel3.Size = new System.Drawing.Size(572, 47);
            this.panel3.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(479, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dgForeignKeys
            // 
            this.dgForeignKeys.AllowUserToResizeRows = false;
            this.dgForeignKeys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColForeignKey,
            this.ColPrimaryKey});
            this.dgForeignKeys.Location = new System.Drawing.Point(64, 95);
            this.dgForeignKeys.MultiSelect = false;
            this.dgForeignKeys.Name = "dgForeignKeys";
            this.dgForeignKeys.RowHeadersWidth = 22;
            this.dgForeignKeys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgForeignKeys.ShowEditingIcon = false;
            this.dgForeignKeys.Size = new System.Drawing.Size(445, 204);
            this.dgForeignKeys.TabIndex = 16;
            this.dgForeignKeys.VirtualMode = true;
            this.dgForeignKeys.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgForeignKeys_CellValueNeeded);
            this.dgForeignKeys.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgForeignKeys_RowsAdded);
            this.dgForeignKeys.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgForeignKeys_DataError);
            this.dgForeignKeys.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgForeignKeys_KeyUp);
            this.dgForeignKeys.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgForeignKeys_CellValuePushed);
            this.dgForeignKeys.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgForeignKeys_RowsRemoved);
            // 
            // ColForeignKey
            // 
            this.ColForeignKey.DataPropertyName = "Column";
            this.ColForeignKey.HeaderText = "Foreign Key";
            this.ColForeignKey.Name = "ColForeignKey";
            this.ColForeignKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColForeignKey.Width = 200;
            // 
            // ColPrimaryKey
            // 
            this.ColPrimaryKey.DataPropertyName = "PrimaryKey";
            this.ColPrimaryKey.HeaderText = "Primary Key";
            this.ColPrimaryKey.Name = "ColPrimaryKey";
            this.ColPrimaryKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColPrimaryKey.Width = 200;
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "Column";
            this.dataGridViewComboBoxColumn1.HeaderText = "Foreign Key";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.Width = 200;
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.DataPropertyName = "PrimaryKey";
            this.dataGridViewComboBoxColumn2.HeaderText = "Primary Key";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn2.Width = 200;
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(288, 79);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(38, 13);
            this.lblTarget.TabIndex = 17;
            this.lblTarget.Text = "Target";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(61, 79);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(41, 13);
            this.lblSource.TabIndex = 18;
            this.lblSource.Text = "Source";
            // 
            // AssociationPropertiesSelectorForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(572, 416);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.dgForeignKeys);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AssociationPropertiesSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Association foreign keys";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgForeignKeys)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgForeignKeys;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColForeignKey;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColPrimaryKey;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Label lblSource;
    }
}