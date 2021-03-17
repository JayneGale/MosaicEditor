using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    /// <summary>
    /// 2DRange
    /// Exists because Rectangle does not nicely represent a range of x,y values
    /// </summary>
    public class Range2D
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public int width
        {
            get
            {
                return Math.Abs(x2 - x1) + 1;
            }
        }

        public int height
        {
            get
            {
                return Math.Abs(y2 - y1) + 1;
            }
        }

        public Range2D()
        {

        }

        public Range2D(int colMin, int rowMin, int colMax, int rowMax)
        {
            this.x1 = colMin;
            this.y1 = rowMin;
            this.x2 = colMax;
            this.y2 = rowMax;
        }

        public void Dump(string label = "bounds")
        {
            Console.WriteLine($"{label}: {x1},{y1} - {x2},{y2}");
        }

        internal bool contains(Hexagon h)
        {
            if (h.col < x1 || h.col > x2) return false;
            if (h.row < y1 || h.row > y2) return false;
            return true;
        }
    }
}
