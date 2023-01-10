namespace InstrumentEditor {
    partial class InstInfoDialog {
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
            this.txtInstName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.grbCategory = new System.Windows.Forms.GroupBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.envelope1 = new InstrumentEditor.Articulations();
            this.groupBox5.SuspendLayout();
            this.grbCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInstName
            // 
            this.txtInstName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInstName.Location = new System.Drawing.Point(4, 17);
            this.txtInstName.Name = "txtInstName";
            this.txtInstName.Size = new System.Drawing.Size(206, 19);
            this.txtInstName.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAdd.Location = new System.Drawing.Point(274, 428);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(59, 24);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox5.Controls.Add(this.txtInstName);
            this.groupBox5.Location = new System.Drawing.Point(5, 6);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox5.Size = new System.Drawing.Size(263, 38);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "音色名";
            // 
            // grbCategory
            // 
            this.grbCategory.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbCategory.Controls.Add(this.cmbCategory);
            this.grbCategory.Location = new System.Drawing.Point(5, 48);
            this.grbCategory.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbCategory.Name = "grbCategory";
            this.grbCategory.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbCategory.Size = new System.Drawing.Size(263, 38);
            this.grbCategory.TabIndex = 2;
            this.grbCategory.TabStop = false;
            this.grbCategory.Text = "カテゴリ";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(4, 16);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(206, 20);
            this.cmbCategory.TabIndex = 0;
            this.cmbCategory.Leave += new System.EventHandler(this.cmbCategory_Leave);
            // 
            // envelope1
            // 
            this.envelope1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.envelope1.Art = null;
            this.envelope1.Location = new System.Drawing.Point(5, 90);
            this.envelope1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.envelope1.Name = "envelope1";
            this.envelope1.Size = new System.Drawing.Size(328, 334);
            this.envelope1.TabIndex = 7;
            // 
            // InstInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 458);
            this.Controls.Add(this.envelope1);
            this.Controls.Add(this.grbCategory);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InstInfoDialog";
            this.Text = "音色追加";
            this.Load += new System.EventHandler(this.InstInfoDialog_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.grbCategory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInstName;
        private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox grbCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private Articulations envelope1;
    }
}