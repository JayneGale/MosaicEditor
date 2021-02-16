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

        public Form1()
        {
            InitializeComponent();
            palette = new ColourPalette(toolStrip1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Note: puzzle does not resize / redraw if the user resizes the window.
            puzzle = new Puzzle(pictureBox1.Width, pictureBox1.Height);
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
                        Console.WriteLine($"Changing color to {palette.currentColour}");
                        // Toggle colour
                        if (selectedTriangle.color == palette.currentColour)
                        {
                            selectedTriangle.color = Constants.BLANK_COLOR;
                        }
                        else
                        {
                            // selectedTriangle.isActive = true;
                            selectedTriangle.color = palette.currentColour;
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
            puzzle.save(palette);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            puzzle.load(palette);
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
                    using (var image = Bitmap.FromFile(dlg.FileName))
                    {
                        // Process this image
                        puzzle = PhotoInput.LoadImage(image);
                        image.Dispose();
                    }
                }
            }
        }
    }
}
