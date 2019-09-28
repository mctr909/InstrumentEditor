namespace InstrumentEditor {
	partial class WaveSelectForm {
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
            this.lstWave = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lstWave
            // 
            this.lstWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstWave.FormattingEnabled = true;
            this.lstWave.ItemHeight = 29;
            this.lstWave.Location = new System.Drawing.Point(12, 12);
            this.lstWave.Name = "lstWave";
            this.lstWave.Size = new System.Drawing.Size(120, 62);
            this.lstWave.TabIndex = 0;
            this.lstWave.DoubleClick += new System.EventHandler(this.lstWave_DoubleClick);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(644, 379);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(97, 35);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "選択";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(81, 382);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 31);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // WaveSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 1209);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lstWave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "WaveSelectForm";
            this.Text = "WaveSelectForm";
            this.Load += new System.EventHandler(this.WaveSelectForm_Load);
            this.SizeChanged += new System.EventHandler(this.WaveSelectForm_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstWave;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.TextBox txtSearch;
	}
}