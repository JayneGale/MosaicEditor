using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor.Classes
{
    class ColourPalette
    {
        public int currentColourIndex = 1;
        private Color currentColour = Color.Red;

        //        private ToolStripButton current;

        List<Color> colors = new List<Color>();

        public ColourPalette(int size)
        {
            colors.Add(Constants.BLANK_COLOR);

            if (size < 1) size = 1;

            int i = 1;
            double hueAngle = 0;
            double delta = 360 / size;
            while (i++ <= size)
            {
                colors.Add(HSLtoRGB(hueAngle, 1, 0.5));
                hueAngle += delta;
            }

            Console.WriteLine("PALETTE:", colors);
        }

        public ColourPalette(List<Color> colors)
        {
            this.colors = colors;
        }

        internal void setColor(int index, Color color)
        {
            if (index > 0 && index < colors.Count)
            {
                colors[index] = color;
            }
        }

        internal string getColourPalette(Color color)
        {
            for(int i = 0; i < colors.Count; i++) {
                if (colors[i] == color)
                {
                    return i.ToString();
                }
            }
            return "?";
        }

        internal List<Color> getColors()
        {
            return colors.ToList();
        }

        internal Color getColor(int colorNo)
        {
            if (colorNo < colors.Count)
            {
                return colors[colorNo];
            }
            return Color.White; // failed
        }

        #region HSL colour handling.  Thanks to the internet!

        /// <summary>
        /// Converts HSL to RGB.
        /// </summary>
        /// <param name="h">Hue, must be in [0, 360].</param>
        /// <param name="s">Saturation, must be in [0, 1].</param>
        /// <param name="l">Luminance, must be in [0, 1].</param>
        /// https://www.codeproject.com/Articles/19045/Manipulating-colors-in-NET-Part-1
        public static Color HSLtoRGB(double h, double s, double l)
        {
            if (s == 0)
            {
                // achromatic color (gray scale)
                return Color.FromArgb(
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        l * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        l * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        l * 255.0)))
                    );
            }
            else
            {
                double q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - (l * s));
                double p = (2.0 * l) - q;

                double Hk = h / 360.0;
                double[] T = new double[3];
                T[0] = Hk + (1.0 / 3.0);    // Tr
                T[1] = Hk;                // Tb
                T[2] = Hk - (1.0 / 3.0);    // Tg

                for (int i = 0; i < 3; i++)
                {
                    if (T[i] < 0) T[i] += 1.0;
                    if (T[i] > 1) T[i] -= 1.0;

                    if ((T[i] * 6) < 1)
                    {
                        T[i] = p + ((q - p) * 6.0 * T[i]);
                    }
                    else if ((T[i] * 2.0) < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
                    {
                        T[i] = q;
                    }
                    else if ((T[i] * 3.0) < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
                    {
                        T[i] = p + (q - p) * ((2.0 / 3.0) - T[i]) * 6.0;
                    }
                    else T[i] = p;
                }

                return Color.FromArgb(
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        T[0] * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        T[1] * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        T[2] * 255.0)))
                    );
            }
        }

        #endregion
    }
}
