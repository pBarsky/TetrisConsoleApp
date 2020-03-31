using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisConsoleApp
{
    public class Board
    {
        protected int[,] tab;
        public readonly int Width, Height;

        public Board(int width = 30, int height = 20)
        {
            this.Width = width;
            this.Height = height;
            tab = new int[height, width];
        }
        public void Show()
        {
            Console.Clear();
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                    Console.Write(tab[i, j] != 0 ? "#" : " ");
                //Console.Write(tab[i, j] != 0 ? (tab[i, j] == 1 ? "A" : "B") : " ");
                Console.WriteLine("|\t" + i.ToString());
            }
        }

        public void InsertBrick(Brick brick)
        {
            for(int i = 0; i < brick.Height; i++)
                for(int j = 0; j < brick.Width; j++)
                    if(brick.Shape[i, j] != 0)
                        tab[brick.PosY + i, brick.PosX + j] = brick.Shape[i, j];
        }

        public void DeepClear()
        {
            for(int i = 0; i < Height; i++)
                for(int j = 0; j < Width; j++)
                    tab[i, j] = 0;
        }

        public void ShallowClear()
        {
            for(int i = 0; i < Height; i++)
                for(int j = 0; j < Width; j++)
                    if(tab[i, j] == 1)
                        tab[i, j] = 0;
        }
        public bool IsColliding(Brick brick, int offsetX, int offsetY)
        {
            for(int i = 0; i < brick.Height; i++)
                for(int j = 0; j < brick.Width; j++)
                {
                    if(brick.Shape[i, j] != 1) continue;
                    if(i + brick.PosY + offsetY < 0 ||
                       i + brick.PosY + offsetY >= Height ||
                       j + brick.PosX + offsetX < 0 ||
                       j + brick.PosX + offsetX >= Width ||
                       tab[brick.PosY + offsetY + i, brick.PosX + offsetX + j] == 2)
                        return true;
                }
            return false;
        }
        public void FreezeBrick(Brick brick)
        {
            for(int i = 0; i < brick.Height; i++)
                for(int j = 0; j < brick.Width; j++)
                    if(brick.Shape[i, j] != 0)
                        tab[brick.PosY + i, brick.PosX + j] = 2;
        }
    }
}
