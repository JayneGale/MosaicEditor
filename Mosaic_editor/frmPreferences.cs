using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mosaic_editor.Classes.Constants;

namespace Mosaic_editor
{
    public partial class frmPreferences : Form
    {
        List<String> puzzleFolders = new List<string>();

        const string DEFAULT_ENGINE_FOLDER = @"D:\Data\Code\vue\mosaic\client\src\js";      // for Brian's machine

        public frmPreferences()
        {
            InitializeComponent();
            //var bindingSource1 = new BindingSource();
            //bindingSource1.DataSource = puzzleFolders;
        }

        private void frmPreferences_Load(object sender, EventArgs e)
        {
            var folders = Properties.Settings.Default.PuzzleFolders;
            if (folders == null || folders.Count == 0)
            {
                var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var defaultFolder = Path.Combine(myDocuments, "mosaic");
                puzzleFolders.Add(defaultFolder);
            }
            else
            {
                foreach(var f in folders)
                {
                    puzzleFolders.Add(f);
                }
            }
            refreshCombo();

            var engineFolder = Properties.Settings.Default.PuzzleEngineFolder;
            if (String.IsNullOrWhiteSpace(engineFolder))
            {
                engineFolder = DEFAULT_ENGINE_FOLDER;
            }
            txtPuzzleEngineFolder.Text = engineFolder;

            chkShowCoords.Checked = Properties.Settings.Default.ShowTileCoordinates;
        }

        private void refreshCombo()
        {
            cmbPuzzlesFolder.SuspendLayout();
            cmbPuzzlesFolder.Items.Clear();
            foreach (var f in puzzleFolders)
            {
                cmbPuzzlesFolder.Items.Add(f);
            }
            cmbPuzzlesFolder.SelectedIndex = 0;
            cmbPuzzlesFolder.ResumeLayout();
        }

        private void btnBrowsePuzzleFolders_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            if (puzzleFolders.Count > 0)
            {
                dlg.SelectedPath = puzzleFolders[0];
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                puzzleFolders = puzzleFolders.Where(pf => pf != dlg.SelectedPath).ToList();             // to prevent duplicates
                puzzleFolders.Insert(0, dlg.SelectedPath);
                refreshCombo();
            }
        }

        private void btnBrowseEngineFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            var current = Properties.Settings.Default.PuzzleEngineFolder;
            if (!String.IsNullOrWhiteSpace(current)) { 
                dlg.SelectedPath = current;
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPuzzleEngineFolder.Text = dlg.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PuzzleFolders = new System.Collections.Specialized.StringCollection();
            foreach(var f in puzzleFolders)
            {
                Properties.Settings.Default.PuzzleFolders.Add(f);
            }

            var engineFolder = txtPuzzleEngineFolder.Text.Trim();
            if (!String.IsNullOrWhiteSpace(engineFolder))
            {
                Properties.Settings.Default.PuzzleEngineFolder = engineFolder;
            }

            Properties.Settings.Default.ShowTileCoordinates = chkShowCoords.Checked;

            Properties.Settings.Default.Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbPuzzlesFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPuzzlesFolder.SelectedIndex > 0)
            {
                // Move the selection to the top
                var temp = (string)cmbPuzzlesFolder.SelectedItem;
                puzzleFolders.RemoveAt(cmbPuzzlesFolder.SelectedIndex);
                puzzleFolders.Insert(0, temp);
                refreshCombo();
            }
        }
    }
}
