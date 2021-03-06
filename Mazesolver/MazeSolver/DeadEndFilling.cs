﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MazeSolver
{
    public class DeadEndFilling : Solver
    {
        private Stack<Cell> _stackCell = new Stack<Cell>();

        public DeadEndFilling()
        {
            _name = "DeadEndFilling";
        }

        public static String staticGetName()
        {
            return ("DeadEndFilling");
        }

        override public void runSolver(Map map, int timeSleepMS)
        {
            startEndTimer(0);
            findAllDeadEnd(map, timeSleepMS);
            findTheRoad(map, timeSleepMS);
            map.getMainWindow().setState("Finish");
            map.getMainWindow().printInfo("Solver Work during : " + startEndTimer(1), Colors.Green);
        }

        private void findTheRoad(Map map, int timeSleepMS)
        {
            _stackCell.Clear();
            _stackCell.Push(map.getStartCell());
            try
            {
                while (_stackCell.Peek().theEndIsHere() == false && _stackCell.Peek().noWay() == false)
                {
                    if ((findBestWay()) == false)
                        changeKindCellUpdateAndWait(_stackCell.Pop(), KindCell.DEADEND, map.getMainWindow(), timeSleepMS);
                    else
                        changeKindCellUpdateAndWait(_stackCell.Peek(), KindCell.ROAD, map.getMainWindow(), timeSleepMS);
                }
            }
            catch (Exception ex)
            {
                map.getMainWindow().printInfo("There is no way to solve the maze", Colors.Red);
            }
        }

        private Boolean findBestWay()
        {
            Boolean dirChange = false;

            foreach (Direction dir in _listDir)
            {
                if (_stackCell.Peek().isFreeWay(dir) == true)
                {
                    dirChange = true;
                    _stackCell.Push(_stackCell.Peek().getCelldir(dir));
                    break;
                }
            }
            return (dirChange);
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
    }
}
