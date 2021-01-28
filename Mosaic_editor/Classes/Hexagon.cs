using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    class Hexagon
    {
        internal List<Triangle> triangles;
        internal int x;
        internal int y;
        internal bool isActive;

        // The box which defines the bounds of this hexagon
        public Rectangle bounds;

        // The path which defines the outline of this hexagon
        private GraphicsPath outline;

        public Hexagon(int x, int y, int gridSpacing)
        {
            bool upsideDown = false;

            // These are the screen coordinates of the centre of the hexagon
            int hexX = x;
            int hexY = y;

            int height = (int)(Constants.COS30 * gridSpacing);
            var width = gridSpacing;

            bounds = new Rectangle(x - width, y - height, width * 2, height * 2);

            triangles = new List<Triangle>();

            for (int i = 0; i < 6; i++)
            {
                var t = new Triangle()
                {
                    parent = this,
                    upsideDown = upsideDown,
                };

                // Set bounds for each triangle: the bounding box's upper left corner
                switch (i)
                {
                    case 0:
                        t.bounds = new Rectangle(hexX, hexY - height, width, height);
                        break;
                    case 1:
                        t.bounds = new Rectangle(hexX, hexY, width, height);
                        break;
                    case 2:
                        t.bounds = new Rectangle(hexX - (width / 2), hexY, width, height);
                        break;
                    case 3:
                        t.bounds = new Rectangle(hexX - width, hexY, width, height);
                        break;
                    case 4:
                        t.bounds = new Rectangle(hexX - width, hexY - height, width, height);
                        break;
                    case 5:
                        t.bounds = new Rectangle(hexX - (width / 2), hexY - height, width, height);
                        break;
                }
                upsideDown = !upsideDown;
                triangles.Add(t);
            }
        }

        // refresh() checks and sets the "isActive" status of this Hexagon depending upon
        // the colours of its triangles.
        internal void refresh()
        {
            isActive = triangles.Any(t => t.isActive);
        }

        // Calculate the outline of this hexagon, given its bounding box.
        // The outline is a closed polygon defined by six points.
        // This function runs once, the first time the hexagon is drawn.
        internal void calculateOutline()
        {
            if (outline != null) return;    // already done; don't repeat

            outline = new GraphicsPath();

            var x1 = bounds.Left + bounds.Width / 4;
            var x2 = bounds.Left + (3 * bounds.Width) / 4;
            var y1 = bounds.Top + bounds.Height / 2;

            outline.AddPolygon(new Point[] {
                new Point(x1, bounds.Top + 2),
                new Point(x2, bounds.Top + 2),
                new Point(bounds.Right - 2, y1),
                new Point(x2, bounds.Bottom - 2),
                new Point(x1, bounds.Bottom - 2),
                new Point(bounds.Left + 2, y1)
            });
        }

        internal void clear()
        {
            triangles.ForEach(t => t.clear());
            this.isActive = false;
        }

        /// <summary>
        /// Draw this hexagon, including its contents (6 triangles)
        /// </summary>
        /// <param name="g"></param>
        internal void draw(Graphics g)
        {
            calculateOutline();

            var pen = new Pen(Color.LightGray, 1);

            if (isActive)
            {
                // Draw my six triangles first
                triangles.ForEach(t => t.draw(g));
                pen.Color = Color.DarkGray;
                pen.Width = 2;
            }
            else
            {
                triangles.ForEach(t => t.draw(g));
            }

            // Draw hexagon outline
            g.DrawPath(pen, outline);
        }

        internal Triangle checkForClick(Point location)
        {
            if (!bounds.Contains(location)) return null;

            Console.WriteLine($"Hexagon ({this.bounds.Left},{this.bounds.Top}) was clicked!");

            foreach (var t in triangles)
            {
                if (t.contains(location))
                {
                    Console.WriteLine($"Clicked on triangle {t.bounds.Left}");
                    return t;
                }
            }

            Console.WriteLine("(...no triangle here was clicked)");
            return null;
        }
    }
}
