using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp.Utilities
{
    class ScoreWriter : ScoreboardManager
    {
        public ScoreWriter() : base(false)
        {

        }
        public void SaveScore(string name, int score)
        {
            try
            {
                using(var streamWriter = new StreamWriter(FilePath, true))
                {
                    SanitizeInput(ref name);
                    streamWriter.WriteLine($"{name}: {score}");
                }
            }
            catch(IOException e)
            {
                Console.WriteLine("COULD NOT SAVE SCORE TO FILE");
                Console.WriteLine(e.Message);
            }
        }
        private void SanitizeInput(ref string input)
        {
            // Removing only ':' (as for now), because i split my records using it.
            input = input.Replace(":", "");
        }
    }
}
