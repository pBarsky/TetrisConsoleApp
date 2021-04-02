using System;

namespace TetrisConsoleApp.Utilities
{
    internal static class ConsoleUtilities
    {
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public static void ColorWrite(string text, ConsoleColor foregroundColor = DefaultForegroundColor, ConsoleColor backgroundColor = DefaultBackgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ForegroundColor = DefaultForegroundColor;
            Console.BackgroundColor = DefaultBackgroundColor;
        }

        public static void ColorWriteLine(string text, ConsoleColor foregroundColor = DefaultForegroundColor, ConsoleColor backgroundColor = DefaultBackgroundColor)
        {
            ColorWrite(text + '\n', foregroundColor, backgroundColor);
        }

        public static void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public static void ShowCursor()
        {
            Console.CursorVisible = true;
        }
    }
}