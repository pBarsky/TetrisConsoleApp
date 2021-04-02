using System.Collections.Generic;

namespace TetrisConsoleApp.AbstractClasses
{
    internal abstract class Brick
    {
        public int[,] Shape { get; private set; }

        public int PosX { get; private set; }

        public int PosY { get; private set; }

        public int Height => Shape.GetLength(0);
        public int Width => Shape.GetLength(1);

        public IEnumerable<string> Buffer
        {
            get
            {
                var buffer = new string[Height];
                for (var i = 0; i < Height; i++)
                {
                    buffer[i] = "";
                    for (var j = 0; j < Width; j++)
                        buffer[i] += Shape[i, j] == 1 ? '#' : ' ';
                }
                return buffer;
            }
        }

        protected Brick(int size = 1, string name = "", int posX = 0, int posY = 0)
        {
            Shape = new int[size, size];
            this.PosX = posX;
            this.PosY = posY;
        }

        private int[,] Rotate(bool clockDirection)
        {
            var size = Shape.GetLength(0);
            var result = new int[size, size];
            return clockDirection ? RotateRight(size, result) : RotateLeft(size, result);
        }

        private int[,] RotateLeft(int size, int[,] result)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    result[size - 1 - j, i] = Shape[i, j];
                }
            }

            return result;
        }

        private int[,] RotateRight(int size, int[,] result)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = size - 1; j >= 0; j--)
                {
                    result[i, size - 1 - j] = Shape[j, i];
                }
            }

            return result;
        }

        public void DoRotate(bool right = true)
        {
            Shape = Rotate(right);
        }

        public void MoveDown()
        {
            Move(0, 1);
        }

        public void MoveLeft()
        {
            Move(-1, 0);
        }

        public void MoveRight()
        {
            Move(1, 0);
        }

        private void Move(int offsetX, int offsetY)
        {
            PosX += offsetX;
            PosY += offsetY;
        }

        public void RestartPosition(int newPosX)
        {
            PosY = 0;
            PosX = newPosX;
        }

        public Brick DeepCopy()
        {
            Brick outputBrick = (Brick)this.MemberwiseClone();
            outputBrick.Shape = (int[,])Shape.Clone();
            return outputBrick;
        }
    }
}