using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TetrisConsoleApp.AbstractClasses;
using TetrisConsoleApp.BoardManagement;
using TetrisConsoleApp.Bricks;
using TetrisConsoleApp.Utilities;

namespace TetrisConsoleApp.Gameplay
{
    internal class Game
    {
        private Brick _currentBrick;
        private readonly Board _board;
        private readonly ScoreWriter _scoreWriter;
        private bool _alive = true;
        private bool _hasChanged;
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private int _score;
        private static List<Brick> _allAvailableBricks;
        private readonly BricksQueue _bricksQueue = new BricksQueue();

        private readonly string[] _helpStrings =
        {
            $"\t{"UpArrow",-10} -> ROTATE",
            $"\t{"DownArrow",-10} -> GO DOWN (+1 POINT)",
            $"\t{"LeftArrow",-10} -> MOVE LEFT",
            $"\t{"RightArrow",-10} -> MOVE RIGHT",
            $"\t{"ESC",-10} -> GIVE UP"
        };

        public Game(int boardHeight = 20, int boardWidth = 10)
        {
            _board = new Board(boardWidth, boardHeight);
            var bricks = typeof(Brick).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Brick)))
                .Select(t => (Brick)Activator.CreateInstance(t));
            _allAvailableBricks = bricks.ToList();
            _scoreWriter = new ScoreWriter();
        }

        private void Show()
        {
            ClearBricksQueueBuffer();
            Console.SetCursorPosition(0, 0);
            var output = PrepareOutput();
            Console.Write(output);
        }

        private string PrepareOutput()
        {
            var output = string.Empty;
            var buffer = _board.Buffer;
            var brickQueueBuffer = _bricksQueue.Buffer;
            buffer[0] += $"\tScore: {_score}\n";
            ;
            for (var i = 1; i < buffer.Length; i++)
            {
                output += buffer[i];
                if (i <= brickQueueBuffer.Length)
                    output += '\t' + brickQueueBuffer[i - 1];
                if (i <= _helpStrings.Length)
                    output += _helpStrings[i - 1];
                output += '\n';
            }

            return output;
        }

        private void ClearBricksQueueBuffer()
        {
            var clearLine = new string(' ', 20);
            for (var i = 1; i < _board.Height; i++)
            {
                Console.SetCursorPosition(_board.Width + 2, 1 + i);
                Console.Write(clearLine);
            }
        }

        public void Play()
        {
            Console.Clear();
            ConsoleUtilities.HideCursor();
            SeedQueue();
            NextBrick();
            Show();
            _alive = true;
            GameLoop();
            GameOver();
        }

        private void GameLoop()
        {
            var stopwatch = new Stopwatch();
            var millisecondsPassed = 0L;
            stopwatch.Start();
            while (_alive)
            {
                if (stopwatch.ElapsedMilliseconds <= 100) continue;
                _board.ShallowClear();
                HandlePlayerMovement(KeyboardHandler.GetDirection(), true);
                if (millisecondsPassed > 1000)
                {
                    _board.ShallowClear();
                    HandlePlayerMovement(KeyCommand.Down);
                    millisecondsPassed = 0L;
                }

                if (_hasChanged)
                    Show();
                _hasChanged = false;
                millisecondsPassed += stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();
            }
        }

        private void GameOver()
        {
            Console.WriteLine($"\n\nGAME OVER\n\tYOU'VE SCORED: {_score} POINTS!!");
            ConsoleUtilities.ShowCursor();
            Console.WriteLine("Please enter your name: ");
            ConsoleUtilities.HideCursor();
            _scoreWriter.SaveScore(Console.ReadLine(), _score);
            Console.WriteLine("RETRY? (y\\n)");
            GameOverInputLoop();
            return;
        }

        private void GameOverInputLoop()
        {
            while (HandleGameOverInput())
            {
            }
        }

        private bool HandleGameOverInput()
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Y:
                    RestartGame();
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

        private void RestartGame()
        {
            _score = 0;
            _board.DeepClear();
            _bricksQueue.Clear();
            Play();
        }

        private void HandlePlayerMovement(KeyCommand direction, bool fastForward = false)
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
                    Suicide();
                    break;
            }
        }

        private void Suicide()
        {
            _alive = false;
        }

        private void HandleUpKeyPress()
        {
            _currentBrick.DoRotate(false);
            if (_board.IsColliding(_currentBrick, 0, 0))
            {
                _currentBrick.DoRotate();
                return;
            }

            _hasChanged = true;
            _board.InsertBrick(_currentBrick);
        }

        private void HandleRightKeyPress()
        {
            if (_board.IsColliding(_currentBrick, 1, 0))
            {
                return;
            }

            _hasChanged = true;
            _currentBrick.MoveRight();
            _board.InsertBrick(_currentBrick);
        }

        private void HandleLeftKeyPress()
        {
            if (_board.IsColliding(_currentBrick, -1, 0))
            {
                return;
            }

            _hasChanged = true;
            _currentBrick.MoveLeft();
            _board.InsertBrick(_currentBrick);
        }

        private void HandleDownKeyPress(bool fastForward)
        {
            if (_board.IsColliding(_currentBrick, 0, 1))
            {
                _board.FreezeBrick(_currentBrick);
                _score += _board.Gravitate(_board.Width);
                NextBrick();
                return;
            }

            _hasChanged = true;
            _currentBrick.MoveDown();
            if (fastForward)
                _score += 1;
            _board.InsertBrick(_currentBrick);
        }

        private void NextBrick()
        {
            _currentBrick = _bricksQueue.Dequeue();
            EnqueueNewBrick();
            _currentBrick.RestartPosition(_random.Next(_board.Width - _currentBrick.Width));
            if (_board.IsColliding(_currentBrick, 0, 0))
                _alive = false;
        }

        private void SeedQueue(int size = 3)
        {
            for (var i = 0; i < size; i++)
                EnqueueNewBrick();
        }

        private void EnqueueNewBrick()
        {
            _bricksQueue.Enqueue(_allAvailableBricks[_random.Next(_allAvailableBricks.Count)].DeepCopy());
        }
    }
}