using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mosaic_editor.Classes.Constants;

namespace Mosaic_editor.Classes
{
    public class Puzzle
    {
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; private set; }

        public List<Hexagon> HexagonList;

        public int windowWidth { get; private set; }
        public int windowHeight { get; private set; }
        public string filename { get; private set; }

        public int gridSpacing = 100;
        private ColourPalette palette;

        public int cols { get; private set; }
        public int rows { get; private set; }
        internal bool IsEmpty
        {
            get
            {
                if (HexagonList == null) return true;
                if (HexagonList.Any(h => h.isActive)) return false;
                return true;
            }
        }

        internal bool isDirty { get; set; }

        public string name = "A First Puzzle";
        // public PuzzleDifficulty difficulty = PuzzleDifficulty.EASY;
        public PuzzleType puzzleType = PuzzleType.MATCH3;

        public int tier = 1;
        public int swaps = 1;
        public int plays = 3;
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
        /// <param name="gridSpacing">in pixels</param>
        // public Puzzle(PictureBox pictureBox, int gridSpacing, ColourPalette palette)
        public Puzzle(int gridSpacing, ColourPalette palette)
        {
            this.gridSpacing = gridSpacing;
            this.palette = palette;
            this.HexagonList = new List<Hexagon>();
        }

        internal void resizeToFit(PictureBox pictureBox)
        {
            this.windowWidth = pictureBox.Width;
            this.windowHeight = pictureBox.Height;
            this.recentre();
        }

        /// <summary>
        /// Creates a list of hexagons enough to fill the given area
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        private void CreateHexArray() // int windowWidth, int windowHeight)
        {
            HexagonList = new List<Hexagon>();

            // Work out how many rows and columns fit into this window
            const int MARGINS = 20;
            var unitsAcross = (windowWidth - MARGINS) / this.gridSpacing;
            this.cols = (int)Math.Floor((unitsAcross - 0.5) / 1.5);

            var triangleHeight = this.gridSpacing * Constants.COS30;
            var unitsDown = (windowHeight - triangleHeight - MARGINS) / (triangleHeight * 2); // var unitsDown = (windowHeight - MARGINS) / triangleHeight;
            this.rows = (int)Math.Floor(unitsDown);

            Console.WriteLine($"this canvas has {cols} columns and {rows} rows");

            // row and col are simple counters
            // int col0 = 1;
            int row = -(int)Math.Ceiling(rows / 2M) - 2;
            for (int y = 0; y < rows + 4; y++)
            {
                int col = -(int)Math.Ceiling(cols / 2M) - 2;
                for (int x = 0; x < cols + 4; x++) //for (int col = col0; col < cols; col += 2)
                {
                    var hex = new Hexagon(row, col);
                    hex.parent = this;
                    HexagonList.Add(hex);
                    col++;
                }
                row++;
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
            if (palette != null)
            {
                return palette.getColor(colorNo);
            }
            Console.WriteLine("ERROR: puzzle palette has not been initialised");
            return Color.Azure;
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

            // crosshairs
            //g.DrawLine(Pens.Red, 0, 0, windowWidth, windowHeight);
            //g.DrawLine(Pens.Red, 0, windowHeight, windowWidth, 0);
        }

        internal void clear()
        {
            foreach (var hex in HexagonList)
            {
                hex.clear();
            }
            isDirty = false;
        }

        internal static Puzzle load(string filename)
        {
            if (File.Exists(filename))
            {
                var text = File.ReadAllText(filename);
                try
                {
                    var result = JsonConvert.DeserializeObject(text, typeof(Puzzle)) as Puzzle;

                    result.RepairLinks();
                    // result.refreshPositions();
                    result.recreatePalette();
                    Console.WriteLine($"Loaded puzzle from {filename}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show($"Cannot find file {filename}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Failed
            return null;
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
                return load(dlg.FileName);
                //if (File.Exists(dlg.FileName))
                //{
                //    var text = File.ReadAllText(dlg.FileName);
                //    try
                //    {
                //        var result = JsonConvert.DeserializeObject(text, typeof(Puzzle)) as Puzzle;

                //        result.RepairLinks();
                //        // result.refreshPositions();
                //        result.recreatePalette();
                //        Console.WriteLine($"Loaded puzzle from {dlg.FileName}");
                //        return result;
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                //else
                //{
                //    MessageBox.Show($"Cannot find file {dlg.FileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }
            // a change

            // Failed!  Return a new empty puzzle
            //var newPuzzle = new Puzzle(10, 10, 100, new ColourPalette(6));
            //newPuzzle.name = "Puzzle failed to load";
            //return newPuzzle;
            return null;
        }

        /// <summary>
        /// After a load, recreate the current palette
        /// </summary>
        private void recreatePalette()
        {
            // this.palette = new ColourPalette(colors);
            this.palette = new ColourPalette(colors.Count);
            this.palette.reload(colors);
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

        internal void centreSelectedTile()
        {
            if (selectedHexagon == null) return;
            var offsetX = selectedHexagon.col;
            var offsetY = selectedHexagon.row;
            Console.WriteLine($"Offset all tiles by [{offsetX},{offsetY}]");
            foreach(var hex in HexagonList)
            {
                if (hex.isActive)
                {
                    hex.col -= offsetX;
                    hex.row -= offsetY;
                }
            }
            refreshPositions();
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

        internal void deleteSelectedTile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Move the active puzzle pieces within the window.
        /// Used for (a) re-centering and (b) normalising to top left corner
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        internal void translate(int dx, int dy)
        {
            var activeHexagons = HexagonList.Where(h => h.isActive);    // get a list of the active ones

            if (activeHexagons.Count() > 0)
            {
                // dx MUST be even.
                if ((dx & 1) == 1)
                {
                    if (dx > 0) dx--;
                    else dx++;
                }

                // Now move the active hexagons by dx, dy
                foreach (var h in activeHexagons)
                {
                    h.col += dx;
                    h.row += dy;
                }
            }

            // Regenerate the blank grid and re-insert the translated active hexagons
            CreateHexArray(); //  width, height);

            foreach (var hex in activeHexagons)
            {
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

            // 
            refreshPositions();
        }

        /// <summary>
        /// Recentres the puzzle in the existing window
        /// </summary>
        /// <param name="width">Window width in pixels</param>
        /// <param name="height">Window height in pixels</param>
        /// <param name="gridSize">Triangle width in pixels</param>
        internal void recentre() // int width, int height, int gridSize = 0)
        {
            // this.windowWidth = width;
            // this.windowHeight = height;
            // if (gridSize > 0) this.gridSpacing = gridSize;  // ..otherwise it remains unchanged from its previous value
            // return;

            var dx = 0;
            var dy = 0;

            var activeHexagons = HexagonList.Where(h => h.isActive);    // save a list of the active ones
            // dumpList("activeHexagons", activeHexagons);
            if (activeHexagons.Count() > 0)
            {
                var currentLeft = activeHexagons.Min(h => h.col);
                var currentTop = activeHexagons.Min(h => h.row);

                var bounds = getActiveRange();   // the bounds of the active puzzle
                Console.WriteLine($"current puzzle bounds: {bounds.x1},{bounds.y1} - {bounds.x2},{bounds.y2}");

                // Reinitialise the HexagonList with new hexagons
                CreateHexArray(); //  width, height);
                var window = getWindowRange(); // the size of the window
                window.Dump("current window bounds");
                /// Console.WriteLine($"(current window bounds: {window.Left},{window.Top} - {window.Right},{window.Bottom})");

                // Offset the puzzle to put it in the centre of the screen.
                int newLeft = (int)((window.width - bounds.width) / 2);     // round to nearest int
                int newTop = (int)((window.height - bounds.height) / 2);
                Console.WriteLine($"..recentre() will move the pieces to {newLeft},{newTop}");

                dx = newLeft - currentLeft;
                dy = newTop - currentTop;

                // restore those active hexagons
                HexagonList = activeHexagons.ToList();
            }

            // translate also regenerates the hex grid
            translate(0, 0); // translate(dx, dy);

        }

        internal void dumpList(string text, IEnumerable<Hexagon> hexagons)
        {
            if (hexagons == null || hexagons.Count() == 0)
            {
                Console.WriteLine($"================== HEXAGON LIST IS EMPTY: {text}");
                return;
            }
            Console.WriteLine($"================== HEXAGON DUMP BEGIN: {text}");
            foreach (var h in hexagons)
            {
                var triangles = h.triangles.Select(t => t.isActive ? "active" : "-");
                Console.WriteLine($"[{h.col},{h.row}] {(h.isActive ? "active" : "")} {(h.isFixed ? "fixed" : "")} colors[{string.Join(",", h.colors)}] triangles[{string.Join(",", triangles)}]");
            }
            Console.WriteLine($"================== HEXAGON DUMP END");
        }

        private void refreshPositions()
        {
            int x0 = (int)(windowWidth / 2);
            int y0 = (int)(windowHeight / 2);
            //x0 = 0;
            //y0 = 0;

            Console.WriteLine($"Puzzle.refreshPositions(): {x0}, {y0}");

            foreach (var h in HexagonList)
            {
                h.refreshPosition(x0, y0, this.gridSpacing);
            }
        }

        private void RepairLinks()
        {
            foreach (var hex in HexagonList)
            {
                hex.parent = this;

                foreach (var t in hex.triangles)
                {
                    t.parent = hex;
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        internal bool save()
        {
            var activeHexagons = HexagonList.Where(h => h.isActive);    // LINQ

            if (activeHexagons.Count() == 0) return false;

            var bounds = this.getActiveRange();

            var puzzle = (Puzzle)this.MemberwiseClone();

            puzzle.colors = this.palette.getColors();

            puzzle.DateCreated = DateTime.Now;
            puzzle.DateUpdated = DateTime.Now;
            puzzle.HexagonList = activeHexagons.ToList(); // usedHexagons.ToList();
            dumpList("saving hexagons", activeHexagons);

            var puzzleFolders = Properties.Settings.Default.PuzzleFolders;
            if (puzzleFolders == null || puzzleFolders.Count == 0 || !Directory.Exists(puzzleFolders[0]))
            {

                MessageBox.Show("No puzzle folder has been set.  Please go to File | Preferences and fix this.", "Mosaic Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            string baseFolder = puzzleFolders[0];

            var tierFolder = Path.Combine(baseFolder, $"tier {puzzle.tier}");
            if (!Directory.Exists(tierFolder))
            {
                Directory.CreateDirectory(tierFolder);
            }

            var dlg = new SaveFileDialog();
            dlg.Filter = "JSON files|*.json";
            // var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // if (!Directory.Exists(dlg.InitialDirectory)) Directory.CreateDirectory(dlg.InitialDirectory);
            dlg.InitialDirectory = tierFolder; // Path.Combine(myDocuments, "mosaic");
            dlg.FileName = puzzle.name + ".json";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                puzzle.filename = dlg.FileName;
                this.filename = puzzle.filename;

                var json = JsonConvert.SerializeObject(puzzle, Formatting.Indented);

                Console.WriteLine($"Saving to {dlg.FileName}");
                File.WriteAllText(dlg.FileName, json);

                var merger = new PuzzleMerger();
                merger.run(baseFolder, PUZZLE_SET_FILE);   // tell the merger where the last puzzle was saved

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
