using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MazeSolver
{
    public class Map
    {

        private List<List<Cell>> _map = new List<List<Cell>>();
        private MainWindow _win = null;
        private String _mapPathToFile = "None";
        private Cell _startCell;
        private Cell _endCell;

        public Map(MainWindow win)
        {
            _win = win;
        }

        public MainWindow getMainWindow()
        {
            return (_win);
        }

        public Boolean loadMap(String PathToFile)
        {
            if ((startLoadMap(PathToFile)) == false)
            {
                _map.Clear();
                _win.printInfo("Error : Load map Impossible (check the map)", Colors.Red);
                _win.printInfo("Map rules : minimum size 3x2\nletters allowed in map are : [s],[e],[x],[.]", Colors.Red);
                return (false);
            }
            _mapPathToFile = PathToFile;
            //printMap();
            //printDebugMap();
            return (true);
        }

        private Boolean startLoadMap(String PathToFile)
        {
            _map.Clear();
            try
            {
                StreamReader rd = new StreamReader(PathToFile);
                int y = 0;

                while (rd.Peek() >= 0)
                {
                    String line = rd.ReadLine();
                    if ((addLineToMap(line, y)) == false)
                        return (false);
                    y++;
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                _win.printInfo(ex.ToString(), Colors.Red);
                return (false);
            }
            if ((checkMap()) == false)
            {
                _map.Clear();
                return (false);
            }
            makeLinkCellMap();
            return (true);
        }

        private Boolean addLineToMap(String line, int y)
        {
            int x = 0;
            List<Cell> listCell = new List<Cell>();

            if (line.Count() < 3)
                return (false);
            foreach (char car in line)
            {
                if (car != 's' && car != 'e' && car != 'x' && car != '.')
                    return (false);
                Cell newCell = new Cell(car);

                newCell.setPos(new Pos(x, y));
                listCell.Add(newCell);
                x++;
            }
            _map.Add(listCell);
            return (true);
        }

        private Boolean checkMap()
        {
            if (_map.Count() < 2)
                return (false);
            foreach (List<Cell> listCell in _map)
            {
                if (listCell.Count() != _map[0].Count())
                    return (false);
            }
            return (true);
        }

        private void makeLinkCellMap()
        {
            int x = 0;
            int y = 0;

            while (y != _map.Count())
            {
                x = 0;
                while (x != _map[y].Count())
                {
                    if (x < _map[y].Count() - 1)
                        _map[y][x].setEnv(Direction.RIGHT, _map[y][x + 1]);
                    if (x > 0)
                        _map[y][x].setEnv(Direction.LEFT, _map[y][x - 1]);
                    if (y > 0)
                        _map[y][x].setEnv(Direction.UP, _map[y - 1][x]);
                    if (y < _map.Count() - 1)
                        _map[y][x].setEnv(Direction.DOWN, _map[y + 1][x]);
                    if (_map[y][x].GetKindCell() == KindCell.START)
                        _startCell = _map[y][x];
                    if (_map[y][x].GetKindCell() == KindCell.END)
                        _endCell = _map[y][x];
                    x++;
                }
                y++;
            }
        }

        public List<List<Cell>> getMap()
        {
            if (_map.Count() < 2)
                return (null);
            return (_map);
        }

        public void printMap()
        {
            if (_map.Count() >= 2)
            {
                foreach (List<Cell> listCell in _map)
                {
                    foreach (Cell cell in listCell)
                    {
                        if (cell.GetKindCell() == KindCell.START)
                            _win.printInfo('s', Colors.Blue);
                        else if (cell.GetKindCell() == KindCell.END)
                            _win.printInfo('e', Colors.Blue);
                        else if (cell.GetKindCell() == KindCell.WALL)
                            _win.printInfo('x', Colors.Blue);
                        else if (cell.GetKindCell() == KindCell.EMPTY)
                            _win.printInfo('.', Colors.Blue);
                        else
                            _win.printInfo('o', Colors.Blue);
                    }
                    _win.printInfo("", Colors.Blue);
                }
                _win.printInfo("_map.count() = " + _map.Count() + " _map[y].Count() = " + _map[0].Count(), Colors.Blue);
            }
        }

        public void printDebugMap()
        {
            if (_map.Count() >= 2)
            {
                foreach (List<Cell> listCell in _map)
                {
                    foreach (Cell cell in listCell)
                    {
                        _win.printInfo(cell.ToString(), Colors.Blue);
                    }
                }
            }
        }

        public String getMapPathToFile()
        {
            return (_mapPathToFile);
        }

        public Cell getStartCell()
        {
            return (_startCell);
        }

        public Cell getEndCell()
        {
            return (_endCell);
        }

        public void clearMap()
        {
            foreach (List<Cell> listCell in _map)
            {
                foreach (Cell cell in listCell)
                {
                    if (cell.GetKindCell() == KindCell.DEADEND || cell.GetKindCell() == KindCell.ROAD)
                    {
                        cell.setKindCell(KindCell.EMPTY);
                        cell.getRect().Fill = new SolidColorBrush(Colors.White);
                    }
                }
            }
        }
    }
}

// s = start
// e = end
// . = empty
// x = wall
// o = road