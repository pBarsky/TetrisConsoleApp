using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TetrisConsoleApp
{
    static class ScoreboardManager
    {
        private const string FilePath = @".\scores.txt";
        public static void SaveScore(string name, int score)
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
        public static List<Tuple<string, int>> ReadScores(bool sorted = true)
        {
            var scores = new List<Tuple<string, int>>();
            try
            {
                string[] lines = File.ReadAllLines(FilePath);
                foreach(string line in lines)
                {
                    string[] keyVal = line.Split(':');
                    scores.Add(new Tuple<string, int>(keyVal[0], int.TryParse(keyVal[1], out int score) ? score : 0));
                }
            }
            catch(IOException e)
            {
                Console.WriteLine("COULD NOT OPEN SCORES FILE.");
                Console.WriteLine(e.Message);
            }

            if(!sorted) return scores;
            var scoreSorting = scores.OrderBy(row => row.Item2);
            return scoreSorting.ToList();

        }

        private static void SanitizeInput(ref string input)
        {
            // Removing only ':' (as for now), because i split my records using it.
            input = input.Replace(":", "");
        }
    }
}
