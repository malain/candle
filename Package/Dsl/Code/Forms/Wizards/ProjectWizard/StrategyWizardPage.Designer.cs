namespace DSLFactory.Candle.SystemModel.Wizard
{
    partial class StrategyWizardPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrategyWizardPage));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstModels = new System.Windows.Forms.ListView();
            this.modelsImageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstModels);
            this.groupBox1.Location = new System.Drawing.Point(14, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(643, 305);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Templates";
            // 
            // lstModels
            // 
            this.lstModels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstModels.LargeImageList = this.modelsImageList;
            this.lstModels.Location = new System.Drawing.Point(6, 19);
            this.lstModels.MultiSelect = false;
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(631, 280);
            this.lstModels.SmallImageList = this.modelsImageList;
            this.lstModels.TabIndex = 6;
            this.lstModels.UseCompatibleStateImageBehavior = false;
            this.lstModels.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstModels_MouseDoubleClick);
            // 
            // modelsImageList
            // 
            this.modelsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("modelsImageList.ImageStream")));
            this.modelsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.modelsImageList.Images.SetKeyName(0, "File.ico");
            this.modelsImageList.Images.SetKeyName(1, "strategy.bmp");
            this.modelsImageList.Images.SetKeyName(2, "Tools.png");
            // 
            // StrategyWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "StrategyWizardPage";
            this.Size = new System.Drawing.Size(669, 334);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lstModels;
        private System.Windows.Forms.ImageList modelsImageList;
    }
}
