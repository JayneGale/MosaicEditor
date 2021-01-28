using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    class Puzzle
    {
        internal List<Hexagon> HexagonList;

        // Limit the maximum number of hexagons
        const int WIDTH_LIMIT = 50;
        const int HEIGHT_LIMIT = 50;
        
        /// <summary>
        /// The constructor takes the dimensions of the picture box as parameters, and
        /// fills the picture box with hexagons.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Puzzle(int width, int height)
        {
            // this.gridSpacing = gridSpacing;
            CreateHexArray(width, height);
        }

        private void CreateHexArray(int width, int height)
        {
            HexagonList = new List<Hexagon>();

            // row and col are simple counters
            // x and y are the positions on screen in pixels of the (centres of) the hexagons
            // dx is the spacing between hex centres across the screen
            // dy is the spacing between hex centres down the screen
            // EVEN rows (0, 2, 4, ..) start from x = gridSpacing
            // ODD rows (1, 3, 5, ..) start from x = 1.5 times the gridSpacing

            int dx = 3 * Constants.GRID_SPACING;
            int dy = (int)(Constants.GRID_SPACING * Constants.COS30);
            int x;
            int x0 = 0; // the starting offset for each row

            int y = dy;
            for (int row = 0; row < HEIGHT_LIMIT; row++)
            {
                x = x0 + Constants.GRID_SPACING;

                for (int col = 0; col < WIDTH_LIMIT; col++)
                {
                    var hex = new Hexagon(x, y, Constants.GRID_SPACING);
                    // HexagonArray[x, y] = hex;
                    HexagonList.Add(hex);
                    x += dx;
                    if (x + Constants.GRID_SPACING > width) break;
                }
                x0 = (int)(Constants.GRID_SPACING * 1.5) - x0;   // this handles the alternating row offsets
                y += dy;
                if (y + Constants.GRID_SPACING > height) break;
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
                if (clickedTriangle != null) return clickedTriangle;
            }
            return null;
        }

        /// <summary>
        /// Draw the current puzzle.
        /// </summary>
        /// <param name="g"></param>
        internal void draw(Graphics g)
        {
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
        internal void load()
        {

        }

        /// <summary>
        /// TODO
        /// </summary>
        internal void save()
        {

        }
    }
}
