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
			this.btnLoopCreate = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.numWaveAmp = new System.Windows.Forms.NumericUpDown();
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
			this.btnPlay.Location = new System.Drawing.Point(6, 6);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(61, 37);
			this.btnPlay.TabIndex = 0;
			this.btnPlay.Text = "再生";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// picSpectrum
			// 
			this.picSpectrum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picSpectrum.Location = new System.Drawing.Point(5, 26);
			this.picSpectrum.Name = "picSpectrum";
			this.picSpectrum.Size = new System.Drawing.Size(765, 66);
			this.picSpectrum.TabIndex = 2;
			this.picSpectrum.TabStop = false;
			// 
			// hsbTime
			// 
			this.hsbTime.Location = new System.Drawing.Point(5, 258);
			this.hsbTime.Name = "hsbTime";
			this.hsbTime.Size = new System.Drawing.Size(762, 28);
			this.hsbTime.TabIndex = 3;
			// 
			// picWave
			// 
			this.picWave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picWave.Location = new System.Drawing.Point(5, 96);
			this.picWave.Name = "picWave";
			this.picWave.Size = new System.Drawing.Size(765, 162);
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
			this.numWaveScale.Location = new System.Drawing.Point(107, 0);
			this.numWaveScale.Maximum = new decimal(new int[] {
            40,
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
			this.numWaveScale.Size = new System.Drawing.Size(58, 19);
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
			this.picLoop.Location = new System.Drawing.Point(5, 22);
			this.picLoop.Name = "picLoop";
			this.picLoop.Size = new System.Drawing.Size(765, 127);
			this.picLoop.TabIndex = 6;
			this.picLoop.TabStop = false;
			// 
			// grbMain
			// 
			this.grbMain.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbMain.Controls.Add(this.btnLoopCreate);
			this.grbMain.Controls.Add(this.btnUpdate);
			this.grbMain.Controls.Add(this.picSpectrum);
			this.grbMain.Controls.Add(this.numWaveAmp);
			this.grbMain.Controls.Add(this.picWave);
			this.grbMain.Controls.Add(this.numWaveScale);
			this.grbMain.Controls.Add(this.hsbTime);
			this.grbMain.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.grbMain.Location = new System.Drawing.Point(6, 48);
			this.grbMain.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbMain.Name = "grbMain";
			this.grbMain.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbMain.Size = new System.Drawing.Size(774, 277);
			this.grbMain.TabIndex = 5;
			this.grbMain.TabStop = false;
			this.grbMain.Text = "ループ範囲選択";
			// 
			// btnLoopCreate
			// 
			this.btnLoopCreate.BackColor = System.Drawing.SystemColors.Control;
			this.btnLoopCreate.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnLoopCreate.Location = new System.Drawing.Point(371, 0);
			this.btnLoopCreate.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.btnLoopCreate.Name = "btnLoopCreate";
			this.btnLoopCreate.Size = new System.Drawing.Size(93, 24);
			this.btnLoopCreate.TabIndex = 2;
			this.btnLoopCreate.Text = "ループ作成";
			this.btnLoopCreate.UseVisualStyleBackColor = true;
			this.btnLoopCreate.Click += new System.EventHandler(this.btnLoopCreate_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.BackColor = System.Drawing.SystemColors.Control;
			this.btnUpdate.Enabled = false;
			this.btnUpdate.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnUpdate.Location = new System.Drawing.Point(252, 0);
			this.btnUpdate.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(117, 24);
			this.btnUpdate.TabIndex = 1;
			this.btnUpdate.Text = "ループ範囲反映";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// numWaveAmp
			// 
			this.numWaveAmp.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numWaveAmp.Location = new System.Drawing.Point(172, 0);
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
			this.numWaveAmp.Size = new System.Drawing.Size(58, 19);
			this.numWaveAmp.TabIndex = 8;
			this.numWaveAmp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWaveAmp.ValueChanged += new System.EventHandler(this.numWaveAmp_ValueChanged);
			// 
			// grbLoop
			// 
			this.grbLoop.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbLoop.Controls.Add(this.numLoopAmp);
			this.grbLoop.Controls.Add(this.numLoopScale);
			this.grbLoop.Controls.Add(this.picLoop);
			this.grbLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grbLoop.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.grbLoop.Location = new System.Drawing.Point(6, 328);
			this.grbLoop.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbLoop.Name = "grbLoop";
			this.grbLoop.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.grbLoop.Size = new System.Drawing.Size(774, 151);
			this.grbLoop.TabIndex = 6;
			this.grbLoop.TabStop = false;
			this.grbLoop.Text = "ループ接続部";
			// 
			// numLoopAmp
			// 
			this.numLoopAmp.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numLoopAmp.Location = new System.Drawing.Point(171, 0);
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
			this.numLoopAmp.Size = new System.Drawing.Size(58, 19);
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
			this.numLoopScale.Location = new System.Drawing.Point(107, 0);
			this.numLoopScale.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.numLoopScale.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.numLoopScale.Name = "numLoopScale";
			this.numLoopScale.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.numLoopScale.Size = new System.Drawing.Size(58, 19);
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
			this.numUnityNote.Location = new System.Drawing.Point(3, 12);
			this.numUnityNote.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.numUnityNote.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numUnityNote.Name = "numUnityNote";
			this.numUnityNote.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.numUnityNote.Size = new System.Drawing.Size(55, 22);
			this.numUnityNote.TabIndex = 0;
			this.numUnityNote.ValueChanged += new System.EventHandler(this.numUnityNote_ValueChanged);
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
			this.numFineTune.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.numFineTune.Size = new System.Drawing.Size(55, 22);
			this.numFineTune.TabIndex = 0;
			this.numFineTune.ValueChanged += new System.EventHandler(this.numFineTune_ValueChanged);
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
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox1.Controls.Add(this.numUnityNote);
			this.groupBox1.Controls.Add(this.lblUnityNote);
			this.groupBox1.Location = new System.Drawing.Point(343, 6);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox1.Size = new System.Drawing.Size(103, 37);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "基準音";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox2.Controls.Add(this.numFineTune);
			this.groupBox2.Location = new System.Drawing.Point(449, 6);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox2.Size = new System.Drawing.Size(75, 37);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "ピッチ(cent)";
			// 
			// lblPitch
			// 
			this.lblPitch.AutoSize = true;
			this.lblPitch.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblPitch.Location = new System.Drawing.Point(3, 14);
			this.lblPitch.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.lblPitch.Name = "lblPitch";
			this.lblPitch.Size = new System.Drawing.Size(40, 15);
			this.lblPitch.TabIndex = 16;
			this.lblPitch.Text = "Gb-2";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox3.Controls.Add(this.lblPitchCent);
			this.groupBox3.Controls.Add(this.lblPitch);
			this.groupBox3.Location = new System.Drawing.Point(527, 6);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox3.Size = new System.Drawing.Size(108, 37);
			this.groupBox3.TabIndex = 17;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "音程検出";
			// 
			// lblPitchCent
			// 
			this.lblPitchCent.BackColor = System.Drawing.Color.Transparent;
			this.lblPitchCent.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblPitchCent.Location = new System.Drawing.Point(41, 14);
			this.lblPitchCent.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.lblPitchCent.Name = "lblPitchCent";
			this.lblPitchCent.Size = new System.Drawing.Size(60, 16);
			this.lblPitchCent.TabIndex = 17;
			this.lblPitchCent.Text = "-50cent";
			this.lblPitchCent.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// btnUpdateAutoTune
			// 
			this.btnUpdateAutoTune.Font = new System.Drawing.Font("MS UI Gothic", 11F);
			this.btnUpdateAutoTune.Location = new System.Drawing.Point(637, 6);
			this.btnUpdateAutoTune.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.btnUpdateAutoTune.Name = "btnUpdateAutoTune";
			this.btnUpdateAutoTune.Size = new System.Drawing.Size(142, 37);
			this.btnUpdateAutoTune.TabIndex = 4;
			this.btnUpdateAutoTune.Text = "検出した音程を反映";
			this.btnUpdateAutoTune.UseVisualStyleBackColor = true;
			this.btnUpdateAutoTune.Click += new System.EventHandler(this.btnUpdateAutoTune_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox4.Controls.Add(this.txtName);
			this.groupBox4.Location = new System.Drawing.Point(71, 6);
			this.groupBox4.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox4.Size = new System.Drawing.Size(192, 37);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "名称";
			// 
			// txtName
			// 
			this.txtName.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.txtName.Location = new System.Drawing.Point(3, 12);
			this.txtName.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(181, 22);
			this.txtName.TabIndex = 0;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// groupBox5
			// 
			this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox5.Controls.Add(this.numVolume);
			this.groupBox5.Location = new System.Drawing.Point(265, 6);
			this.groupBox5.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.groupBox5.Size = new System.Drawing.Size(75, 37);
			this.groupBox5.TabIndex = 4;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "音量(db)";
			// 
			// numVolume
			// 
			this.numVolume.DecimalPlaces = 1;
			this.numVolume.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numVolume.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numVolume.Location = new System.Drawing.Point(3, 12);
			this.numVolume.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.numVolume.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
			this.numVolume.Minimum = new decimal(new int[] {
            48,
            0,
            0,
            -2147483648});
			this.numVolume.Name = "numVolume";
			this.numVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.numVolume.Size = new System.Drawing.Size(55, 22);
			this.numVolume.TabIndex = 0;
			this.numVolume.ValueChanged += new System.EventHandler(this.numVolume_ValueChanged);
			// 
			// WaveInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(785, 486);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.btnUpdateAutoTune);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grbLoop);
			this.Controls.Add(this.grbMain);
			this.Controls.Add(this.btnPlay);
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