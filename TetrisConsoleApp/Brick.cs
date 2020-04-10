using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    public abstract class Brick
    {
        protected int posX, posY; //polozenie klocka
        public string Name { get; }

        protected int[,] shape; //definicja kształtu
        public int[,] Shape => shape;
        public int PosX => posX;
        public int PosY => posY;
        public int Height => shape.GetLength(0);
        public int Width => shape.GetLength(1);

        public string[] Buffer {
            get {
                string[] buffer = new string[Height];
                for(int i = 0; i < Height; i++)
                {
                    buffer[i] = "";
                    for(int j = 0; j < Width; j++)
                        if(shape[i, j] != 0)
                            buffer[i] += shape[i, j] == 1 ? '#' : ' ';
                }
                return buffer;
            }
        }

        protected Brick(int size = 1, string name = "", int posX = 0, int posY = 0)
        {
            shape = new int[size, size];
            this.Name = name;
            this.posX = posX;
            this.posY = posY;
        }

        private int[,] Rotate(bool clockDirection)
        {
            int size = shape.GetLength(0);
            int[,] result = new int[size, size];
            if(!clockDirection)
                for(int i = 0; i < size; i++)
                    for(int j = 0; j < size; j++)
                        result[size - 1 - j, i] = shape[i, j];
            else
                for(int i = 0; i < size; i++)
                    for(int j = size - 1; j >= 0; j--)
                        result[i, size - 1 - j] = shape[j, i];
            return result;
        }

        public void ShowBrick()
        {
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                    Console.Write(shape[i, j]);
                Console.Write("\n");
            }
        }

        public void DoRotate(bool right = true)
        {
            shape = Rotate(right);
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
            posX += offsetX;
            posY += offsetY;
        }

        public void RestartPosition(int newPosX)
        {
            posY = 0;
            posX = newPosX;
        }

        public Brick DeepCopy()
        {
            Brick outputBrick = (Brick)this.MemberwiseClone();
            outputBrick.shape = (int[,])shape.Clone();
            return outputBrick;
        }
    }
}
