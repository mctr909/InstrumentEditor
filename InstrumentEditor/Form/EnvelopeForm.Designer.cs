namespace InstrumentEditor {
	partial class EnvelopeForm {
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
            this.ampEnvelope = new InstrumentEditor.Envelope();
            this.SuspendLayout();
            // 
            // ampEnvelope
            // 
            this.ampEnvelope.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.ampEnvelope.Art = null;
            this.ampEnvelope.Location = new System.Drawing.Point(12, 12);
            this.ampEnvelope.Name = "ampEnvelope";
            this.ampEnvelope.Size = new System.Drawing.Size(2153, 663);
            this.ampEnvelope.TabIndex = 1;
            // 
            // EnvelopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2177, 687);
            this.Controls.Add(this.ampEnvelope);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "EnvelopeForm";
            this.Text = "InstInfoForm";
            this.ResumeLayout(false);

		}

        #endregion
        private Envelope ampEnvelope;
    }
}