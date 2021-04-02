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
        public Brick CurrentBrick { get; private set; }
        public Board CurrentBoard { get; }
        private readonly ScoreWriter _scoreWriter;
        private bool Alive { get; set; } = true;
        public bool HasChanged { get; set; }
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        public int Score { get; set; }
        private static List<Brick> _allAvailableBricks;
        private readonly BricksQueue _bricksQueue = new BricksQueue();
        private readonly InputHandler _inputHandler;

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
            _inputHandler = new InputHandler(this);
            CurrentBoard = new Board(boardWidth, boardHeight);
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
            var buffer = CurrentBoard.Buffer;
            var brickQueueBuffer = _bricksQueue.Buffer;
            buffer[0] += $"\tScore: {Score}\n";
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
            for (var i = 1; i < CurrentBoard.Height; i++)
            {
                Console.SetCursorPosition(CurrentBoard.Width + 2, 1 + i);
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
            Alive = true;
            GameLoop();
            GameOver();
        }

        private void GameLoop()
        {
            var stopwatch = new Stopwatch();
            var millisecondsPassed = 0L;
            stopwatch.Start();
            while (Alive)
            {
                if (stopwatch.ElapsedMilliseconds <= 100) continue;
                CurrentBoard.ShallowClear();
                _inputHandler.HandlePlayerMovement(KeyboardHandler.GetDirection(), true);
                if (millisecondsPassed > 1000)
                {
                    CurrentBoard.ShallowClear();
                    _inputHandler.HandlePlayerMovement(KeyCommand.Down);
                    millisecondsPassed = 0L;
                }

                if (HasChanged)
                    Show();
                HasChanged = false;
                millisecondsPassed += stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();
            }
        }

        private void GameOver()
        {
            Console.WriteLine($"\n\nGAME OVER\n\tYOU'VE SCORED: {Score} POINTS!!");
            ConsoleUtilities.ShowCursor();
            Console.WriteLine("Please enter your name: ");
            ConsoleUtilities.HideCursor();
            _scoreWriter.SaveScore(Console.ReadLine(), Score);
            Console.WriteLine("RETRY? (y\\n)");
            GameOverInputLoop();
        }

        private void GameOverInputLoop()
        {
            while (_inputHandler.HandleGameOverInput())
            {
            }
        }

        public void RestartGame()
        {
            Score = 0;
            CurrentBoard.DeepClear();
            _bricksQueue.Clear();
            Play();
        }

        public void Suicide()
        {
            Alive = false;
        }

        public void NextBrick()
        {
            CurrentBrick = _bricksQueue.Dequeue();
            EnqueueNewBrick();
            CurrentBrick.RestartPosition(_random.Next(CurrentBoard.Width - CurrentBrick.Width));
            if (CurrentBoard.IsColliding(CurrentBrick, 0, 0))
                Alive = false;
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