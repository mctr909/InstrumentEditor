namespace InstrumentEditor {
    partial class InstForm
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
            this.txtInstName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInstName.Location = new System.Drawing.Point(10, 33);
            this.txtInstName.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtInstName.Name = "txtInstName";
            this.txtInstName.Size = new System.Drawing.Size(783, 37);
            this.txtInstName.TabIndex = 0;
            // 
            // lstPrgNo
            // 
            this.lstPrgNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstPrgNo.FormattingEnabled = true;
            this.lstPrgNo.ItemHeight = 24;
            this.lstPrgNo.Location = new System.Drawing.Point(10, 33);
            this.lstPrgNo.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lstPrgNo.Name = "lstPrgNo";
            this.lstPrgNo.Size = new System.Drawing.Size(455, 364);
            this.lstPrgNo.TabIndex = 0;
            this.lstPrgNo.SelectedIndexChanged += new System.EventHandler(this.lstPrgNo_SelectedIndexChanged);
            // 
            // lstBankMSB
            // 
            this.lstBankMSB.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstBankMSB.FormattingEnabled = true;
            this.lstBankMSB.ItemHeight = 24;
            this.lstBankMSB.Location = new System.Drawing.Point(10, 33);
            this.lstBankMSB.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lstBankMSB.Name = "lstBankMSB";
            this.lstBankMSB.Size = new System.Drawing.Size(144, 364);
            this.lstBankMSB.TabIndex = 0;
            this.lstBankMSB.SelectedIndexChanged += new System.EventHandler(this.lstBankMSB_SelectedIndexChanged);
            // 
            // lstBankLSB
            // 
            this.lstBankLSB.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstBankLSB.FormattingEnabled = true;
            this.lstBankLSB.ItemHeight = 24;
            this.lstBankLSB.Location = new System.Drawing.Point(10, 33);
            this.lstBankLSB.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lstBankLSB.Name = "lstBankLSB";
            this.lstBankLSB.Size = new System.Drawing.Size(144, 364);
            this.lstBankLSB.TabIndex = 0;
            this.lstBankLSB.SelectedIndexChanged += new System.EventHandler(this.lstBankLSB_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAdd.Location = new System.Drawing.Point(666, 630);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(166, 48);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // rbDrum
            // 
            this.rbDrum.AutoSize = true;
            this.rbDrum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rbDrum.Location = new System.Drawing.Point(173, 36);
            this.rbDrum.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.rbDrum.Name = "rbDrum";
            this.rbDrum.Size = new System.Drawing.Size(118, 34);
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
            this.rbNote.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rbNote.Location = new System.Drawing.Point(14, 36);
            this.rbNote.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.rbNote.Name = "rbNote";
            this.rbNote.Size = new System.Drawing.Size(104, 34);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox1.Size = new System.Drawing.Size(305, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "音色の種類";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.lstPrgNo);
            this.groupBox2.Location = new System.Drawing.Point(12, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(476, 410);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "プログラムナンバー";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox3.Controls.Add(this.lstBankMSB);
            this.groupBox3.Location = new System.Drawing.Point(494, 211);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(166, 410);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "バンクMSB";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox4.Controls.Add(this.lstBankLSB);
            this.groupBox4.Location = new System.Drawing.Point(666, 211);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(166, 410);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "バンクLSB";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox5.Controls.Add(this.txtInstName);
            this.groupBox5.Location = new System.Drawing.Point(12, 121);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(820, 81);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "音色名";
            // 
            // InstForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 693);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "InstForm";
            this.Text = "音色追加";
            this.Load += new System.EventHandler(this.InstAddForm_Load);
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