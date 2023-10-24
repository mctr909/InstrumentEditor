namespace InstrumentEditor {
    partial class InstInfoDialog
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
			this.txtInstName = new System.Windows.Forms.TextBox();
			this.lstPrgNo = new System.Windows.Forms.ListBox();
			this.lstBankMSB = new System.Windows.Forms.ListBox();
			this.lstBankLSB = new System.Windows.Forms.ListBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.rbTypeDrum = new System.Windows.Forms.RadioButton();
			this.rbTypeNote = new System.Windows.Forms.RadioButton();
			this.grbType = new System.Windows.Forms.GroupBox();
			this.grbProg = new System.Windows.Forms.GroupBox();
			this.grbMSB = new System.Windows.Forms.GroupBox();
			this.grbLSB = new System.Windows.Forms.GroupBox();
			this.grbPreset = new System.Windows.Forms.GroupBox();
			this.grbCategory = new System.Windows.Forms.GroupBox();
			this.cmbCategory = new System.Windows.Forms.ComboBox();
			this.artList = new InstrumentEditor.Articulations();
			this.grbType.SuspendLayout();
			this.grbProg.SuspendLayout();
			this.grbMSB.SuspendLayout();
			this.grbLSB.SuspendLayout();
			this.grbPreset.SuspendLayout();
			this.grbCategory.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtInstName
			// 
			this.txtInstName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.txtInstName.Location = new System.Drawing.Point(4, 12);
			this.txtInstName.Name = "txtInstName";
			this.txtInstName.Size = new System.Drawing.Size(224, 22);
			this.txtInstName.TabIndex = 0;
			// 
			// lstPrgNo
			// 
			this.lstPrgNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstPrgNo.FormattingEnabled = true;
			this.lstPrgNo.ItemHeight = 12;
			this.lstPrgNo.Items.AddRange(new object[] {
            "000 * 0----+----1----+----2----"});
			this.lstPrgNo.Location = new System.Drawing.Point(5, 16);
			this.lstPrgNo.Name = "lstPrgNo";
			this.lstPrgNo.Size = new System.Drawing.Size(214, 124);
			this.lstPrgNo.TabIndex = 0;
			this.lstPrgNo.SelectedIndexChanged += new System.EventHandler(this.lstPrgNo_SelectedIndexChanged);
			// 
			// lstBankMSB
			// 
			this.lstBankMSB.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstBankMSB.FormattingEnabled = true;
			this.lstBankMSB.ItemHeight = 12;
			this.lstBankMSB.Items.AddRange(new object[] {
            "999*"});
			this.lstBankMSB.Location = new System.Drawing.Point(5, 16);
			this.lstBankMSB.Name = "lstBankMSB";
			this.lstBankMSB.Size = new System.Drawing.Size(49, 124);
			this.lstBankMSB.TabIndex = 0;
			this.lstBankMSB.SelectedIndexChanged += new System.EventHandler(this.lstBankMSB_SelectedIndexChanged);
			// 
			// lstBankLSB
			// 
			this.lstBankLSB.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstBankLSB.FormattingEnabled = true;
			this.lstBankLSB.ItemHeight = 12;
			this.lstBankLSB.Items.AddRange(new object[] {
            "999*"});
			this.lstBankLSB.Location = new System.Drawing.Point(5, 17);
			this.lstBankLSB.Name = "lstBankLSB";
			this.lstBankLSB.Size = new System.Drawing.Size(49, 124);
			this.lstBankLSB.TabIndex = 0;
			// 
			// btnApply
			// 
			this.btnApply.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnApply.Location = new System.Drawing.Point(362, 490);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(59, 24);
			this.btnApply.TabIndex = 6;
			this.btnApply.Text = "反映";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// rbTypeDrum
			// 
			this.rbTypeDrum.AutoSize = true;
			this.rbTypeDrum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.rbTypeDrum.Location = new System.Drawing.Point(7, 46);
			this.rbTypeDrum.Name = "rbTypeDrum";
			this.rbTypeDrum.Size = new System.Drawing.Size(62, 19);
			this.rbTypeDrum.TabIndex = 1;
			this.rbTypeDrum.TabStop = true;
			this.rbTypeDrum.Text = "ドラム";
			this.rbTypeDrum.UseVisualStyleBackColor = true;
			this.rbTypeDrum.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
			// 
			// rbTypeNote
			// 
			this.rbTypeNote.AutoSize = true;
			this.rbTypeNote.Checked = true;
			this.rbTypeNote.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.rbTypeNote.Location = new System.Drawing.Point(7, 19);
			this.rbTypeNote.Name = "rbTypeNote";
			this.rbTypeNote.Size = new System.Drawing.Size(85, 19);
			this.rbTypeNote.TabIndex = 0;
			this.rbTypeNote.TabStop = true;
			this.rbTypeNote.Text = "音程楽器";
			this.rbTypeNote.UseVisualStyleBackColor = true;
			// 
			// grbType
			// 
			this.grbType.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbType.Controls.Add(this.rbTypeNote);
			this.grbType.Controls.Add(this.rbTypeDrum);
			this.grbType.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.grbType.Location = new System.Drawing.Point(5, 8);
			this.grbType.Name = "grbType";
			this.grbType.Size = new System.Drawing.Size(108, 79);
			this.grbType.TabIndex = 0;
			this.grbType.TabStop = false;
			this.grbType.Text = "音色の種類";
			// 
			// grbProg
			// 
			this.grbProg.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbProg.Controls.Add(this.lstPrgNo);
			this.grbProg.Location = new System.Drawing.Point(5, 92);
			this.grbProg.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbProg.Name = "grbProg";
			this.grbProg.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbProg.Size = new System.Drawing.Size(224, 145);
			this.grbProg.TabIndex = 3;
			this.grbProg.TabStop = false;
			this.grbProg.Text = "プログラムナンバー";
			// 
			// grbMSB
			// 
			this.grbMSB.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbMSB.Controls.Add(this.lstBankMSB);
			this.grbMSB.Location = new System.Drawing.Point(231, 92);
			this.grbMSB.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbMSB.Name = "grbMSB";
			this.grbMSB.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbMSB.Size = new System.Drawing.Size(59, 145);
			this.grbMSB.TabIndex = 4;
			this.grbMSB.TabStop = false;
			this.grbMSB.Text = "MSB";
			// 
			// grbLSB
			// 
			this.grbLSB.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbLSB.Controls.Add(this.lstBankLSB);
			this.grbLSB.Location = new System.Drawing.Point(292, 92);
			this.grbLSB.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbLSB.Name = "grbLSB";
			this.grbLSB.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbLSB.Size = new System.Drawing.Size(59, 145);
			this.grbLSB.TabIndex = 5;
			this.grbLSB.TabStop = false;
			this.grbLSB.Text = "LSB";
			// 
			// grbPreset
			// 
			this.grbPreset.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbPreset.Controls.Add(this.txtInstName);
			this.grbPreset.Location = new System.Drawing.Point(118, 8);
			this.grbPreset.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbPreset.Name = "grbPreset";
			this.grbPreset.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbPreset.Size = new System.Drawing.Size(233, 38);
			this.grbPreset.TabIndex = 1;
			this.grbPreset.TabStop = false;
			this.grbPreset.Text = "音色名";
			// 
			// grbCategory
			// 
			this.grbCategory.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbCategory.Controls.Add(this.cmbCategory);
			this.grbCategory.Location = new System.Drawing.Point(118, 49);
			this.grbCategory.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbCategory.Name = "grbCategory";
			this.grbCategory.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbCategory.Size = new System.Drawing.Size(233, 38);
			this.grbCategory.TabIndex = 2;
			this.grbCategory.TabStop = false;
			this.grbCategory.Text = "カテゴリ";
			// 
			// cmbCategory
			// 
			this.cmbCategory.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.cmbCategory.FormattingEnabled = true;
			this.cmbCategory.Location = new System.Drawing.Point(4, 12);
			this.cmbCategory.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.cmbCategory.Name = "cmbCategory";
			this.cmbCategory.Size = new System.Drawing.Size(224, 23);
			this.cmbCategory.TabIndex = 0;
			this.cmbCategory.Leave += new System.EventHandler(this.cmbCategory_Leave);
			// 
			// artList
			// 
			this.artList.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.artList.Art = null;
			this.artList.Location = new System.Drawing.Point(5, 241);
			this.artList.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.artList.Name = "artList";
			this.artList.Size = new System.Drawing.Size(416, 244);
			this.artList.TabIndex = 7;
			// 
			// InstInfoDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(428, 517);
			this.Controls.Add(this.artList);
			this.Controls.Add(this.grbCategory);
			this.Controls.Add(this.grbPreset);
			this.Controls.Add(this.grbLSB);
			this.Controls.Add(this.grbMSB);
			this.Controls.Add(this.grbProg);
			this.Controls.Add(this.grbType);
			this.Controls.Add(this.btnApply);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InstInfoDialog";
			this.Text = "音色情報";
			this.grbType.ResumeLayout(false);
			this.grbType.PerformLayout();
			this.grbProg.ResumeLayout(false);
			this.grbMSB.ResumeLayout(false);
			this.grbLSB.ResumeLayout(false);
			this.grbPreset.ResumeLayout(false);
			this.grbPreset.PerformLayout();
			this.grbCategory.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInstName;
        private System.Windows.Forms.ListBox lstPrgNo;
        private System.Windows.Forms.ListBox lstBankMSB;
        private System.Windows.Forms.ListBox lstBankLSB;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.RadioButton rbTypeDrum;
        private System.Windows.Forms.RadioButton rbTypeNote;
        private System.Windows.Forms.GroupBox grbType;
		private System.Windows.Forms.GroupBox grbProg;
		private System.Windows.Forms.GroupBox grbMSB;
		private System.Windows.Forms.GroupBox grbLSB;
		private System.Windows.Forms.GroupBox grbPreset;
        private System.Windows.Forms.GroupBox grbCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
		private Articulations artList;
	}
}