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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            palette = new ColourPalette(10);
            picker = new ColourPicker(toolStrip1, palette);
            picker.onPaletteChanged += Picker_onPaletteChanged;
            // Note: puzzle does not resize / redraw if the user resizes the window.
            puzzle = new Puzzle(pictureBox1.Width, pictureBox1.Height, Constants.DEFAULT_GRID_SPACING, palette);
        }

        private void Picker_onPaletteChanged(EventArgs e)
        {
            pictureBox1.Invalidate();
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
                    }
                    else
                    {
                        Console.WriteLine($"Changing to color {picker.currentColourIndex}");
                        // Toggle colour
                        if (selectedTriangle.colorNo == picker.currentColourIndex)
                        {
                            selectedTriangle.colorNo = 0;  // Constants.BLANK_COLOR;
                        }
                        else
                        {
                            // selectedTriangle.isActive = true;
                            selectedTriangle.colorNo = picker.currentColourIndex;
                        }
                        selectedTriangle.isActive = true;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // Just select; don't change the colour
                }
                puzzle.refresh();
                pictureBox1.Invalidate();   // force a redraw
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
                pictureBox1.Invalidate();
            }
        }

        private void fixedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (puzzle.selectedHexagon != null)
            {
                puzzle.selectedHexagon.isFixed = !puzzle.selectedHexagon.isFixed;
                pictureBox1.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            puzzle.save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            puzzle = Puzzle.load(); //  palette);
            picker.reloadPalette();
            puzzle.recentre(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
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
            dlg.GridSize = puzzle.GridSpacing;
            if (dlg.ShowDialog() == DialogResult.OK) 
            {
                // puzzle = new Puzzle(pictureBox1.Width, pictureBox1.Height, dlg.GridSize);
                puzzle.recentre(pictureBox1.Width, pictureBox1.Height, dlg.GridSize);
                pictureBox1.Invalidate();
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
            recenterPuzzle();
        }

        private void recenterPuzzle()
        {
            if (puzzle == null) return;
            puzzle.recentre(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // This handles keyboard shortcuts
            if (e.Control)
            {
               switch(e.KeyCode)
                {
                    case Keys.O:
                        openToolStripMenuItem_Click(this, e);
                        break;
                    case Keys.S:
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
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
