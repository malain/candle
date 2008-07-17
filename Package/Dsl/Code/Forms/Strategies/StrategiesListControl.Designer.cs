namespace DSLFactory.Candle.SystemModel.Strategies
{
    partial class StrategiesListControl
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ColumnHeader colStrategyName;
            this.lvStrategies = new System.Windows.Forms.ListView();
            this.tvStrategies = new System.Windows.Forms.TreeView();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            colStrategyName = new System.Windows.Forms.ColumnHeader();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(this.lvStrategies);
            splitContainer1.Panel1.Controls.Add(this.tvStrategies);
            splitContainer1.Panel1.Controls.Add(this.txtComment);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.btnAdd);
            splitContainer1.Panel2.Controls.Add(this.btnRemove);
            splitContainer1.Panel2.Controls.Add(this.propGrid);
            splitContainer1.Size = new System.Drawing.Size(732, 350);
            splitContainer1.SplitterDistance = 332;
            splitContainer1.TabIndex = 3;
            // 
            // lvStrategies
            // 
            this.lvStrategies.CausesValidation = false;
            this.lvStrategies.CheckBoxes = true;
            this.lvStrategies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colStrategyName});
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            this.lvStrategies.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.lvStrategies.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStrategies.HideSelection = false;
            this.lvStrategies.Location = new System.Drawing.Point(33, 116);
            this.lvStrategies.MultiSelect = false;
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new System.Drawing.Size(253, 106);
            this.lvStrategies.TabIndex = 4;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = System.Windows.Forms.View.Details;
            this.lvStrategies.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvStrategies_ItemChecked);
            this.lvStrategies.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvStrategies_ItemSelectionChanged);
            // 
            // colStrategyName
            // 
            colStrategyName.Text = "Name";
            colStrategyName.Width = 180;
            // 
            // tvStrategies
            // 
            this.tvStrategies.CheckBoxes = true;
            this.tvStrategies.HideSelection = false;
            this.tvStrategies.Location = new System.Drawing.Point(16, 3);
            this.tvStrategies.Name = "tvStrategies";
            this.tvStrategies.PathSeparator = "/";
            this.tvStrategies.ShowRootLines = false;
            this.tvStrategies.Size = new System.Drawing.Size(313, 94);
            this.tvStrategies.TabIndex = 3;
            this.tvStrategies.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvStrategies_NodeMouseDoubleClick);
            this.tvStrategies.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvStrategies_AfterCheck);
            this.tvStrategies.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvStrategies_AfterSelect);
            // 
            // txtComment
            // 
            this.txtComment.BackColor = System.Drawing.SystemColors.Window;
            this.txtComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtComment.Location = new System.Drawing.Point(0, 258);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ReadOnly = true;
            this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComment.Size = new System.Drawing.Size(332, 92);
            this.txtComment.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(186, 322);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(97, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add a strategy";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(289, 322);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(98, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove strategy";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(396, 316);
            this.propGrid.TabIndex = 5;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propGrid_PropertyValueChanged);
            // 
            // StrategiesListControl
            // 
            this.Controls.Add(splitContainer1);
            this.Name = "StrategiesListControl";
            this.Size = new System.Drawing.Size(732, 350);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TreeView tvStrategies;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListView lvStrategies;

    }
}