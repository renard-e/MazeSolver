using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MazeSolver
{
    public class DeadEndFilling : ISolver
    {
        public String _name = "DeadEndFilling";
        private int _start = 0;

        public DeadEndFilling()
        {
        }

        public void runSolver(Map map, int timeSleepMS)
        {
            startEndTimer(0);
            findAllDeadEnd(map, timeSleepMS);
            map.getMainWindow().setState("Finish");
            map.getMainWindow().printInfo("Solver Work during : " + startEndTimer(1), Colors.Green);
        }

        private void findAllDeadEnd(Map map, int timeSleepMS)
        {
            Boolean change = true;

            while (change == true)
            {
                change = false;
                foreach (List<Cell> listCell in map.getMap())
                {
                    foreach (Cell cell in listCell)
                    {
                        if (cell.isDeadEnd())
                        {
                            changeKindCellUpdateAndWait(cell, KindCell.DEADEND, map.getMainWindow(), timeSleepMS);
                            change = true;
                        }
                    }
                }
            }
        }

        public void changeKindCellUpdateAndWait(Cell cell, KindCell newKindCell, MainWindow win, int timeSleepMS)
        {
            cell.setKindCell(newKindCell);
            win.updateCell(cell);
            Thread.Sleep(timeSleepMS);
        }

        private String startEndTimer(int opt)
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

        public String getNameSolver()
        {
            return (_name);
        }
    }
}
