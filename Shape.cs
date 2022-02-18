using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGUI
{
    class Shape
    {
        public int Weight;
        public int Height;
        public int[,] Dots;

        private int[,] backupDots;

        public void turn()
        {
            backupDots = Dots;
            Dots = new int[Weight, Height];
            for(int i = 0; i < Weight; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    Dots[i, j] = backupDots[Height - 1 - j, i];
                }
            }
            var temp = Weight;
            Weight = Height;
            Height = temp;
        }
        public void rollback()
        {
            Dots = backupDots;

            var temp = Weight;
            Weight = Height;
            Height = temp;
        }
    }
}
