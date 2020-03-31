using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsoleApp
{
    public class Brick
    {
        protected int posX, posY; //polozenie klocka
        public string Name { get; }

        protected int[,] shape; //definicja kształtu
        public int[,] Shape => shape;
        public int PosX => posX;
        public int PosY => posY;
        public int Height => shape.GetLength(0);
        public int Width => shape.GetLength(1);

        public Brick(int size, string name, int posX, int posY)
        {
            shape = new int[size, size];
            this.Name = name;
            this.posX = posX;
            this.posY = posY;
        }

        private int[,] rotate(bool clockDirection)
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
            for(int i = 0; i < shape.GetLength(0); i++)
            {
                for(int j = 0; j < shape.GetLength(0); j++)
                    Console.Write(shape[i, j]);
                Console.Write("\n");
            }
        }

        public void DoRotate(bool right = true)
        {
            shape = rotate(right);
        }

        public void MoveDown()
        {
            move(0, 1);
        }

        public void MoveLeft()
        {
            move(-1, 0);
        }
        public void MoveRight()
        {
            move(1, 0);
        }
        private void move(int offsetX, int offsetY)
        {
            posX += offsetX;
            posY += offsetY;
        }
    }
}
