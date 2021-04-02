using System.Collections.Generic;
using System.Linq;
using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.Bricks
{
    internal class BricksQueue
    {
        private readonly Queue<Brick> _bricksQueue;

        public string[] Buffer
        {
            get
            {
                var height = _bricksQueue.Sum(brick => brick.Height) + _bricksQueue.Count;
                var buffer = new string[height];
                var lineCounter = 0;
                var brickCounter = 0;
                foreach (var brick in _bricksQueue)
                {
                    buffer[lineCounter++] = $"Brick {++brickCounter}.:";
                    foreach (var s in brick.Buffer)
                    {
                        buffer[lineCounter++] = '\t' + s;
                    }
                }
                return buffer;
            }
        }

        public BricksQueue()
        {
            _bricksQueue = new Queue<Brick>();
        }

        public void Enqueue(Brick brick)
        {
            _bricksQueue.Enqueue(brick);
        }

        public Brick Dequeue()
        {
            return _bricksQueue.Dequeue();
        }

        public void Clear()
        {
            _bricksQueue.Clear();
        }
    }
}