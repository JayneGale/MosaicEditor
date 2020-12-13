using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor
{
    class Triangle
    {
        public Color color;
        public Hexagon parent;
        public bool upsideDown;

        public int x1;
        public int x2;
        public int y1;
        public int y2;

        // The constructor for Triangle
        public Triangle()
        {
            color = Color.Red;  // a default
        }

        internal bool contains(int x, int y)
        {
            // TODO: work out if x,y falls within this triangle
            if (x < x1 || x > x2) return false;
            if (y < y1 || y > y2) return false;

            if (upsideDown)
            {
                // TODO
            }
            else
            {
                // TODO
            }
            return false;
        }

        internal void draw(PaintEventArgs e)
        {
            // TODO: Draw and fill this triangle
            //var outline = new GraphicsPath(....);
            //var brush = new SolidBrush(color);
            //e.Graphics.FillPath(brush, outline);
        }
    }

    class Hexagon
    {
        internal List<Triangle> triangles;
        internal int x;
        internal int y;

        public Hexagon(int x, int y, int gridSpacing)
        {
            bool upsideDown = false;

            int hexX = x;
            int hexY = y;

            triangles = new List<Triangle>();

            var width = gridSpacing;
            var height = (int)(gridSpacing * Math.Cos(30 * Math.PI / 180));

            for (int i = 0; i < 6; i++)
            {
                var t = new Triangle()
                {
                    parent = this,
                    upsideDown = upsideDown,
                };

                // Set (x1, y1) for each triangle: the bounding box's upper left corner
                switch (i)
                {
                    case 0:
                        t.x1 = hexX;
                        t.y1 = hexY - height;
                        break;
                    case 1:
                        t.x1 = hexX;
                        t.y1 = hexY;
                        break;
                    case 2:
                        t.x1 = hexX - (width / 2);
                        t.y1 = hexY;
                        break;
                    case 3:
                        t.x1 = hexX - width;
                        t.y1 = hexY;
                        break;
                    case 4:
                        t.x1 = hexX - width;
                        t.y1 = hexY - height;
                        break;
                    case 5:
                        t.x1 = hexX - (width / 2);
                        t.y1 = hexY - height;
                        break;
                }

                // All triangles bounding boxes are the same width and height...
                t.x2 = t.x1 + width;
                t.y2 = t.y1 + height;

                upsideDown = !upsideDown;

                triangles.Add(t);
            }
        }

        internal void draw(PaintEventArgs e)
        {
            foreach(var tri in triangles)
            {
                tri.draw(e);
            }
            // TODO: Draw hexagon outline
            //var outline = new GraphicsPath(....);
            // var pen = new Pen(Color.Blue, 3);
            // e.Graphics.DrawPath(pen, outline);
        }
    }


}
