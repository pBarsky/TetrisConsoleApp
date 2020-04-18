using System;

namespace TetrisConsoleApp
{
    enum KeyCommand
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Enter = 4,
        Escape = 5,
        None = -1
    }

    static class KeyboardHandler
    {
        public static KeyCommand GetDirection()
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
                        resultKeyCommand = KeyCommand.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        resultKeyCommand = KeyCommand.Down;
                        break;
                    case ConsoleKey.Enter:
                        resultKeyCommand = KeyCommand.Enter;
                        break;
                    case ConsoleKey.Escape:
                        resultKeyCommand = KeyCommand.Escape;
                        break;
                }
            }
            return resultKeyCommand;
        }
    }
}
