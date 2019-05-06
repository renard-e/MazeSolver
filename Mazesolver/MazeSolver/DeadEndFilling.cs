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

        public void runSolver(Map map)
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
                            cell.setKindCell(KindCell.WALL);
                            map.getMainWindow().updateCell(cell);
                            change = true;
                            Thread.Sleep(400);
                        }
                    }
                }
            }
        }

        public String getNameSolver()
        {
            return (_name);
        }
    }
}
