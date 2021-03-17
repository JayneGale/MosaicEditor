using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor.Classes
{
   internal class ColourPicker
    {
        public int currentColourIndex = 1;
        private Color currentColour = Color.Red;

        private ToolStripButton current;
        private ToolStrip toolStrip;
        private ColourPalette palette;

        internal delegate void PaletteChangeDelegate(EventArgs e);
        public event PaletteChangeDelegate onPaletteChanged;

        public ColourPicker(ToolStrip toolStrip, ColourPalette palette)
        {
            this.toolStrip = toolStrip;
            this.palette = palette;

            reloadPalette();
        }

        internal void reloadPalette()
        {
            toolStrip.Items.Clear();

            current = new ToolStripButton
            {
                BackColor = Color.Red,
                Text = "Current Colour",
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Margin = new Padding(0, 0, 6, 3),
            };
            toolStrip.Items.Add(current);

            toolStrip.Items.Add(new ToolStripLabel
            {
                Text = "Colours:"
            });

            int index = 0;
            foreach (var c in palette.getColors())
            {
                var b = new ToolStripButton
                {
                    BackColor = c,
                    DisplayStyle = ToolStripItemDisplayStyle.None,
                    Margin = new Padding(0, 0, 6, 3),
                    Tag = index
                };
                b.Click += (object sender, EventArgs e) => {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        // Open the colour picker
                        var dlg = new ColorDialog();
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            palette.setColor((int)b.Tag, dlg.Color);        // colors[(int)b.Tag] = dlg.Color;
                            b.BackColor = dlg.Color;
                            onPaletteChanged(new EventArgs());
                        }
                    }
                    else
                    {
                        currentColourIndex = (int)b.Tag;
                        currentColour = b.BackColor;
                        current.BackColor = b.BackColor;
                    }
                };
                toolStrip.Items.Add(b);
                index++;
            }

            toolStrip.Items.Add(new ToolStripLabel
            {
                Text = "Click anywhere to set colours.  Right-click to select without changing the colour.  Shift-click to erase a hexagon."
            });

        }
    }
}
