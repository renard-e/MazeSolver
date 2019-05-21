using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MazeSolver
{
    public class Astar : Solver
    {
        Stack<Cell> _openList = new Stack<Cell>();
        Stack<Cell> _closedList = new Stack<Cell>();

        public Astar()
        {
            _name = "Astar";
        }

        public static String staticGetName()
        {
            return ("Astar");
        }

        public override void runSolver(Map map, int timeSleepMS)
        {
            startEndTimer(0);
            solveMaze(map, timeSleepMS);
            map.getMainWindow().setState("Finish");
            map.getMainWindow().printInfo("Solver Work during : " + startEndTimer(1), Colors.Green);
        }

        private void solveMaze(Map map, int timeSleepMS)
        {
            Boolean finish = false;
            _openList.Clear();
            _closedList.Clear();
            _openList.Push(map.getStartCell());
            while (_openList.Count() > 0)
            {
                Cell cell = _openList.Pop();
                if (cell.theEndIsHere())
                {
                    _closedList.Push(cell);
                    finish = true;
                    reconstructRoad(map, timeSleepMS);
                    break;
                }
                foreach (KeyValuePair<Direction, Cell> v in cell.getEnv())
                {
                    if (v.Value.GetKindCell() == KindCell.EMPTY)
                    {
                        if (_openList.Contains(v.Value) || _closedList.Contains(v.Value))
                           ;
                        else
                        {
                            v.Value.setCout(cell.getCout() + 1);
                            v.Value.setHeuristique(v.Value.getCout() + (int)distance(v.Value.getPos().getX(), v.Value.getPos().getY(), map.getEndCell().getPos().getX(), map.getEndCell().getPos().getY()));
                            addAndSortOpenList(v.Value);
                        }
                    }
                }
                _closedList.Push(cell);
            }
            if (finish != true)
                map.getMainWindow().printInfo("NO PATH FOUND IN MAP", Colors.Red);
        }

        private double distance(int posVX, int posVY, int posObjX, int posObjY)
        {
            return (Math.Sqrt(Math.Pow(posVX - posObjX, 2) + Math.Pow(posVY - posObjY, 2)));
        }

        private void reconstructRoad(Map map, int timeSleepMS)
        {
            int tmpCout = _closedList.Peek().getCout();
            Cell tmpCell = _closedList.Peek();
            Stack<Cell> finalStack = new Stack<Cell>();

            while (_closedList.Count() != 0)
            {
                if (checkConnection(tmpCell, _closedList.Peek()))
                {
                    if (_closedList.Peek().getCout() == tmpCout)
                    {
                        if (_closedList.Peek().GetKindCell() != KindCell.START)
                            finalStack.Push(_closedList.Peek());
                        else
                            _closedList.Pop();
                        tmpCout--;
                        if (_closedList.Count() != 0)
                            tmpCell = _closedList.Pop();
                    }
                    else
                        _closedList.Pop();
                }
                else
                    _closedList.Pop();
            }
            while (finalStack.Count() != 0)
                changeKindCellUpdateAndWait(finalStack.Pop(), KindCell.ROAD, map.getMainWindow(), timeSleepMS);
        }

        private Boolean checkConnection(Cell tmpCell, Cell currentCell)
        {
            if (tmpCell == currentCell)
                return (true);
            foreach (KeyValuePair<Direction, Cell> cellEnv in tmpCell.getEnv())
            {
                if (cellEnv.Value == currentCell)
                    return (true);
            }
            return (false);
        }

        private void addAndSortOpenList(Cell cellToAdd)
        {
            Boolean moove = false;
            Stack<Cell> tmpStack = new Stack<Cell>();

            while (moove == false)
            {
                if (_openList.Count() == 0 || cellToAdd.getHeuristique() < _openList.Peek().getHeuristique())
                {
                    _openList.Push(cellToAdd);
                    while (tmpStack.Count() != 0)
                        _openList.Push(tmpStack.Pop());
                    moove = true;
                }
                else
                    tmpStack.Push(_openList.Pop());
            }
        }
    }
}
