using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Windows.Threading;

namespace MazeSolver
{
    public partial class MainWindow : Window
    {
        private OpenFileDialog _fileDialog = new OpenFileDialog();
        private Map _map = null;
        private String _state = "In progress";
        private Dictionary<KindCell, Color> _tabColorsKindCell = new Dictionary<KindCell, Color>();
        private String _directoryMazeName = "/map_maze/";
        private Thread _threadSolver = null;
        private Dictionary<String, ISolver> _allSolver = new Dictionary<String, ISolver>();
        private int _timeSleep = 0;
        private IGenerator _gen = new genAntoine();

        public MainWindow()
        {
            InitializeComponent();
            CanvasBorder.BorderThickness = new Thickness(1);
            _map = new Map(this);
            setTabColors();
            setMazeSolverName();
        }

        private void setMazeSolverName()
        {            
            _allSolver.Add(DeadEndFilling.staticGetName(), new DeadEndFilling());
            _allSolver.Add(BackTracking.staticGetName(), new BackTracking());
            _allSolver.Add(Astar.staticGetName(), new Astar());

            foreach (KeyValuePair<String, ISolver> entry in _allSolver)
                listViewSolver.Items.Add(entry.Key);
            listViewSolver.SelectedIndex = 0;
        }

        private void setTabColors()
        {
            _tabColorsKindCell.Add(KindCell.EMPTY, Colors.White);
            _tabColorsKindCell.Add(KindCell.WALL, Colors.Black);
            _tabColorsKindCell.Add(KindCell.ROAD, Colors.Green);
            _tabColorsKindCell.Add(KindCell.START, Colors.Red);
            _tabColorsKindCell.Add(KindCell.END, Colors.Blue);
            _tabColorsKindCell.Add(KindCell.DEADEND, Colors.Gray);
        }

        private void button_cancel_process(object sender, RoutedEventArgs e)
        {
            if (_threadSolver != null)
                _threadSolver.Abort();
        }

        private void button_clear_all(object sender, RoutedEventArgs e)
        {
            clearInfo();
            if (_map != null)
            {
                _map.clearMap();
                CanvasMaze.UpdateLayout();
            }
        }

        private void button_generate_maze(object sender, RoutedEventArgs e)
        {
            String x = Interaction.InputBox("Choose Width (x) :","Generate maze", "10");
            String y = Interaction.InputBox("Choose height (y) :", "Generate maze", "10");
            String fileName = Interaction.InputBox("Choose file name :", "Generate maze", "test.txt");

            if ((checkInputMaze(x, y, fileName)) == false)
            {
                MessageBox.Show("Error : x > 1 and y > 1 and x <= 100 and y <= 100 or the file already exists", "Error Input Map", MessageBoxButton.OK, MessageBoxImage.Warning);
                printInfo("Error : x > 1 and y > 1 and x <= 100 and y <= 100 or the file already exists", Colors.Red);
            }
            else if ((_gen.makeMaze(int.Parse(x), int.Parse(y), Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName + _directoryMazeName + fileName)) == false)
            {
                MessageBox.Show("Error : something wrong happen during the generation", "Error generate map", MessageBoxButton.OK, MessageBoxImage.Warning);
                printInfo("Error : something wrong happened during generation", Colors.Red);
            }
            else
            {
                MessageBox.Show("maze generation path : " + Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName + _directoryMazeName + fileName, "Maze generation success ", MessageBoxButton.OK, MessageBoxImage.Information);
                printInfo("maze generation path : " + Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName + _directoryMazeName + fileName, Colors.Green);
            }
        }

        private Boolean checkInputMaze(String x, String y, String fileName)
        {
            if (String.IsNullOrEmpty(x) || String.IsNullOrEmpty(y) || checkNumber(x) || checkNumber(y))
                return (false);
            if (int.Parse(x) <= 1 || int.Parse(y) <= 1 || int.Parse(x) > 100 || int.Parse(y) > 100)
                return (false);
            if (File.Exists(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName + _directoryMazeName + fileName))
                return (false);
            return (true);
        }

        private Boolean checkNumber(String str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return (true);
            }
            return (false);
        }

        private void button_choose_maze(object sender, RoutedEventArgs e)
        {
            _fileDialog.FilterIndex = 2;
            _fileDialog.RestoreDirectory = true;

            if (_fileDialog.ShowDialog() == true)
            {
                String filePath = _fileDialog.FileName;

                clearInfo();
                printInfo("Loading Maze at location : " + filePath, Colors.Red);
                _map.loadMap(filePath);
                if (_map.getMap() != null)
                {
                    drawMap(_map);
                    printInfo("Loading Maze at location : " + filePath + " Done", Colors.Green);
                    _state = "In progress";
                }
            }
        }

        private void button_run_maze_solver(object sender, RoutedEventArgs e)
        {
            if (_map.getMap() == null)
            {
                MessageBox.Show("Error : no map loaded.", "Error run", MessageBoxButton.OK, MessageBoxImage.Warning);
                printInfo("Error : no map loaded.", Colors.Red);
            }
            else
            {
                ISolver solver = _allSolver[listViewSolver.SelectedItems[0].ToString()];

                if (checkNumber(textBoxTimeSleep.Text) == false && String.IsNullOrEmpty(textBoxTimeSleep.Text.ToString()) == false)
                    _timeSleep = int.Parse(textBoxTimeSleep.Text.ToString());
                _state = "In progress";
                _threadSolver = new Thread(() => solver.runSolver(_map, _timeSleep));
                _threadSolver.Start();
            }
        }

        private void button_show_info_maze_solver(object sender, RoutedEventArgs e)
        {
            String info = String.Empty;

            info += "Map Path : " + _map.getMapPathToFile() + "\n";
            info += "Solver Choose : " + listViewSolver.SelectedItems[0].ToString() + "\n";
            info += "State : " + _state + "\n";
            info += "Time Sleep : " + _timeSleep + "\n";
            MessageBox.Show(info, "Infomations about MazeSolver", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void printInfo(String str, Color color)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                textBlockInfo.Inlines.Add(new Run(str + "\n") { Foreground = new SolidColorBrush(color) });
                scrollViewInfo.ScrollToBottom();
            }), DispatcherPriority.Background);
        }

        public void printInfo(Char c, Color color)
        {
            String str = c.ToString();
            textBlockInfo.Inlines.Add(new Run(str) { Foreground = new SolidColorBrush(color) });
            scrollViewInfo.ScrollToBottom();
        }

        public void clearInfo()
        {
            textBlockInfo.Text = "";
        }

        public void drawMap(Map mapObj)
        {
            int startPosX = 10;
            int startPosY = 10;
            int x = 0;
            int y = 0;
            List<List<Cell>> map = mapObj.getMap();

            CanvasMaze.Children.Clear();
            foreach (List<Cell> listCell in map)
            {
                x = 0;
                foreach (Cell cell in listCell)
                {
                    Rectangle rect = new Rectangle();

                    rect.Width = ((CanvasMaze.ActualWidth - (2 * startPosX)) / map[0].Count());
                    rect.Height = ((CanvasMaze.ActualHeight - (2 * startPosY)) / map.Count());
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    rect.Fill = new SolidColorBrush(_tabColorsKindCell[cell.GetKindCell()]);
                    rect.ToolTip = "X = " + cell.getPos().getX() + " Y = " + cell.getPos().getY();
                    Canvas.SetLeft(rect, startPosX + (rect.Width * x));
                    Canvas.SetTop(rect, startPosY + (rect.Height * y));
                    cell.setRect(rect);
                    x++;
                    CanvasMaze.Children.Add(rect);
                }
                y++;
            }
        }

        public void updateCell(Cell cell)
        {
            if (cell.getRect() != null)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    cell.getRect().Fill = new SolidColorBrush(_tabColorsKindCell[cell.GetKindCell()]);
                    cell.getRect().UpdateLayout();
                }), DispatcherPriority.Background);
                
            }
        }

        public void setState(String newState)
        {
            _state = newState;
        }
    }
}
