using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.Bricks
{
    internal class SquareBrick : Brick
    {
        public SquareBrick(int size = 2, int x = 0, int y = 0) : base(size, "SquareBrick", x, y)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    Shape[i, j] = 1;
                }
            }
        }

        public SquareBrick() : this(2, 0, 0)
        {
        }
    }
}