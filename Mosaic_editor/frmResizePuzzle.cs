using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor
{
    public partial class frmResizePuzzle : Form
    {
        public frmResizePuzzle()
        {
            InitializeComponent();
        }

        public int GridSize
        {
            get
            {
                if (int.TryParse(txtGridSize.Text, out int result))
                {
                    return result;
                }
                return 100; // default
            }
            internal set
            {
                txtGridSize.Text = value.ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
