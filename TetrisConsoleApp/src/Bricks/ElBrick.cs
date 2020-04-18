namespace TetrisConsoleApp
{
    class ElBrick : Brick
    {
        public ElBrick(int size = 3, int x = 0, int y = 0) : base(size, "CrossBrick", x, y)
        {
            for(int i = 0; i < size; i++)
                shape[0, i] = 1;
            for(int i = 0; i < size - 1; i++)
                shape[i, size - 1] = 1;
        }

        public ElBrick() : this(3, 0, 0)
        {
        }
    }
}
