using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    class TestBrick : Brick
    {
        public TestBrick(int x = 0, int y = 0) : base(0, "TestBrick", x, y)
        {
            shape = new int[3, 3]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
        }
    }
}
