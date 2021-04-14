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
        public int currentColourIndex
        {
            get
            {
                return _currentColourIndex;
            }
            set
            {
                _currentColourIndex = value;
                var color = palette.getColor(value);
                currentColour = color;
                current.BackColor = color;
            }
        }
        private int _currentColourIndex = 1;

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

            reloadColourPicker(palette.getColors());
        }

        internal void reloadColourPicker(List<Color> colors)
        {
            palette.setColors(colors);

            // toolStrip.Items.Clear(); => triggers a picturebox resize!
            // Work-around: insert a temp placeholder
            toolStrip.Items.Insert(0, new ToolStripLabel { Text = "TEMPORARY LABEL!" });

            // Remove everything except Items[0] (the temporary placeholder)
            while (toolStrip.Items.Count > 1)
            {
                toolStrip.Items.RemoveAt(1);
            }

            // Now add the toolbar controls we actually want...
            current = new ToolStripButton
            {
                BackColor = Color.Red,
                Text = "Current Colour",
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Margin = new Padding(0, 0, 6, 3),
            };
            toolStrip.Items.Add(current);

            // ..now can safely remove that placeholder
            toolStrip.Items.RemoveAt(0);

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
                            onPaletteChanged(new EventArgs());              // fire an "onPaletteChanged" event
                        }
                    }
                    else
                    {
                        currentColourIndex = (int)b.Tag;
                        //currentColour = b.BackColor;
                        //current.BackColor = b.BackColor;
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
