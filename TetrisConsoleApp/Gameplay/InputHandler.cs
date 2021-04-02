using System;
using TetrisConsoleApp.Utilities;

namespace TetrisConsoleApp.Gameplay
{
    internal class InputHandler
    {
        private readonly Game _game;

        public InputHandler(Game game)
        {
            _game = game;
        }

        public bool HandleGameOverInput()
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Y:
                    _game.RestartGame();
                    break;

                case ConsoleKey.N:
                case ConsoleKey.Escape:
                    return true;

                default:
                    Console.WriteLine("RETRY? (y\\n)");
                    break;
            }

            return false;
        }

        public void HandlePlayerMovement(KeyCommand direction, bool fastForward = false)
        {
            switch (direction)
            {
                case KeyCommand.Down:
                    HandleDownKeyPress(fastForward);
                    break;

                case KeyCommand.Left:
                    HandleLeftKeyPress();
                    break;

                case KeyCommand.Right:
                    HandleRightKeyPress();
                    break;

                case KeyCommand.Up:
                    HandleUpKeyPress();
                    break;

                case KeyCommand.Escape:
                    _game.Suicide();
                    break;
            }
        }

        private void HandleUpKeyPress()
        {
            _game.CurrentBrick.DoRotate(false);
            if (_game.CurrentBoard.IsColliding(_game.CurrentBrick, 0, 0))
            {
                _game.CurrentBrick.DoRotate();
                return;
            }
            _game.HasChanged = true;
            _game.CurrentBoard.InsertBrick(_game.CurrentBrick);
        }

        private void HandleRightKeyPress()
        {
            if (_game.CurrentBoard.IsColliding(_game.CurrentBrick, 1, 0))
            {
                return;
            }
            _game.HasChanged = true;
            _game.CurrentBrick.MoveRight();
            _game.CurrentBoard.InsertBrick(_game.CurrentBrick);
        }

        private void HandleLeftKeyPress()
        {
            if (_game.CurrentBoard.IsColliding(_game.CurrentBrick, -1, 0))
            {
                return;
            }
            _game.HasChanged = true;
            _game.CurrentBrick.MoveLeft();
            _game.CurrentBoard.InsertBrick(_game.CurrentBrick);
        }

        private void HandleDownKeyPress(bool fastForward)
        {
            if (_game.CurrentBoard.IsColliding(_game.CurrentBrick, 0, 1))
            {
                _game.CurrentBoard.FreezeBrick(_game.CurrentBrick);
                _game.Score += _game.CurrentBoard.Gravitate(_game.CurrentBoard.Width);
                _game.NextBrick();
                return;
            }
            _game.HasChanged = true;
            _game.CurrentBrick.MoveDown();
            if (fastForward)
                _game.Score += 1;
            _game.CurrentBoard.InsertBrick(_game.CurrentBrick);
        }
    }
}