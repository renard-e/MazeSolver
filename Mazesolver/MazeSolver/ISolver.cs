using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    interface ISolver
    {
        void runSolver(Map map, int timeSleepMS);

        void changeKindCellUpdateAndWait(Cell cell, KindCell newKindCell, MainWindow win, int timeSleepMS);

        String getNameSolver();
    }
}
