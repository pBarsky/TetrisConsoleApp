using System;
using System.Collections.Generic;
using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp
{
    class AppManager : ControllableMenu
    {
        private Game _game;
        private ScoreboardManager _scoreboardManager;
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
            _menuActions.Add(new Tuple<string, Action>("Scoreboard", _scoreboardManager.Run));
            _menuActions.Add(new Tuple<string, Action>("Exit", () => _running = false));
            _running = true;
            _refresh = true;
        }

        protected override void Show(int index)
        {
            Console.SetCursorPosition(0, 3);
            for(int i = 0; i < _menuActions.Count; i++)
            {
                if(i == index)
                    ConsoleUtilities.ColorWriteLine($"{_menuActions[i].Item1,16}", ConsoleColor.Black, ConsoleColor.White);
                else
                    Console.WriteLine($"{_menuActions[i].Item1,-16}" + new string(' ', 20));
            }
            foreach(string helpString in _helpStrings)
            {
                Console.WriteLine(helpString);
            }
        }

        protected override void HandleInput()
        {
            var key = KeyboardHandler.GetDirection();
            switch(key)
            {
                case KeyCommand.Down:
                    _refresh = true;
                    _offset = ++_offset % _menuActions.Count;
                    break;
                case KeyCommand.Up:
                    _refresh = true;
                    _offset = _offset == 0 ? _menuActions.Count - 1 : --_offset % _menuActions.Count;
                    break;
                case KeyCommand.Enter:
                    _refresh = true;
                    _menuActions[_offset].Item2();
                    Console.Clear();
                    break;
                case KeyCommand.Escape:
                    _running = false;
                    break;
            }
        }
    }
}
