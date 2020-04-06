using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class BeamBrick : Brick
    {
        public BeamBrick(int size = 3, int x = 0, int y = 0) : base(size, "BeamBrick", x, y)
        {
            for(int i = 0; i < size; i++)
                shape[size / 2, i] = 1;
        }

        public BeamBrick() : this(3, 0, 0)
        {
        }
    }
}
