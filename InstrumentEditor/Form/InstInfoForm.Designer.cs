namespace InstrumentEditor {
	partial class InstInfoForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtInstName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbInstKeyword = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtInstComment = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.txtInstName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(828, 80);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "名称";
            // 
            // txtInstName
            // 
            this.txtInstName.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.txtInstName.Location = new System.Drawing.Point(6, 30);
            this.txtInstName.Name = "txtInstName";
            this.txtInstName.Size = new System.Drawing.Size(809, 37);
            this.txtInstName.TabIndex = 1;
            this.txtInstName.Leave += new System.EventHandler(this.txtInstName_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.cmbInstKeyword);
            this.groupBox2.Location = new System.Drawing.Point(12, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(828, 80);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "カテゴリー";
            // 
            // cmbInstKeyword
            // 
            this.cmbInstKeyword.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbInstKeyword.FormattingEnabled = true;
            this.cmbInstKeyword.Location = new System.Drawing.Point(9, 30);
            this.cmbInstKeyword.Name = "cmbInstKeyword";
            this.cmbInstKeyword.Size = new System.Drawing.Size(806, 37);
            this.cmbInstKeyword.TabIndex = 10;
            this.cmbInstKeyword.SelectedIndexChanged += new System.EventHandler(this.cmbInstKeyword_SelectedIndexChanged);
            this.cmbInstKeyword.Leave += new System.EventHandler(this.cmbInstKeyword_Leave);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.txtInstComment);
            this.groupBox3.Location = new System.Drawing.Point(12, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(828, 353);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "コメント";
            // 
            // txtInstComment
            // 
            this.txtInstComment.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.txtInstComment.Location = new System.Drawing.Point(9, 30);
            this.txtInstComment.Multiline = true;
            this.txtInstComment.Name = "txtInstComment";
            this.txtInstComment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInstComment.Size = new System.Drawing.Size(806, 312);
            this.txtInstComment.TabIndex = 5;
            this.txtInstComment.Leave += new System.EventHandler(this.txtInstComment_Leave);
            // 
            // InstInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 550);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InstInfoForm";
            this.Text = "InstInfoForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtInstName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbInstKeyword;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtInstComment;
    }
}