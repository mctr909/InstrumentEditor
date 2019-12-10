namespace InstrumentEditor
{
	partial class WaveInfoForm
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
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.btnPlay = new System.Windows.Forms.Button();
            this.picSpectrum = new System.Windows.Forms.PictureBox();
            this.hsbTime = new System.Windows.Forms.HScrollBar();
            this.picWave = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.numWaveScale = new System.Windows.Forms.NumericUpDown();
            this.picLoop = new System.Windows.Forms.PictureBox();
            this.grbMain = new System.Windows.Forms.GroupBox();
            this.numWaveAmp = new System.Windows.Forms.NumericUpDown();
            this.btnLoopCreate = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.grbLoop = new System.Windows.Forms.GroupBox();
            this.numLoopAmp = new System.Windows.Forms.NumericUpDown();
            this.numLoopScale = new System.Windows.Forms.NumericUpDown();
            this.numUnityNote = new System.Windows.Forms.NumericUpDown();
            this.numFineTune = new System.Windows.Forms.NumericUpDown();
            this.lblUnityNote = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblPitch = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblPitchCent = new System.Windows.Forms.Label();
            this.btnUpdateAutoTune = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numVolume = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaveScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).BeginInit();
            this.grbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWaveAmp)).BeginInit();
            this.grbLoop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopAmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFineTune)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.btnPlay.Location = new System.Drawing.Point(12, 12);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(132, 74);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "再生";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // picSpectrum
            // 
            this.picSpectrum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSpectrum.Location = new System.Drawing.Point(10, 51);
            this.picSpectrum.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picSpectrum.Name = "picSpectrum";
            this.picSpectrum.Size = new System.Drawing.Size(1652, 176);
            this.picSpectrum.TabIndex = 2;
            this.picSpectrum.TabStop = false;
            // 
            // hsbTime
            // 
            this.hsbTime.Location = new System.Drawing.Point(10, 565);
            this.hsbTime.Name = "hsbTime";
            this.hsbTime.Size = new System.Drawing.Size(1652, 28);
            this.hsbTime.TabIndex = 3;
            // 
            // picWave
            // 
            this.picWave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picWave.Location = new System.Drawing.Point(10, 239);
            this.picWave.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picWave.Name = "picWave";
            this.picWave.Size = new System.Drawing.Size(1652, 320);
            this.picWave.TabIndex = 4;
            this.picWave.TabStop = false;
            this.picWave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWave_MouseDown);
            this.picWave.MouseEnter += new System.EventHandler(this.picWave_MouseEnter);
            this.picWave.MouseLeave += new System.EventHandler(this.picWave_MouseLeave);
            this.picWave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWave_MouseMove);
            this.picWave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWave_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // numWaveScale
            // 
            this.numWaveScale.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numWaveScale.Location = new System.Drawing.Point(211, 0);
            this.numWaveScale.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.numWaveScale.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.numWaveScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWaveScale.Name = "numWaveScale";
            this.numWaveScale.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numWaveScale.Size = new System.Drawing.Size(125, 31);
            this.numWaveScale.TabIndex = 0;
            this.numWaveScale.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numWaveScale.ValueChanged += new System.EventHandler(this.numWaveScale_ValueChanged);
            // 
            // picLoop
            // 
            this.picLoop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLoop.Location = new System.Drawing.Point(10, 43);
            this.picLoop.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.picLoop.Name = "picLoop";
            this.picLoop.Size = new System.Drawing.Size(1652, 250);
            this.picLoop.TabIndex = 6;
            this.picLoop.TabStop = false;
            // 
            // grbMain
            // 
            this.grbMain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbMain.Controls.Add(this.numWaveAmp);
            this.grbMain.Controls.Add(this.btnLoopCreate);
            this.grbMain.Controls.Add(this.picSpectrum);
            this.grbMain.Controls.Add(this.picWave);
            this.grbMain.Controls.Add(this.numWaveScale);
            this.grbMain.Controls.Add(this.hsbTime);
            this.grbMain.Controls.Add(this.btnUpdate);
            this.grbMain.Location = new System.Drawing.Point(12, 96);
            this.grbMain.Name = "grbMain";
            this.grbMain.Size = new System.Drawing.Size(1676, 604);
            this.grbMain.TabIndex = 5;
            this.grbMain.TabStop = false;
            this.grbMain.Text = "ループ範囲選択";
            // 
            // numWaveAmp
            // 
            this.numWaveAmp.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numWaveAmp.Location = new System.Drawing.Point(350, 0);
            this.numWaveAmp.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.numWaveAmp.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numWaveAmp.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWaveAmp.Name = "numWaveAmp";
            this.numWaveAmp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numWaveAmp.Size = new System.Drawing.Size(125, 31);
            this.numWaveAmp.TabIndex = 8;
            this.numWaveAmp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWaveAmp.ValueChanged += new System.EventHandler(this.numWaveAmp_ValueChanged);
            // 
            // btnLoopCreate
            // 
            this.btnLoopCreate.BackColor = System.Drawing.SystemColors.Control;
            this.btnLoopCreate.Location = new System.Drawing.Point(759, 0);
            this.btnLoopCreate.Name = "btnLoopCreate";
            this.btnLoopCreate.Size = new System.Drawing.Size(161, 42);
            this.btnLoopCreate.TabIndex = 2;
            this.btnLoopCreate.Text = "ループ作成";
            this.btnLoopCreate.UseVisualStyleBackColor = true;
            this.btnLoopCreate.Click += new System.EventHandler(this.btnLoopCreate_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(554, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(199, 42);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "ループ範囲反映";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // grbLoop
            // 
            this.grbLoop.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbLoop.Controls.Add(this.numLoopAmp);
            this.grbLoop.Controls.Add(this.numLoopScale);
            this.grbLoop.Controls.Add(this.picLoop);
            this.grbLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grbLoop.Location = new System.Drawing.Point(12, 706);
            this.grbLoop.Name = "grbLoop";
            this.grbLoop.Size = new System.Drawing.Size(1676, 302);
            this.grbLoop.TabIndex = 6;
            this.grbLoop.TabStop = false;
            this.grbLoop.Text = "ループ範囲表示";
            // 
            // numLoopAmp
            // 
            this.numLoopAmp.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numLoopAmp.Location = new System.Drawing.Point(350, 0);
            this.numLoopAmp.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.numLoopAmp.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numLoopAmp.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLoopAmp.Name = "numLoopAmp";
            this.numLoopAmp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numLoopAmp.Size = new System.Drawing.Size(125, 31);
            this.numLoopAmp.TabIndex = 7;
            this.numLoopAmp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLoopAmp.ValueChanged += new System.EventHandler(this.numLoopAmp_ValueChanged);
            // 
            // numLoopScale
            // 
            this.numLoopScale.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numLoopScale.Location = new System.Drawing.Point(211, 0);
            this.numLoopScale.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.numLoopScale.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.numLoopScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLoopScale.Name = "numLoopScale";
            this.numLoopScale.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numLoopScale.Size = new System.Drawing.Size(125, 31);
            this.numLoopScale.TabIndex = 0;
            this.numLoopScale.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numLoopScale.ValueChanged += new System.EventHandler(this.numLoopScale_ValueChanged);
            // 
            // numUnityNote
            // 
            this.numUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUnityNote.Location = new System.Drawing.Point(6, 25);
            this.numUnityNote.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numUnityNote.Name = "numUnityNote";
            this.numUnityNote.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numUnityNote.Size = new System.Drawing.Size(120, 36);
            this.numUnityNote.TabIndex = 0;
            this.numUnityNote.ValueChanged += new System.EventHandler(this.numUnityNote_ValueChanged);
            // 
            // numFineTune
            // 
            this.numFineTune.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numFineTune.Location = new System.Drawing.Point(6, 25);
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
            this.numFineTune.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numFineTune.Size = new System.Drawing.Size(120, 36);
            this.numFineTune.TabIndex = 0;
            this.numFineTune.ValueChanged += new System.EventHandler(this.numFineTune_ValueChanged);
            // 
            // lblUnityNote
            // 
            this.lblUnityNote.AutoSize = true;
            this.lblUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnityNote.Location = new System.Drawing.Point(132, 27);
            this.lblUnityNote.Name = "lblUnityNote";
            this.lblUnityNote.Size = new System.Drawing.Size(83, 29);
            this.lblUnityNote.TabIndex = 11;
            this.lblUnityNote.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.numUnityNote);
            this.groupBox1.Controls.Add(this.lblUnityNote);
            this.groupBox1.Location = new System.Drawing.Point(743, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 74);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基準音";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.numFineTune);
            this.groupBox2.Location = new System.Drawing.Point(973, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 74);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ピッチ(cent)";
            // 
            // lblPitch
            // 
            this.lblPitch.AutoSize = true;
            this.lblPitch.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPitch.Location = new System.Drawing.Point(6, 28);
            this.lblPitch.Name = "lblPitch";
            this.lblPitch.Size = new System.Drawing.Size(77, 29);
            this.lblPitch.TabIndex = 16;
            this.lblPitch.Text = "Gb-2";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox3.Controls.Add(this.lblPitchCent);
            this.groupBox3.Controls.Add(this.lblPitch);
            this.groupBox3.Location = new System.Drawing.Point(1141, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(234, 74);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "音程検出";
            // 
            // lblPitchCent
            // 
            this.lblPitchCent.BackColor = System.Drawing.Color.Transparent;
            this.lblPitchCent.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPitchCent.Location = new System.Drawing.Point(89, 28);
            this.lblPitchCent.Name = "lblPitchCent";
            this.lblPitchCent.Size = new System.Drawing.Size(130, 33);
            this.lblPitchCent.TabIndex = 17;
            this.lblPitchCent.Text = "-50cent";
            this.lblPitchCent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnUpdateAutoTune
            // 
            this.btnUpdateAutoTune.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.btnUpdateAutoTune.Location = new System.Drawing.Point(1381, 12);
            this.btnUpdateAutoTune.Name = "btnUpdateAutoTune";
            this.btnUpdateAutoTune.Size = new System.Drawing.Size(307, 74);
            this.btnUpdateAutoTune.TabIndex = 4;
            this.btnUpdateAutoTune.Text = "検出した音程を反映";
            this.btnUpdateAutoTune.UseVisualStyleBackColor = true;
            this.btnUpdateAutoTune.Click += new System.EventHandler(this.btnUpdateAutoTune_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox4.Controls.Add(this.txtName);
            this.groupBox4.Location = new System.Drawing.Point(154, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(415, 74);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "名称";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName.Location = new System.Drawing.Point(6, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(388, 36);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox5.Controls.Add(this.numVolume);
            this.groupBox5.Location = new System.Drawing.Point(575, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(162, 74);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "音量(%)";
            // 
            // numVolume
            // 
            this.numVolume.DecimalPlaces = 1;
            this.numVolume.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numVolume.Location = new System.Drawing.Point(6, 25);
            this.numVolume.Name = "numVolume";
            this.numVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numVolume.Size = new System.Drawing.Size(120, 36);
            this.numVolume.TabIndex = 0;
            this.numVolume.ValueChanged += new System.EventHandler(this.numVolume_ValueChanged);
            // 
            // WaveInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1701, 1020);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnUpdateAutoTune);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbLoop);
            this.Controls.Add(this.grbMain);
            this.Controls.Add(this.btnPlay);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "WaveInfoForm";
            this.Text = "波形情報";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WaveInfoForm_FormClosing);
            this.Load += new System.EventHandler(this.WaveInfoForm_Load);
            this.SizeChanged += new System.EventHandler(this.WaveInfoForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaveScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).EndInit();
            this.grbMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numWaveAmp)).EndInit();
            this.grbLoop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLoopAmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFineTune)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.PictureBox picSpectrum;
		private System.Windows.Forms.HScrollBar hsbTime;
		private System.Windows.Forms.PictureBox picWave;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.NumericUpDown numWaveScale;
		private System.Windows.Forms.PictureBox picLoop;
		private System.Windows.Forms.GroupBox grbMain;
		private System.Windows.Forms.GroupBox grbLoop;
		private System.Windows.Forms.NumericUpDown numLoopScale;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.NumericUpDown numUnityNote;
		private System.Windows.Forms.NumericUpDown numFineTune;
		private System.Windows.Forms.Label lblUnityNote;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblPitch;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnUpdateAutoTune;
		private System.Windows.Forms.Button btnLoopCreate;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblPitchCent;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.NumericUpDown numVolume;
        private System.Windows.Forms.NumericUpDown numLoopAmp;
        private System.Windows.Forms.NumericUpDown numWaveAmp;
    }
}