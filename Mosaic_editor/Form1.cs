using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mosaic_editor.Classes;

namespace Mosaic_editor
{
    public partial class Form1 : Form
    {
        Puzzle puzzle;
        ColourPalette palette;
        ColourPicker picker;

        const int MIN_GRID_SPACING = 10;
        const int MAX_GRID_SPACING = 500;
        const int DELTA_GRID_SPACING = 10;

        internal string filename { 
            get { return _filename; } 
            set {
                _filename = value;
                var formTitle = "Jayne's Mosaic Editor";
                if (_filename.Length > 0)
                {
                    formTitle += " - " + _filename;
                }
                this.Text = formTitle;
            }
        }
        internal string _filename;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MouseWheel += Form1_MouseWheel;
            palette = new ColourPalette(10);
            picker = new ColourPicker(toolStrip1, palette);
            picker.onPaletteChanged += Picker_onPaletteChanged;
            // Note: puzzle does not resize / redraw if the user resizes the window.
            puzzle = new Puzzle(Constants.DEFAULT_GRID_SPACING, palette);
            puzzle.resizeToFit(pictureBox1);
            refresh();
            loadFileList();
        }

        private void refresh()
        {
            pictureBox1.Invalidate();
        }

        private void loadFileList()
        {
            var puzzleFolders = Properties.Settings.Default.PuzzleFolders;
            if (puzzleFolders  == null || puzzleFolders.Count == 0 || !Directory.Exists(puzzleFolders[0])) return;
            string baseFolder = puzzleFolders[0];
            tvFiles.Nodes.Clear();
            var root = new TreeNode
            {
                Text = baseFolder,
                Tag = "root"
            };
            tvFiles.Nodes.Add(root);
            root.Nodes.AddRange(Directory.GetDirectories(baseFolder).Select(f =>
                new TreeNode
                {
                    Text = Path.GetFileName(f),
                    Tag = "folder=" + f,
                }
            ).ToArray());
            foreach (TreeNode folder in root.Nodes)
            {
                folder.Nodes.AddRange(Directory.GetFiles(folder.FullPath, "*.json").Select(f =>
                new TreeNode
                    {
                        Text = Path.GetFileName(f),
                        Tag = "file=" + f
                    }
                ).ToArray());
            }
            root.ExpandAll();
        }

        private void Picker_onPaletteChanged(EventArgs e)
        {
            puzzle.isDirty = true;
            refresh();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.Print($"You clicked on {e.X}, {e.Y} ({e.Button})");

            // What triangle was that?
            Triangle selectedTriangle = puzzle.checkForClick(e.Location);
            if (selectedTriangle != null)
            {
                // We've found it!
                if (e.Button == MouseButtons.Left)
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        selectedTriangle.parent.clear();
                        puzzle.isDirty = true;
                    }
                    else if (Control.ModifierKeys == Keys.Control)
                    {
                        picker.currentColourIndex = selectedTriangle.colorNo;
                    }
                    else
                    {
                        Console.WriteLine($"Changing to color {picker.currentColourIndex}");
                        // Toggle colour
                        if (selectedTriangle.colorNo == picker.currentColourIndex)
                        {
                            selectedTriangle.colorNo = 0;  // Constants.BLANK_COLOR;
                            puzzle.isDirty = true;
                        }
                        else
                        {
                            // selectedTriangle.isActive = true;
                            selectedTriangle.colorNo = picker.currentColourIndex;
                            puzzle.isDirty = true;
                        }
                        selectedTriangle.isActive = true;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // Just select; don't change the colour
                }
                Console.WriteLine($"Before refresh: {puzzle.HexagonList.Count(h => h.isActive)} active hex");
                puzzle.refresh();
                Console.WriteLine($"After refresh: {puzzle.HexagonList.Count(h => h.isActive)} active hex");

                refresh();   // force a redraw
            }

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // All drawing on pictureBox1 is done within this paint event handler

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            puzzle.draw(e.Graphics);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset this puzzle?", "Confirm Clear", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                puzzle.clear();
                if (puzzle.selectedHexagon != null)
                {
                    puzzle.selectedHexagon.isSelected = false;
                    puzzle.selectedHexagon = null;
                }
                refresh();
                filename = "";
            }
        }

        private void fixedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (puzzle.selectedHexagon != null)
            {
                puzzle.selectedHexagon.isFixed = !puzzle.selectedHexagon.isFixed;
                puzzle.isDirty = true;
                refresh();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (puzzle.IsEmpty)
            {
                MessageBox.Show("This puzzle has no pieces yet!", "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var dlg = new frmSave(puzzle);
                dlg.ShowDialog();
                if (dlg.filename != null)
                {
                    filename = dlg.filename;
                    loadFileList();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadPuzzle();
        }

        private void loadPuzzle(string filename = "")
        {
            var newPuzzle = Puzzle.load(filename);
            if (newPuzzle != null)
            {
                puzzle = newPuzzle;
                picker.reloadColourPicker(puzzle.colors);
                // picker.onPaletteChanged += Picker_onPaletteChanged;
                puzzle.resizeToFit(pictureBox1); //  pictureBox1.Width, pictureBox1.Height);
                refresh();
                puzzle.isDirty = false;
                this.filename = filename;
            }
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "JPEG files|*.jpg";
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                Debug.Print($"Loading image {dlg.FileName}");
                if (File.Exists(dlg.FileName))
                {
                    // Load the image
                    using (var image = Bitmap.FromFile(dlg.FileName) as Bitmap)
                    {
                        // Process this image
                        puzzle = PhotoInput.LoadImage(image);
                        image.Dispose();
                    }
                }
            }
        }

        private void resizePuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new frmResizePuzzle();
            dlg.GridSize = puzzle.gridSpacing;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // puzzle = new Puzzle(pictureBox1.Width, pictureBox1.Height, dlg.GridSize);
                // puzzle.recentre(pictureBox1.Width, pictureBox1.Height, dlg.GridSize);
                puzzle.gridSpacing = Math.Max(10, Math.Max(dlg.GridSize, 500));
                puzzle.resizeToFit(pictureBox1);
                refresh();
            }
        }

        //private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Save as is not implemented", "Save As", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearToolStripMenuItem_Click(sender, e);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            if (puzzle == null) return;
            puzzle.resizeToFit(pictureBox1);
            // recenterPuzzle();
            refresh();
        }

        //private void recenterPuzzle()
        //{
        //    if (puzzle == null) return;
        //    puzzle.recentre(); //  pictureBox1.Width, pictureBox1.Height);
        //    pictureBox1.Invalidate();
        //}

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // This handles keyboard shortcuts
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.O:        // Control-O
                        openToolStripMenuItem_Click(this, e);
                        break;
                    case Keys.S:        // Control-S
                        saveToolStripMenuItem_Click(this, e);
                        break;
                    case Keys.N:
                        newToolStripMenuItem_Click(this, e);
                        break;
                    case Keys.X:
                        exitToolStripMenuItem_Click(this, e);
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.C:    // set as centre tile [0,0]
                        puzzle.centreSelectedTile();
                        puzzle.isDirty = true;
                        refresh();
                        break;

                    case Keys.D:
                        puzzle.deleteSelectedTile();
                        puzzle.isDirty = true;
                        refresh();
                        break;

                    case Keys.F:    // toggle "fixed"
                        fixedToolStripMenuItem_Click(this, e);
                        puzzle.isDirty = true;
                        break;

                    case Keys.F1:   // Help
                        helpToolStripMenuItem_Click(this, e);
                        break;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"Form1_MouseWheel: {e.Delta}");
            if (e.Delta < -100 & puzzle.gridSpacing > MIN_GRID_SPACING)
            {
                puzzle.gridSpacing = Math.Max(puzzle.gridSpacing - DELTA_GRID_SPACING, MIN_GRID_SPACING);
                puzzle.resizeToFit(pictureBox1);
                refresh();
            }
            else if (e.Delta > 100 & puzzle.gridSpacing < MAX_GRID_SPACING)
            {
                puzzle.gridSpacing = Math.Min(puzzle.gridSpacing + DELTA_GRID_SPACING, MAX_GRID_SPACING);
                puzzle.resizeToFit(pictureBox1);
                refresh();
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            puzzle.dumpList("debug:", puzzle.HexagonList.Where(h => h.isActive));
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new frmHelp();
            f.ShowDialog();
        }

        private void saveToMosaicEngineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var source = Path.Combine(myDocuments, "mosaic", Constants.PUZZLE_SET_FILE);
            //var dest = Path.Combine(@"D:\Data\Code\vue\mosaic\client\src\js", Constants.PUZZLE_SET_FILE);

            var puzzleFolders = Properties.Settings.Default.PuzzleFolders;
            if (puzzleFolders.Count == 0)
            {
                MessageBox.Show("No puzzle folder has been set.  Please go to File | Preferences and fix this.", "Mosaic Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var sourceFolder = puzzleFolders[0];
            var source = Path.Combine(sourceFolder, Constants.PUZZLE_SET_FILE);
            if (!File.Exists(source))
            {
                MessageBox.Show($"Cannot find puzzle set \"{source}\"!", "Mosaic Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var destFolder = Properties.Settings.Default.PuzzleEngineFolder;
            if (!Directory.Exists(destFolder))
            {
                MessageBox.Show("No puzzle engine folder has been set.  Please go to File | Preferences and fix this.", "Mosaic Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dest = Path.Combine(destFolder, Constants.PUZZLE_SET_FILE);

            if (MessageBox.Show($@"Copy the current puzzle set to the Mosaic Engine folder?

Copy from {source}.

Copy to {dest}.", "Confirm copy", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    File.Copy(source, dest, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new frmPreferences();
            dlg.ShowDialog();
        }

        private void tvFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvFiles.SelectedNode == null) return;
            var filename = tvFiles.SelectedNode.FullPath;

            if (filename.EndsWith(".json"))
            {
                Console.WriteLine($"User selected file {filename}");
                if (puzzle.isDirty)
                {
                    switch (MessageBox.Show("Save changes to the current puzzle first?", "Mosaic Editor", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.Yes:
                            puzzle.save();
                            break;
                        case DialogResult.No:
                            // carry on...
                            break;
                    }
                }
                loadPuzzle(filename);
                loadFileList();
            }
        }

        private void makeThisTheCentreTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            puzzle.centreSelectedTile();
            puzzle.isDirty = true;
        }

        #region TreeView file drag-drop support

        private void tvFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var item = (TreeNode)e.Item;
            if (((String)item.Tag).StartsWith("file="))
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void tvFiles_DragEnter(object sender, DragEventArgs e)
        {
            // e.Effect = DragDropEffects.Move;
        }


        private void tvFiles_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                var tv = (TreeView)sender;
                Point pt = tv.PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = tv.GetNodeAt(pt);
                if (destinationNode != null)
                {
                    var tag = (String)destinationNode.Tag;
                    if (tag.StartsWith("folder=") || tag.StartsWith("file="))
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void tvFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                var tv = (TreeView)sender;
                Point pt = tv.PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = tv.GetNodeAt(pt);
                if (destinationNode != null)
                {
                    var tag = (String)destinationNode.Tag;
                    if (tag.StartsWith("file="))
                    {
                        destinationNode = destinationNode.Parent;
                    }
                    tag = (String)destinationNode.Tag;
                    if (tag.StartsWith("folder="))
                    {
                        TreeNode draggedItem = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                        movePuzzleFile((String)draggedItem.Tag, tag);
                        destinationNode.Nodes.Add((TreeNode)draggedItem.Clone());
                        destinationNode.Expand();
                        draggedItem.Remove();   // remove original node
                        loadFileList();
                    }
                }
            }
        }

        private void movePuzzleFile(string sourceFile, string destFolder)
        {
            sourceFile = sourceFile.Replace("file=", "");
            destFolder = destFolder.Replace("folder=", "");

            Console.WriteLine($"movePuzzleFile {destFolder} => {sourceFile}");
            try
            {
                var destFile = Path.Combine(destFolder, Path.GetFileName(sourceFile));
                File.Move(sourceFile, destFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        #endregion
    }
}
