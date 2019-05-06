using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace MazeSolver
{
    public enum KindCell
    {
        START = 0, // s
        END, // e
        WALL, // x
        EMPTY, // .
        ROAD // o
    }

    public enum Direction
    {
        LEFT = 0,
        RIGHT,
        UP,
        DOWN
    }

    public class Cell
    {
        private Dictionary<Direction, Cell> _env = new Dictionary<Direction, Cell>();
        private KindCell _kindCell;
        private Pos _pos = new Pos();
        private Rectangle _rect = null;

        public Cell(KindCell kindCell)
        {
            _kindCell = kindCell;
        }

        public Cell(char carKindCell)
        {
            if (carKindCell == 's')
                _kindCell = KindCell.START;
            if (carKindCell == 'e')
                _kindCell = KindCell.END;
            if (carKindCell == '.')
                _kindCell = KindCell.EMPTY;
            if (carKindCell == 'x')
                _kindCell = KindCell.WALL;
        }

        public Cell(Cell left, Cell right, Cell up, Cell Down, KindCell kindCell)
        {
            _env.Add(Direction.LEFT, left);
            _env.Add(Direction.RIGHT, right);
            _env.Add(Direction.UP, up);
            _env.Add(Direction.DOWN, Down);
            _kindCell = kindCell;
        }

        public void setEnv(Direction dir, Cell newCell)
        {
            try
            {
                _env.Add(dir, newCell);
            }
            catch(ArgumentException ex)
            {
                _env[dir] = newCell;
            }
        }

        public void setPos(Pos newPos)
        {
            _pos = newPos;
        }

        public KindCell GetKindCell()
        {
            return (_kindCell);
        }

        public void setKindCell(KindCell kindCell)
        {
            _kindCell = kindCell;
        }

        public Boolean isDeadEnd()
        {
            int count = 0;

            if (_kindCell == KindCell.EMPTY)
            {
                if (isFreeWay(Direction.LEFT) == false)
                    count++;
                if (isFreeWay(Direction.RIGHT) == false)
                    count++;
                if (isFreeWay(Direction.DOWN) == false)
                    count++;
                if (isFreeWay(Direction.UP) == false)
                    count++;
            }
            if (count >= 3)
                return (true);
            return (false);
        }

        public Boolean isFreeWay(Direction dir)
        {
            if (_env.ContainsKey(dir))
            {
                if (_env[dir].GetKindCell() == KindCell.WALL)
                    return (false);
                return (true);
            }
            return (false);
        }

        override public String ToString()
        {
            String debug = "";

            debug += "-----------------------------------------\n";
            debug += "DEBUG CELL : \n";
            debug += "KIND CELL : " + _kindCell.ToString() + "\n";
            debug += "POS :\n\ty = " + _pos.getY().ToString() + "\n\tx = " + _pos.getX().ToString() + "\n";
            debug += "LEFT :\n\t" + this.isFreeWay(Direction.LEFT) + "\n";
            if ((this.isFreeWay(Direction.LEFT)) == true)
                debug += "\tKIND CELL LEFT : " + _env[Direction.LEFT].GetKindCell() + "\n";
            debug += "RIGHT :\n\t" + this.isFreeWay(Direction.RIGHT) + "\n";
            if ((this.isFreeWay(Direction.RIGHT)) == true)
                debug += "\tKIND CELL RIGHT : " + _env[Direction.RIGHT].GetKindCell() + "\n";
            debug += "UP :\n\t" + this.isFreeWay(Direction.UP) + "\n";
            if ((this.isFreeWay(Direction.UP)) == true)
                debug += "\tKIND CELL UP : " + _env[Direction.UP].GetKindCell() + "\n";
            debug += "DOWN :\n\t" + this.isFreeWay(Direction.DOWN) + "\n";
            if ((this.isFreeWay(Direction.DOWN)) == true)
                debug += "\tKIND CELL DOWN : " + _env[Direction.DOWN].GetKindCell() + "\n";
            return (debug);
        }

        public Rectangle getRect()
        {
            return (_rect);
        }

        public void setRect(Rectangle rect)
        {
            _rect = rect;
        }
    }
}
