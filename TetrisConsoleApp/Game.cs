using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    enum KeyCommand
    {
        Down = 0,
        Left = 1,
        Right = 2,
        RotateLeft = 3,
        None = -1
    }
    class Game
    {
        private Brick _currentBrick = new BeamBrick();
        private Board _board = new Board();
        private bool _alive = true;
        private bool _hasChanged = false;
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private int _score;
        private static List<Brick> _allAvailableBrick;
        private static Game _instance = new Game();
        public static Game Instance => _instance;
        static Game()
        {
            IEnumerable<Brick> bricks = typeof(Brick).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Brick)))
                .Select(t => (Brick)Activator.CreateInstance(t));
            _allAvailableBrick = bricks.Cast<Brick>().ToList();
        }

        private void Show()
        {
            Console.SetCursorPosition(0, 0);
            string output = "";
            string[] buffer = _board.Buffer;
            buffer[0] += "\tScore: " + _score.ToString() + "\n";
            output += buffer[0];
            for(var i = 1; i < buffer.Length; i++)
            {
                output += buffer[i] + "\n";
            }
            Console.Write(output);
        }

        public void Play()
        {
            Console.Clear();
            Console.CursorVisible = false;

            _alive = true;
            Stopwatch stopwatch = new Stopwatch();
            var millisecondsPassed = 0L;
            stopwatch.Start();
            while(_alive)
            {
                if(stopwatch.ElapsedMilliseconds <= 100) continue;
                _board.ShallowClear();
                HandleInput(GetDirection(), true);
                if(millisecondsPassed > 1000)
                {
                    _board.ShallowClear();
                    HandleInput(KeyCommand.Down);
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
            SaveScore(Console.ReadLine());
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
                        key = Console.ReadKey().Key;
                        break;
                }
            }

        }

        private void SaveScore(string name)
        {
            try
            {
                using(var streamWriter = new StreamWriter(@".\scores.txt", true))
                {
                    streamWriter.WriteLine($"{name}: {_score}");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("COULD NOT SAVE SCORE TO FILE");
                Console.WriteLine(e.Message);
            }

        }
        private void RestartGame()
        {
            _score = 0;
            _board.DeepClear();
            Play();
        }

        private void HandleInput(KeyCommand direction, bool fastForward = false)
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
                        _score += _board.Gravitate(_board.CheckBoard(), _board.Width);
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
                case KeyCommand.RotateLeft:
                    _currentBrick.DoRotate(false);
                    if(!_board.IsColliding(_currentBrick, 0, 0))
                    {
                        _hasChanged = true;
                        _board.InsertBrick(_currentBrick);
                    }
                    else
                    {
                        _currentBrick.DoRotate(true);
                    }
                    break;
                case KeyCommand.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private KeyCommand GetDirection()
        {
            KeyCommand resultKeyCommand = KeyCommand.None;
            while(Console.KeyAvailable)
            {
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                        resultKeyCommand = KeyCommand.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        resultKeyCommand = KeyCommand.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        resultKeyCommand = KeyCommand.RotateLeft;
                        break;
                    case ConsoleKey.DownArrow:
                        resultKeyCommand = KeyCommand.Down;
                        break;
                }
            }
            return resultKeyCommand;
        }

        private void NextBrick()
        {
            _currentBrick = _allAvailableBrick[_random.Next(_allAvailableBrick.Count)].DeepCopy();
            _currentBrick.RestartPosition(_random.Next(_board.Width - _currentBrick.Width));
            if(_board.IsColliding(_currentBrick, 0, 0))
                _alive = false;
        }
    }
}
