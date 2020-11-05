﻿namespace InstrumentEditor {
	partial class LayerInfoDialog {
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
            this.numKeyLow = new System.Windows.Forms.NumericUpDown();
            this.numKeyHigh = new System.Windows.Forms.NumericUpDown();
            this.numVelocityHigh = new System.Windows.Forms.NumericUpDown();
            this.numVelocityLow = new System.Windows.Forms.NumericUpDown();
            this.grbKey = new System.Windows.Forms.GroupBox();
            this.lblKeyLow = new System.Windows.Forms.Label();
            this.lblKeyHigh = new System.Windows.Forms.Label();
            this.grbVelocity = new System.Windows.Forms.GroupBox();
            this.txtInst = new System.Windows.Forms.TextBox();
            this.grbInst = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.grbFineTune = new System.Windows.Forms.GroupBox();
            this.numFineTune = new System.Windows.Forms.NumericUpDown();
            this.grbUnityNote = new System.Windows.Forms.GroupBox();
            this.numUnityNote = new System.Windows.Forms.NumericUpDown();
            this.lblUnityNote = new System.Windows.Forms.Label();
            this.grbVolume = new System.Windows.Forms.GroupBox();
            this.numVolume = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).BeginInit();
            this.grbKey.SuspendLayout();
            this.grbVelocity.SuspendLayout();
            this.grbInst.SuspendLayout();
            this.grbFineTune.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFineTune)).BeginInit();
            this.grbUnityNote.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).BeginInit();
            this.grbVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // numKeyLow
            // 
            this.numKeyLow.Location = new System.Drawing.Point(3, 15);
            this.numKeyLow.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numKeyLow.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numKeyLow.Name = "numKeyLow";
            this.numKeyLow.Size = new System.Drawing.Size(55, 19);
            this.numKeyLow.TabIndex = 0;
            this.numKeyLow.ValueChanged += new System.EventHandler(this.numKeyLow_ValueChanged);
            // 
            // numKeyHigh
            // 
            this.numKeyHigh.Location = new System.Drawing.Point(61, 15);
            this.numKeyHigh.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numKeyHigh.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numKeyHigh.Name = "numKeyHigh";
            this.numKeyHigh.Size = new System.Drawing.Size(55, 19);
            this.numKeyHigh.TabIndex = 1;
            this.numKeyHigh.ValueChanged += new System.EventHandler(this.numKeyHigh_ValueChanged);
            // 
            // numVelocityHigh
            // 
            this.numVelocityHigh.Location = new System.Drawing.Point(61, 15);
            this.numVelocityHigh.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numVelocityHigh.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numVelocityHigh.Name = "numVelocityHigh";
            this.numVelocityHigh.Size = new System.Drawing.Size(55, 19);
            this.numVelocityHigh.TabIndex = 1;
            this.numVelocityHigh.ValueChanged += new System.EventHandler(this.numVelocityHigh_ValueChanged);
            // 
            // numVelocityLow
            // 
            this.numVelocityLow.Location = new System.Drawing.Point(3, 15);
            this.numVelocityLow.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numVelocityLow.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numVelocityLow.Name = "numVelocityLow";
            this.numVelocityLow.Size = new System.Drawing.Size(55, 19);
            this.numVelocityLow.TabIndex = 0;
            this.numVelocityLow.ValueChanged += new System.EventHandler(this.numVelocityLow_ValueChanged);
            // 
            // grbKey
            // 
            this.grbKey.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbKey.Controls.Add(this.lblKeyLow);
            this.grbKey.Controls.Add(this.lblKeyHigh);
            this.grbKey.Controls.Add(this.numKeyLow);
            this.grbKey.Controls.Add(this.numKeyHigh);
            this.grbKey.Location = new System.Drawing.Point(6, 6);
            this.grbKey.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbKey.Name = "grbKey";
            this.grbKey.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbKey.Size = new System.Drawing.Size(129, 48);
            this.grbKey.TabIndex = 0;
            this.grbKey.TabStop = false;
            this.grbKey.Text = "音程";
            // 
            // lblKeyLow
            // 
            this.lblKeyLow.AutoSize = true;
            this.lblKeyLow.Location = new System.Drawing.Point(3, 32);
            this.lblKeyLow.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblKeyLow.Name = "lblKeyLow";
            this.lblKeyLow.Size = new System.Drawing.Size(41, 12);
            this.lblKeyLow.TabIndex = 6;
            this.lblKeyLow.Text = "Bb-2 5";
            // 
            // lblKeyHigh
            // 
            this.lblKeyHigh.AutoSize = true;
            this.lblKeyHigh.Location = new System.Drawing.Point(59, 32);
            this.lblKeyHigh.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblKeyHigh.Name = "lblKeyHigh";
            this.lblKeyHigh.Size = new System.Drawing.Size(41, 12);
            this.lblKeyHigh.TabIndex = 7;
            this.lblKeyHigh.Text = "Bb-2 5";
            // 
            // grbVelocity
            // 
            this.grbVelocity.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbVelocity.Controls.Add(this.numVelocityLow);
            this.grbVelocity.Controls.Add(this.numVelocityHigh);
            this.grbVelocity.Location = new System.Drawing.Point(138, 6);
            this.grbVelocity.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbVelocity.Name = "grbVelocity";
            this.grbVelocity.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbVelocity.Size = new System.Drawing.Size(129, 48);
            this.grbVelocity.TabIndex = 1;
            this.grbVelocity.TabStop = false;
            this.grbVelocity.Text = "強弱";
            // 
            // txtInst
            // 
            this.txtInst.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInst.Location = new System.Drawing.Point(3, 15);
            this.txtInst.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.txtInst.Name = "txtInst";
            this.txtInst.ReadOnly = true;
            this.txtInst.Size = new System.Drawing.Size(190, 19);
            this.txtInst.TabIndex = 0;
            // 
            // grbInst
            // 
            this.grbInst.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbInst.Controls.Add(this.btnSelect);
            this.grbInst.Controls.Add(this.txtInst);
            this.grbInst.Location = new System.Drawing.Point(6, 58);
            this.grbInst.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbInst.Name = "grbInst";
            this.grbInst.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbInst.Size = new System.Drawing.Size(261, 38);
            this.grbInst.TabIndex = 2;
            this.grbInst.TabStop = false;
            this.grbInst.Text = "音色";
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSelect.Location = new System.Drawing.Point(193, 13);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(42, 20);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "選択";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAdd.Location = new System.Drawing.Point(212, 140);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 26);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grbFineTune
            // 
            this.grbFineTune.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbFineTune.Controls.Add(this.numFineTune);
            this.grbFineTune.Location = new System.Drawing.Point(112, 99);
            this.grbFineTune.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbFineTune.Name = "grbFineTune";
            this.grbFineTune.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbFineTune.Size = new System.Drawing.Size(75, 37);
            this.grbFineTune.TabIndex = 4;
            this.grbFineTune.TabStop = false;
            this.grbFineTune.Text = "ピッチ(cent)";
            // 
            // numFineTune
            // 
            this.numFineTune.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numFineTune.Location = new System.Drawing.Point(3, 12);
            this.numFineTune.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numFineTune.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.numFineTune.Minimum = new decimal(new int[] {
            1200,
            0,
            0,
            -2147483648});
            this.numFineTune.Name = "numFineTune";
            this.numFineTune.Size = new System.Drawing.Size(55, 22);
            this.numFineTune.TabIndex = 0;
            // 
            // grbUnityNote
            // 
            this.grbUnityNote.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbUnityNote.Controls.Add(this.numUnityNote);
            this.grbUnityNote.Controls.Add(this.lblUnityNote);
            this.grbUnityNote.Location = new System.Drawing.Point(6, 99);
            this.grbUnityNote.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbUnityNote.Name = "grbUnityNote";
            this.grbUnityNote.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbUnityNote.Size = new System.Drawing.Size(103, 37);
            this.grbUnityNote.TabIndex = 3;
            this.grbUnityNote.TabStop = false;
            this.grbUnityNote.Text = "基準音";
            // 
            // numUnityNote
            // 
            this.numUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUnityNote.Location = new System.Drawing.Point(3, 12);
            this.numUnityNote.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numUnityNote.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numUnityNote.Name = "numUnityNote";
            this.numUnityNote.Size = new System.Drawing.Size(55, 22);
            this.numUnityNote.TabIndex = 0;
            this.numUnityNote.ValueChanged += new System.EventHandler(this.numUnityNote_ValueChanged);
            // 
            // lblUnityNote
            // 
            this.lblUnityNote.AutoSize = true;
            this.lblUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnityNote.Location = new System.Drawing.Point(61, 14);
            this.lblUnityNote.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblUnityNote.Name = "lblUnityNote";
            this.lblUnityNote.Size = new System.Drawing.Size(43, 15);
            this.lblUnityNote.TabIndex = 11;
            this.lblUnityNote.Text = "label1";
            // 
            // grbVolume
            // 
            this.grbVolume.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbVolume.Controls.Add(this.numVolume);
            this.grbVolume.Location = new System.Drawing.Point(192, 99);
            this.grbVolume.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbVolume.Name = "grbVolume";
            this.grbVolume.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.grbVolume.Size = new System.Drawing.Size(75, 37);
            this.grbVolume.TabIndex = 5;
            this.grbVolume.TabStop = false;
            this.grbVolume.Text = "音量(%)";
            // 
            // numVolume
            // 
            this.numVolume.DecimalPlaces = 1;
            this.numVolume.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numVolume.Location = new System.Drawing.Point(3, 12);
            this.numVolume.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.numVolume.Name = "numVolume";
            this.numVolume.Size = new System.Drawing.Size(55, 22);
            this.numVolume.TabIndex = 0;
            this.numVolume.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // LayerInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 177);
            this.Controls.Add(this.grbVolume);
            this.Controls.Add(this.grbFineTune);
            this.Controls.Add(this.grbUnityNote);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grbInst);
            this.Controls.Add(this.grbVelocity);
            this.Controls.Add(this.grbKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "LayerInfoDialog";
            this.Text = "レイヤー情報編集";
            ((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).EndInit();
            this.grbKey.ResumeLayout(false);
            this.grbKey.PerformLayout();
            this.grbVelocity.ResumeLayout(false);
            this.grbInst.ResumeLayout(false);
            this.grbInst.PerformLayout();
            this.grbFineTune.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numFineTune)).EndInit();
            this.grbUnityNote.ResumeLayout(false);
            this.grbUnityNote.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).EndInit();
            this.grbVolume.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown numKeyLow;
		private System.Windows.Forms.NumericUpDown numKeyHigh;
		private System.Windows.Forms.NumericUpDown numVelocityHigh;
		private System.Windows.Forms.NumericUpDown numVelocityLow;
		private System.Windows.Forms.GroupBox grbKey;
		private System.Windows.Forms.GroupBox grbVelocity;
		private System.Windows.Forms.Label lblKeyLow;
		private System.Windows.Forms.Label lblKeyHigh;
		private System.Windows.Forms.TextBox txtInst;
		private System.Windows.Forms.GroupBox grbInst;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox grbFineTune;
		private System.Windows.Forms.NumericUpDown numFineTune;
		private System.Windows.Forms.GroupBox grbUnityNote;
		private System.Windows.Forms.NumericUpDown numUnityNote;
		private System.Windows.Forms.Label lblUnityNote;
		private System.Windows.Forms.GroupBox grbVolume;
		private System.Windows.Forms.NumericUpDown numVolume;
    }
}