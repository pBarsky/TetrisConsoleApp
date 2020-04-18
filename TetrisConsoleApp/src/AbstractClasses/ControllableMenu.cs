using System;
using System.Diagnostics;

namespace TetrisConsoleApp.AbstractClasses
{
    abstract class ControllableMenu
    {
        protected bool _running;
        protected bool _refresh;
        protected int _offset;
        protected abstract void Show(int param);

        public virtual void Run()
        {
            Console.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Show(0);
            ConsoleUtilities.HideCursor();
            _offset = 0;
            _running = true;
            while(_running)
            {
                if(stopwatch.ElapsedMilliseconds < 50) continue;

                HandleInput();
                if(_refresh)
                {
                    Show(_offset);
                    _refresh = false;
                }
                stopwatch.Restart();
            }
        }
        protected abstract void HandleInput();

    }
}
