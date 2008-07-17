namespace DSLFactory.Candle.SystemModel.Editor
{
    partial class TargetModelSelector
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
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode( "SoftwareComponent" );
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode( "BinaryComponent" );
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode( "Component", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12} );
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode( "DALLayer" );
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode( "BusinessLayer" );
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode( "DataLayer" );
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode( "UIWorkflowLayer" );
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode( "Layer", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17} );
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode( "DotnetAssembly" );
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode( "BaseLayer", new System.Windows.Forms.TreeNode[] {
            treeNode18,
            treeNode19} );
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tvModelTypes = new System.Windows.Forms.TreeView();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 358, 46 );
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add( this.btnOK );
            this.panel2.Controls.Add( this.btnCancel );
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point( 0, 315 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 358, 46 );
            this.panel2.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point( 190, 12 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 271, 12 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tvModelTypes
            // 
            this.tvModelTypes.CheckBoxes = true;
            this.tvModelTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvModelTypes.Location = new System.Drawing.Point( 0, 46 );
            this.tvModelTypes.Name = "tvModelTypes";
            treeNode11.Name = "Noeud1";
            treeNode11.Text = "SoftwareComponent";
            treeNode12.Name = "Noeud2";
            treeNode12.Text = "BinaryComponent";
            treeNode13.Name = "Noeud0";
            treeNode13.Text = "Component";
            treeNode14.Name = "Noeud6";
            treeNode14.Text = "DALLayer";
            treeNode15.Name = "Noeud7";
            treeNode15.Text = "BusinessLayer";
            treeNode16.Name = "Noeud8";
            treeNode16.Text = "DataLayer";
            treeNode17.Name = "Noeud9";
            treeNode17.Text = "UIWorkflowLayer";
            treeNode18.Name = "Noeud4";
            treeNode18.Text = "Layer";
            treeNode19.Name = "Noeud5";
            treeNode19.Text = "DotnetAssembly";
            treeNode20.Name = "Noeud3";
            treeNode20.Text = "BaseLayer";
            this.tvModelTypes.Nodes.AddRange( new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode20} );
            this.tvModelTypes.ShowNodeToolTips = true;
            this.tvModelTypes.Size = new System.Drawing.Size( 358, 269 );
            this.tvModelTypes.TabIndex = 2;
            this.tvModelTypes.AfterCheck += new System.Windows.Forms.TreeViewEventHandler( tvModelTypes_AfterCheck );
            // 
            // TargetModelSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 358, 361 );
            this.Controls.Add( this.tvModelTypes );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.panel1 );
            this.Name = "TargetModelSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Target Models Selector";
            this.Load += new System.EventHandler( this.TargetModelSelector_Load );
            this.panel2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TreeView tvModelTypes;
        private System.Windows.Forms.Button btnOK;
    }
}