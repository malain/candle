using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace DSLFactory.Candle.SystemModel.Wizard
{
    partial class CandleWizardForm
    {
        private Panel _buttonPanel;
        private Panel _headerPanel;
        private Panel _interiorPagePanel;
        private Panel _buttonLine;
        private Panel _pagePanel;

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
            this._headerPanel = new System.Windows.Forms.Panel();
            this._headerLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._pagePanel = new System.Windows.Forms.Panel();
            this._buttonLine = new System.Windows.Forms.Panel();
            this._interiorPagePanel = new System.Windows.Forms.Panel();
            this._buttonPanel = new System.Windows.Forms.Panel();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this._headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this._pagePanel.SuspendLayout();
            this._buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _headerPanel
            // 
            this._headerPanel.BackColor = System.Drawing.SystemColors.Window;
            this._headerPanel.Controls.Add(this._headerLabel);
            this._headerPanel.Controls.Add(this.pictureBox1);
            this._headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerPanel.Location = new System.Drawing.Point(0, 0);
            this._headerPanel.Margin = new System.Windows.Forms.Padding(0);
            this._headerPanel.MaximumSize = new System.Drawing.Size(0, 64);
            this._headerPanel.MinimumSize = new System.Drawing.Size(0, 64);
            this._headerPanel.Name = "_headerPanel";
            this._headerPanel.Size = new System.Drawing.Size(669, 64);
            this._headerPanel.TabIndex = 0;
            // 
            // _headerLabel
            // 
            this._headerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._headerLabel.Location = new System.Drawing.Point(64, 0);
            this._headerLabel.Margin = new System.Windows.Forms.Padding(12, 4, 8, 4);
            this._headerLabel.Name = "_headerLabel";
            this._headerLabel.Size = new System.Drawing.Size(605, 64);
            this._headerLabel.TabIndex = 1;
            this._headerLabel.Text = "label1";
            this._headerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // _pagePanel
            // 
            this._pagePanel.Controls.Add(this._buttonLine);
            this._pagePanel.Controls.Add(this._interiorPagePanel);
            this._pagePanel.Controls.Add(this._buttonPanel);
            this._pagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pagePanel.Location = new System.Drawing.Point(0, 64);
            this._pagePanel.Margin = new System.Windows.Forms.Padding(0);
            this._pagePanel.Name = "_pagePanel";
            this._pagePanel.Size = new System.Drawing.Size(669, 361);
            this._pagePanel.TabIndex = 6;
            // 
            // _buttonLine
            // 
            this._buttonLine.BackColor = System.Drawing.SystemColors.Window;
            this._buttonLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._buttonLine.Location = new System.Drawing.Point(0, 360);
            this._buttonLine.Name = "_buttonLine";
            this._buttonLine.Size = new System.Drawing.Size(669, 1);
            this._buttonLine.TabIndex = 7;
            // 
            // _interiorPagePanel
            // 
            this._interiorPagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._interiorPagePanel.Location = new System.Drawing.Point(0, 0);
            this._interiorPagePanel.Margin = new System.Windows.Forms.Padding(10);
            this._interiorPagePanel.Name = "_interiorPagePanel";
            this._interiorPagePanel.Size = new System.Drawing.Size(669, 314);
            this._interiorPagePanel.TabIndex = 0;
            // 
            // _buttonPanel
            // 
            this._buttonPanel.AutoSize = true;
            this._buttonPanel.Controls.Add(this.btnPrevious);
            this._buttonPanel.Controls.Add(this.btnNext);
            this._buttonPanel.Controls.Add(this.btnFinish);
            this._buttonPanel.Controls.Add(this.btnCancel);
            this._buttonPanel.Location = new System.Drawing.Point(0, 314);
            this._buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this._buttonPanel.Name = "_buttonPanel";
            this._buttonPanel.Padding = new System.Windows.Forms.Padding(0, 15, 15, 15);
            this._buttonPanel.Size = new System.Drawing.Size(684, 53);
            this._buttonPanel.TabIndex = 6;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(348, 12);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(429, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(510, 12);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(591, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CandleWizardForm
            // 
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(669, 425);
            this.Controls.Add(this._pagePanel);
            this.Controls.Add(this._headerPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CandleWizardForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this._headerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this._pagePanel.ResumeLayout(false);
            this._pagePanel.PerformLayout();
            this._buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private Label _headerLabel;
        private Button btnPrevious;
        private Button btnNext;
        private Button btnFinish;
        private Button btnCancel;
    }
}