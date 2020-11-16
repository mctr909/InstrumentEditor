using System;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
    public partial class Envelope : UserControl {
        private Lart mLart;
        private static int TimeGamma = 1000;
        private static int TrackBarWidth = 320;

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


            chkAmpAttack.Left = trbAmpAttack.Right;
            chkAmpAttack.Top = trbAmpAttack.Top + (trbAmpAttack.Height - chkAmpAttack.Height) / 2;

            chkAmpHold.Left = trbAmpHold.Right;
            chkAmpHold.Top = trbAmpHold.Top + (trbAmpHold.Height - chkAmpHold.Height) / 2;

            chkAmpDecay.Left = trbAmpDecay.Right;
            chkAmpDecay.Top = trbAmpDecay.Top + (trbAmpDecay.Height - chkAmpDecay.Height) / 2;

            chkAmpSustain.Left = trbAmpSustain.Right;
            chkAmpSustain.Top = trbAmpSustain.Top + (trbAmpSustain.Height - chkAmpSustain.Height) / 2;

            chkAmpReleace.Left = trbAmpReleace.Right;
            chkAmpReleace.Top = trbAmpReleace.Top + (trbAmpReleace.Height - chkAmpReleace.Height) / 2;


            lblAttack.Left = chkAmpAttack.Right;
            lblAttack.Top = trbAmpAttack.Top + (trbAmpAttack.Height - lblAttack.Height) / 2;

            lblHold.Left = chkAmpHold.Right;
            lblHold.Top = trbAmpHold.Top + (trbAmpHold.Height - lblHold.Height) / 2;

            lblDecay.Left = chkAmpDecay.Right;
            lblDecay.Top = trbAmpDecay.Top + (trbAmpDecay.Height - lblDecay.Height) / 2;

            lblSustain.Left = chkAmpSustain.Right;
            lblSustain.Top = trbAmpSustain.Top + (trbAmpSustain.Height - lblSustain.Height) / 2;

            lblReleace.Left = chkAmpReleace.Right;
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


            chkEqAttack.Left = trbEqAttack.Right;
            chkEqAttack.Top = trbEqAttack.Top + (trbEqAttack.Height - chkEqAttack.Height) / 2;

            chkEqHold.Left = trbEqHold.Right;
            chkEqHold.Top = trbEqHold.Top + (trbEqHold.Height - chkEqHold.Height) / 2;

            chkEqDecay.Left = trbEqDecay.Right;
            chkEqDecay.Top = trbEqDecay.Top + (trbEqDecay.Height - chkEqDecay.Height) / 2;

            chkEqSustain.Left = trbEqSustain.Right;
            chkEqSustain.Top = trbEqSustain.Top + (trbEqSustain.Height - chkEqSustain.Height) / 2;

            chkEqReleace.Left = trbEqReleace.Right;
            chkEqReleace.Top = trbEqReleace.Top + (trbEqReleace.Height - chkEqReleace.Height) / 2;


            lblEqAttack.Left = chkEqAttack.Right;
            lblEqAttack.Top = trbEqAttack.Top + (trbEqAttack.Height - lblEqAttack.Height) / 2;

            lblEqHold.Left = chkEqHold.Right;
            lblEqHold.Top = trbEqHold.Top + (trbEqHold.Height - lblEqHold.Height) / 2;

            lblEqDecay.Left = chkEqDecay.Right;
            lblEqDecay.Top = trbEqDecay.Top + (trbEqDecay.Height - lblEqDecay.Height) / 2;

            lblEqSustain.Left = chkEqSustain.Right;
            lblEqSustain.Top = trbEqSustain.Top + (trbEqSustain.Height - lblEqSustain.Height) / 2;

            lblEqReleace.Left = chkEqReleace.Right;
            lblEqReleace.Top = trbEqReleace.Top + (trbEqReleace.Height - lblEqReleace.Height) / 2;


            grpEq.Width = lblEqReleace.Location.X + lblEqReleace.Width + 8;
            grpEq.Height = lblEqReleace.Location.Y + lblEqReleace.Height + 16;
            #endregion

            Width = grpEq.Width + 4;
            Height = grpEq.Location.Y + grpEq.Height + 8;

            disp();
        }

        public Lart Art {
            get { return mLart; }
            set {
                mLart = value;
                disp();
            }
        }

        public void SetList(Lart list) {
            if (chkAmpAttack.Checked) {
                list.Update(ART_TYPE.EG_AMP_ATTACK,(float)ampAttack);
            }

            if (chkAmpHold.Checked) {
                list.Update(ART_TYPE.EG_AMP_HOLD, (float)ampHold);
            }

            if (chkAmpDecay.Checked) {
                list.Update(ART_TYPE.EG_AMP_DECAY, (float)ampDecay);
            }

            if (chkAmpSustain.Checked) {
                list.Update(ART_TYPE.EG_AMP_SUSTAIN, (float)ampSustain);
            }

            if (chkAmpReleace.Checked) {
                list.Update(ART_TYPE.EG_AMP_RELEASE, (float)ampReleace);
            }

            if (chkEqAttack.Checked) {
                list.Update(ART_TYPE.EG_CUTOFF_ATTACK, (float)eqAttack);
            }

            if (chkEqHold.Checked) {
                list.Update(ART_TYPE.EG_CUTOFF_HOLD, (float)eqHold);
            }

            if (chkEqDecay.Checked) {
                list.Update(ART_TYPE.EG_CUTOFF_DECAY, (float)eqDecay);
            }

            if (chkEqSustain.Checked) {
                list.Update(ART_TYPE.EG_CUTOFF_SUSTAIN, (float)eqSustain);
            }

            if (chkEqReleace.Checked) {
                list.Update(ART_TYPE.EG_CUTOFF_RELEASE, (float)eqReleace);
            }
        }

        #region AMP
        private double ampAttack {
            get { return trbToValue(trbAmpAttack); }
            set { setValue(trbAmpAttack, chkAmpAttack, lblAttack, value); }
        }

        private double ampHold {
            get { return trbToValue(trbAmpHold); }
            set { setValue(trbAmpHold, chkAmpHold, lblHold, value); }
        }

        private double ampDecay {
            get { return trbToValue(trbAmpDecay); }
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
            get { return trbToValue(trbAmpReleace); }
            set { setValue(trbAmpReleace, chkAmpReleace, lblReleace, value); }
        }

        private void trbAmpAttack_ValueChanged(object sender, EventArgs e) {
            lblAttack.Text = trbToText(trbAmpAttack);
        }

        private void trbAmpHold_ValueChanged(object sender, EventArgs e) {
            lblHold.Text = trbToText(trbAmpHold);
        }

        private void trbAmpDecay_ValueChanged(object sender, EventArgs e) {
            lblDecay.Text = trbToText(trbAmpDecay);
        }

        private void trbAmpSustain_ValueChanged(object sender, EventArgs e) {
            lblSustain.Text = string.Format("{0}%", trbAmpSustain.Value * 0.1);
        }

        private void trbAmpReleace_ValueChanged(object sender, EventArgs e) {
            lblReleace.Text = trbToText(trbAmpReleace);
        }

        private void chkAttack_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpAttack.Checked) {
                trbAmpAttack.Enabled = true;
                lblAttack.Text = trbToText(trbAmpAttack);
            }
            else {
                trbAmpAttack.Enabled = false;
                lblAttack.Text = "----";
            }
        }

        private void chkHold_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpHold.Checked) {
                trbAmpHold.Enabled = true;
                lblHold.Text = trbToText(trbAmpHold);
            }
            else {
                trbAmpHold.Enabled = false;
                lblHold.Text = "----";
            }
        }

        private void chkDecay_CheckedChanged(object sender, EventArgs e) {
            if (chkAmpDecay.Checked) {
                trbAmpDecay.Enabled = true;
                lblDecay.Text = trbToText(trbAmpDecay);
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
                lblReleace.Text = trbToText(trbAmpReleace);
            }
            else {
                trbAmpReleace.Enabled = false;
                lblReleace.Text = "----";
            }
        }
        #endregion

        #region EQ
        private double eqAttack {
            get { return trbToValue(trbEqAttack); }
            set { setValue(trbEqAttack, chkEqAttack, lblEqAttack, value); }
        }

        private double eqHold {
            get { return trbToValue(trbEqHold); }
            set { setValue(trbEqHold, chkEqHold, lblEqHold, value); }
        }

        private double eqDecay {
            get { return trbToValue(trbEqDecay); }
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
            get { return trbToValue(trbEqReleace); }
            set { setValue(trbEqReleace, chkEqReleace, lblEqReleace, value); }
        }

        private void trbEqAttack_ValueChanged(object sender, EventArgs e) {
            lblEqAttack.Text = trbToText(trbEqAttack);
        }

        private void trbEqHold_ValueChanged(object sender, EventArgs e) {
            lblEqHold.Text = trbToText(trbEqHold);
        }

        private void trbEqDecay_ValueChanged(object sender, EventArgs e) {
            lblEqDecay.Text = trbToText(trbEqDecay);
        }

        private void trbEqSustain_ValueChanged(object sender, EventArgs e) {
            lblEqSustain.Text = string.Format("{0}%", trbEqSustain.Value * 0.1);
        }

        private void trbEqReleace_ValueChanged(object sender, EventArgs e) {
            lblEqReleace.Text = trbToText(trbEqReleace);
        }

        private void chkEqAttack_CheckedChanged(object sender, EventArgs e) {
            if (chkEqAttack.Checked) {
                trbEqAttack.Enabled = true;
                lblEqAttack.Text = trbToText(trbEqAttack);
            }
            else {
                trbEqAttack.Enabled = false;
                lblEqAttack.Text = "----";
            }
        }

        private void chkEqHold_CheckedChanged(object sender, EventArgs e) {
            if (chkEqHold.Checked) {
                trbEqHold.Enabled = true;
                lblEqHold.Text = trbToText(trbEqHold);
            }
            else {
                trbEqHold.Enabled = false;
                lblEqHold.Text = "----";
            }
        }

        private void chkEqDecay_CheckedChanged(object sender, EventArgs e) {
            if (chkEqDecay.Checked) {
                trbEqDecay.Enabled = true;
                lblEqDecay.Text = trbToText(trbEqDecay);
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
                lblEqReleace.Text = trbToText(trbEqReleace);
            }
            else {
                trbEqReleace.Enabled = false;
                lblEqReleace.Text = "----";
            }
        }
        #endregion

        private string trbToText(TrackBar trb) {
            var v = trbToValue(trb);

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

        private double trbToValue(TrackBar trb) {
            return (Math.Pow(TimeGamma, (double)trb.Value / trb.Maximum) - 1.0) / (TimeGamma - 1.0) * 40.0;
        }

        private int valueToTrb(double hsb, TrackBar trb) {
            return (int)(Math.Log(hsb / 40.0 * (TimeGamma - 1) + 1.0, TimeGamma) * trb.Maximum);
        }

        private void setValue(TrackBar trb, CheckBox chk, Label lbl, double value) {
            if (value <= 0) {
                trb.Value = 1;
                trb.Enabled = false;
                chk.Checked = false;
                lbl.Text = "----";
            }
            else if (39 < value) {
                trb.Value = valueToTrb(39, trb);
                trb.Enabled = true;
                chk.Checked = true;
            }
            else {
                trb.Value = valueToTrb(value, trb);
                trb.Enabled = true;
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

            if (null != mLart) {
                foreach (var art in mLart.ToArray()) {
                    switch (art.Type) {
                    case ART_TYPE.EG_AMP_ATTACK:
                        ampAttack = art.Value;
                        break;
                    case ART_TYPE.EG_AMP_HOLD:
                        ampHold = art.Value;
                        break;
                    case ART_TYPE.EG_AMP_DECAY:
                        ampDecay = art.Value;
                        break;
                    case ART_TYPE.EG_AMP_SUSTAIN:
                        ampSustain = art.Value;
                        break;
                    case ART_TYPE.EG_AMP_RELEASE:
                        ampReleace = art.Value;
                        break;

                    case ART_TYPE.EG_CUTOFF_ATTACK:
                        eqAttack = art.Value;
                        break;
                    case ART_TYPE.EG_CUTOFF_HOLD:
                        eqHold = art.Value;
                        break;
                    case ART_TYPE.EG_CUTOFF_DECAY:
                        eqDecay = art.Value;
                        break;
                    case ART_TYPE.EG_CUTOFF_SUSTAIN:
                        eqSustain = art.Value;
                        break;
                    case ART_TYPE.EG_CUTOFF_RELEASE:
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