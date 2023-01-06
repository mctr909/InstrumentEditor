namespace InstrumentEditor {
	partial class LayerAssignForm {
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
            this.pnlLayer = new System.Windows.Forms.Panel();
            this.picLayer = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddRange = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteRange = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tscLayer = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtLayer = new System.Windows.Forms.ToolStripTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlLayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLayer)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayer
            // 
            this.pnlLayer.AutoScroll = true;
            this.pnlLayer.Controls.Add(this.picLayer);
            this.pnlLayer.Location = new System.Drawing.Point(4, 57);
            this.pnlLayer.Name = "pnlLayer";
            this.pnlLayer.Size = new System.Drawing.Size(816, 368);
            this.pnlLayer.TabIndex = 7;
            this.pnlLayer.Visible = false;
            // 
            // picLayer
            // 
            this.picLayer.InitialImage = null;
            this.picLayer.Location = new System.Drawing.Point(0, 0);
            this.picLayer.Name = "picLayer";
            this.picLayer.Size = new System.Drawing.Size(814, 512);
            this.picLayer.TabIndex = 0;
            this.picLayer.TabStop = false;
            this.picLayer.DoubleClick += new System.EventHandler(this.picLayer_DoubleClick);
            this.picLayer.MouseEnter += new System.EventHandler(this.picLayer_MouseEnter);
            this.picLayer.MouseLeave += new System.EventHandler(this.picLayer_MouseLeave);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddRange,
            this.tsbDeleteRange,
            this.toolStripSeparator3,
            this.tscLayer,
            this.toolStripSeparator1,
            this.txtLayer});
            this.toolStrip1.Location = new System.Drawing.Point(4, 4);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(221, 25);
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
            this.tsbAddRange.Click += new System.EventHandler(this.tsbAddLayer_Click);
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
            this.tsbDeleteRange.Click += new System.EventHandler(this.tsbDeleteLayer_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tscLayer
            // 
            this.tscLayer.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tscLayer.Name = "tscLayer";
            this.tscLayer.Size = new System.Drawing.Size(75, 25);
            this.tscLayer.SelectedIndexChanged += new System.EventHandler(this.tscLayer_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtLayer
            // 
            this.txtLayer.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtLayer.Name = "txtLayer";
            this.txtLayer.ReadOnly = true;
            this.txtLayer.Size = new System.Drawing.Size(71, 25);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LayerAssignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 434);
            this.Controls.Add(this.pnlLayer);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "LayerAssignForm";
            this.Text = "InstInfoForm";
            this.SizeChanged += new System.EventHandler(this.InstInfoForm_SizeChanged);
            this.pnlLayer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLayer)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
        private System.Windows.Forms.Panel pnlLayer;
        private System.Windows.Forms.PictureBox picLayer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddRange;
        private System.Windows.Forms.ToolStripButton tsbDeleteRange;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripTextBox txtLayer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripComboBox tscLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}