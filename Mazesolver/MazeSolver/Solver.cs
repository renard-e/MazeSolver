using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeSolver
{
    public abstract class Solver : ISolver
    {
        protected String _name = "";
        protected int _start;
        protected List<Direction> _listDir = new List<Direction>();

        abstract public void runSolver(Map map, int timeSleepMS);

        public Solver()
        {
            _listDir.Add(Direction.DOWN);
            _listDir.Add(Direction.LEFT);
            _listDir.Add(Direction.RIGHT);
            _listDir.Add(Direction.UP);
        }

        public String getNameSolver()
        {
            return (_name);
        }

        public String startEndTimer(int opt)
        {
            if (opt == 0)
                _start = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            else if (opt == 1)
            {
                int tmp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                tmp = tmp - _start;
                return (TimeSpan.FromSeconds(tmp).ToString(@"hh\:mm\:ss"));
            }
            return ("");
        }

        public void changeKindCellUpdateAndWait(Cell cell, KindCell newKindCell, MainWindow win, int timeSleepMS)
        {
            cell.setKindCell(newKindCell);
            win.updateCell(cell);
            Thread.Sleep(timeSleepMS);
        }

    }
}
