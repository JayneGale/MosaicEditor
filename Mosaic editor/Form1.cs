using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor
{
    public partial class Form1 : Form
    {
        const int WIDTH = 10;
        const int HEIGHT = 10;

        double COS30 = Math.Cos(30 * Math.PI / 180);   // about 0.8660

        Color[] palette = new Color[] {
            Color.Blue,
            Color.Red,
            Color.Pink,
            Color.Beige
        };

        int gridSpacing = 100; // ..set on run time

        // Hexagon[,] HexagonArray = new Hexagon[WIDTH, HEIGHT];

        List<Hexagon> HexagonList;

        public Form1()
        {
            InitializeComponent();

            gridSpacing = this.Width / 15;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateHexArray();
            // DrawTriangleGrid();  - - moved to Paint event handler below (it's proper location)
        }

        private void CreateHexArray()
        {
            HexagonList = new List<Hexagon>();

            // x and y are simple counters
            // xx and yy are the positions on screen in pixels of the (centres of) the hexagons
            // dx is the spacing between hex centres across the screen
            // dy is the spacing between hex centres down the screen
            // EVEN rows (0, 2, 4, ..) start from x = 0
            // ODD rows (1, 3, 5, ..) start from x = 1.5 times the gridSpacing

            int dx = 3 * gridSpacing;
            int dy = (int)(gridSpacing * COS30);
            int xx = 0;
            int x0 = 0; // the starting offset for each row

            int yy = 0;
            for (int y = 0; y < HEIGHT; y++)
            {
                xx = x0;
                for (int x = 0; x < WIDTH; x++)
                {
                    var hex = new Hexagon(xx, yy, gridSpacing);
                    // HexagonArray[x, y] = hex;
                    HexagonList.Add(hex);
                    xx += dx;
                }
                x0 = (int)(gridSpacing * 1.5) - x0;   // this handles the alternating row offsets
                yy += dy;
            }
        }

        private void DrawTriangleGrid(PaintEventArgs e)
        {
            // throw new NotImplementedException();
            var yStep = gridSpacing * COS30;

            int W = (int)e.Graphics.ClipBounds.Width;
            int H = (int)e.Graphics.ClipBounds.Height;

            // Draw horiz lines
            double y = 0;
            while (y < pictureBox1.Height)
            {
                // drawline (0,y) to (picture1.Width,y), light grey
                e.Graphics.DrawLine(Pens.Gray, 0, (float)y, W, (float)y);
                y += yStep;   // maintain accuracy
            }

            // Draw the right-sloping lines
            int x1 = 0;
            int x2 = (int)(-H / Math.Tan(60 * Math.PI / 180));
            while (x2 < W)
            {
                e.Graphics.DrawLine(Pens.Gray, x1, 0, x2, H);
                x1 += gridSpacing;
                x2 += gridSpacing;
            }

            // Draw the left-sloping lines
            x1 = -10 * gridSpacing;
            x2 = x1 + (int)(H / Math.Tan(60 * Math.PI / 180));
            while (x1 < W)
            {
                e.Graphics.DrawLine(Pens.Gray, x1, 0, x2, H);
                x1 += gridSpacing;
                x2 += gridSpacing;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Debug.Print("Clicked on the box!");
            pictureBox1.BackColor = Color.Aqua;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.Print($"You clicked on {e.X}, {e.Y}");
            // What triangle was that?
            foreach(var hex in HexagonList)
            {
                foreach(var tri in hex.triangles)
                {
                    if (tri.contains(e.X, e.Y))
                    {
                        // TODO: do stuff
                        // TODO: toggle the triangle color on and off, or set the triangle color?
                        Debug.Print("You clicked on me!");
                    }
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // All drawing on pictureBox1 is done within this paint event handler
            DrawTriangleGrid(e);
            foreach(var hex in HexagonList)
            {
                hex.draw(e);
            }
        }
    }
}
