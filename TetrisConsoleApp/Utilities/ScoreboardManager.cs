using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.Utilities
{
    internal class ScoreboardManager : ControllableMenu
    {
        private List<Tuple<string, int>> _records;
        protected const string FilePath = @".\scores.txt";
        private const int Range = 10;

        private static readonly string[] HelpStrings =
        {
            new string(' ', 32),
            $"{"DownArrow",-10} -> SCROLL DOWN",
            $"{"UpArrow",-10} -> SCROLL UP",
            $"{"ENTER",-10} -> REFRESH",
            $"{"ESC",-10} -> BACK"
        };

        public ScoreboardManager(bool readData = true)
        {
            if (readData)
                RefreshData();
            Running = true;
        }

        private List<Tuple<string, int>> ReadScores(bool sorted = true)
        {
            var scores = new List<Tuple<string, int>>();
            try
            {
                var lines = File.ReadAllLines(FilePath);
                foreach (string line in lines)
                {
                    var keyVal = line.Split(':');
                    scores.Add(new Tuple<string, int>(keyVal[0], int.TryParse(keyVal[1], out var score) ? score : 0));
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("COULD NOT OPEN SCORES FILE.");
                Console.WriteLine(e.Message);
            }

            if (!sorted) return scores;
            var scoreSorting = scores.OrderByDescending(row => row.Item2);
            return scoreSorting.ToList();
        }

        private void RefreshData()
        {
            _records = ReadScores();
        }

        public void PrepareThenRun()
        {
            RefreshData();
            Run();
        }

        protected override void Show(int offset)
        {
            var output = "";
            Console.SetCursorPosition(0, 0);
            output = PrintRecords(offset, output);
            output = PrintHelpStringsForRecords(output);
            output = CoverOldOutput(offset, output);

            Console.WriteLine(output);
        }

        private string CoverOldOutput(int offset, string output)
        {
            var rowsLeftToCover = _records.Count - offset < 10 ? 10 : 0;
            for (var i = 0; i < rowsLeftToCover; i++)
            {
                output += new string(' ', 64) + '\n';
            }

            return output;
        }

        private static string PrintHelpStringsForRecords(string output)
        {
            foreach (var helpString in HelpStrings)
            {
                output += helpString + new string(' ', helpString.Length) + '\n';
            }

            return output;
        }

        private string PrintRecords(int offset, string output)
        {
            for (var i = offset; i < offset + Range && i < _records.Count; i++)
            {
                var (name, score) = _records[i];
                output += ($"{i + 1,3}.{name,-16}:{score,10}\n");
            }

            return output;
        }

        protected override void HandleInput()
        {
            var key = KeyboardHandler.GetDirection();
            switch (key)
            {
                case KeyCommand.Down:
                    HandleDownPress();
                    break;

                case KeyCommand.Up:
                    HandleUpPress();
                    break;

                case KeyCommand.Enter:
                    RefreshData();
                    Refresh = true;
                    break;

                case KeyCommand.Escape:
                    Running = false;
                    break;
            }
        }

        private void HandleUpPress()
        {
            if (Offset - Range < 0) return;
            Offset -= Range;
            Refresh = true;
        }

        private void HandleDownPress()
        {
            if (Offset + Range >= _records.Count) return;
            Offset += Range;
            Refresh = true;
        }
    }
}