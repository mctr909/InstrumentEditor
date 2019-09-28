using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace InstrumentEditor {
    public partial class Envelope : UserControl {
        private DLS.ART mArt;
        private static int TrackBarWidth = 600;

        public Envelope() {
            InitializeComponent();

            #region AMP
            picAttack.Width = picAttack.Image.Width;
            picAttack.Height = picAttack.Image.Height + 4;
            picHold.Width = picHold.Image.Width;
            picHold.Height = picHold.Image.Height + 4;
            picDecay.Width = picDecay.Image.Width;
            picDecay.Height = picDecay.Image.Height + 4;
            picSustain.Width = picSustain.Image.Width;
            picSustain.Height = picSustain.Image.Height + 4;
            picReleace.Width = picReleace.Image.Width;
            picReleace.Height = picReleace.Image.Height + 4;

            picHold.Top = picAttack.Top + picAttack.Height;
            picDecay.Top = picHold.Top + picHold.Height;
            picSustain.Top = picDecay.Top + picDecay.Height;
            picReleace.Top = picSustain.Top + picSustain.Height;


            trbAmpAttack.Left = picAttack.Right + 4;
            trbAmpAttack.Top = picAttack.Top - 4;
            trbAmpAttack.Width = TrackBarWidth;
            trbAmpAttack.Height = picAttack.Height;

            trbAmpHold.Left = picHold.Right + 4;
            trbAmpHold.Top = picHold.Top - 4;
            trbAmpHold.Width = TrackBarWidth;
            trbAmpHold.Height = picHold.Height;

            trbAmpDecay.Left = picDecay.Right + 4;
            trbAmpDecay.Top = picDecay.Top - 4;
            trbAmpDecay.Width = TrackBarWidth;
            trbAmpDecay.Height = picDecay.Height;

            trbAmpSustain.Left = picSustain.Right + 4;
            trbAmpSustain.Top = picSustain.Top - 4;
            trbAmpSustain.Width = TrackBarWidth;
            trbAmpSustain.Height = picSustain.Height;

            trbAmpReleace.Left = picReleace.Right + 4;
            trbAmpReleace.Top = picReleace.Top - 4;
            trbAmpReleace.Width = TrackBarWidth;
            trbAmpReleace.Height = picReleace.Height;


            chkAmpAttack.Left = trbAmpAttack.Right + 4;
            chkAmpAttack.Top = trbAmpAttack.Top + (trbAmpAttack.Height - chkAmpAttack.Height) / 2;

            chkAmpHold.Left = trbAmpHold.Right + 4;
            chkAmpHold.Top = trbAmpHold.Top + (trbAmpHold.Height - chkAmpHold.Height) / 2;

            chkAmpDecay.Left = trbAmpDecay.Right + 4;
            chkAmpDecay.Top = trbAmpDecay.Top + (trbAmpDecay.Height - chkAmpDecay.Height) / 2;

            chkAmpSustain.Left = trbAmpSustain.Right + 4;
            chkAmpSustain.Top = trbAmpSustain.Top + (trbAmpSustain.Height - chkAmpSustain.Height) / 2;

            chkAmpReleace.Left = trbAmpReleace.Right + 4;
            chkAmpReleace.Top = trbAmpReleace.Top + (trbAmpReleace.Height - chkAmpReleace.Height) / 2;


            lblAttack.Left = chkAmpAttack.Right + 4;
            lblAttack.Top = trbAmpAttack.Top + (trbAmpAttack.Height - lblAttack.Height) / 2;

            lblHold.Left = chkAmpHold.Right + 4;
            lblHold.Top = trbAmpHold.Top + (trbAmpHold.Height - lblHold.Height) / 2;

            lblDecay.Left = chkAmpDecay.Right + 4;
            lblDecay.Top = trbAmpDecay.Top + (trbAmpDecay.Height - lblDecay.Height) / 2;

            lblSustain.Left = chkAmpSustain.Right + 4;
            lblSustain.Top = trbAmpSustain.Top + (trbAmpSustain.Height - lblSustain.Height) / 2;

            lblReleace.Left = chkAmpReleace.Right + 4;
            lblReleace.Top = trbAmpReleace.Top + (trbAmpReleace.Height - lblReleace.Height) / 2;


            grpAmp.Width = lblReleace.Location.X + lblReleace.Width + 8;
            grpAmp.Height = lblReleace.Location.Y + lblReleace.Height + 16;
            #endregion

            #region EQ
            grpEq.Top = grpAmp.Bottom + 8;

            picEqAttack.Width = picEqAttack.Image.Width;
            picEqAttack.Height = picEqAttack.Image.Height + 4;
            picEqHold.Width = picEqHold.Image.Width;
            picEqHold.Height = picEqHold.Image.Height + 4;
            picEqDecay.Width = picEqDecay.Image.Width;
            picEqDecay.Height = picEqDecay.Image.Height + 4;
            picEqSustain.Width = picEqSustain.Image.Width;
            picEqSustain.Height = picEqSustain.Image.Height + 4;
            picEqReleace.Width = picEqReleace.Image.Width;
            picEqReleace.Height = picEqReleace.Image.Height + 4;

            picEqHold.Top = picEqAttack.Top + picEqAttack.Height;
            picEqDecay.Top = picEqHold.Top + picEqHold.Height;
            picEqSustain.Top = picEqDecay.Top + picEqDecay.Height;
            picEqReleace.Top = picEqSustain.Top + picEqSustain.Height;


            trbEqAttack.Left = picEqAttack.Right + 4;
            trbEqAttack.Top = picEqAttack.Top - 4;
            trbEqAttack.Width = TrackBarWidth;
            trbEqAttack.Height = picEqAttack.Height;

            trbEqHold.Left = picEqHold.Right + 4;
            trbEqHold.Top = picEqHold.Top - 4;
            trbEqHold.Width = TrackBarWidth;
            trbEqHold.Height = picEqHold.Height;

            trbEqDecay.Left = picEqDecay.Right + 4;
            trbEqDecay.Top = picEqDecay.Top - 4;
            trbEqDecay.Width = TrackBarWidth;
            trbEqDecay.Height = picEqDecay.Height;

            trbEqSustain.Left = picEqSustain.Right + 4;
            trbEqSustain.Top = picEqSustain.Top - 4;
            trbEqSustain.Width = TrackBarWidth;
            trbEqSustain.Height = picEqSustain.Height;

            trbEqReleace.Left = picEqReleace.Right + 4;
            trbEqReleace.Top = picEqReleace.Top - 4;
            trbEqReleace.Width = TrackBarWidth;
            trbEqReleace.Height = picEqReleace.Height;


            chkEqAttack.Left = trbEqAttack.Right + 4;
            chkEqAttack.Top = trbEqAttack.Top + (trbEqAttack.Height - chkEqAttack.Height) / 2;

            chkEqHold.Left = trbEqHold.Right + 4;
            chkEqHold.Top = trbEqHold.Top + (trbEqHold.Height - chkEqHold.Height) / 2;

            chkEqDecay.Left = trbEqDecay.Right + 4;
            chkEqDecay.Top = trbEqDecay.Top + (trbEqDecay.Height - chkEqDecay.Height) / 2;

            chkEqSustain.Left = trbEqSustain.Right + 4;
            chkEqSustain.Top = trbEqSustain.Top + (trbEqSustain.Height - chkEqSustain.Height) / 2;

            chkEqReleace.Left = trbEqReleace.Right + 4;
            chkEqReleace.Top = trbEqReleace.Top + (trbEqReleace.Height - chkEqReleace.Height) / 2;


            lblEqAttack.Left = chkEqAttack.Right + 4;
            lblEqAttack.Top = trbEqAttack.Top + (trbEqAttack.Height - lblEqAttack.Height) / 2;

            lblEqHold.Left = chkEqHold.Right + 4;
            lblEqHold.Top = trbEqHold.Top + (trbEqHold.Height - lblEqHold.Height) / 2;

            lblEqDecay.Left = chkEqDecay.Right + 4;
            lblEqDecay.Top = trbEqDecay.Top + (trbEqDecay.Height - lblEqDecay.Height) / 2;

            lblEqSustain.Left = chkEqSustain.Right + 4;
            lblEqSustain.Top = trbEqSustain.Top + (trbEqSustain.Height - lblEqSustain.Height) / 2;

            lblEqReleace.Left = chkEqReleace.Right + 4;
            lblEqReleace.Top = trbEqReleace.Top + (trbEqReleace.Height - lblEqReleace.Height) / 2;


            grpEq.Width = lblEqReleace.Location.X + lblEqReleace.Width + 8;
            grpEq.Height = lblEqReleace.Location.Y + lblEqReleace.Height + 16;
            #endregion

            Width = grpEq.Width + 4;
            Height = grpEq.Location.Y + grpEq.Height + 8;

            disp();
        }

        public DLS.ART Art {
            get { return mArt; }
            set {
                mArt = value;
                disp();
            }
        }

        public void SetList(Dictionary<int, DLS.Connection> list) {
            if (chkAmpAttack.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_ATTACK_TIME;
                v.Value = ampAttack;
                list.Add(list.Count, v);
            }

            if (chkAmpHold.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_HOLD_TIME;
                v.Value = ampHold;
                list.Add(list.Count, v);
            }

            if (chkAmpDecay.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_DECAY_TIME;
                v.Value = ampDecay;
                list.Add(list.Count, v);
            }

            if (chkAmpSustain.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_SUSTAIN_LEVEL;
                v.Value = ampSustain;
                list.Add(list.Count, v);
            }

            if (chkAmpReleace.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_RELEASE_TIME;
                v.Value = ampReleace;
                list.Add(list.Count, v);
            }

            if (chkEqAttack.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_ATTACK_TIME;
                v.Value = eqAttack;
                list.Add(list.Count, v);
            }

            if (chkEqHold.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_HOLD_TIME;
                v.Value = eqHold;
                list.Add(list.Count, v);
            }

            if (chkEqDecay.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_DECAY_TIME;
                v.Value = eqDecay;
                list.Add(list.Count, v);
            }

            if (chkEqSustain.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_SUSTAIN_LEVEL;
                v.Value = eqSustain;
                list.Add(list.Count, v);
            }

            if (chkEqReleace.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_RELEASE_TIME;
                v.Value = eqReleace;
                list.Add(list.Count, v);
            }
        }

        #region AMP
        private double ampAttack {
            get { return trbToValue(trbAmpAttack.Value); }
            set { setValue(trbAmpAttack, chkAmpAttack, lblAttack, value); }
        }

        private double ampHold {
            get { return trbToValue(trbAmpHold.Value); }
            set { setValue(trbAmpHold, chkAmpHold, lblHold, value); }
        }

        private double ampDecay {
            get { return trbToValue(trbAmpDecay.Value); }
            set { setValue(trbAmpDecay, chkAmpDecay, lblDecay, value); }
        }

        private double ampSustain {
            get { return trbAmpSustain.Value * 0.1; }
            set {
                chkAmpSustain.Checked = true;
                trbAmpSustain.Enabled = true;
                trbAmpSustain.Value = (int)(value * 10);
            }
        }

        private double ampReleace {
            get { return trbToValue(trbAmpReleace.Value); }
            set { setValue(trbAmpReleace, chkAmpReleace, lblReleace, value); }
        }

        private void trbAmpAttack_ValueChanged(object sender, EventArgs e) {
            lblAttack.Text = trbToText(trbAmpAttack.Value);
        }

        private void trbAmpHold_ValueChanged(object sender, EventArgs e) {
            lblHold.Text = trbToText(trbAmpHold.Value);
        }

        private void trbAmpDecay_ValueChanged(object sender, EventArgs e) {
            lblDecay.Text = trbToText(trbAmpDecay.Value);
        }

        private void trbAmpSustain_ValueChanged(object sender, EventArgs e) {
            lblSustain.Text = string.Format("{0}%", trbAmpSustain.Value * 0.1);
        }

        private void trbAmpReleace_ValueChanged(object sender, EventArgs e) {
            lblReleace.Text = trbToText(trbAmpReleace.Value);
        }

        private void chkAttack_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpAttack.Checked) {
                trbAmpAttack.Enabled = true;
                lblAttack.Text = trbToText(trbAmpAttack.Value);
            }
            else {
                trbAmpAttack.Enabled = false;
                lblAttack.Text = "----";
            }
        }

        private void chkHold_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpHold.Checked) {
                trbAmpHold.Enabled = true;
                lblHold.Text = trbToText(trbAmpHold.Value);
            }
            else {
                trbAmpHold.Enabled = false;
                lblHold.Text = "----";
            }
        }

        private void chkDecay_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpDecay.Checked) {
                trbAmpDecay.Enabled = true;
                lblDecay.Text = trbToText(trbAmpDecay.Value);
            }
            else {
                trbAmpDecay.Enabled = false;
                lblDecay.Text = "----";
            }
        }

        private void chkSustain_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpSustain.Checked) {
                trbAmpSustain.Enabled = true;
                lblSustain.Text = string.Format("{0}%", trbAmpSustain.Value * 0.1);
            }
            else {
                trbAmpSustain.Enabled = false;
                lblSustain.Text = "----";
            }
        }

        private void chkReleace_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpReleace.Checked) {
                trbAmpReleace.Enabled = true;
                lblReleace.Text = trbToText(trbAmpReleace.Value);
            }
            else {
                trbAmpReleace.Enabled = false;
                lblReleace.Text = "----";
            }
        }
        #endregion

        #region EQ
        private double eqAttack {
            get { return trbToValue(trbEqAttack.Value); }
            set { setValue(trbEqAttack, chkEqAttack, lblEqAttack, value); }
        }

        private double eqHold {
            get { return trbToValue(trbEqHold.Value); }
            set { setValue(trbEqHold, chkEqHold, lblEqHold, value); }
        }

        private double eqDecay {
            get { return trbToValue(trbEqDecay.Value); }
            set { setValue(trbEqDecay, chkEqDecay, lblEqDecay, value); }
        }

        private double eqSustain {
            get { return trbEqSustain.Value * 0.1; }
            set {
                chkEqSustain.Checked = true;
                trbEqSustain.Enabled = true;
                trbEqSustain.Value = (int)(value * 10);
            }
        }

        private double eqReleace {
            get { return trbToValue(trbEqReleace.Value); }
            set { setValue(trbEqReleace, chkEqReleace, lblEqReleace, value); }
        }

        private void trbEqAttack_ValueChanged(object sender, EventArgs e) {
            lblEqAttack.Text = trbToText(trbEqAttack.Value);
        }

        private void trbEqHold_ValueChanged(object sender, EventArgs e) {
            lblEqHold.Text = trbToText(trbEqHold.Value);
        }

        private void trbEqDecay_ValueChanged(object sender, EventArgs e) {
            lblEqDecay.Text = trbToText(trbEqDecay.Value);
        }

        private void trbEqSustain_ValueChanged(object sender, EventArgs e) {
            lblEqSustain.Text = string.Format("{0}%", trbEqSustain.Value * 0.1);
        }

        private void trbEqReleace_ValueChanged(object sender, EventArgs e) {
            lblEqReleace.Text = trbToText(trbEqReleace.Value);
        }

        private void chkEqAttack_CheckedChanged(object sender, EventArgs e) {
            if (chkEqAttack.Checked) {
                trbEqAttack.Enabled = true;
                lblEqAttack.Text = trbToText(trbEqAttack.Value);
            }
            else {
                trbEqAttack.Enabled = false;
                lblEqAttack.Text = "----";
            }
        }

        private void chkEqHold_CheckedChanged(object sender, EventArgs e) {
            if (chkEqHold.Checked) {
                trbEqHold.Enabled = true;
                lblEqHold.Text = trbToText(trbEqHold.Value);
            }
            else {
                trbEqHold.Enabled = false;
                lblEqHold.Text = "----";
            }
        }

        private void chkEqDecay_CheckedChanged(object sender, EventArgs e) {
            if (chkEqDecay.Checked) {
                trbEqDecay.Enabled = true;
                lblEqDecay.Text = trbToText(trbEqDecay.Value);
            }
            else {
                trbEqDecay.Enabled = false;
                lblEqDecay.Text = "----";
            }
        }

        private void chkEqSustain_CheckedChanged(object sender, EventArgs e) {
            if (chkEqSustain.Checked) {
                trbEqSustain.Enabled = true;
                lblEqSustain.Text = string.Format("{0}%", trbEqSustain.Value * 0.1);
            }
            else {
                trbEqSustain.Enabled = false;
                lblEqSustain.Text = "----";
            }
        }

        private void chkEqReleace_CheckedChanged(object sender, EventArgs e) {
            if (chkEqReleace.Checked) {
                trbEqReleace.Enabled = true;
                lblEqReleace.Text = trbToText(trbEqReleace.Value);
            }
            else {
                trbEqReleace.Enabled = false;
                lblEqReleace.Text = "----";
            }
        }
        #endregion

        private string trbToText(int value) {
            var v = trbToValue(value);

            if (v < 1.0) {
                return string.Format("{0}ms", (int)(1000 * v));
            }
            else if (v < 10.0) {
                return string.Format("{0}s", ((int)(100 * v) / 100.0).ToString("0.00"));
            }
            else {
                return string.Format("{0}s", ((int)(10 * v) / 10.0).ToString("0.0"));
            }
        }

        private double trbToValue(int value) {
            return 40 * Math.Pow(64, value / 2500.0) / 64 - 0.626;
        }

        private int valueToHsb(double hsb) {
            return (int)(Math.Log((hsb * 1000 + 626) * 64 / 40000, 64) * 2500 + 1);
        }

        private void setValue(TrackBar tkb, CheckBox chk, Label lbl, double value) {
            if (value <= 0) {
                tkb.Value = 1;
                tkb.Enabled = false;
                chk.Checked = false;
                lbl.Text = "----";
            }
            else if (39 < value) {
                tkb.Value = valueToHsb(39);
                tkb.Enabled = true;
                chk.Checked = true;
            }
            else {
                tkb.Value = valueToHsb(value);
                tkb.Enabled = true;
                chk.Checked = true;
            }
        }

        private void disp() {
            trbAmpAttack.Enabled = false;
            chkAmpAttack.Checked = false;
            lblAttack.Text = "----";

            trbAmpHold.Enabled = false;
            chkAmpHold.Checked = false;
            lblHold.Text = "----";

            trbAmpDecay.Enabled = false;
            chkAmpDecay.Checked = false;
            lblDecay.Text = "----";

            trbAmpSustain.Enabled = false;
            chkAmpSustain.Checked = false;
            lblSustain.Text = "----";

            trbAmpReleace.Enabled = false;
            chkAmpReleace.Checked = false;
            lblReleace.Text = "----";


            trbEqAttack.Enabled = false;
            chkEqAttack.Checked = false;
            lblEqAttack.Text = "----";

            trbEqHold.Enabled = false;
            chkEqHold.Checked = false;
            lblEqHold.Text = "----";

            trbEqDecay.Enabled = false;
            chkEqDecay.Checked = false;
            lblEqDecay.Text = "----";

            trbEqSustain.Enabled = false;
            chkEqSustain.Checked = false;
            lblEqSustain.Text = "----";

            trbEqReleace.Enabled = false;
            chkEqReleace.Checked = false;
            lblEqReleace.Text = "----";


            ampAttack = 0;
            ampHold = 0;
            ampDecay = 0;
            ampReleace = 0;

            eqAttack = 0;
            eqHold = 0;
            eqDecay = 0;
            eqReleace = 0;

            if (null != mArt) {
                foreach (var art in mArt.List.Values) {
                    if (DLS.Connection.SRC_TYPE.NONE != art.Source) {
                        continue;
                    }

                    if (DLS.Connection.SRC_TYPE.NONE != art.Control) {
                        continue;
                    }

                    switch (art.Destination) {
                        case DLS.Connection.DST_TYPE.EG1_ATTACK_TIME:
                            ampAttack = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_HOLD_TIME:
                            ampHold = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_DECAY_TIME:
                            ampDecay = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
                            ampSustain = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_RELEASE_TIME:
                            ampReleace = art.Value;
                            break;

                        case DLS.Connection.DST_TYPE.EG2_ATTACK_TIME:
                            eqAttack = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_HOLD_TIME:
                            eqHold = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_DECAY_TIME:
                            eqDecay = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_SUSTAIN_LEVEL:
                            eqSustain = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_RELEASE_TIME:
                            eqReleace = art.Value;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}