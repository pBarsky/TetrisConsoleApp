using TetrisConsoleApp.AbstractClasses;

namespace TetrisConsoleApp.BoardManagement
{
    internal class Board
    {
        protected readonly int[,] Tab;
        public int Width { get; }

        public int Height { get; }

        public string[] Buffer
        {
            get
            {
                var buffer = new string[Height + 1];
                buffer[0] = buffer[Height] = " " + new string('=', Width);
                for (var i = 1; i < Height; i++)
                {
                    buffer[i] += "|";
                    for (var j = 0; j < Width; j++)
                    {
                        buffer[i] += Tab[i, j] != 0 ? "#" : " ";
                    }

                    buffer[i] += "|";
                }

                return buffer;
            }
        }

        public Board(int width = 10, int height = 20)
        {
            this.Width = width;
            this.Height = height;
            Tab = new int[height, width];
        }

        public void InsertBrick(Brick brick)
        {
            for (var i = 0; i < brick.Height; i++)
            {
                for (var j = 0; j < brick.Width; j++)
                {
                    if (brick.Shape[i, j] != 0)
                    {
                        Tab[brick.PosY + i, brick.PosX + j] = brick.Shape[i, j];
                    }
                }
            }
        }

        public void DeepClear()
        {
            for (var i = 0; i < Height; i++)
            {
                {
                    for (var j = 0; j < Width; j++)
                    {
                        Tab[i, j] = 0;
                    }
                }
            }
        }

        public void ShallowClear()
        {
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    if (Tab[i, j] == 1)
                    {
                        Tab[i, j] = 0;
                    }
                }
            }
        }

        public bool IsColliding(Brick brick, int offsetX, int offsetY)
        {
            for (var i = 0; i < brick.Height; i++)
            {
                for (var j = 0; j < brick.Width; j++)
                {
                    if (brick.Shape[i, j] != 1)
                    {
                        continue;
                    }

                    if (IsBrickColliding(brick, offsetX, offsetY, i, j))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsBrickColliding(Brick brick, int offsetX, int offsetY, int i, int j)
        {
            return i + brick.PosY + offsetY < 0 ||
                   i + brick.PosY + offsetY >= Height ||
                   j + brick.PosX + offsetX < 0 ||
                   j + brick.PosX + offsetX >= Width ||
                   Tab[brick.PosY + offsetY + i, brick.PosX + offsetX + j] == 2;
        }

        public void FreezeBrick(Brick brick)
        {
            for (var i = 0; i < brick.Height; i++)
            {
                for (var j = 0; j < brick.Width; j++)
                {
                    if (brick.Shape[i, j] != 0)
                    {
                        Tab[brick.PosY + i, brick.PosX + j] = 2;
                    }
                }
            }
        }

        private int CheckBoard()
        {
            var counter = 0;
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    if (Tab[i, j] != 2)
                    {
                        break;
                    }
                    counter++;
                }

                if (counter == Width)
                {
                    return i;
                }
                counter = 0;
            }

            return -1;
        }

        public int Gravitate(int multiplier = 10)
        {
            var level = CheckBoard();
            var score = 1;
            while (level != -1)
            {
                for (var i = level - 1; i >= 0; i--)
                {
                    for (var j = 0; j < Width; j++)
                    {
                        Tab[i + 1, j] = Tab[i, j];
                    }
                }

                level = CheckBoard();
                score *= multiplier;
                multiplier++;
            }

            return score;
        }
    }
}