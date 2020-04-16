using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    static class ConsoleUtilities
    {
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
        public static void ColorConsoleWrite(string text,
            ConsoleColor foregroundColor = DefaultForegroundColor,
            ConsoleColor backgroundColor = DefaultBackgroundColor
            )
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ForegroundColor = DefaultForegroundColor;
            Console.BackgroundColor = DefaultBackgroundColor;
        }

        public static void ColorConsoleWriteLine(string text,
            ConsoleColor foregroundColor = DefaultForegroundColor,
            ConsoleColor backgroundColor = DefaultBackgroundColor
            )
        {
            ColorConsoleWrite(text + '\n', foregroundColor, backgroundColor);
        }


    }
}
