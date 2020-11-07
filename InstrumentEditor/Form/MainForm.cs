﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

using Instruments;
using SF2;

namespace InstrumentEditor {
    public partial class MainForm : Form {
        private string mFilePath;
        private Instruments.File mFile = new Instruments.File();
        private Instruments.Preset mClipboardPreset;
        private Instruments.Inst mClipboardInst;

        public MainForm() {
            InitializeComponent();
            SetTabSize();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            SetTabSize();
        }

        #region メニューバー[ファイル]
        private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e) {
            mFile = new Instruments.File();
            DispPresetList();
            DispInstList();
            DispWaveList();
            tabControl.SelectedIndex = 0;
            mFilePath = "";
        }

        private void 開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "SF2ファイル(*.sf2)|*.sf2|DLSファイル(*.dls)|*.dls|INSファイル(*.ins)|*.ins";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.ShowDialog();
            var filePath = openFileDialog1.FileName;
            if (!System.IO.File.Exists(filePath)) {
                return;
            }

            switch(Path.GetExtension(filePath)) {
            case ".sf2":
                mFile = new SF2.File(filePath).ToIns();
                break;
            case ".dls":
                mFile = new DLS.File(filePath).ToIns();
                break;
            case ".ins":
                mFile = new Instruments.File(filePath);
                break;
            }

            txtSearchPreset.Text = "";
            txtSearchWave.Text = "";

            DispWaveList();
            DispInstList();
            DispPresetList();

            if(0 < lstPreset.Items.Count) {
                lstPreset.SelectedIndex = 0;
            }

            tabControl.SelectedIndex = 0;
            mFilePath = filePath;
        }

        private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(mFilePath) || !System.IO.File.Exists(mFilePath)) {
                名前を付けて保存ToolStripMenuItem_Click(sender, e);
            }
            mFile.Save(mFilePath);
        }

        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "INSファイル(*.ins)|*.ins";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.ShowDialog();
            var filePath = saveFileDialog1.FileName;
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
                return;
            }

            switch (Path.GetExtension(filePath)) {
            case ".ins":
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
                case "tbpPresetList":
                    AddPreset();
                    break;
            }
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    DeleteWave();
                    break;
                case "tbpPresetList":
                    DeletePreset();
                    break;
            }
        }

        private void コピーCToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    break;
                case "tbpPresetList":
                    CopyPreset();
                    break;
            }
        }

        private void 貼り付けPToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    break;
                case "tbpPresetList":
                    PastePreset();
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

        #region ツールストリップ[プリセット]
        private void tsbAddPreset_Click(object sender, EventArgs e) {
            AddPreset();
        }

        private void tsbDeletePreset_Click(object sender, EventArgs e) {
            DeletePreset();
        }

        private void tsbCopyPreset_Click(object sender, EventArgs e) {
            CopyPreset();
        }

        private void tsbPastePreset_Click(object sender, EventArgs e) {
            PastePreset();
        }

        private void txtSearchPreset_Leave(object sender, EventArgs e) {
            DispPresetList();
        }

        private void txtSearchPreset_TextChanged(object sender, EventArgs e) {
            DispPresetList();
        }
        #endregion

        #region ツールストリップ[音色]
        private void tsbAddInst_Click(object sender, EventArgs e) {

        }

        private void tsbDeleteInst_Click(object sender, EventArgs e) {

        }

        private void tsbCopyInst_Click(object sender, EventArgs e) {

        }

        private void tsbPasteInst_Click(object sender, EventArgs e) {

        }

        private void txtSearchInst_Leave(object sender, EventArgs e) {

        }

        private void txtSearchInst_TextChanged(object sender, EventArgs e) {

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

            SetPresetListSize();
            SetInstListSize();
            SetWaveListSize();
        }

        private void SetPresetListSize() {
            var offsetX = 16;
            var offsetY = 60;
            var width = tabControl.Width - offsetX + 6;
            var height = tabControl.Height - offsetY - 4;

            lstPreset.Left = 0;
            lstPreset.Top = toolStrip2.Height + 4;
            lstPreset.Width = width;
            lstPreset.Height = height;
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
                if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
                    wave.ToFile(Path.Combine(folderPath, string.Format("Wave{0}.wav", idx)));
                }
                else {
                    wave.ToFile(Path.Combine(folderPath, wave.Info.Name + ".wav"));
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
                mFile.Wave.Add(new Wave(filePath));
            }

            DispWaveList();
        }

        private void DeleteWave() {
            var idxs = new List<int>();
            foreach (var item in lstWave.SelectedItems) {
                idxs.Add(int.Parse(((string)item).Split('|')[0]));
            }
            mFile.DeleteWave(idxs);
            DispWaveList();
        }

        private void DispWaveList() {
            var idx = lstWave.SelectedIndex;

            lstWave.Items.Clear();
            int count = 0;
            for (var iWave = 0; iWave < mFile.Wave.Count; iWave++) {
                var wave = mFile.Wave[iWave];
                var name = "";
                if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
                    name = string.Format("Wave[{0}]", count);
                } else {
                    name = wave.Info.Name;
                }

                if (!string.IsNullOrEmpty(txtSearchWave.Text)
                    && name.IndexOf(txtSearchWave.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }

                var use = false;
                foreach (var inst in mFile.Inst.Array) {
                    foreach (var rgn in inst.Region.Array) {
                        foreach (var art in rgn.Art.Array) {
                            if (art.Type != ART_TYPE.WAVE_INDEX) {
                                continue;
                            }
                            if (iWave == (int)art.Value) {
                                use = true;
                                break;
                            }
                        }
                        if (use) {
                            break;
                        }
                    }
                    if (use) {
                        break;
                    }
                }

                lstWave.Items.Add(string.Format(
                    "{0}|{1}|{2}|{3}|{4}",
                    iWave.ToString("0000"),
                    (use ? "use" : "   "),
                    (0 < wave.Header.LoopEnable ? "loop" : "    "),
                    Wave.NoteName[wave.Header.UnityNote % 12]
                        + (wave.Header.UnityNote < 12 ? "" : "+")
                        + (wave.Header.UnityNote / 12 - 2).ToString("00"),
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

        #region プリセット一覧
        private void lstPreset_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) {
                return;
            }
            MultiSelectPreset();
        }

        private void lstPreset_DoubleClick(object sender, EventArgs e) {
            EditPreset();
        }

        private void lstPreset_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                MultiSelectPreset();
                return;
            }
            if (e.KeyCode == Keys.Enter && e.Shift) {
                EditPreset();
                return;
            }
        }

        private void AddPreset() {
            var fm = new AddPresetDialog(mFile);
            fm.ShowDialog();
            DispInstList();
            DispPresetList();
        }

        private void DeletePreset() {
            if (0 == lstPreset.Items.Count) {
                return;
            }

            var index = lstPreset.SelectedIndex;
            var indices = lstPreset.SelectedIndices;
            foreach (int idx in indices) {
                mFile.Preset.Remove(GetPresetLocale(idx));
            }

            DispInstList();
            DispPresetList();
            DispWaveList();

            if (index < lstPreset.Items.Count) {
                lstPreset.SelectedIndex = index;
            } else {
                lstPreset.SelectedIndex = lstPreset.Items.Count - 1;
            }
        }

        private void CopyPreset() {
            var inst = GetSelectedPreset();
            if (null == inst) {
                return;
            }

            mClipboardPreset = new Instruments.Preset();
            mClipboardPreset.Header = inst.Header;

            // Layer
            mClipboardPreset.Layer.Clear();
            foreach (var layer in inst.Layer.Array) {
                mClipboardPreset.Layer.Add(new Layer {
                    Header = layer.Header,
                    Art = layer.Art
                });
            }

            // Articulations
            mClipboardPreset.Art.Clear();
            foreach (var art in inst.Art.Array) {
                mClipboardPreset.Art.Add(art);
            }

            // Info
            mClipboardPreset.Info = new Riff.Info();
            mClipboardPreset.Info.Name = inst.Info.Name;
            mClipboardPreset.Info.Category = inst.Info.Category;
            mClipboardPreset.Info.Comments = inst.Info.Comments;
        }

        private void PastePreset() {
            if (null == mClipboardPreset) {
                return;
            }

            var fm = new AddPresetDialog(mFile, mClipboardPreset);
            fm.ShowDialog();

            DispPresetList();
        }
 
        private void EditPreset() {
            var preset = GetSelectedPreset();
            if (null == preset) {
                return;
            }
            var fm = new LayerAssignForm(mFile, preset);
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();
            DispPresetList();
        }

        private void MultiSelectPreset() {
            var lst = GetSelectedPresets();
            if (1 == lst.Count) {
                var fm = new PresetInfoDialog(mFile, lst[0]);
                fm.StartPosition = FormStartPosition.CenterParent;
                fm.ShowDialog();
                DispPresetList();
                return;
            }
            if (1 < lst.Count) {
                var preset = new Instruments.Preset();
                var fm = new PresetInfoDialog(mFile, preset);
                fm.StartPosition = FormStartPosition.CenterParent;
                fm.ShowDialog();
                foreach (var p in lst) {
                    p.Info.Category = preset.Info.Category;
                }
                DispPresetList();
                return;
            }
        }

        private void DispPresetList() {
            var idx = lstPreset.SelectedIndex;

            lstPreset.Items.Clear();
            foreach (var preset in mFile.Preset.Values) {
                if (!string.IsNullOrEmpty(txtSearchPreset.Text)
                    && preset.Info.Name.IndexOf(txtSearchPreset.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }

                lstPreset.Items.Add(string.Format(
                    "{0}|{1}|{2}|{3}|{4}|{5}",
                    (preset.Header.BankFlg & 1) == 1 ? "Drum" : "Note",
                    preset.Header.ProgNum.ToString("000"),
                    preset.Header.BankMSB.ToString("000"),
                    preset.Header.BankLSB.ToString("000"),
                    preset.Info.Category.PadRight(10, ' ').Substring(0, 10),
                    preset.Info.Name
                ));
            }

            if (lstPreset.Items.Count <= idx) {
                idx = lstPreset.Items.Count - 1;
            }
            lstPreset.SelectedIndex = idx;
        }

        private PREH GetPresetLocale(int index) {
            if (0 == lstPreset.Items.Count) {
                return new PREH();
            }
            if (index < 0) {
                return new PREH();
            }

            var cols = lstPreset.Items[index].ToString().Split('|');

            var locale = new PREH();
            locale.BankFlg = (byte)("Drum" == cols[0] ? 1 : 0);
            locale.ProgNum = byte.Parse(cols[1]);
            locale.BankMSB = byte.Parse(cols[2]);
            locale.BankLSB = byte.Parse(cols[3]);

            return locale;
        }

        private List<PREH> GetPresetLocales(ListBox.SelectedIndexCollection indeces) {
            var list = new List<PREH>();

            if (0 == lstPreset.Items.Count) {
                return list;
            }
            if (indeces.Count < 0) {
                return list;
            }

            foreach (int index in indeces) {
                var cols = lstPreset.Items[index].ToString().Split('|');
                var locale = new PREH();
                locale.BankFlg = (byte)("Drum" == cols[0] ? 1 : 0);
                locale.ProgNum = byte.Parse(cols[1]);
                locale.BankMSB = byte.Parse(cols[2]);
                locale.BankLSB = byte.Parse(cols[3]);
                list.Add(locale);
            }
            return list;
        }

        private Instruments.Preset GetSelectedPreset() {
            var locale = GetPresetLocale(lstPreset.SelectedIndex);
            if (!mFile.Preset.ContainsKey(locale)) {
                return null;
            }
            return mFile.Preset[locale];
        }

        private List<Instruments.Preset> GetSelectedPresets() {
            var locales = GetPresetLocales(lstPreset.SelectedIndices);
            var presets = new List<Instruments.Preset>();
            foreach (var locale in locales) {
                if (mFile.Preset.ContainsKey(locale)) {
                    presets.Add(mFile.Preset[locale]);
                }
            }
            return presets;
        }
        #endregion

        #region 音色一覧
        private void lstInst_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) {
                return;
            }
        }

        private void lstInst_DoubleClick(object sender, EventArgs e) {

        }

        private void lstInst_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                return;
            }
            if (e.KeyCode == Keys.Enter && e.Shift) {
                return;
            }
        }

        private void DispInstList() {
            var idx = lstInst.SelectedIndex;

            lstInst.Items.Clear();
            for (var iInst = 0; iInst < mFile.Inst.Count; iInst++) {
                var inst = mFile.Inst[iInst];
                var use = false;
                foreach (var ptrset in mFile.Preset.Values) {
                    foreach (var layer in ptrset.Layer.Array) {
                        foreach (var art in layer.Art.Array) {
                            if (art.Type != ART_TYPE.INST_INDEX) {
                                continue;
                            }
                            if (iInst == (int)art.Value) {
                                use = true;
                                break;
                            }
                        }
                        if(use) {
                            break;
                        }
                    }
                    if (use) {
                        break;
                    }
                }
                lstInst.Items.Add(string.Format(
                    "{0}|{1}|{2}|{3}",
                    iInst.ToString("0000"),
                    use ? "use" : "   ",
                    inst.Info.Category.PadRight(10, ' ').Substring(0, 10),
                    inst.Info.Name
                ));
            }

            if (lstInst.Items.Count <= idx) {
                idx = lstInst.Items.Count - 1;
            }
            lstInst.SelectedIndex = idx;
        }
        #endregion

    }
}