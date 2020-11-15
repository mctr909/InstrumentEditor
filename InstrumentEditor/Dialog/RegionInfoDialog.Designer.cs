namespace InstrumentEditor {
    partial class RegionInfoDialog {
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
            this.txtWave = new System.Windows.Forms.TextBox();
            this.grbWave = new System.Windows.Forms.GroupBox();
            this.btnSelectWave = new System.Windows.Forms.Button();
            this.btnEditWave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.grbFineTune = new System.Windows.Forms.GroupBox();
            this.numFineTune = new System.Windows.Forms.NumericUpDown();
            this.grbUnityNote = new System.Windows.Forms.GroupBox();
            this.numUnityNote = new System.Windows.Forms.NumericUpDown();
            this.lblUnityNote = new System.Windows.Forms.Label();
            this.grbVolume = new System.Windows.Forms.GroupBox();
            this.numVolume = new System.Windows.Forms.NumericUpDown();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.envelope1 = new InstrumentEditor.Envelope();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).BeginInit();
            this.grbKey.SuspendLayout();
            this.grbVelocity.SuspendLayout();
            this.grbWave.SuspendLayout();
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
            this.numKeyLow.Location = new System.Drawing.Point(7, 30);
            this.numKeyLow.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.numKeyLow.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numKeyLow.Name = "numKeyLow";
            this.numKeyLow.Size = new System.Drawing.Size(119, 31);
            this.numKeyLow.TabIndex = 0;
            this.numKeyLow.ValueChanged += new System.EventHandler(this.numKeyLow_ValueChanged);
            // 
            // numKeyHigh
            // 
            this.numKeyHigh.Location = new System.Drawing.Point(132, 30);
            this.numKeyHigh.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.numKeyHigh.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numKeyHigh.Name = "numKeyHigh";
            this.numKeyHigh.Size = new System.Drawing.Size(119, 31);
            this.numKeyHigh.TabIndex = 1;
            this.numKeyHigh.ValueChanged += new System.EventHandler(this.numKeyHigh_ValueChanged);
            // 
            // numVelocityHigh
            // 
            this.numVelocityHigh.Location = new System.Drawing.Point(132, 30);
            this.numVelocityHigh.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.numVelocityHigh.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numVelocityHigh.Name = "numVelocityHigh";
            this.numVelocityHigh.Size = new System.Drawing.Size(119, 31);
            this.numVelocityHigh.TabIndex = 1;
            this.numVelocityHigh.ValueChanged += new System.EventHandler(this.numVelocityHigh_ValueChanged);
            // 
            // numVelocityLow
            // 
            this.numVelocityLow.Location = new System.Drawing.Point(7, 30);
            this.numVelocityLow.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.numVelocityLow.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numVelocityLow.Name = "numVelocityLow";
            this.numVelocityLow.Size = new System.Drawing.Size(119, 31);
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
            this.grbKey.Location = new System.Drawing.Point(13, 12);
            this.grbKey.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbKey.Name = "grbKey";
            this.grbKey.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbKey.Size = new System.Drawing.Size(280, 96);
            this.grbKey.TabIndex = 0;
            this.grbKey.TabStop = false;
            this.grbKey.Text = "音程";
            // 
            // lblKeyLow
            // 
            this.lblKeyLow.AutoSize = true;
            this.lblKeyLow.Location = new System.Drawing.Point(7, 64);
            this.lblKeyLow.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblKeyLow.Name = "lblKeyLow";
            this.lblKeyLow.Size = new System.Drawing.Size(80, 24);
            this.lblKeyLow.TabIndex = 6;
            this.lblKeyLow.Text = "Bb-2 5";
            // 
            // lblKeyHigh
            // 
            this.lblKeyHigh.AutoSize = true;
            this.lblKeyHigh.Location = new System.Drawing.Point(128, 64);
            this.lblKeyHigh.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblKeyHigh.Name = "lblKeyHigh";
            this.lblKeyHigh.Size = new System.Drawing.Size(80, 24);
            this.lblKeyHigh.TabIndex = 7;
            this.lblKeyHigh.Text = "Bb-2 5";
            // 
            // grbVelocity
            // 
            this.grbVelocity.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbVelocity.Controls.Add(this.numVelocityLow);
            this.grbVelocity.Controls.Add(this.numVelocityHigh);
            this.grbVelocity.Location = new System.Drawing.Point(299, 12);
            this.grbVelocity.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbVelocity.Name = "grbVelocity";
            this.grbVelocity.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbVelocity.Size = new System.Drawing.Size(280, 96);
            this.grbVelocity.TabIndex = 1;
            this.grbVelocity.TabStop = false;
            this.grbVelocity.Text = "強弱";
            // 
            // txtWave
            // 
            this.txtWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtWave.Location = new System.Drawing.Point(7, 30);
            this.txtWave.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtWave.Name = "txtWave";
            this.txtWave.ReadOnly = true;
            this.txtWave.Size = new System.Drawing.Size(407, 31);
            this.txtWave.TabIndex = 0;
            // 
            // grbWave
            // 
            this.grbWave.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbWave.Controls.Add(this.btnSelectWave);
            this.grbWave.Controls.Add(this.btnEditWave);
            this.grbWave.Controls.Add(this.txtWave);
            this.grbWave.Location = new System.Drawing.Point(13, 116);
            this.grbWave.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbWave.Name = "grbWave";
            this.grbWave.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbWave.Size = new System.Drawing.Size(566, 76);
            this.grbWave.TabIndex = 2;
            this.grbWave.TabStop = false;
            this.grbWave.Text = "波形";
            // 
            // btnSelectWave
            // 
            this.btnSelectWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSelectWave.Location = new System.Drawing.Point(418, 26);
            this.btnSelectWave.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSelectWave.Name = "btnSelectWave";
            this.btnSelectWave.Size = new System.Drawing.Size(91, 40);
            this.btnSelectWave.TabIndex = 1;
            this.btnSelectWave.Text = "選択";
            this.btnSelectWave.UseVisualStyleBackColor = true;
            this.btnSelectWave.Click += new System.EventHandler(this.btnSelectWave_Click);
            // 
            // btnEditWave
            // 
            this.btnEditWave.Location = new System.Drawing.Point(477, 26);
            this.btnEditWave.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnEditWave.Name = "btnEditWave";
            this.btnEditWave.Size = new System.Drawing.Size(91, 40);
            this.btnEditWave.TabIndex = 2;
            this.btnEditWave.Text = "編集";
            this.btnEditWave.UseVisualStyleBackColor = true;
            this.btnEditWave.Click += new System.EventHandler(this.btnEditWave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAdd.Location = new System.Drawing.Point(472, 700);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(119, 64);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grbFineTune
            // 
            this.grbFineTune.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbFineTune.Controls.Add(this.numFineTune);
            this.grbFineTune.Location = new System.Drawing.Point(243, 198);
            this.grbFineTune.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbFineTune.Name = "grbFineTune";
            this.grbFineTune.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbFineTune.Size = new System.Drawing.Size(163, 70);
            this.grbFineTune.TabIndex = 4;
            this.grbFineTune.TabStop = false;
            this.grbFineTune.Text = "ピッチ(cent)";
            // 
            // numFineTune
            // 
            this.numFineTune.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numFineTune.Location = new System.Drawing.Point(7, 24);
            this.numFineTune.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
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
            this.numFineTune.Size = new System.Drawing.Size(119, 31);
            this.numFineTune.TabIndex = 0;
            // 
            // grbUnityNote
            // 
            this.grbUnityNote.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbUnityNote.Controls.Add(this.numUnityNote);
            this.grbUnityNote.Controls.Add(this.lblUnityNote);
            this.grbUnityNote.Location = new System.Drawing.Point(13, 198);
            this.grbUnityNote.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbUnityNote.Name = "grbUnityNote";
            this.grbUnityNote.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbUnityNote.Size = new System.Drawing.Size(223, 70);
            this.grbUnityNote.TabIndex = 3;
            this.grbUnityNote.TabStop = false;
            this.grbUnityNote.Text = "基準音";
            // 
            // numUnityNote
            // 
            this.numUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numUnityNote.Location = new System.Drawing.Point(7, 24);
            this.numUnityNote.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.numUnityNote.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numUnityNote.Name = "numUnityNote";
            this.numUnityNote.Size = new System.Drawing.Size(119, 31);
            this.numUnityNote.TabIndex = 0;
            this.numUnityNote.ValueChanged += new System.EventHandler(this.numUnityNote_ValueChanged);
            // 
            // lblUnityNote
            // 
            this.lblUnityNote.AutoSize = true;
            this.lblUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnityNote.Location = new System.Drawing.Point(132, 28);
            this.lblUnityNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUnityNote.Name = "lblUnityNote";
            this.lblUnityNote.Size = new System.Drawing.Size(67, 24);
            this.lblUnityNote.TabIndex = 11;
            this.lblUnityNote.Text = "label1";
            // 
            // grbVolume
            // 
            this.grbVolume.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grbVolume.Controls.Add(this.numVolume);
            this.grbVolume.Location = new System.Drawing.Point(416, 198);
            this.grbVolume.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbVolume.Name = "grbVolume";
            this.grbVolume.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.grbVolume.Size = new System.Drawing.Size(163, 70);
            this.grbVolume.TabIndex = 5;
            this.grbVolume.TabStop = false;
            this.grbVolume.Text = "音量(db)";
            // 
            // numVolume
            // 
            this.numVolume.DecimalPlaces = 1;
            this.numVolume.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numVolume.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numVolume.Location = new System.Drawing.Point(7, 24);
            this.numVolume.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
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
            this.numVolume.Size = new System.Drawing.Size(119, 31);
            this.numVolume.TabIndex = 0;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(13, 700);
            this.chkLoop.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(148, 28);
            this.chkLoop.TabIndex = 7;
            this.chkLoop.Text = "ループ有効";
            this.chkLoop.UseVisualStyleBackColor = true;
            // 
            // envelope1
            // 
            this.envelope1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.envelope1.Art = null;
            this.envelope1.AutoSize = true;
            this.envelope1.Location = new System.Drawing.Point(13, 278);
            this.envelope1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.envelope1.Name = "envelope1";
            this.envelope1.Size = new System.Drawing.Size(1281, 386);
            this.envelope1.TabIndex = 8;
            // 
            // RegionInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 776);
            this.Controls.Add(this.envelope1);
            this.Controls.Add(this.chkLoop);
            this.Controls.Add(this.grbVolume);
            this.Controls.Add(this.grbFineTune);
            this.Controls.Add(this.grbUnityNote);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grbWave);
            this.Controls.Add(this.grbVelocity);
            this.Controls.Add(this.grbKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "RegionInfoForm";
            this.Text = "リージョン情報編集";
            ((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).EndInit();
            this.grbKey.ResumeLayout(false);
            this.grbKey.PerformLayout();
            this.grbVelocity.ResumeLayout(false);
            this.grbWave.ResumeLayout(false);
            this.grbWave.PerformLayout();
            this.grbFineTune.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numFineTune)).EndInit();
            this.grbUnityNote.ResumeLayout(false);
            this.grbUnityNote.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).EndInit();
            this.grbVolume.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TextBox txtWave;
        private System.Windows.Forms.GroupBox grbWave;
        private System.Windows.Forms.Button btnSelectWave;
        private System.Windows.Forms.Button btnEditWave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox grbFineTune;
        private System.Windows.Forms.NumericUpDown numFineTune;
        private System.Windows.Forms.GroupBox grbUnityNote;
        private System.Windows.Forms.NumericUpDown numUnityNote;
        private System.Windows.Forms.Label lblUnityNote;
        private System.Windows.Forms.GroupBox grbVolume;
        private System.Windows.Forms.NumericUpDown numVolume;
        private System.Windows.Forms.CheckBox chkLoop;
        private Envelope envelope1;
    }
}