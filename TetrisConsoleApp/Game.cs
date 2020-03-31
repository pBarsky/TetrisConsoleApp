using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    enum KeyCommand
    {
        Down = 0,
        Left = 1,
        Right = 2,
        FastDown = 3,
        RotateLeft = 4,
    }
    class Game
    {
        private Brick currentBrick = new BeamBrick();
        private Board board = new Board();
        private bool alive = true;
        private bool hasChanged = false;
        public void Play()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while(alive)
            {
                KeyCommand direction = GetDirection();
                if(stopwatch.ElapsedMilliseconds > 1000)
                {
                    board.ShallowClear();
                    HandleInput(direction);
                    if(hasChanged)
                        board.Show();
                    hasChanged = false;
                    stopwatch.Restart();
                }
            }
        }

        private KeyCommand GetInput()
        {
            return GetDirection();
        }
        private void HandleInput(KeyCommand direction)
        {
            switch(direction)
            {
                case KeyCommand.Down:
                    if(!board.IsColliding(currentBrick, 0, 1))
                    {
                        hasChanged = true;
                        currentBrick.MoveDown();
                        board.InsertBrick(currentBrick);
                    }
                    else
                    {
                        board.FreezeBrick(currentBrick);
                    }
                    break;
                case KeyCommand.Left:
                    if(!board.IsColliding(currentBrick, -1, 0))
                    {
                        hasChanged = true;
                        currentBrick.MoveLeft();
                        board.InsertBrick(currentBrick);
                    }
                    break;
                case KeyCommand.Right:
                    if(!board.IsColliding(currentBrick, 1, 0))
                    {
                        hasChanged = true;
                        currentBrick.MoveRight();
                        board.InsertBrick(currentBrick);
                    }
                    break;
                case KeyCommand.FastDown:
                    // ALERT: FORCES GAME TO UPDATE VIEW
                    if(!board.IsColliding(currentBrick, 0, 1))
                    {
                        // TODO: finish
                        currentBrick.MoveDown();
                        board.InsertBrick(currentBrick);
                        board.Show();
                    }
                    else
                    {
                        board.FreezeBrick(currentBrick);
                    }
                    break;
                case KeyCommand.RotateLeft:
                    currentBrick.DoRotate(false);
                    if(!board.IsColliding(currentBrick, 0, 0))
                    {
                        if(!board.IsColliding(currentBrick, 0, 1))
                        {
                            hasChanged = true;
                            currentBrick.MoveDown();
                            board.InsertBrick(currentBrick);
                        }
                        else
                        {
                            board.FreezeBrick(currentBrick);
                        }
                    }
                    else
                    {
                        currentBrick.DoRotate(true);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private KeyCommand GetDirection()
        {
            KeyCommand resultKeyCommand = KeyCommand.Down;
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
                        resultKeyCommand = KeyCommand.FastDown;
                        break;
                }
            }
            return resultKeyCommand;
        }
    }
}
