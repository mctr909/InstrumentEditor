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
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tbpInstInfo = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtInstComment = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbInstKeyword = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtInstName = new System.Windows.Forms.TextBox();
            this.tbpInstAttribute = new System.Windows.Forms.TabPage();
            this.ampEnvelope = new InstrumentEditor.Envelope();
            this.tbpRegion = new System.Windows.Forms.TabPage();
            this.lstRegion = new System.Windows.Forms.ListBox();
            this.pnlRegion = new System.Windows.Forms.Panel();
            this.picRegion = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddRange = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteRange = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRangeList = new System.Windows.Forms.ToolStripButton();
            this.tsbRangeKey = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.txtRegion = new System.Windows.Forms.ToolStripTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl.SuspendLayout();
            this.tbpInstInfo.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tbpInstAttribute.SuspendLayout();
            this.tbpRegion.SuspendLayout();
            this.pnlRegion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRegion)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tbpInstInfo);
            this.tabControl.Controls.Add(this.tbpInstAttribute);
            this.tabControl.Controls.Add(this.tbpRegion);
            this.tabControl.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl.Location = new System.Drawing.Point(16, 15);
            this.tabControl.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(767, 734);
            this.tabControl.TabIndex = 5;
            // 
            // tbpInstInfo
            // 
            this.tbpInstInfo.Controls.Add(this.groupBox3);
            this.tbpInstInfo.Controls.Add(this.groupBox2);
            this.tbpInstInfo.Controls.Add(this.groupBox1);
            this.tbpInstInfo.Location = new System.Drawing.Point(8, 39);
            this.tbpInstInfo.Name = "tbpInstInfo";
            this.tbpInstInfo.Size = new System.Drawing.Size(751, 687);
            this.tbpInstInfo.TabIndex = 4;
            this.tbpInstInfo.Text = "音色情報";
            this.tbpInstInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.txtInstComment);
            this.groupBox3.Location = new System.Drawing.Point(3, 175);
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
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.cmbInstKeyword);
            this.groupBox2.Location = new System.Drawing.Point(3, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(500, 80);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "キーワード";
            // 
            // cmbInstKeyword
            // 
            this.cmbInstKeyword.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbInstKeyword.FormattingEnabled = true;
            this.cmbInstKeyword.Location = new System.Drawing.Point(9, 30);
            this.cmbInstKeyword.Name = "cmbInstKeyword";
            this.cmbInstKeyword.Size = new System.Drawing.Size(479, 37);
            this.cmbInstKeyword.TabIndex = 10;
            this.cmbInstKeyword.SelectedIndexChanged += new System.EventHandler(this.cmbInstKeyword_SelectedIndexChanged);
            this.cmbInstKeyword.Leave += new System.EventHandler(this.cmbInstKeyword_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.txtInstName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 80);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "名称";
            // 
            // txtInstName
            // 
            this.txtInstName.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.txtInstName.Location = new System.Drawing.Point(6, 30);
            this.txtInstName.Name = "txtInstName";
            this.txtInstName.Size = new System.Drawing.Size(482, 37);
            this.txtInstName.TabIndex = 1;
            this.txtInstName.Leave += new System.EventHandler(this.txtInstName_Leave);
            // 
            // tbpInstAttribute
            // 
            this.tbpInstAttribute.Controls.Add(this.ampEnvelope);
            this.tbpInstAttribute.Location = new System.Drawing.Point(8, 39);
            this.tbpInstAttribute.Name = "tbpInstAttribute";
            this.tbpInstAttribute.Padding = new System.Windows.Forms.Padding(3);
            this.tbpInstAttribute.Size = new System.Drawing.Size(751, 687);
            this.tbpInstAttribute.TabIndex = 6;
            this.tbpInstAttribute.Text = "音色属性";
            this.tbpInstAttribute.UseVisualStyleBackColor = true;
            // 
            // ampEnvelope
            // 
            this.ampEnvelope.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.ampEnvelope.Art = null;
            this.ampEnvelope.Location = new System.Drawing.Point(6, 6);
            this.ampEnvelope.Name = "ampEnvelope";
            this.ampEnvelope.Size = new System.Drawing.Size(1840, 720);
            this.ampEnvelope.TabIndex = 1;
            // 
            // tbpRegion
            // 
            this.tbpRegion.Controls.Add(this.lstRegion);
            this.tbpRegion.Controls.Add(this.pnlRegion);
            this.tbpRegion.Controls.Add(this.toolStrip1);
            this.tbpRegion.Location = new System.Drawing.Point(8, 39);
            this.tbpRegion.Name = "tbpRegion";
            this.tbpRegion.Size = new System.Drawing.Size(751, 687);
            this.tbpRegion.TabIndex = 5;
            this.tbpRegion.Text = "音程/強弱割り当て";
            this.tbpRegion.UseVisualStyleBackColor = true;
            // 
            // lstRegion
            // 
            this.lstRegion.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstRegion.FormattingEnabled = true;
            this.lstRegion.ItemHeight = 29;
            this.lstRegion.Location = new System.Drawing.Point(18, 44);
            this.lstRegion.Name = "lstRegion";
            this.lstRegion.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstRegion.Size = new System.Drawing.Size(120, 62);
            this.lstRegion.TabIndex = 8;
            this.lstRegion.DoubleClick += new System.EventHandler(this.lstRegion_DoubleClick);
            // 
            // pnlRegion
            // 
            this.pnlRegion.AutoScroll = true;
            this.pnlRegion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlRegion.Controls.Add(this.picRegion);
            this.pnlRegion.Location = new System.Drawing.Point(19, 150);
            this.pnlRegion.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pnlRegion.Name = "pnlRegion";
            this.pnlRegion.Size = new System.Drawing.Size(1764, 731);
            this.pnlRegion.TabIndex = 7;
            this.pnlRegion.Visible = false;
            // 
            // picRegion
            // 
            this.picRegion.BackgroundImage = global::InstrumentEditor.Properties.Resources.region;
            this.picRegion.InitialImage = null;
            this.picRegion.Location = new System.Drawing.Point(0, 0);
            this.picRegion.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picRegion.Name = "picRegion";
            this.picRegion.Size = new System.Drawing.Size(1764, 1024);
            this.picRegion.TabIndex = 0;
            this.picRegion.TabStop = false;
            this.picRegion.DoubleClick += new System.EventHandler(this.picRegion_DoubleClick);
            this.picRegion.MouseEnter += new System.EventHandler(this.picRegion_MouseEnter);
            this.picRegion.MouseLeave += new System.EventHandler(this.picRegion_MouseLeave);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddRange,
            this.tsbDeleteRange,
            this.toolStripSeparator3,
            this.tsbRangeList,
            this.tsbRangeKey,
            this.toolStripSeparator4,
            this.txtRegion});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(469, 31);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddRange
            // 
            this.tsbAddRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddRange.Image = global::InstrumentEditor.Properties.Resources.plus;
            this.tsbAddRange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAddRange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRange.Name = "tsbAddRange";
            this.tsbAddRange.Size = new System.Drawing.Size(23, 28);
            this.tsbAddRange.Text = "追加";
            this.tsbAddRange.Click += new System.EventHandler(this.tsbAddRange_Click);
            // 
            // tsbDeleteRange
            // 
            this.tsbDeleteRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteRange.Image = global::InstrumentEditor.Properties.Resources.minus;
            this.tsbDeleteRange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDeleteRange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteRange.Name = "tsbDeleteRange";
            this.tsbDeleteRange.Size = new System.Drawing.Size(23, 28);
            this.tsbDeleteRange.Text = "削除";
            this.tsbDeleteRange.Click += new System.EventHandler(this.tsbDeleteRange_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbRangeList
            // 
            this.tsbRangeList.Checked = true;
            this.tsbRangeList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbRangeList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRangeList.Image = global::InstrumentEditor.Properties.Resources.list;
            this.tsbRangeList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbRangeList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRangeList.Name = "tsbRangeList";
            this.tsbRangeList.Size = new System.Drawing.Size(23, 28);
            this.tsbRangeList.Text = "リスト表示";
            this.tsbRangeList.Click += new System.EventHandler(this.tsbRangeList_Click);
            // 
            // tsbRangeKey
            // 
            this.tsbRangeKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRangeKey.Image = global::InstrumentEditor.Properties.Resources.key;
            this.tsbRangeKey.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbRangeKey.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRangeKey.Name = "tsbRangeKey";
            this.tsbRangeKey.Size = new System.Drawing.Size(23, 28);
            this.tsbRangeKey.Text = "グラフィック表示";
            this.tsbRangeKey.Click += new System.EventHandler(this.tsbRangeKey_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // txtRegion
            // 
            this.txtRegion.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.ReadOnly = true;
            this.txtRegion.Size = new System.Drawing.Size(350, 31);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // InstInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1559, 1028);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "InstInfoForm";
            this.Text = "InstInfoForm";
            this.SizeChanged += new System.EventHandler(this.InstInfoForm_SizeChanged);
            this.tabControl.ResumeLayout(false);
            this.tbpInstInfo.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbpInstAttribute.ResumeLayout(false);
            this.tbpRegion.ResumeLayout(false);
            this.tbpRegion.PerformLayout();
            this.pnlRegion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRegion)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tbpInstInfo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtInstComment;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtInstName;
        private System.Windows.Forms.TabPage tbpInstAttribute;
        private Envelope ampEnvelope;
        private System.Windows.Forms.TabPage tbpRegion;
        private System.Windows.Forms.ListBox lstRegion;
        private System.Windows.Forms.Panel pnlRegion;
        private System.Windows.Forms.PictureBox picRegion;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddRange;
        private System.Windows.Forms.ToolStripButton tsbDeleteRange;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbRangeList;
        private System.Windows.Forms.ToolStripButton tsbRangeKey;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripTextBox txtRegion;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cmbInstKeyword;
    }
}