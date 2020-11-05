namespace InstrumentEditor {
    partial class AddPresetDialog
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.rbDrum = new System.Windows.Forms.RadioButton();
            this.rbNote = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInstName
            // 
            this.txtInstName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInstName.Location = new System.Drawing.Point(4, 13);
            this.txtInstName.Name = "txtInstName";
            this.txtInstName.Size = new System.Drawing.Size(261, 19);
            this.txtInstName.TabIndex = 0;
            // 
            // lstPrgNo
            // 
            this.lstPrgNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstPrgNo.FormattingEnabled = true;
            this.lstPrgNo.ItemHeight = 12;
            this.lstPrgNo.Items.AddRange(new object[] {
            "000 * 0----+----1----+----2----+----3--"});
            this.lstPrgNo.Location = new System.Drawing.Point(5, 16);
            this.lstPrgNo.Name = "lstPrgNo";
            this.lstPrgNo.Size = new System.Drawing.Size(259, 184);
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
            this.lstBankMSB.Size = new System.Drawing.Size(49, 184);
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
            this.lstBankLSB.Size = new System.Drawing.Size(49, 184);
            this.lstBankLSB.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAdd.Location = new System.Drawing.Point(324, 267);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(77, 24);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // rbDrum
            // 
            this.rbDrum.AutoSize = true;
            this.rbDrum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rbDrum.Location = new System.Drawing.Point(59, 16);
            this.rbDrum.Name = "rbDrum";
            this.rbDrum.Size = new System.Drawing.Size(53, 16);
            this.rbDrum.TabIndex = 1;
            this.rbDrum.TabStop = true;
            this.rbDrum.Text = "ドラム";
            this.rbDrum.UseVisualStyleBackColor = true;
            this.rbDrum.CheckedChanged += new System.EventHandler(this.rbDrum_CheckedChanged);
            // 
            // rbNote
            // 
            this.rbNote.AutoSize = true;
            this.rbNote.Checked = true;
            this.rbNote.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rbNote.Location = new System.Drawing.Point(6, 16);
            this.rbNote.Name = "rbNote";
            this.rbNote.Size = new System.Drawing.Size(47, 16);
            this.rbNote.TabIndex = 0;
            this.rbNote.TabStop = true;
            this.rbNote.Text = "音階";
            this.rbNote.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.rbNote);
            this.groupBox1.Controls.Add(this.rbDrum);
            this.groupBox1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "プリセットの種類";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.lstPrgNo);
            this.groupBox2.Location = new System.Drawing.Point(10, 57);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox2.Size = new System.Drawing.Size(269, 205);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "プログラムナンバー";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox3.Controls.Add(this.lstBankMSB);
            this.groupBox3.Location = new System.Drawing.Point(281, 57);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox3.Size = new System.Drawing.Size(59, 205);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "MSB";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox4.Controls.Add(this.lstBankLSB);
            this.groupBox4.Location = new System.Drawing.Point(342, 57);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox4.Size = new System.Drawing.Size(59, 205);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "LSB";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox5.Controls.Add(this.txtInstName);
            this.groupBox5.Location = new System.Drawing.Point(131, 12);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox5.Size = new System.Drawing.Size(270, 40);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "プリセット名";
            // 
            // AddPresetDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 295);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddPresetDialog";
            this.Text = "プリセット追加";
            this.Load += new System.EventHandler(this.AddPresetDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInstName;
        private System.Windows.Forms.ListBox lstPrgNo;
        private System.Windows.Forms.ListBox lstBankMSB;
        private System.Windows.Forms.ListBox lstBankLSB;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.RadioButton rbDrum;
        private System.Windows.Forms.RadioButton rbNote;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
	}
}