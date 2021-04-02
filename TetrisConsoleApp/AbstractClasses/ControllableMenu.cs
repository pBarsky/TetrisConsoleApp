using System;
using System.Diagnostics;
using TetrisConsoleApp.Utilities;

namespace TetrisConsoleApp.AbstractClasses
{
    internal abstract class ControllableMenu
    {
        protected bool Running;
        protected bool Refresh;
        protected int Offset;

        protected abstract void Show(int param);

        public void Run()
        {
            Console.Clear();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Show(0);
            ConsoleUtilities.HideCursor();
            Offset = 0;
            Running = true;
            while (Running)
            {
                if (stopwatch.ElapsedMilliseconds < 50) continue;

                HandleInput();
                if (Refresh)
                {
                    Show(Offset);
                    Refresh = false;
                }
                stopwatch.Restart();
            }
        }

        protected abstract void HandleInput();
    }
}