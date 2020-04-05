using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class ZigZagBrick : Brick
    {
        public ZigZagBrick(int size = 3, int posX = 0, int posY = 0) : base(size, "ZigZagBrick", posX, posY)
        {
            shape[0, 0] = 1;
            shape[0, 1] = 1;
            shape[1, 1] = 1;
            shape[1, 2] = 1;
        }
    }
}
