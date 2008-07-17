namespace DSLFactory.Candle.SystemModel.Commands
{
    partial class ConfigurationEditorDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCurrent = new System.Windows.Forms.TabPage();
            this.lstConfigurations = new System.Windows.Forms.DataGridView();
            this.tabExternal = new System.Windows.Forms.TabPage();
            this.lstInheritedConfigurations = new System.Windows.Forms.DataGridView();
            this.colInheritedName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInheritedValue = new CandleGridViewMultilineTextBoxColumn();
            this.ColLayer = new DSLFactory.Candle.SystemModel.Utilities.DataGridViewDropDownListColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new CandleGridViewMultilineTextBoxColumn();
            this.colVisibility = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.bodyPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstConfigurations)).BeginInit();
            this.tabExternal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstInheritedConfigurations)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(72, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "You can disable a configuration to ignore it.";
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.label1);
            this.headerPanel.Controls.Add(this.pictureBox1);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(738, 64);
            this.headerPanel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::DSLFactory.Candle.SystemModel.Properties.Resources.Logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Controls.Add(this.btnOK);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 369);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(738, 47);
            this.bottomPanel.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(660, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(579, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // bodyPanel
            // 
            this.bodyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyPanel.BackColor = System.Drawing.SystemColors.Control;
            this.bodyPanel.Controls.Add(this.tabControl1);
            this.bodyPanel.Location = new System.Drawing.Point(0, 64);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(738, 304);
            this.bodyPanel.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCurrent);
            this.tabControl1.Controls.Add(this.tabExternal);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(738, 304);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabCurrent.Controls.Add(this.lstConfigurations);
            this.tabCurrent.Location = new System.Drawing.Point(4, 22);
            this.tabCurrent.Name = "tabPage1";
            this.tabCurrent.Padding = new System.Windows.Forms.Padding(3);
            this.tabCurrent.Size = new System.Drawing.Size(730, 278);
            this.tabCurrent.TabIndex = 0;
            this.tabCurrent.Text = "Current";
            this.tabCurrent.UseVisualStyleBackColor = true;
            // 
            // lstConfigurations
            // 
            this.lstConfigurations.AllowUserToAddRows = false;
            this.lstConfigurations.AllowUserToDeleteRows = false;
            this.lstConfigurations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstConfigurations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColLayer,
            this.ColName,
            this.colValue,
            this.colVisibility,
            this.ColEnabled});
            this.lstConfigurations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConfigurations.Location = new System.Drawing.Point(3, 3);
            this.lstConfigurations.MultiSelect = false;
            this.lstConfigurations.Name = "lstConfigurations";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.lstConfigurations.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.lstConfigurations.RowHeadersVisible = false;
            this.lstConfigurations.RowHeadersWidth = 40;
            this.lstConfigurations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lstConfigurations.ShowEditingIcon = false;
            this.lstConfigurations.Size = new System.Drawing.Size(724, 272);
            this.lstConfigurations.TabIndex = 0;
            this.lstConfigurations.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.lstConfigurations_RowsAdded);
            // 
            // tabPage2
            // 
            this.tabExternal.Controls.Add(this.lstInheritedConfigurations);
            this.tabExternal.Location = new System.Drawing.Point(4, 22);
            this.tabExternal.Name = "tabPage2";
            this.tabExternal.Padding = new System.Windows.Forms.Padding(3);
            this.tabExternal.Size = new System.Drawing.Size(730, 278);
            this.tabExternal.TabIndex = 1;
            this.tabExternal.Text = "External";
            this.tabExternal.UseVisualStyleBackColor = true;
            // 
            // lstInheritedConfigurations
            // 
            this.lstInheritedConfigurations.AllowUserToAddRows = false;
            this.lstInheritedConfigurations.AllowUserToDeleteRows = false;
            this.lstInheritedConfigurations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstInheritedConfigurations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInheritedName,
            this.colInheritedValue});
            this.lstInheritedConfigurations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstInheritedConfigurations.Location = new System.Drawing.Point(3, 3);
            this.lstInheritedConfigurations.MultiSelect = false;
            this.lstInheritedConfigurations.Name = "lstInheritedConfigurations";
            this.lstInheritedConfigurations.ReadOnly = true;
            this.lstInheritedConfigurations.RowHeadersVisible = false;
            this.lstInheritedConfigurations.RowHeadersWidth = 25;
            this.lstInheritedConfigurations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lstInheritedConfigurations.ShowEditingIcon = false;
            this.lstInheritedConfigurations.Size = new System.Drawing.Size(724, 272);
            this.lstInheritedConfigurations.TabIndex = 1;
            // 
            // colInheritedName
            // 
            this.colInheritedName.DataPropertyName = "Name";
            this.colInheritedName.FillWeight = 150F;
            this.colInheritedName.HeaderText = "Name";
            this.colInheritedName.MinimumWidth = 100;
            this.colInheritedName.Name = "colInheritedName";
            this.colInheritedName.ReadOnly = true;
            this.colInheritedName.Width = 150;
            // 
            // colInheritedValue
            // 
            this.colInheritedValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colInheritedValue.DataPropertyName = "XmlContent";
            this.colInheritedValue.FillWeight = 350F;
            this.colInheritedValue.HeaderText = "Value";
            this.colInheritedValue.MinimumWidth = 250;
            this.colInheritedValue.Name = "colInheritedValue";
            this.colInheritedValue.ReadOnly = true;
            // 
            // ColLayer
            // 
            this.ColLayer.DataPropertyName = "LayerName";
            this.ColLayer.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ColLayer.FillWeight = 200F;
            this.ColLayer.HeaderText = "Layer";
            this.ColLayer.MinimumWidth = 150;
            this.ColLayer.Name = "ColLayer";
            this.ColLayer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColLayer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColLayer.Width = 150;
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "ConfigName";
            this.ColName.FillWeight = 150F;
            this.ColName.HeaderText = "Name";
            this.ColName.MinimumWidth = 100;
            this.ColName.Name = "ColName";
            this.ColName.Width = 150;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.DataPropertyName = "XmlContent";
            this.colValue.FillWeight = 350F;
            this.colValue.HeaderText = "Value";
            this.colValue.MinimumWidth = 250;
            this.colValue.Name = "colValue";
            // 
            // colVisibility
            // 
            this.colVisibility.DataPropertyName = "Visibility";
            this.colVisibility.HeaderText = "Visibility";
            this.colVisibility.Name = "colVisibility";
            // 
            // ColEnabled
            // 
            this.ColEnabled.DataPropertyName = "Enabled";
            this.ColEnabled.FillWeight = 50F;
            this.ColEnabled.HeaderText = "Enabled";
            this.ColEnabled.MinimumWidth = 50;
            this.ColEnabled.Name = "ColEnabled";
            this.ColEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColEnabled.Width = 65;
            // 
            // ConfigurationEditorDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(738, 416);
            this.Controls.Add(this.bodyPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigurationEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurations";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bodyPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabCurrent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstConfigurations)).EndInit();
            this.tabExternal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstInheritedConfigurations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel bodyPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView lstConfigurations;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCurrent;
        private System.Windows.Forms.TabPage tabExternal;
        private System.Windows.Forms.DataGridView lstInheritedConfigurations;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInheritedName;
        private CandleGridViewMultilineTextBoxColumn colInheritedValue;
        private DSLFactory.Candle.SystemModel.Utilities.DataGridViewDropDownListColumn ColLayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private CandleGridViewMultilineTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVisibility;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColEnabled;
    }
}