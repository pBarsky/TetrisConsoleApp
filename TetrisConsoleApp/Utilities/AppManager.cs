using System;
using System.Collections.Generic;
using TetrisConsoleApp.AbstractClasses;
using TetrisConsoleApp.Gameplay;

namespace TetrisConsoleApp.Utilities
{
    internal class AppManager : ControllableMenu
    {
        private readonly Game _game;
        private readonly ScoreboardManager _scoreboardManager;
        private readonly List<Tuple<string, Action>> _menuActions = new List<Tuple<string, Action>>();

        private readonly string[] _helpStrings = {
            $"\n{"DownArrow",-10} -> scroll down",
            $"{"UpArrow",-10} -> scroll up",
            $"{"ESC",-10} -> EXIT",
            $"{"ENTER",-10} -> select"
        };

        public AppManager()
        {
            _game = new Game();
            _scoreboardManager = new ScoreboardManager();
            _menuActions.Add(new Tuple<string, Action>("Play", _game.Play));
            _menuActions.Add(new Tuple<string, Action>("Scoreboard", _scoreboardManager.PrepareThenRun));
            _menuActions.Add(new Tuple<string, Action>("Exit", () => Running = false));
            Running = true;
            Refresh = true;
        }

        protected override void Show(int index)
        {
            Console.SetCursorPosition(0, 3);
            PrintMenuActions(index);
            PrintHelpStrings();
        }

        private void PrintHelpStrings()
        {
            foreach (var helpString in _helpStrings)
            {
                Console.WriteLine(helpString);
            }
        }

        private void PrintMenuActions(int index)
        {
            for (var i = 0; i < _menuActions.Count; i++)
            {
                if (i == index)
                    ConsoleUtilities.ColorWriteLine($"{_menuActions[i].Item1,16}", ConsoleColor.Black, ConsoleColor.White);
                else
                    Console.WriteLine($"{_menuActions[i].Item1,-16}" + new string(' ', 20));
            }
        }

        protected override void HandleInput()
        {
            var key = KeyboardHandler.GetDirection();
            switch (key)
            {
                case KeyCommand.Down:
                    Refresh = true;
                    Offset = ++Offset % _menuActions.Count;
                    break;

                case KeyCommand.Up:
                    Refresh = true;
                    Offset = Offset == 0 ? _menuActions.Count - 1 : --Offset % _menuActions.Count;
                    break;

                case KeyCommand.Enter:
                    Refresh = true;
                    _menuActions[Offset].Item2();
                    Console.Clear();
                    break;

                case KeyCommand.Escape:
                    Running = false;
                    break;
            }
        }
    }
}