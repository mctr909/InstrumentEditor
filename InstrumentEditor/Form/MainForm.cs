﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class MainForm : Form {
        private string mFilePath;
        private DLS.File mFile = new DLS.File();
        private INS mClipboardInst;

        public MainForm() {
            InitializeComponent();
            SetTabSize();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            SetTabSize();
        }

        #region メニューバー[ファイル]
        private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e) {
            mFile = new DLS.File();
            DispInstList();
            DispWaveList();
            tabControl.SelectedIndex = 0;
            mFilePath = "";
        }

        private void 開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "SF2ファイル(*.sf2)|*.sf2|DLSファイル(*.dls)|*.dls";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.ShowDialog();
            var filePath = openFileDialog1.FileName;
            if (!System.IO.File.Exists(filePath)) {
                return;
            }

            switch(Path.GetExtension(filePath)) {
            case ".sf2":
                mFile = new SF2.File(filePath).ToPack();
                break;
            case ".dls":
                mFile = new DLS.File(filePath);
                break;
            }

            txtSearchWave.Text = "";

            DispWaveList();
            DispInstList();

            tabControl.SelectedIndex = 0;
            mFilePath = filePath;
        }

        private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(mFilePath) || !System.IO.File.Exists(mFilePath)) {
                名前を付けて保存ToolStripMenuItem_Click(sender, e);
            }
        }

        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "DLSファイル(*.ins)|*.dls";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.ShowDialog();
            var filePath = saveFileDialog1.FileName;
            if (string.IsNullOrWhiteSpace(filePath) || !Directory.Exists(Path.GetDirectoryName(filePath))) {
                return;
            }

            switch (Path.GetExtension(filePath)) {
            case ".dls":
                mFile.Save(filePath);
                break;
            }

            mFilePath = filePath;
        }
        #endregion

        #region メニューバー[編集]
        private void 追加AToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    AddWave();
                    break;
            }
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    DeleteWave();
                    break;
            }
        }

        private void コピーCToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    break;
            }
        }

        private void 貼り付けPToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    break;
            }
        }
        #endregion

        #region ツールストリップ[波形]
        private void tsbAddWave_Click(object sender, EventArgs e) {
            AddWave();
        }

        private void tsbDeleteWave_Click(object sender, EventArgs e) {
            DeleteWave();
        }

        private void tsbOutputWave_Click(object sender, EventArgs e) {
            WaveFileOut();
        }

        private void txtSearchWave_Leave(object sender, EventArgs e) {
            DispWaveList();
        }

        private void txtSearchWave_TextChanged(object sender, EventArgs e) {
            DispWaveList();
        }
        #endregion

        #region ツールストリップ[音色]
        private void tsbAddInst_Click(object sender, EventArgs e) {
            AddInst();
        }

        private void tsbDeleteInst_Click(object sender, EventArgs e) {
            DeleteInst();
        }

        private void tsbCopyInst_Click(object sender, EventArgs e) {
            CopyInst();
        }

        private void tsbPasteInst_Click(object sender, EventArgs e) {
            PasteInst();
        }

        private void txtSearchInst_Leave(object sender, EventArgs e) {
            DispInstList();
        }

        private void txtSearchInst_TextChanged(object sender, EventArgs e) {
            DispInstList();
        }
        #endregion

        #region サイズ調整
        private void SetTabSize() {
            var offsetX = 28;
            var offsetY = 70;
            var width = Width - offsetX;
            var height = Height - offsetY;

            if (width < 100) {
                return;
            }

            if (height < 100) {
                return;
            }

            tabControl.Width = width;
            tabControl.Height = height;

            SetInstListSize();
            SetWaveListSize();
        }

        private void SetInstListSize() {
            var offsetX = 16;
            var offsetY = 60;
            var width = tabControl.Width - offsetX + 6;
            var height = tabControl.Height - offsetY - 4;

            lstInst.Left = 0;
            lstInst.Top = toolStrip2.Height + 4;
            lstInst.Width = width;
            lstInst.Height = height;
        }

        private void SetWaveListSize() {
            var offsetX = 16;
            var offsetY = 60;
            var width = tabControl.Width - offsetX + 6;
            var height = tabControl.Height - offsetY - 4;

            lstWave.Left = 0;
            lstWave.Top = toolStrip3.Height + 4;
            lstWave.Width = width;
            lstWave.Height = height;
        }
        #endregion

        #region 波形一覧
        private void lstWave_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) {
                return;
            }
            var indexes = lstWave.SelectedIndices;
            var waves = new List<WAVE>();
            foreach (int index in indexes) {
                var cols = ((string)lstWave.Items[index]).Split('|');
                waves.Add(mFile.Wave[int.Parse(cols[0])]);
            }
            if (0 == waves.Count) {
                return;
            }
            var wave = new WAVE();
            var fm = new GroupAssignDialog(mFile, wave);
            fm.ShowDialog();
            foreach (var p in waves) {
                p.Info[Info.TYPE.ICAT] = wave.Info[Info.TYPE.ICAT];
            }
            DispWaveList();
        }

        private void lstWave_DoubleClick(object sender, EventArgs e) {
            if (0 == lstWave.Items.Count) {
                return;
            }
            var cols = lstWave.SelectedItem.ToString().Split('|');
            var idx = int.Parse(cols[0]);
            var fm = new WaveInfoForm(mFile, idx);
            var index = lstWave.SelectedIndex;
            fm.ShowDialog();
            DispWaveList();
            lstWave.SelectedIndex = index;
        }

        private void WaveFileOut() {
            folderBrowserDialog1.ShowDialog();
            var folderPath = folderBrowserDialog1.SelectedPath;
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) {
                return;
            }

            var indices = lstWave.SelectedIndices;
            foreach (var idx in indices) {
                var cols = lstWave.Items[(int)idx].ToString().Split('|');
                var wave = mFile.Wave[int.Parse(cols[0])];
                if (string.IsNullOrWhiteSpace(wave.Info[Info.TYPE.INAM])) {
                    wave.ToFile(Path.Combine(folderPath, string.Format("Wave{0}.wav", idx)));
                }
                else {
                    wave.ToFile(Path.Combine(folderPath, wave.Info[Info.TYPE.INAM] + ".wav"));
                }
            }
        }

        private void AddWave() {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "wavファイル(*.wav)|*.wav";
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            var filePaths = openFileDialog1.FileNames;

            foreach (var filePath in filePaths) {
                if (!System.IO.File.Exists(filePath)) {
                    continue;
                }
                mFile.Wave.Add(new WAVE(filePath));
            }

            DispWaveList();
        }

        private void DeleteWave() {
            var idxs = new List<uint>();
            foreach (var item in lstWave.SelectedItems) {
                idxs.Add(uint.Parse(((string)item).Split('|')[0]));
            }
            if (mFile.RemoveWaves(idxs)) {
                DispWaveList();
            }
        }

        private void DispWaveList() {
            var idx = lstWave.SelectedIndex;

            lstWave.Items.Clear();
            int count = 0;
            for (uint iWave = 0; iWave < mFile.Wave.Count; iWave++) {
                var wave = mFile.Wave[(int)iWave];
                string name;
                if (string.IsNullOrWhiteSpace(wave.Info[Info.TYPE.INAM])) {
                    name = string.Format("Wave[{0}]", count);
                } else {
                    name = wave.Info[Info.TYPE.INAM];
                }

                if (!string.IsNullOrEmpty(txtSearchWave.Text)
                    && name.IndexOf(txtSearchWave.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }

                var use = false;
                foreach (var inst in mFile.Inst.List.Values) {
                    foreach (var rgn in inst.Regions.Array) {
                        if (iWave == rgn.WaveLink.TableIndex) {
                            use = true;
                            break;
                        }
                    }
                    if (use) {
                        break;
                    }
                }

                lstWave.Items.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    iWave.ToString("0000"),
                    (use ? "use" : "   "),
                    (0 < wave.Loops.Count ? "loop" : "    "),
                    Const.NoteName[wave.Sampler.UnityNote % 12]
                        + (wave.Sampler.UnityNote / 12 - 2).ToString().PadLeft(2, ' '),
                    wave.Info[Info.TYPE.ICAT].PadRight(16, ' ').Substring(0, 16),
                    name
                ));
                ++count;
            }

            if (lstWave.Items.Count <= idx) {
                idx = lstWave.Items.Count - 1;
            }
            lstWave.SelectedIndex = idx;
        }
        #endregion

        #region 音色一覧
        private void lstInst_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) {
                return;
            }
            SelectInst();
        }

        private void lstInst_DoubleClick(object sender, EventArgs e) {
            EditInst();
        }

        private void lstInst_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                SelectInst();
                return;
            }
            if (e.KeyCode == Keys.Enter && e.Shift) {
                EditInst();
                return;
            }
        }

        private void AddInst() {
            var fm = new InstInfoDialog(mFile);
            fm.ShowDialog();
            DispInstList();
        }

        private void DeleteInst() {
            if (0 == lstInst.Items.Count) {
                return;
            }

            var index = lstInst.SelectedIndex;
            var deleteList = new List<MidiLocale>();
            foreach (var item in lstInst.SelectedItems) {
                var line = (string)item;
                var cols = line.Split('|');
                deleteList.Add(new MidiLocale() {
                    BankFlg = (byte)(cols[0] == "Drum" ? 0x80 : 0x00),
                    ProgNum = byte.Parse(cols[1]),
                    BankMSB = byte.Parse(cols[2]),
                    BankLSB = byte.Parse(cols[3])
                });
            }
            if (mFile.Inst.RemoveRange(deleteList)) {
                DispInstList();
                DispWaveList();
                if (index < lstInst.Items.Count) {
                    lstInst.SelectedIndex = index;
                } else {
                    lstInst.SelectedIndex = lstInst.Items.Count - 1;
                }
            }
        }

        private void CopyInst() {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            mClipboardInst = new INS();
            mClipboardInst.Locale = inst.Locale;

            // Region
            foreach (var layer in inst.Regions.Array) {
                var rgn = new RGN {
                    Header = layer.Header,
                    Layer = layer.Layer,
                    WaveLink = layer.WaveLink,
                    Sampler = layer.Sampler
                };
                rgn.Loops.AddRange(layer.Loops);
                rgn.Articulations.AddRange(layer.Articulations.List);
                mClipboardInst.Regions.Add(rgn);
            }
            // Articulations
            foreach (var art in inst.Articulations.List) {
                mClipboardInst.Articulations.Add(art);
            }
            // Info
            mClipboardInst.Info.CopyFrom(inst.Info);
        }

        private void PasteInst() {
            if (null == mClipboardInst) {
                return;
            }
            var fm = new InstInfoDialog(mFile, mClipboardInst);
            fm.ShowDialog();
            DispInstList();
        }

        private void DispInstList() {
            var idx = lstInst.SelectedIndex;

            lstInst.Items.Clear();
            foreach (var inst in mFile.Inst.List.Values) {
                if (!string.IsNullOrEmpty(txtSearchInst.Text)
                    && inst.Info[Info.TYPE.INAM]
                        .IndexOf(txtSearchInst.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }
                lstInst.Items.Add(string.Format(
                    "{0}|{1}|{2}|{3}|{4}|{5}",
                    inst.Locale.BankFlg == 0x80 ? "Drum" : "Note",
                    inst.Locale.ProgNum.ToString().PadLeft(3, '0'),
                    inst.Locale.BankMSB.ToString().PadLeft(3, '0'),
                    inst.Locale.BankLSB.ToString().PadLeft(3, '0'),
                    inst.Info[Info.TYPE.ICAT].PadRight(16, ' ').Substring(0, 16),
                    inst.Info[Info.TYPE.INAM]
                ));
            }
            if (lstInst.Items.Count <= idx) {
                idx = lstInst.Items.Count - 1;
            }
            lstInst.SelectedIndex = idx;
        }

        private void EditInst() {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }
            var fm = new RegionAssignForm(mFile, inst);
            fm.ShowDialog();
            DispInstList();
        }

        private void SelectInst() {
            var lst = GetSelectedInsts();
            if (1 == lst.Count) {
                var fm = new InstInfoDialog(mFile, lst[0]);
                fm.ShowDialog();
                DispInstList();
                return;
            }
            if (1 < lst.Count) {
                var inst = new INS();
                var fm = new GroupAssignDialog(mFile, inst);
                fm.ShowDialog();
                foreach (var p in lst) {
                    p.Info[Info.TYPE.ICAT] = inst.Info[Info.TYPE.ICAT];
                }
                DispInstList();
                return;
            }
        }

        private MidiLocale GetInstLocale(int index) {
            var locale = new MidiLocale();
            if (0 == lstInst.Items.Count) {
                return locale;
            }
            if (index < 0) {
                return locale;
            }
            var cols = lstInst.Items[index].ToString().Split('|');
            locale.BankFlg = (byte)(cols[0] == "Drum" ? 0x80 : 0x00);
            locale.ProgNum = byte.Parse(cols[1]);
            locale.BankMSB = byte.Parse(cols[2]);
            locale.BankLSB = byte.Parse(cols[3]);
            return locale;
        }

        private List<MidiLocale> GetInstLocales(ListBox.SelectedIndexCollection indeces) {
            var list = new List<MidiLocale>();
            if (0 == lstInst.Items.Count) {
                return list;
            }
            if (indeces.Count < 0) {
                return list;
            }
            foreach (int index in indeces) {
                list.Add(GetInstLocale(index));
            }
            return list;
        }

        private INS GetSelectedInst() {
            var locale = GetInstLocale(lstInst.SelectedIndex);
            if (mFile.Inst.ContainsKey(locale)) {
                return mFile.Inst[locale];
            }
            return null;
        }

        private List<INS> GetSelectedInsts() {
            var locales = GetInstLocales(lstInst.SelectedIndices);
            var insts = new List<INS>();
            foreach (var locale in locales) {
                if (mFile.Inst.ContainsKey(locale)) {
                    insts.Add(mFile.Inst[locale]);
                }
            }
            return insts;
        }
        #endregion
    }
}