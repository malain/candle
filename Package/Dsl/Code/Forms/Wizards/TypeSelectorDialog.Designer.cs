namespace DSLFactory.Candle.SystemModel
{
    partial class TypeSelectorDialog
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstTypes = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Location = new System.Drawing.Point( 3, 0 );
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size( 429, 47 );
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Select the good type or select an action";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
            this.tableLayoutPanel1.Controls.Add( this.lblHeader, 0, 0 );
            this.tableLayoutPanel1.Controls.Add( this.panel1, 0, 2 );
            this.tableLayoutPanel1.Controls.Add( this.panel2, 0, 1 );
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 19.35484F ) );
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 80.64516F ) );
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute, 50F ) );
            this.tableLayoutPanel1.Size = new System.Drawing.Size( 435, 295 );
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.btnSelect );
            this.panel1.Controls.Add( this.btnCancel );
            this.panel1.Controls.Add( this.btnNew );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point( 3, 247 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 429, 45 );
            this.panel1.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point( 264, 13 );
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size( 75, 23 );
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnCancel.Location = new System.Drawing.Point( 345, 13 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Do nothing";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNew.Location = new System.Drawing.Point( 9, 13 );
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size( 104, 23 );
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Create new";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add( this.lstTypes );
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point( 3, 50 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 429, 191 );
            this.panel2.TabIndex = 2;
            // 
            // lstTypes
            // 
            this.lstTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTypes.FormattingEnabled = true;
            this.lstTypes.Location = new System.Drawing.Point( 0, 0 );
            this.lstTypes.Name = "lstTypes";
            this.lstTypes.Size = new System.Drawing.Size( 429, 186 );
            this.lstTypes.TabIndex = 0;
            this.lstTypes.SelectedIndexChanged += new System.EventHandler( this.lstTypes_SelectedIndexChanged );
            // 
            // TypeSelectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 435, 295 );
            this.Controls.Add( this.tableLayoutPanel1 );
            this.Name = "TypeSelectorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New type detected";
            this.tableLayoutPanel1.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.panel2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lstTypes;
        private System.Windows.Forms.Button btnSelect;
    }
}