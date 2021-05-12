using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    public class Hexagon
    {
        public int row;
        public int col;
        public bool isActive;
        internal bool verbose = true;

        public bool isFixed = false;
        // public int matchOn = 3;             // number of adjacent triangles which must match
        internal bool isSelected = false;

        // The box which defines the bounds of this hexagon
        internal Rectangle bounds;

        // The path which defines the outline of this hexagon
        private GraphicsPath outline;

        internal Puzzle parent;

        internal List<Triangle> triangles;

        public string colors {
            get
            {
                var result = new int[6];
                foreach(int i in Enumerable.Range(0,result.Length))
                {
                    result[i] = triangles[i].colorNo;
                }
                return String.Join(",", result);
            }
            set
            {
                // This runs when a puzzle is loaded from file
                var colorNos = value.Split(',');
                foreach (int i in Enumerable.Range(0, colorNos.Length))
                {
                    triangles[i].colorNo = 0;   // default
                    if (int.TryParse(colorNos[i], out int c))
                    {
                        triangles[i].colorNo = c;
                    }
                    triangles[i].isActive = true; // triangles[i].colorNo > 0;
                }
            }
        }

        /// <summary>
        /// Construct a Hexagon at a specified row and col.
        /// Actual screen position unknown at this point.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public Hexagon(int row, int col)
        {
            this.row = row;
            this.col = col;

            bool upsideDown = false;

            this.row = row;
            this.col = col;

            triangles = new List<Triangle>();

            for (int i = 0; i < 6; i++)
            {
                var t = new Triangle()
                {
                    parent = this,
                    index = i,
                    upsideDown = upsideDown,
                };
                triangles.Add(t);
                upsideDown = !upsideDown;

            }
        }

        internal Color getColor(int colorNo)
        {
            return parent.getColor(colorNo);
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
        internal void calculateOutline(bool forced = false)
        {
            if (!forced && outline != null) return;    // already done; don't repeat

            outline = new GraphicsPath();

            var x1 = bounds.Left + bounds.Width / 4;
            var x2 = bounds.Left + (3 * bounds.Width) / 4;
            var y1 = bounds.Top + bounds.Height / 2;

            outline.AddPolygon(new Point[] {
                new Point(x1, bounds.Top),
                new Point(x2, bounds.Top),
                new Point(bounds.Right, y1),
                new Point(x2, bounds.Bottom),
                new Point(x1, bounds.Bottom),
                new Point(bounds.Left, y1)
            });
        }

        internal void clear()
        {
            triangles.ForEach(t => t.clear());
            this.isActive = false;
            this.isFixed = false;
            this.isSelected = false;
        }

        /// <summary>
        /// Draw this hexagon, including its contents (6 triangles)
        /// </summary>
        /// <param name="g"></param>
        internal void draw(Graphics g)
        {
            // Console.WriteLine($"Hexagon.draw() {this.col},{this.row}");

            calculateOutline();

            var pen = new Pen(Color.LightGray, 1);

            if (col == 0 && row == 0)
            {
                Console.WriteLine($"drawing tile[0,0] at {bounds.Left}, {bounds.Top}");
            }

            if (isActive)
            {
                // Draw my six triangles first
                triangles.ForEach(t => t.draw(g, isFixed));
                // Set up the pen for the hex outline
                pen.Color = Color.FromArgb(30, 30, 30); // Color.DarkGray;
                pen.Width = 3;  // pen for hex outline
            }
            else
            {
                triangles.ForEach(t => t.draw(g));
            }

            if (isSelected)
            {
                pen.Color = Color.Red;
                pen.DashStyle = DashStyle.Dash;
            }

            // Draw hexagon outline
            g.DrawPath(pen, outline);

            if (isFixed)
            {
                pen.Color = Color.Gold;
                pen.Width = 5;
                g.DrawPath(pen, outline);
            }

            if (Properties.Settings.Default.ShowTileCoordinates || (col == 0 && row == 0))
            {
                var fontSize = (int)(bounds.Height / 5);
                var font = new Font("Arial", fontSize);
                var x = bounds.Left + bounds.Width / 2;
                var y = bounds.Top + bounds.Height / 2;
                var text = $"{col},{row}";
                var box = g.MeasureString(text, font);
                g.DrawString(text, font, Brushes.DarkBlue, x - box.Width/2, y - box.Height/2);
            }
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


        /// <summary>
        /// Recalculate the hexagon position, given row, column, and gridSpacing
        /// Including all its contained triangles
        /// </summary>
        /// <param name="gridSpacing"></param>
        internal void refreshPosition(int x0, int y0, int gridSpacing)
        {
            var width = gridSpacing;    // the width of a triangle
            var height = (int)(gridSpacing * Constants.COS30);  // the height of a triangle

            // var margin = 0; //  20;    // px
            //var x = x0 + (int)(margin + width + (col * width * 1.5));
            //var y = y0 + (int)(margin + height + (row * height * 2));
            var x = x0 + (int)(col * width * 1.5);
            var y = y0 + row * height * 2;
            if ((col & 1) == 1)
            {
                // 1, 3, 5, ...
                y -= height;
            }

            bounds = new Rectangle(x - width, y - height, width * 2, height * 2);   // ..twice the width and height of a triangle
            if (col == 0 && row == 0)
            {
                Console.WriteLine($"positioned tile[0,0] at {bounds.Left}, {bounds.Top}; centred on {x}, {y}");
            }

            triangles[0].bounds = new Rectangle(x, y - height, width, height);
            triangles[1].bounds = new Rectangle(x, y, width, height);
            triangles[2].bounds = new Rectangle(x - (width / 2), y, width, height);
            triangles[3].bounds = new Rectangle(x - width, y, width, height);
            triangles[4].bounds = new Rectangle(x - width, y - height, width, height);
            triangles[5].bounds = new Rectangle(x - (width / 2), y - height, width, height);

            this.calculateOutline(true);

            int i = 0;
            foreach (var t in triangles)
            {
                t.index = i++;
                t.upsideDown = (i & 1) == 0;    // odd-numbered ones are upside down
                t.calculateOutline(true);
            }
        }
    }
}
