using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mosaic_editor.Classes
{
    class PuzzleMerger
    {
        internal void run(string baseFolder, string puzzleSetName)
        {
            //var baseFolder = Properties.Settings.Default.PuzzleEngineFolder;
            //if (!Directory.Exists(baseFolder))
            //{
            //    MessageBox.Show("No puzzle engine folder has been set.  Please go to File | Preferences and fix this.", "Mosaic Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // bundle all puzzles in this folder into one set of puzzles

            // var folder = Path.GetDirectoryName(filename);
            var tierFolders = Directory.GetDirectories(baseFolder, "tier *");
            // var result = "export default [\n    [\n";
            var sb = new StringBuilder();
            sb.AppendLine("export default [");
            // sb.AppendLine("  [");
            var firstFolder = true;
            foreach (var folder in tierFolders)
            {
                if (!firstFolder) sb.AppendLine(",");
                firstFolder = false;
                sb.AppendLine("  [");   // start of tier

                var firstPuzzle = true;
                foreach (var path in Directory.GetFiles(folder, "*.json"))
                {
                    var puzzle = File.ReadAllText(path);          // read one complete puzzle (JSON)
                    // result += comma + "\n" + puzzle + "\n";
                    /// comma = ",";
                    if (!firstPuzzle) sb.AppendLine(",");
                    sb.Append(puzzle);
                    firstPuzzle = false;
                }

                sb.Append("  ]");   // end of tier
            }
            sb.AppendLine();
            sb.AppendLine("]"); // end of tiers

            // result += "   ]\n];";

            var outputFile = Path.Combine(baseFolder, puzzleSetName);
            File.WriteAllText(outputFile, sb.ToString());
            Console.WriteLine($"puzzle set written to \"{outputFile}\"");
        }
    }
}
