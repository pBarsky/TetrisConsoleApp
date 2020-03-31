using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class ElBrick : Brick
    {
        public ElBrick(int size = 3, int x = 0, int y = 0) : base(size, "CrossBrick", x, y)
        {
            for(int i = 0; i < size; i++)
            {
                shape[i, size - 1] = 1;
                shape[0, i] = 1;
            }
        }
    }
}
