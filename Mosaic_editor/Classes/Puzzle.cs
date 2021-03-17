using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor.Classes
{
    class Puzzle
    {
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; private set; }

        public List<Hexagon> HexagonList;

        public int width { get; private set; }
        public int height { get; private set; }
        public string filename { get; private set; }

        public int GridSpacing = 100;
        private ColourPalette palette;

        public int cols { get; private set; }
        public int rows { get; private set; }

        public string name = "A First Puzzle";
        public string difficulty = "Hard";
        public int repeats = 3;
        public bool dontMangle = false;
        public bool fixedColors = false;

        // Limit the maximum number of hexagons
        const int WIDTH_LIMIT = 50;
        const int HEIGHT_LIMIT = 50;

        public List<Color> colors = new List<Color>();

        internal Hexagon selectedHexagon = null;

        /// <summary>
        /// The constructor takes the dimensions of the picture box as parameters, and
        /// fills the picture box with hexagons.
        /// </summary>
        /// <param name="width">in pixels</param>
        /// <param name="height">in pixels</param>
        /// <param name="gridSpacing">in pixels</param>
        public Puzzle(int width, int height, int gridSpacing, ColourPalette palette)
        {
            this.width = width;
            this.height = height;
            this.GridSpacing = gridSpacing;
            this.palette = palette;

            CreateHexArray(width, height);
            refreshPositions();
        }

        /// <summary>
        /// Creates a list of hexagons enough to fill the given area
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        private void CreateHexArray(int windowWidth, int windowHeight)
        {
            HexagonList = new List<Hexagon>();

            // Work out how many rows and columns fit into this window
            const int MARGINS = 20;
            var unitsAcross = (windowWidth - MARGINS) / this.GridSpacing;
            this.cols = (int)Math.Floor((unitsAcross - 0.5) / 1.5);

            var triangleHeight = this.GridSpacing * Constants.COS30;
            var unitsDown = (windowHeight - MARGINS) / triangleHeight;
            this.rows = (int)Math.Floor(unitsDown - 1);
            Console.WriteLine($"this puzzle has {cols} columns and {rows} rows");

            // row and col are simple counters
            int col0 = 1;
            for (int row = 0; row < rows; row++)
            {
                for (int col = col0; col < cols; col += 2)
                {
                    var hex = new Hexagon(row, col);
                    hex.parent = this;
                    HexagonList.Add(hex);
                }
                col0 = 1 - col0;
            }
        }

        /// <summary>
        /// The sole purpose of refresh() is to check to see every hexagon
        /// to set it to active or not active.
        /// </summary>
        internal void refresh()
        {
            HexagonList.ForEach(h => h.refresh());
        }

        internal Color getColor(int colorNo)
        {
            return palette.getColor(colorNo);
        }

        /// <summary>
        /// Scan all hexagons to see which one was clicked upon
        /// </summary>
        /// <param name="location"></param>
        /// <returns>A triangle, if any was clicked upon; otherwise null.</returns>
        internal Triangle checkForClick(Point location)
        {
            foreach (var hex in HexagonList)
            {
                Triangle clickedTriangle = hex.checkForClick(location);
                if (clickedTriangle != null)
                {
                    if (selectedHexagon != null) selectedHexagon.isSelected = false;
                    selectedHexagon = clickedTriangle.parent;
                    selectedHexagon.isSelected = true;
                    return clickedTriangle;
                }
            }
            return null;
        }

        /// <summary>
        /// Draw the current puzzle.
        /// </summary>
        /// <param name="g"></param>
        internal void draw(Graphics g)
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            foreach (var hex in HexagonList)
            {
                hex.draw(g);
            }
        }

        internal void clear()
        {
            foreach(var hex in HexagonList)
            {
                hex.clear();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        internal static Puzzle load() // ColourPalette palette)
        {
            var dlg = new OpenFileDialog();
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.InitialDirectory = Path.Combine(myDocuments, "mosaic");
            if (!Directory.Exists(dlg.InitialDirectory)) Directory.CreateDirectory(dlg.InitialDirectory);

            dlg.Filter = "JSON files|*.json";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    var text = File.ReadAllText(dlg.FileName);
                    try
                    {
                        var result = JsonConvert.DeserializeObject(text, typeof(Puzzle)) as Puzzle;
                        result.RepairLinks();
                        result.refreshPositions();
                        result.recreatePalette();
                        Console.WriteLine($"Loaded puzzle from {dlg.FileName}");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show($"Cannot find file {dlg.FileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            // a change

            // Failed!  Return a new empty puzzle
            var newPuzzle = new Puzzle(10, 10, 100, new ColourPalette(6));
            newPuzzle.name = "Puzzle failed to load";
            return newPuzzle;
        }

        /// <summary>
        /// After a load, recreate the current palette
        /// </summary>
        private void recreatePalette()
        {
            this.palette = new ColourPalette(colors);
        }

        internal Range2D getActiveRange()
        {
            var result = new Range2D();

            var activeHexagons = HexagonList.Where(h => h.isActive);    // get a list of the active ones
            if (activeHexagons.Count() > 0)
            {
                var colMin = activeHexagons.Select(h => h.col).Min();
                var colMax = activeHexagons.Select(h => h.col).Max();
                var rowMin = activeHexagons.Select(h => h.row).Min();
                var rowMax = activeHexagons.Select(h => h.row).Max();

                result = new Range2D(colMin, rowMin, colMax, rowMax);
            }

            return result;
        }
        
        internal Range2D getWindowRange()
        {
            if (HexagonList.Count > 0)
            {
                var colMin = HexagonList.Select(h => h.col).Min();
                var colMax = HexagonList.Select(h => h.col).Max();
                var rowMin = HexagonList.Select(h => h.row).Min();
                var rowMax = HexagonList.Select(h => h.row).Max();
                return new Range2D(colMin, rowMin, colMax, rowMax);
            }
            else
            {
                return new Range2D(); // empty rectangle
            }
        }

        internal void translate(int x, int y)
        {
            var activeHexagons = HexagonList.Where(h => h.isActive);    // get a list of the active ones
            foreach(var h in activeHexagons)
            {
                h.col += x;
                h.row += y;
            }
            refreshPositions();
        }

        /// <summary>
        /// Recentres the puzzle in the existing window
        /// </summary>
        /// <param name="width">Window width in pixels</param>
        /// <param name="height">Window height in pixels</param>
        /// <param name="gridSize">Triangle width in pixels</param>
        internal void recentre(int width, int height, int gridSize = 0)
        {
            this.width = width;
            this.height = height;
            if (gridSize > 0) this.GridSpacing = gridSize;  // ..otherwise it remains unchanged from its previous value

            var activeHexagons = HexagonList.Where(h => h.isActive);    // save a list of the active ones
            var bounds = getActiveRange();   // the bounds of the active puzzle
            Console.WriteLine($"current puzzle bounds: {bounds.x1},{bounds.y1} - {bounds.x2},{bounds.y2}");

            // Reinitialise the HexagonList with new hexagons
            CreateHexArray(width, height);
            var window = getWindowRange(); // the size of the window
            window.Dump("current window bounds");
            /// Console.WriteLine($"(current window bounds: {window.Left},{window.Top} - {window.Right},{window.Bottom})");

            // Offset the puzzle to put it in the centre of the screen.
            int xOffset = (int)((window.width - bounds.width) / 2);     // round to nearest int
            int yOffset = (int)((window.height - bounds.height) / 2);
            Console.WriteLine($"..recentre() puts the window centre at {xOffset},{yOffset}");
            // Adjust to the nearest valid hex position
            if ((yOffset & 1) == 1)
            {
                // Its in an odd row, so must be in an even column
                if ((xOffset & 1) == 1)
                {
                    xOffset = (xOffset > 0) ? xOffset - 1 : xOffset + 1;
                }
            }
            else
            {
                // Its in an even row, so has to be in an odd column
                if ((xOffset & 1) == 0)
                {
                    xOffset = (xOffset > 0) ? xOffset - 1 : xOffset + 1;
                }
            }
            Console.WriteLine($"..recentre() puts the corrected window centre at {xOffset},{yOffset}");

            xOffset -= bounds.x1;
            yOffset -= bounds.y1;
            // NOTE: Not all offsets are valid!  Row 0 only has columns 1, 3, 5, ... and row 1 has columns 0, 2, 4, ... etc.
            // Valid offsets include:
            //      1,-1 1,+1 3,-1 3+1 etc.
            //      2,0 4,0 6,0 etc..
            // i.e. if xOffset is odd, yOffset must also be odd.
            //      if xOffset is even, yOffset must also be even.

            if ((xOffset & 1) != (yOffset & 1)) {
                // One is odd and one is even.  Fix!
                yOffset = (yOffset > 0) ? yOffset - 1 : yOffset + 1;
            }
            Console.WriteLine($"recentre will offset this puzzle by {xOffset},{yOffset}");

            // re-apply the active ones into the new list
            foreach (var hex in activeHexagons)
            {
                hex.col += xOffset;
                hex.row += yOffset;
                Console.WriteLine($"Hex moved to: {hex.col}, {hex.row}");
                var match = HexagonList.FirstOrDefault(hex2 => hex2.col == hex.col && hex2.row == hex.row);
                if (match != null)
                {
                    HexagonList.Add(hex);
                    HexagonList.Remove(match);
                }
                else
                {
                    Console.WriteLine($"..found no place for tile[{hex.col},{hex.row}]!");
                }
            }
            refreshPositions();
        }

        private void refreshPositions()
        {
            foreach(var h in HexagonList)
            {
                h.refreshPosition(this.GridSpacing);
            }
        }

        private void RepairLinks()
        {
            foreach(var hex in HexagonList)
            {
                hex.parent = this;

                foreach(var t in hex.triangles)
                {
                    t.parent = hex;
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        internal void save()
        {
            var activeHexagons = HexagonList.Where(h => h.isActive);    // LINQ

            if (activeHexagons.Count() == 0) return;

            var bounds = this.getActiveRange();

            //Console.WriteLine($"crop: {colMin},{rowMin} - {colMax},{ rowMax}");
            bounds.Dump();
            int xOffset = bounds.x1;
            int yOffset = bounds.y1;
            if ((yOffset & 1) != (yOffset & 1))
            {
                // MUST both be odd or both even!  Not the case this time, so fix it:
                yOffset = yOffset < 1 ? yOffset++ : yOffset--;
            }
            if (xOffset == 0 && yOffset == 0)   // [0,0] is invalid
            {
                xOffset++;
                // yOffset++;
            }
            Console.WriteLine($"save() is moving the puzzle [-{xOffset},-{yOffset}]");
            // this.translate(-xOffset, -yOffset);  // align the active hexagons to top left corner

            bounds = this.getActiveRange();
            bounds.Dump("after aligning bounds");
            // Console.WriteLine($"after aligning bounds: {bounds.Left},{bounds.Top} - {bounds.Right},{bounds.Bottom}");

            // var usedHexagons = HexagonList.Where(h => h.row >= rowMin && h.row <= rowMax && h.col >= colMin && h.col <= colMax);
            var usedHexagons = HexagonList.Where(h => bounds.contains(h)); // h.row >= bounds.Top && h.row < bounds.Bottom && h.col >= bounds.Left && h.col < bounds.Right);

            var puzzle =(Puzzle)this.MemberwiseClone();

            puzzle.colors = this.palette.getColors();

            // puzzle.Name = this.Name;
            // puzzle.Difficulty = this.Difficulty;
            puzzle.DateCreated= DateTime.Now;
            puzzle.DateUpdated = DateTime.Now;
            puzzle.HexagonList = activeHexagons.ToList(); // usedHexagons.ToList();

            var dlg = new SaveFileDialog();
            dlg.Filter = "JSON files|*.json";
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.InitialDirectory = Path.Combine(myDocuments, "mosaic");
            if (!Directory.Exists(dlg.InitialDirectory)) Directory.CreateDirectory(dlg.InitialDirectory);
            dlg.FileName = "mosaic puzzles.json";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                puzzle.filename = dlg.FileName;

                //var settings = new JsonSerializerSettings { 
                //    Formatting = Formatting.Indented,
                //    MaxDepth = 20,
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //};
                var json = JsonConvert.SerializeObject(puzzle, Formatting.Indented);

                Console.WriteLine($"Saving to {dlg.FileName}");
                File.WriteAllText(dlg.FileName, json);

                var merger = new PuzzleMerger();
                merger.run(dlg.FileName, "puzzleSet1.js");   // tell the merger where the last puzzle was saved
            }

        }
    }
}
