using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.Bricks
{
    internal class BeamBrick : Brick
    {
        public BeamBrick(int size = 3, int x = 0, int y = 0) : base(size, "BeamBrick", x, y)
        {
            for (var i = 0; i < size; i++)
            {
                Shape[size / 2, i] = 1;
            }
        }

        public BeamBrick() : this(3, 0, 0)
        {
        }
    }
}