using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using DLS;
using InstPack;

namespace InstrumentEditor {
    public partial class MainForm : Form {
        private string mFilePath;
        private Pack mPack = new Pack();
        private Preset mClipboardPreset;
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
            mPack = new Pack();
            DispPresetList();
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
                mPack = new SF2.File(filePath).ToPack();
                break;
            case ".dls":
                mPack = new DLS.File(filePath).ToPack();
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
        }

        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "DLSファイル(*.ins)|*.dls";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.ShowDialog();
            var filePath = saveFileDialog1.FileName;
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
                return;
            }

            switch (Path.GetExtension(filePath)) {
            case ".dls":
                DLS.File.SaveFromPack(filePath, mPack);
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
            var fm = new WaveInfoForm(mPack, idx);
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
                var wave = mPack.Wave.List[int.Parse(cols[0])];
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
                mPack.Wave.List.Add(new WAVE(filePath));
            }

            DispWaveList();
        }

        private void DeleteWave() {
            var idxs = new List<uint>();
            foreach (var item in lstWave.SelectedItems) {
                idxs.Add(uint.Parse(((string)item).Split('|')[0]));
            }
            if (mPack.DeleteWave(idxs)) {
                DispWaveList();
            }
        }

        private void DispWaveList() {
            var idx = lstWave.SelectedIndex;

            lstWave.Items.Clear();
            int count = 0;
            for (uint iWave = 0; iWave < mPack.Wave.List.Count; iWave++) {
                var wave = mPack.Wave.List[(int)iWave];
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
                foreach (var inst in mPack.Inst.ToArray()) {
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
                        + (wave.Sampler.UnityNote < 12 ? "" : "+")
                        + (wave.Sampler.UnityNote / 12 - 2).ToString("00"),
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
            var fm = new AddPresetDialog(mPack);
            fm.ShowDialog();
            DispPresetList();
        }

        private void DeletePreset() {
            if (0 == lstPreset.Items.Count) {
                return;
            }
            var index = lstPreset.SelectedIndex;
            var indices = lstPreset.SelectedIndices;
            foreach (int idx in indices) {
                mPack.Preset.Remove(GetPresetLocale(idx));
            }
            DispPresetList();
            if (index < lstPreset.Items.Count) {
                lstPreset.SelectedIndex = index;
            } else {
                lstPreset.SelectedIndex = lstPreset.Items.Count - 1;
            }
        }

        private void CopyPreset() {
            var preset = GetSelectedPreset();
            if (null == preset) {
                return;
            }
            mClipboardPreset = new Preset();
            mClipboardPreset.Header = preset.Header;
            // Layer
            mClipboardPreset.Regions.Clear();
            foreach (var layer in preset.Regions.ToArray()) {
                var tmp = new Region {
                    Header = layer.Header
                };
                tmp.Articulations.AddRange(layer.Articulations.List);
                mClipboardPreset.Regions.Add(tmp);
            }
            // Articulations
            mClipboardPreset.Articulations.Clear();
            foreach (var art in preset.Articulations.List) {
                mClipboardPreset.Articulations.Add(art);
            }
            // Info
            mClipboardPreset.Info.CopyFrom(preset.Info);
        }

        private void PastePreset() {
            if (null == mClipboardPreset) {
                return;
            }
            var fm = new AddPresetDialog(mPack, mClipboardPreset);
            fm.ShowDialog();
            DispPresetList();
        }
 
        private void EditPreset() {
            var preset = GetSelectedPreset();
            if (null == preset) {
                return;
            }
            var fm = new LayerAssignForm(mPack, preset);
            fm.ShowDialog();
            DispPresetList();
        }

        private void DispPresetList() {
            var idx = lstPreset.SelectedIndex;

            lstPreset.Items.Clear();
            foreach (var preset in mPack.Preset.Values) {
                if (!string.IsNullOrEmpty(txtSearchPreset.Text)
                    && preset.Info[Info.TYPE.INAM]
                        .IndexOf(txtSearchPreset.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }

                lstPreset.Items.Add(string.Format(
                    "{0}|{1}|{2}|{3}|{4}|{5}",
                    preset.Header.IsDrum ? "Drum" : "Note",
                    preset.Header.ProgNum.ToString("000"),
                    preset.Header.BankMSB.ToString("000"),
                    preset.Header.BankLSB.ToString("000"),
                    preset.Info[Info.TYPE.ICAT].PadRight(16, ' ').Substring(0, 16),
                    preset.Info[Info.TYPE.INAM]
                ));
            }

            if (lstPreset.Items.Count <= idx) {
                idx = lstPreset.Items.Count - 1;
            }
            lstPreset.SelectedIndex = idx;
        }

        private void MultiSelectPreset() {
            var lst = GetSelectedPresets();
            if (1 == lst.Count) {
                var fm = new PresetInfoDialog(mPack, lst[0]);
                fm.ShowDialog();
                DispPresetList();
                return;
            }
            if (1 < lst.Count) {
                var preset = new Preset();
                var fm = new PresetInfoDialog(mPack, preset, true);
                fm.ShowDialog();
                foreach (var p in lst) {
                    p.Info[Info.TYPE.ICAT] = preset.Info[Info.TYPE.ICAT];
                }
                DispPresetList();
                return;
            }
        }

        private PREH GetPresetLocale(int index) {
            if (0 == lstPreset.Items.Count) {
                return new PREH();
            }
            if (index < 0) {
                return new PREH();
            }

            var cols = lstPreset.Items[index].ToString().Split('|');

            return new PREH() {
                IsDrum = "Drum" == cols[0],
                ProgNum = byte.Parse(cols[1]),
                BankMSB = byte.Parse(cols[2]),
                BankLSB = byte.Parse(cols[3])
            };
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
                list.Add(new PREH() {
                    IsDrum = "Drum" == cols[0],
                    ProgNum = byte.Parse(cols[1]),
                    BankMSB = byte.Parse(cols[2]),
                    BankLSB = byte.Parse(cols[3])
                });
            }
            return list;
        }

        private Preset GetSelectedPreset() {
            var locale = GetPresetLocale(lstPreset.SelectedIndex);
            if (!mPack.Preset.ContainsKey(locale)) {
                return null;
            }
            return mPack.Preset[locale];
        }

        private List<Preset> GetSelectedPresets() {
            var locales = GetPresetLocales(lstPreset.SelectedIndices);
            var presets = new List<Preset>();
            foreach (var locale in locales) {
                if (mPack.Preset.ContainsKey(locale)) {
                    presets.Add(mPack.Preset[locale]);
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
            MultiSelectInst();
        }

        private void lstInst_DoubleClick(object sender, EventArgs e) {
            EditInst();
        }

        private void lstInst_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                MultiSelectInst();
                return;
            }
            if (e.KeyCode == Keys.Enter && e.Shift) {
                EditInst();
                return;
            }
        }

        private void AddInst() {
            var fm = new InstInfoDialog(mPack);
            fm.ShowDialog();
            DispInstList();
            DispPresetList();
        }

        private void DeleteInst() {
            if (0 == lstInst.Items.Count) {
                return;
            }

            var index = lstInst.SelectedIndex;
            var idxs = new List<int>();
            foreach (var item in lstInst.SelectedItems) {
                idxs.Add(int.Parse(((string)item).Split('|')[0]));
            }
            if (mPack.DeleteInst(idxs)) {
                DispInstList();
                DispPresetList();
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

            // Region
            mClipboardInst.Regions.Clear();
            foreach (var layer in inst.Regions.Array) {
                var rgn = new RGN {
                    Header = layer.Header
                };
                rgn.Articulations.AddRange(layer.Articulations.List);
                mClipboardInst.Regions.Add(rgn);
            }

            // Articulations
            mClipboardInst.Articulations.Clear();
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
            var fm = new InstInfoDialog(mPack, mClipboardInst);
            fm.ShowDialog();
            DispInstList();
        }

        private void DispInstList() {
            var idx = lstInst.SelectedIndex;

            lstInst.Items.Clear();
            for (var iInst = 0; iInst < mPack.Inst.Count; iInst++) {
                if (!string.IsNullOrEmpty(txtSearchInst.Text)
                    && mPack.Inst[iInst].Info[Info.TYPE.INAM]
                        .IndexOf(txtSearchInst.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }
                var inst = mPack.Inst[iInst];
                var use = false;
                foreach (var preset in mPack.Preset.Values) {
                    foreach (var layer in preset.Regions.ToArray()) {
                        if (iInst == layer.InstIndex) {
                            use = true;
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
            var fm = new RegionAssignForm(mPack, inst);
            fm.ShowDialog();
            DispInstList();
        }

        private void MultiSelectInst() {
            var lst = GetSelectedInsts();
            if (1 == lst.Count) {
                var fm = new InstInfoDialog(mPack, lst[0]);
                fm.ShowDialog();
                DispInstList();
                return;
            }
            if (1 < lst.Count) {
                var inst = new INS();
                var fm = new InstInfoDialog(mPack, inst);
                fm.ShowDialog();
                foreach (var p in lst) {
                    p.Info[Info.TYPE.ICAT] = inst.Info[Info.TYPE.ICAT];
                }
                DispInstList();
                return;
            }
        }

        private int GetInstLocale(int index) {
            if (0 == lstInst.Items.Count) {
                return -1;
            }
            if (index < 0) {
                return -1;
            }
            var cols = lstInst.Items[index].ToString().Split('|');
            return int.Parse(cols[0]);
        }

        private List<int> GetInstLocales(ListBox.SelectedIndexCollection indeces) {
            var list = new List<int>();
            if (0 == lstInst.Items.Count) {
                return list;
            }
            if (indeces.Count < 0) {
                return list;
            }
            foreach (int index in indeces) {
                var cols = lstInst.Items[index].ToString().Split('|');
                list.Add(int.Parse(cols[0]));
            }
            return list;
        }

        private INS GetSelectedInst() {
            var locale = GetInstLocale(lstInst.SelectedIndex);
            if (mPack.Inst.Count <= locale) {
                return null;
            }
            return mPack.Inst[locale];
        }

        private List<INS> GetSelectedInsts() {
            var locales = GetInstLocales(lstInst.SelectedIndices);
            var insts = new List<INS>();
            foreach (var locale in locales) {
                if (locale < mPack.Inst.Count) {
                    insts.Add(mPack.Inst[locale]);
                }
            }
            return insts;
        }
        #endregion
    }
}