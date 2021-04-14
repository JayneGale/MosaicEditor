using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    public class Constants
    {
        internal static int DEFAULT_GRID_SPACING = 70;

        internal static double COS30 = Math.Cos(30 * Math.PI / 180);   // about 0.8660

        internal static Color BLANK_COLOR = Color.Silver;

        public enum PuzzleDifficulty
        {
            EASY = 1,
            MEDIUM,
            HARD
        }

        public enum PuzzleType
        {
            MATCH1 = 1,
            MATCH2,
            MATCH3
        }

        public static string PUZZLE_SET_FILE = "puzzleSet1.js";

    }
}
