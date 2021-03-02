using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    class Triangle
    {
        public Color color;
        internal Hexagon parent;
        public bool upsideDown;

        public bool isActive;
        internal bool verbose = false;

        // The bounding box of this triangle
        public Rectangle bounds;

        // The path which defines the outline of this triangle
        private GraphicsPath outline;

        public int index { get; internal set; }

        /// The constructor for Triangle
        internal Triangle()
        {
            isActive = false;
            color = Constants.BLANK_COLOR;  // the default
        }

        /// Calculate the outline of this triangle, given its bounding box.
        /// The outline is a closed polygon defined by three points.
        /// This function runs once, the first time the triangle is drawn.
        internal void calculateOutline(bool forced = false)
        {
            if (!forced && outline != null) return;    // already done; don't repeat
            Point[] points = new Point[3];
            if (upsideDown)
            {
                points[0] = new Point(bounds.Left, bounds.Top);
                points[1] = new Point(bounds.Right, bounds.Top);
                points[2] = new Point((bounds.Left + bounds.Right) / 2, bounds.Bottom);
            }
            else
            {
                points[0] = new Point((bounds.Left + bounds.Right) / 2, bounds.Top);
                points[1] = new Point(bounds.Right, bounds.Bottom);
                points[2] = new Point(bounds.Left, bounds.Bottom);
            }
            outline = new GraphicsPath();
            outline.AddPolygon(points);
        }

        /// <summary>
        /// Draw this triangle - fill and outline.
        /// </summary>
        /// <param name="g"></param>
        internal void draw(Graphics g, bool isFixed = false)
        {
            // Console.WriteLine($"Triangle.draw()");

            calculateOutline();
            // Draw and fill this triangle

            var pen = new Pen(Color.Black, 1);
            if (parent.isActive)
            {
                dumpOutline();
                Brush brush = new SolidBrush(color);
                if (isFixed)
                {
                    brush = new HatchBrush(HatchStyle.Percent40, color, Color.White);
                }
                g.FillPath(brush, outline);
            }
            else
            {
                pen.Color = Color.FromArgb(230,230,230); // Color.LightGray;
            }
            g.DrawPath(pen, outline);

            if (verbose)
            {
                var font = new Font("Arial", 10);
                g.DrawString(index.ToString(), font, Brushes.Green, bounds.Left + (bounds.Width / 2) - 6, bounds.Top + (bounds.Height / 2) - 10);
            }
        }

        private void dumpOutline()
        {
            foreach(var p in outline.PathPoints)
            {
                Console.Write($"{p.X},{p.Y}; ");
            }
            Console.WriteLine();
        }

        internal bool contains(Point mouseXY)
        {
            // Work out if x,y falls within this triangle
            if (!bounds.Contains(mouseXY)) return false;

            var x = mouseXY.X - bounds.Left;
            var y = mouseXY.Y - bounds.Top;

            // Examine each half separately.  Which half are we in?
            var halfWidth = bounds.Width / 2;
            var inLeftHalf = x < halfWidth;

            // If in the right half, measure x from the midway point.
            if (!inLeftHalf) x -= halfWidth;

            // So each half-triangle is a simple right-angled triangle, sloping one way or the other.
            // Calculate the threshold at which y crosses from below the line to above it:
            var slope = bounds.Height / (bounds.Width / 2.0);
            var yThreshold = x * slope;

            if (upsideDown)
            {
                if (inLeftHalf)
                {
                    return y < yThreshold;
                }
                else // ..in right half
                {
                    return y < bounds.Height - yThreshold;
                }
            }
            else // .. not upside down
            {
                if (inLeftHalf)
                {
                    return y > bounds.Height - yThreshold;
                }
                else // ..in right half
                {
                    return y > yThreshold;
                }
            }
        }

        /// <summary>
        /// Clear the triangle
        /// </summary>
        internal void clear()
        {
            isActive = false;
            color = Constants.BLANK_COLOR;
        }

    }
}
