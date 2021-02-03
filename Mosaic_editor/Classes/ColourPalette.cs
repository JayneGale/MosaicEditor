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
        public Color currentColour = Color.Red;

        private ToolStripButton current;

        Color[] colors = new Color[] {
                Constants.BLANK_COLOR,
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Cyan,
                Color.Yellow,
                Color.Orange
            };

        public ColourPalette(ToolStrip toolStrip)
        {
            toolStrip.Items.Clear();

            current = new ToolStripButton {
                BackColor = Color.Red,
                Text = "Current Colour",
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Margin = new Padding(0, 0, 6, 3),
            };
            toolStrip.Items.Add(current);

            toolStrip.Items.Add(new ToolStripLabel { 
                Text = "Colours:"
            });

            foreach(var c in colors)
            {
                var b = new ToolStripButton
                {
                    BackColor = c,
                    DisplayStyle = ToolStripItemDisplayStyle.None,
                    Margin = new Padding(0,0,6,3)
                };
                b.Click += (object sender, EventArgs e) => {
                    currentColour = c;
                    current.BackColor = c;
                };
                toolStrip.Items.Add(b);
            }

            toolStrip.Items.Add(new ToolStripLabel
            {
                Text = "Click anywhere to set colours.  Right-click to select without changing the colour.  Shift-click to erase a hexagon."
            });

        }

        internal string getColourPalette(Color color)
        {
            for(int i = 0; i < colors.Length; i++) {
                if (colors[i] == color)
                {
                    return i.ToString();
                }
            }
            return "?";
        }
    }
}
