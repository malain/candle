namespace DSLFactory.Candle.SystemModel.Repository.Forms
{
    partial class RepositoryTreeControl
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripLabel toolStripLabel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryTreeControl));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtFilterName = new System.Windows.Forms.ToolStripTextBox();
            this.lstComponent = new System.Windows.Forms.ToolStripDropDownButton();
            this.componentTypeSoftware = new System.Windows.Forms.ToolStripMenuItem();
            this.componentTypeLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this._mainPanel = new System.Windows.Forms.Panel();
            this._treeCtrl = new System.Windows.Forms.TreeView();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this.lbConnected = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._toolStrip.SuspendLayout();
            this._mainPanel.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(61, 22);
            toolStripLabel1.Text = "Start with :";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "Librairies.png");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "RepositoryFolder.bmp");
            this.imageList.Images.SetKeyName(4, "RepositoryModel.bmp");
            // 
            // _toolStrip
            // 
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.toolStripSeparator1,
            toolStripLabel1,
            this.txtFilterName,
            this.lstComponent,
            this.btnSelect});
            this._toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._toolStrip.Size = new System.Drawing.Size(286, 42);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtFilterName
            // 
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(100, 25);
            // 
            // lstComponent
            // 
            this.lstComponent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lstComponent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.componentTypeSoftware,
            this.componentTypeLibrary});
            this.lstComponent.Image = ((System.Drawing.Image)(resources.GetObject("lstComponent.Image")));
            this.lstComponent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lstComponent.Name = "lstComponent";
            this.lstComponent.Size = new System.Drawing.Size(44, 22);
            this.lstComponent.Text = "Type";
            // 
            // componentTypeSoftware
            // 
            this.componentTypeSoftware.Checked = true;
            this.componentTypeSoftware.CheckOnClick = true;
            this.componentTypeSoftware.CheckState = System.Windows.Forms.CheckState.Checked;
            this.componentTypeSoftware.Name = "componentTypeSoftware";
            this.componentTypeSoftware.Size = new System.Drawing.Size(129, 22);
            this.componentTypeSoftware.Text = "Software";
            // 
            // componentTypeLibrary
            // 
            this.componentTypeLibrary.Checked = true;
            this.componentTypeLibrary.CheckOnClick = true;
            this.componentTypeLibrary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.componentTypeLibrary.Name = "componentTypeLibrary";
            this.componentTypeLibrary.Size = new System.Drawing.Size(129, 22);
            this.componentTypeLibrary.Text = "Library";
            // 
            // btnSelect
            // 
            this.btnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelect.Image = global::DSLFactory.Candle.SystemModel.Properties.Resources.picto_arrow_down;
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(23, 22);
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // _mainPanel
            // 
            this._mainPanel.Controls.Add(this._treeCtrl);
            this._mainPanel.Controls.Add(this._statusStrip);
            this._mainPanel.Controls.Add(this._toolStrip);
            this._mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainPanel.Location = new System.Drawing.Point(0, 0);
            this._mainPanel.Name = "_mainPanel";
            this._mainPanel.Size = new System.Drawing.Size(286, 124);
            this._mainPanel.TabIndex = 3;
            // 
            // _treeCtrl
            // 
            this._treeCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeCtrl.FullRowSelect = true;
            this._treeCtrl.ImageIndex = 0;
            this._treeCtrl.ImageList = this.imageList;
            this._treeCtrl.Location = new System.Drawing.Point(0, 42);
            this._treeCtrl.Name = "_treeCtrl";
            this._treeCtrl.SelectedImageIndex = 0;
            this._treeCtrl.ShowNodeToolTips = true;
            this._treeCtrl.ShowRootLines = false;
            this._treeCtrl.Size = new System.Drawing.Size(286, 60);
            this._treeCtrl.TabIndex = 2;
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbConnected});
            this._statusStrip.Location = new System.Drawing.Point(0, 102);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(286, 22);
            this._statusStrip.TabIndex = 1;
            this._statusStrip.Text = "statusStrip1";
            // 
            // lbConnected
            // 
            this.lbConnected.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbConnected.Name = "lbConnected";
            this.lbConnected.Size = new System.Drawing.Size(85, 17);
            this.lbConnected.Text = "[Not connected]";
            this.lbConnected.Click += new System.EventHandler(this.lbConnected_Click);
            // 
            // RepositoryTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._mainPanel);
            this.Name = "RepositoryTreeControl";
            this.Size = new System.Drawing.Size(286, 124);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._mainPanel.ResumeLayout(false);
            this._mainPanel.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripTextBox txtFilterName;
        private System.Windows.Forms.ToolStripDropDownButton lstComponent;
        private System.Windows.Forms.ToolStripMenuItem componentTypeSoftware;
        private System.Windows.Forms.ToolStripMenuItem componentTypeLibrary;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel _mainPanel;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lbConnected;
        private System.Windows.Forms.TreeView _treeCtrl;
    }
}
