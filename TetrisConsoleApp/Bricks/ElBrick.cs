using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.Bricks
{
    internal class ElBrick : Brick
    {
        public ElBrick(int size = 3, int x = 0, int y = 0) : base(size, "CrossBrick", x, y)
        {
            for (var i = 0; i < size; i++)
            {
                Shape[0, i] = 1;
            }

            for (var i = 0; i < size - 1; i++)
            {
                Shape[i, size - 1] = 1;
            }
        }

        public ElBrick() : this(3, 0, 0)
        {
        }
    }
}