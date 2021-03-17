using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor.Classes
{
    class PuzzleMerger
    {
        internal void run(string filename, string outputName)
        {
            var folder = Path.GetDirectoryName(filename);
            if (Directory.Exists(folder))
            {
                // bundle all puzzles in this folder into one set of puzzles
                var dest = Path.Combine(folder, outputName);
                var result = "export default [\n    [\n";

                var comma = "";
                foreach (var path in Directory.GetFiles(folder, "*.json"))
                {
                    var json = File.ReadAllText(path);
                    //Console.WriteLine(json);
                    result += comma + "\n" + json + "\n";
                    comma = ",";
                }

                result += "   ]\n];";

                File.WriteAllText(dest, result);
                Console.WriteLine($"puzzle set written to \"{dest}\"");
            }
        }
    }
}
