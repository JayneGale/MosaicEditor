using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mosaic_editor.Classes.Constants;

namespace Mosaic_editor
{
    public partial class frmSave : Form
    {
        Classes.Puzzle puzzle;

        public string filename;

        public frmSave(Classes.Puzzle puzzle)
        {
            InitializeComponent();
            this.puzzle = puzzle;
        }

        private void frmSave_Load(object sender, EventArgs e)
        {
            txtName.Text = puzzle.name;

            txtTier.Value = puzzle.tier;

            rbMatch1.Checked = puzzle.puzzleType == PuzzleType.MATCH1;
            rbMatch3.Checked = puzzle.puzzleType == PuzzleType.MATCH3;
            rbMatch5.Checked = puzzle.puzzleType == PuzzleType.MATCH5;

            chkDontMangle.Checked = puzzle.dontMangle;
            chkFixedColors.Checked = puzzle.fixedColors;

            txtSwaps.Value = puzzle.swaps;
            txtPlays.Value = puzzle.plays;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            puzzle.name = txtName.Text.Trim();

            if (rbMatch1.Checked)
                puzzle.puzzleType = PuzzleType.MATCH1;
            else if (rbMatch3.Checked)
                puzzle.puzzleType = PuzzleType.MATCH3;
            else
                puzzle.puzzleType = PuzzleType.MATCH5;

            puzzle.dontMangle = chkDontMangle.Checked;
            puzzle.fixedColors = chkFixedColors.Checked;

            puzzle.tier = (int)txtTier.Value;
            puzzle.swaps = (int)txtSwaps.Value;
            puzzle.plays = (int)txtPlays.Value;

            if (puzzle.save())
            {
                this.filename = puzzle.filename;
                puzzle.isDirty = false;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
