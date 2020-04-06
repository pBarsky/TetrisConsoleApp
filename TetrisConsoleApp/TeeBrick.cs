using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class TeeBrick : Brick
    {
        public TeeBrick(int size = 3, int x = 0, int y = 0) : base(size, "TeeBrick", x, y)
        {
            for(int i = 0; i < size; i++)
                shape[0, i] = 1;
            for(int i = 0; i < size - 1; i++)
                shape[i, size / 2] = 1;
        }
        public TeeBrick() : this(3, 0, 0)
        {
        }
    }
}
