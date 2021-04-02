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

        private static string[] _helpStrings =
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
            _running = true;
        }

        private List<Tuple<string, int>> ReadScores(bool sorted = true)
        {
            var scores = new List<Tuple<string, int>>();
            try
            {
                string[] lines = File.ReadAllLines(FilePath);
                foreach (string line in lines)
                {
                    string[] keyVal = line.Split(':');
                    scores.Add(new Tuple<string, int>(keyVal[0], int.TryParse(keyVal[1], out int score) ? score : 0));
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
            // memory wise, very bad. May fix later.
            // TODO: Implement pagination. Please, just do it.
            string output = "";
            Console.SetCursorPosition(0, 0);
            for (int i = offset; i < offset + Range && i < _records.Count; i++)
            {
                (string name, int score) = _records[i];
                output += ($"{i + 1,3}.{name,-16}:{score,10}\n");
            }
            foreach (string helpString in _helpStrings)
            {
                output += helpString + new string(' ', helpString.Length) + '\n';
            }
            // newN -> number of rows left to 'cover', so that old data doesnt remain visible
            int newN = _records.Count - offset < 10 ? 10 : 0;
            for (int i = 0; i < newN; i++)
                output += new string(' ', 64) + '\n';
            Console.WriteLine(output);
        }

        protected override void HandleInput()
        {
            var key = KeyboardHandler.GetDirection();
            switch (key)
            {
                case KeyCommand.Down:
                    if (_offset + Range < _records.Count)
                    {
                        _offset += Range;
                        _refresh = true;
                    }
                    break;

                case KeyCommand.Up:
                    if (_offset - Range >= 0)
                    {
                        _offset -= Range;
                        _refresh = true;
                    }
                    break;

                case KeyCommand.Enter:
                    RefreshData();
                    _refresh = true;
                    break;

                case KeyCommand.Escape:
                    _running = false;
                    break;
            }
        }
    }
}