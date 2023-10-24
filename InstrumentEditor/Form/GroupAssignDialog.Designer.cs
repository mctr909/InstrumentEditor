namespace InstrumentEditor {
    partial class GroupAssignDialog
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
			this.btnApply = new System.Windows.Forms.Button();
			this.cmbGroup = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnApply
			// 
			this.btnApply.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnApply.Location = new System.Drawing.Point(169, 31);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(59, 24);
			this.btnApply.TabIndex = 6;
			this.btnApply.Text = "反映";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// cmbGroup
			// 
			this.cmbGroup.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.cmbGroup.FormattingEnabled = true;
			this.cmbGroup.Location = new System.Drawing.Point(4, 4);
			this.cmbGroup.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.cmbGroup.Name = "cmbGroup";
			this.cmbGroup.Size = new System.Drawing.Size(224, 23);
			this.cmbGroup.TabIndex = 0;
			this.cmbGroup.Leave += new System.EventHandler(this.cmbGroup_Leave);
			// 
			// GroupAssignDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(232, 59);
			this.Controls.Add(this.cmbGroup);
			this.Controls.Add(this.btnApply);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupAssignDialog";
			this.Text = "グループ割り当て";
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox cmbGroup;
	}
}