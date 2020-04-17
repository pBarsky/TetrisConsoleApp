using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace TetrisConsoleApp
{

    class Game
    {
        private Brick _currentBrick;
        private Board _board;
        private bool _alive = true;
        private bool _hasChanged;
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private int _score;
        private static List<Brick> _allAvailableBricks;
        private BricksQueue _bricksQueue = new BricksQueue();

        public Game(int boardHeight = 20, int boardWidth = 10)
        {
            _board = new Board(boardWidth, boardHeight);
            IEnumerable<Brick> bricks = typeof(Brick).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Brick)))
                .Select(t => (Brick)Activator.CreateInstance(t));
            _allAvailableBricks = bricks.ToList();
        }

        private void Show()
        {
            string output = "";
            string[] buffer = _board.Buffer;
            string[] brickQueueBuffer = _bricksQueue.Buffer;
            buffer[0] += "\tScore: " + _score.ToString() + "\n";
            output += buffer[0];
            ClearBricksQueueBuffer();
            Console.SetCursorPosition(0, 0);
            for(int i = 1; i < buffer.Length; i++)
            {
                output += buffer[i];
                if(i <= brickQueueBuffer.Length)
                    output += '\t' + brickQueueBuffer[i - 1];
                output += '\n';
            }
            Console.Write(output);
        }

        private void ClearBricksQueueBuffer()
        {
            string clearLine = new string(' ', 20);
            for(int i = 1; i < _board.Height; i++)
            {
                Console.SetCursorPosition(_board.Width + 2, 1 + i);
                Console.Write(clearLine);
            }
        }

        public void Play()
        {
            Console.Clear();
            Console.CursorVisible = false;
            SeedQueue();
            NextBrick();
            _alive = true;
            Stopwatch stopwatch = new Stopwatch();
            long millisecondsPassed = 0L;
            stopwatch.Start();
            while(_alive)
            {
                if(stopwatch.ElapsedMilliseconds <= 100) continue;
                _board.ShallowClear();
                HandlePlayerMovement(KeyboardHandler.GetDirection(), true);
                if(millisecondsPassed > 1000)
                {
                    _board.ShallowClear();
                    HandlePlayerMovement(KeyCommand.Down);
                    millisecondsPassed = 0L;
                }
                if(_hasChanged)
                    Show();
                _hasChanged = false;
                millisecondsPassed += stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();
            }
            GameOver();
        }

        private void GameOver()
        {
            Console.WriteLine($"\n\nGAME OVER\n\tYOU'VE SCORED: {_score} POINTS!!");
            Console.CursorVisible = true;
            Console.WriteLine("Please enter your name: ");
            ScoreboardManager.SaveScore(Console.ReadLine(), _score);
            Console.WriteLine("RETRY? (y\\n)");
            while(true)
            {
                var key = Console.ReadKey(true).Key;
                switch(key)
                {
                    case ConsoleKey.Y:
                        RestartGame();
                        break;
                    case ConsoleKey.N:
                    case ConsoleKey.Escape:
                        return;
                    default:
                        Console.WriteLine("RETRY? (y\\n)");
                        break;
                }
            }

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
            switch(direction)
            {
                case KeyCommand.Down:
                    if(!_board.IsColliding(_currentBrick, 0, 1))
                    {
                        _hasChanged = true;
                        _currentBrick.MoveDown();
                        if(fastForward)
                            _score += 1;
                        _board.InsertBrick(_currentBrick);
                    }
                    else
                    {
                        _board.FreezeBrick(_currentBrick);
                        _score += _board.Gravitate(_board.Width);
                        NextBrick();
                    }
                    break;
                case KeyCommand.Left:
                    if(!_board.IsColliding(_currentBrick, -1, 0))
                    {
                        _hasChanged = true;
                        _currentBrick.MoveLeft();
                        _board.InsertBrick(_currentBrick);
                    }
                    break;
                case KeyCommand.Right:
                    if(!_board.IsColliding(_currentBrick, 1, 0))
                    {
                        _hasChanged = true;
                        _currentBrick.MoveRight();
                        _board.InsertBrick(_currentBrick);
                    }
                    break;
                case KeyCommand.Up:
                    _currentBrick.DoRotate(false);
                    if(!_board.IsColliding(_currentBrick, 0, 0))
                    {
                        _hasChanged = true;
                        _board.InsertBrick(_currentBrick);
                    }
                    else
                    {
                        _currentBrick.DoRotate();
                    }
                    break;
                case KeyCommand.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void NextBrick()
        {
            _currentBrick = _bricksQueue.Dequeue();
            EnqueueNewBrick();
            _currentBrick.RestartPosition(_random.Next(_board.Width - _currentBrick.Width));
            if(_board.IsColliding(_currentBrick, 0, 0))
                _alive = false;
        }

        private void SeedQueue(int size = 3)
        {
            for(int i = 0; i < size; i++)
                EnqueueNewBrick();
        }

        private void EnqueueNewBrick()
        {
            _bricksQueue.Enqueue(_allAvailableBricks[_random.Next(_allAvailableBricks.Count)].DeepCopy());
        }
    }
}
