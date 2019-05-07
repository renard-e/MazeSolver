﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    interface ISolver
    {
        void runSolver(Map map, int timeSleepMS);

        String getNameSolver();
    }
}
