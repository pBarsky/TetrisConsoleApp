using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    static class AppManager
    {
        private static Game _game;
        private static bool _running;
        private static readonly List<Tuple<string, Action>> _menuActions = new List<Tuple<string, Action>>();
        private static int _currentAction;
        static AppManager()
        {
            _game = new Game();
            _menuActions.Add(new Tuple<string, Action>("Play", _game.Play));
            _menuActions.Add(new Tuple<string, Action>("Leaderboard", _game.Play));
            _menuActions.Add(new Tuple<string, Action>("Exit", () => _running = false));
            _currentAction = 0;
        }

        public void MainMenu()
        {
            Stopwatch stopwatch = new Stopwatch();
            while(_running)
            {
                if(stopwatch.ElapsedMilliseconds < 50) continue;
                var move = KeyboardHandler.GetDirection();

            }
        }

        private void ShowMainMenu(int index)
        {
            Console.Clear();
            for(int i = 0; i < _menuActions.Count; i++)
            {
                if(i == index)
                    ConsoleUtilities.ColorConsoleWriteLine($"{i + 1}. {_menuActions[i].Item1}", ConsoleColor.Black, ConsoleColor.White);
                else
                    Console.WriteLine(_menuActions[i].Item1);
            }
        }
        private void ShowScoreboard()
        {

        }

        private void HandleInput()
        {
            var key = KeyboardHandler.GetDirection();
            switch(key)
            {
                case KeyCommand.Down:
                    _currentAction = ++_currentAction % _menuActions.Count;
                    break;
                case KeyCommand.Up:
                    _currentAction = --_currentAction % _menuActions.Count;
                    break;
                case KeyCommand.Enter:
                    break;
                case KeyCommand.Escape:
                    _running = false;
                    break;
                case KeyCommand.None:
                default:

                    break;
            }
        }
    }
}
