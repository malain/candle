
namespace DSLFactory.Candle.SystemModel.Designer
{
    partial class OperationsDesignerForm
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
            this.lblWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font( "Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblWarning.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblWarning.Location = new System.Drawing.Point( 201, 49 );
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size( 128, 16 );
            this.lblWarning.TabIndex = 1;
            this.lblWarning.Text = "Select an element";
            this.lblWarning.Visible = false;
            // 
            // OperationsDesignerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 915, 215 );
            this.Controls.Add( this.lblWarning );
            this.Font = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.Name = "OperationsDesignerForm";
            this.Text = "OperationsDesignerForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWarning;
    }
}