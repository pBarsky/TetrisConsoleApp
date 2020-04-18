using System;
using System.IO;

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
            input = input.Trim();
            input = input.Replace(":", "");
            if(input.Length > 16)
                input = input.Substring(0, 16);
        }
    }
}
