namespace DSLFactory.Candle.SystemModel.Commands
{
    partial class ShowDependenciesForm
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
            this.button2 = new System.Windows.Forms.Button();
            this.lstArtefacts = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbPublish = new System.Windows.Forms.RadioButton();
            this.rbCompilation = new System.Windows.Forms.RadioButton();
            this.rbRuntime = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(548, 413);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Fermer";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lstArtefacts
            // 
            this.lstArtefacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstArtefacts.FormattingEnabled = true;
            this.lstArtefacts.HorizontalScrollbar = true;
            this.lstArtefacts.Location = new System.Drawing.Point(12, 89);
            this.lstArtefacts.Name = "lstArtefacts";
            this.lstArtefacts.Size = new System.Drawing.Size(611, 316);
            this.lstArtefacts.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbPublish);
            this.groupBox1.Controls.Add(this.rbCompilation);
            this.groupBox1.Controls.Add(this.rbRuntime);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 71);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scope";
            // 
            // rbPublish
            // 
            this.rbPublish.AutoSize = true;
            this.rbPublish.Location = new System.Drawing.Point(126, 21);
            this.rbPublish.Name = "rbPublish";
            this.rbPublish.Size = new System.Drawing.Size(59, 17);
            this.rbPublish.TabIndex = 2;
            this.rbPublish.Text = "Publish";
            this.rbPublish.UseVisualStyleBackColor = true;
            // 
            // rbCompilation
            // 
            this.rbCompilation.AutoSize = true;
            this.rbCompilation.Checked = true;
            this.rbCompilation.Location = new System.Drawing.Point(16, 21);
            this.rbCompilation.Name = "rbCompilation";
            this.rbCompilation.Size = new System.Drawing.Size(79, 17);
            this.rbCompilation.TabIndex = 1;
            this.rbCompilation.TabStop = true;
            this.rbCompilation.Text = "Compilation";
            this.rbCompilation.UseVisualStyleBackColor = true;
            // 
            // rbRuntime
            // 
            this.rbRuntime.AutoSize = true;
            this.rbRuntime.Location = new System.Drawing.Point(126, 44);
            this.rbRuntime.Name = "rbRuntime";
            this.rbRuntime.Size = new System.Drawing.Size(64, 17);
            this.rbRuntime.TabIndex = 0;
            this.rbRuntime.Text = "Runtime";
            this.rbRuntime.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(542, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Liste";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(220, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Configuration Mode :";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(332, 20);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(103, 21);
            this.cbMode.TabIndex = 6;
            // 
            // cbAction
            // 
            this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Location = new System.Drawing.Point(332, 57);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(103, 21);
            this.cbAction.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(282, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Action :";
            // 
            // ShowDependenciesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 448);
            this.Controls.Add(this.cbAction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstArtefacts);
            this.Controls.Add(this.button2);
            this.Name = "ShowDependenciesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show dependencies";
            this.Load += new System.EventHandler(this.ShowDependenciesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lstArtefacts;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbCompilation;
        private System.Windows.Forms.RadioButton rbRuntime;
        private System.Windows.Forms.RadioButton rbPublish;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.ComboBox cbAction;
        private System.Windows.Forms.Label label2;
    }
}