using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class DeadEndFilling : ISolver
    {
        public String _name = "DeadEndFilling";

        public DeadEndFilling()
        {
        }

        public void runSolver(Map map, int timeSleepMS)
        {
            findAllDeadEnd(map, timeSleepMS);
            map.getMainWindow().setState("Finish");
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

        public String getNameSolver()
        {
            return (_name);
        }
    }
}
