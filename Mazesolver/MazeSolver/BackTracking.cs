using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MazeSolver
{
    public class BackTracking : Solver
    {
        private Stack<Cell> _stackCell = new Stack<Cell>();

        public BackTracking()
        {
            _name = "BackTracking";
        }

        public static String staticGetName()
        {
            return ("BackTracking");
        }
        override public void runSolver(Map map, int timeSleepMS)
        {
            startEndTimer(0);
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
    }
}
