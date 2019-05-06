using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class Pos
    {
        private int _x;
        private int _y;

        public Pos()
        {
            _x = -1;
            _y = -1;
        }

        public Pos(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void setX(int newX)
        {
            _x = newX;
        }

        public void setY(int newY)
        {
            _y = newY;
        }

        public int getX()
        {
            return (_x);
        }

        public int getY()
        {
            return (_y);
        }
    }
}
