namespace DSLFactory.Candle.SystemModel
{
    partial class CandleLogWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CandleLogWindow));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lstMessage = new System.Windows.Forms.ListView();
            this.colMessage = new System.Windows.Forms.ColumnHeader();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.chkAutoClose = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 12);
            this.progressBar.MarqueeAnimationSpeed = 35;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(442, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // lstMessage
            // 
            this.lstMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMessage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMessage});
            this.lstMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMessage.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstMessage.Location = new System.Drawing.Point(12, 41);
            this.lstMessage.MultiSelect = false;
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.ShowItemToolTips = true;
            this.lstMessage.Size = new System.Drawing.Size(442, 262);
            this.lstMessage.SmallImageList = this.imageList;
            this.lstMessage.TabIndex = 2;
            this.lstMessage.UseCompatibleStateImageBehavior = false;
            this.lstMessage.View = System.Windows.Forms.View.Details;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 432;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList.Images.SetKeyName(0, "log_error.bmp");
            this.imageList.Images.SetKeyName(1, "Log_warning.bmp");
            this.imageList.Images.SetKeyName(2, "Log_run.bmp");
            this.imageList.Images.SetKeyName(3, "Log_OK.bmp");
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(379, 318);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkAutoClose
            // 
            this.chkAutoClose.AutoSize = true;
            this.chkAutoClose.Checked = true;
            this.chkAutoClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoClose.Location = new System.Drawing.Point(12, 324);
            this.chkAutoClose.Name = "chkAutoClose";
            this.chkAutoClose.Size = new System.Drawing.Size(168, 17);
            this.chkAutoClose.TabIndex = 4;
            this.chkAutoClose.Text = "Close automatically if no errors";
            this.chkAutoClose.UseVisualStyleBackColor = true;
            // 
            // CandleLogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 353);
            this.ControlBox = false;
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.chkAutoClose);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(20, 20);
            this.Name = "CandleLogWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Candle process log";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListView lstMessage;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkAutoClose;
    }
}

