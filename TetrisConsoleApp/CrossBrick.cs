using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class CrossBrick : Brick
    {
        public CrossBrick(int size = 3, int x = 0, int y = 0) : base(size, "CrossBrick", x, y)
        {
            for(int i = 0; i < size; i++)
            {
                shape[size / 2, i] = 1;
                shape[i, size / 2] = 1;
            }
        }
        public CrossBrick() : this(3, 0, 0)
        {
        }
    }
}
