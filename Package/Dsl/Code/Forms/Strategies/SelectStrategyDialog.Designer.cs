namespace DSLFactory.Candle.SystemModel.Strategies
{
    partial class SelectStrategyDialog
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel headerPanel;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Panel bottomPanel;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Panel bodyPanel;
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ColumnHeader colStrategyName;
            System.Windows.Forms.ColumnHeader colType;
            System.Windows.Forms.ColumnHeader colPackage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectStrategyDialog));
            this.rbTree = new System.Windows.Forms.RadioButton();
            this.rbList = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lvStrategies = new System.Windows.Forms.ListView();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.tvStrategies = new System.Windows.Forms.TreeView();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            label1 = new System.Windows.Forms.Label();
            headerPanel = new System.Windows.Forms.Panel();
            groupBox1 = new System.Windows.Forms.GroupBox();
            bottomPanel = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            bodyPanel = new System.Windows.Forms.Panel();
            colStrategyName = new System.Windows.Forms.ColumnHeader();
            colType = new System.Windows.Forms.ColumnHeader();
            colPackage = new System.Windows.Forms.ColumnHeader();
            headerPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            bottomPanel.SuspendLayout();
            bodyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(72, 20);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(104, 13);
            label1.TabIndex = 1;
            label1.Text = "Select a strategy";
            // 
            // headerPanel
            // 
            headerPanel.BackColor = System.Drawing.SystemColors.Control;
            headerPanel.Controls.Add(groupBox1);
            headerPanel.Controls.Add(label1);
            headerPanel.Controls.Add(this.pictureBox1);
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Location = new System.Drawing.Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new System.Drawing.Size(572, 64);
            headerPanel.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.rbTree);
            groupBox1.Controls.Add(this.rbList);
            groupBox1.Location = new System.Drawing.Point(461, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(99, 55);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Display";
            // 
            // rbTree
            // 
            this.rbTree.AutoSize = true;
            this.rbTree.Location = new System.Drawing.Point(20, 32);
            this.rbTree.Name = "rbTree";
            this.rbTree.Size = new System.Drawing.Size(73, 17);
            this.rbTree.TabIndex = 1;
            this.rbTree.Text = "Tree View";
            this.rbTree.UseVisualStyleBackColor = true;
            this.rbTree.CheckedChanged += new System.EventHandler(this.rbTree_CheckedChanged);
            // 
            // rbList
            // 
            this.rbList.AutoSize = true;
            this.rbList.Checked = true;
            this.rbList.Location = new System.Drawing.Point(20, 13);
            this.rbList.Name = "rbList";
            this.rbList.Size = new System.Drawing.Size(67, 17);
            this.rbList.TabIndex = 0;
            this.rbList.TabStop = true;
            this.rbList.Text = "List View";
            this.rbList.UseVisualStyleBackColor = true;
            this.rbList.CheckedChanged += new System.EventHandler(this.rbList_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = System.Drawing.SystemColors.Control;
            bottomPanel.Controls.Add(this.btnGo);
            bottomPanel.Controls.Add(label2);
            bottomPanel.Controls.Add(this.txtUrl);
            bottomPanel.Controls.Add(this.btnCancel);
            bottomPanel.Controls.Add(this.btnOK);
            bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            bottomPanel.Location = new System.Drawing.Point(0, 380);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new System.Drawing.Size(572, 47);
            bottomPanel.TabIndex = 1;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(314, 12);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(30, 23);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 17);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(26, 13);
            label2.TabIndex = 3;
            label2.Text = "Url :";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(35, 14);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(273, 20);
            this.txtUrl.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(485, 12);
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
            this.btnOK.Location = new System.Drawing.Point(404, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // bodyPanel
            // 
            bodyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            bodyPanel.BackColor = System.Drawing.SystemColors.Control;
            bodyPanel.Controls.Add(this.lvStrategies);
            bodyPanel.Controls.Add(this.txtDescription);
            bodyPanel.Controls.Add(this.tvStrategies);
            bodyPanel.Location = new System.Drawing.Point(0, 64);
            bodyPanel.Name = "bodyPanel";
            bodyPanel.Size = new System.Drawing.Size(572, 315);
            bodyPanel.TabIndex = 2;
            // 
            // lvStrategies
            // 
            this.lvStrategies.CausesValidation = false;
            this.lvStrategies.CheckBoxes = true;
            this.lvStrategies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colStrategyName,
            colType,
            colPackage});
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            this.lvStrategies.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.lvStrategies.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStrategies.Location = new System.Drawing.Point(3, 6);
            this.lvStrategies.MultiSelect = false;
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new System.Drawing.Size(566, 97);
            this.lvStrategies.TabIndex = 2;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = System.Windows.Forms.View.Details;
            this.lvStrategies.SelectedIndexChanged += new System.EventHandler(this.lvStrategies_SelectedIndexChanged);
            // 
            // colStrategyName
            // 
            colStrategyName.Text = "Name";
            colStrategyName.Width = 220;
            // 
            // colType
            // 
            colType.Text = "Type";
            colType.Width = 210;
            // 
            // colPackage
            // 
            colPackage.Text = "Package";
            colPackage.Width = 115;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDescription.Location = new System.Drawing.Point(0, 250);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(572, 65);
            this.txtDescription.TabIndex = 1;
            // 
            // tvStrategies
            // 
            this.tvStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvStrategies.CheckBoxes = true;
            this.tvStrategies.Location = new System.Drawing.Point(0, 0);
            this.tvStrategies.Name = "tvStrategies";
            this.tvStrategies.Size = new System.Drawing.Size(572, 246);
            this.tvStrategies.TabIndex = 0;
            this.tvStrategies.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvStrategies_NodeMouseDoubleClick);
            this.tvStrategies.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvStrategies_AfterSelect);
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.treeViewImageList.Images.SetKeyName(0, "ServicesLayerShapeIcon.bmp");
            this.treeViewImageList.Images.SetKeyName(1, "folderopen.ico");
            // 
            // SelectStrategyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(572, 427);
            this.Controls.Add(bodyPanel);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectStrategyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Strategy selection";
            this.Load += new System.EventHandler(this.SelectStrategyDialog_Load);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            bodyPanel.ResumeLayout(false);
            bodyPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TreeView tvStrategies;
        private System.Windows.Forms.ImageList treeViewImageList;
        private System.Windows.Forms.ListView lvStrategies;
        private System.Windows.Forms.RadioButton rbTree;
        private System.Windows.Forms.RadioButton rbList;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtUrl;
    }
}