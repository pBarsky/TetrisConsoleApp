using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.Bricks
{
    internal class TeeBrick : Brick
    {
        public TeeBrick(int size = 3, int x = 0, int y = 0) : base(size, "TeeBrick", x, y)
        {
            for (var i = 0; i < size; i++)
            {
                Shape[0, i] = 1;
            }

            for (var i = 0; i < size - 1; i++)
            {
                Shape[i, size / 2] = 1;
            }
        }

        public TeeBrick() : this(3, 0, 0)
        {
        }
    }
}