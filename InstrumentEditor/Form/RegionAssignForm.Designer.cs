namespace InstrumentEditor {
	partial class RegionAssignForm {
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
			this.pnlRegion = new System.Windows.Forms.Panel();
			this.picRegion = new System.Windows.Forms.PictureBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbAddRange = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteRange = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.txtRegion = new System.Windows.Forms.ToolStripTextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.pnlRegion.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picRegion)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlRegion
			// 
			this.pnlRegion.AutoScroll = true;
			this.pnlRegion.Controls.Add(this.picRegion);
			this.pnlRegion.Location = new System.Drawing.Point(4, 57);
			this.pnlRegion.Name = "pnlRegion";
			this.pnlRegion.Size = new System.Drawing.Size(816, 368);
			this.pnlRegion.TabIndex = 7;
			this.pnlRegion.Visible = false;
			// 
			// picRegion
			// 
			this.picRegion.InitialImage = null;
			this.picRegion.Location = new System.Drawing.Point(0, 0);
			this.picRegion.Name = "picRegion";
			this.picRegion.Size = new System.Drawing.Size(814, 512);
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
            this.txtRegion});
			this.toolStrip1.Location = new System.Drawing.Point(4, 4);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
			this.toolStrip1.Size = new System.Drawing.Size(246, 25);
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
			this.tsbAddRange.Size = new System.Drawing.Size(23, 22);
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
			this.tsbDeleteRange.Size = new System.Drawing.Size(23, 22);
			this.tsbDeleteRange.Text = "削除";
			this.tsbDeleteRange.Click += new System.EventHandler(this.tsbDeleteRange_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// txtRegion
			// 
			this.txtRegion.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.txtRegion.Name = "txtRegion";
			this.txtRegion.ReadOnly = true;
			this.txtRegion.Size = new System.Drawing.Size(150, 25);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// RegionAssignForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(830, 434);
			this.Controls.Add(this.pnlRegion);
			this.Controls.Add(this.toolStrip1);
			this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.Name = "RegionAssignForm";
			this.Text = "InstInfoForm";
			this.SizeChanged += new System.EventHandler(this.InstInfoForm_SizeChanged);
			this.pnlRegion.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picRegion)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion
        private System.Windows.Forms.Panel pnlRegion;
        private System.Windows.Forms.PictureBox picRegion;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddRange;
        private System.Windows.Forms.ToolStripButton tsbDeleteRange;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripTextBox txtRegion;
        private System.Windows.Forms.Timer timer1;
    }
}