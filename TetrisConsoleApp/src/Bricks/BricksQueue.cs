using System.Collections.Generic;
using System.Linq;

namespace TetrisConsoleApp
{
    class BricksQueue
    {


        private Queue<Brick> _bricksQueue;
        public string[] Buffer {
            get {
                int height = _bricksQueue.Sum(brick => brick.Height) + _bricksQueue.Count;
                string[] buffer = new string[height];
                int lineCounter = 0;
                int brickCounter = 0;
                foreach(Brick brick in _bricksQueue)
                {
                    buffer[lineCounter++] = $"Brick {++brickCounter}.:";
                    foreach(string s in brick.Buffer)
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
