using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisConsoleApp
{
    public class Board
    {
        protected int[,] tab;
        private readonly int _width;
        private readonly int _height;
        public int Width => _width;
        public int Height => _height;

        public string[] Buffer {
            get {
                string[] buffer = new string[_height];
                for(int i = 0; i < _height; i++)
                {
                    for(int j = 0; j < _width; j++)
                    {
                        buffer[i] += tab[i, j] != 0 ? "#" : " ";
                        //buffer[i] += tab[i, j] != 0 ? (tab[i, j] == 1 ? "+" : "#") : " ";
                    }

                    buffer[i] += "|\t" + i.ToString();
                }
                return buffer;
            }
        }
        public Board(int width = 10, int height = 20)
        {
            this._width = width;
            this._height = height;
            tab = new int[height, width];
        }
        public void Show()
        {
            Console.Clear();
            for(int i = 0; i < _height; i++)
            {
                for(int j = 0; j < _width; j++)
                    Console.Write(tab[i, j] != 0 ? "#" : " ");
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
            for(int i = 0; i < _height; i++)
                for(int j = 0; j < _width; j++)
                    tab[i, j] = 0;
        }

        public void ShallowClear()
        {
            for(int i = 0; i < _height; i++)
                for(int j = 0; j < _width; j++)
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
                       i + brick.PosY + offsetY >= _height ||
                       j + brick.PosX + offsetX < 0 ||
                       j + brick.PosX + offsetX >= _width ||
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

        public int CheckBoard()
        {
            int counter = 0;
            for(int i = 0; i < _height; i++)
            {
                for(int j = 0; j < _width; j++)
                {
                    if(tab[i, j] != 2)
                        break;
                    counter++;
                }
                if(counter == _width)
                    return i;
                counter = 0;
            }
            return -1;
        }

        public int Gravitate(int multiplier = 10)
        {
            int level = CheckBoard();
            int score = 1;
            while(level != -1)
            {
                for(int i = level - 1; i >= 0; i--)
                    for(int j = 0; j < _width; j++)
                        tab[i + 1, j] = tab[i, j];
                level = CheckBoard();
                score *= multiplier;
                multiplier++;
            }
            return score;
        }
    }
}
