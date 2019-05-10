using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class ScriptGenerator : IGenerator
    {
        public Boolean makeMaze(int sizeX, int sizeY, String pathFile)
        {
            Process proc = new Process();

            proc.StartInfo.FileName = "MazeGenerator.bat";
            proc.StartInfo.WorkingDirectory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            proc.StartInfo.Arguments = sizeX + " " + sizeY + " x > " + pathFile;//_directoryMazeName.Substring(1) + fileName;
            proc.Start();
            proc.WaitForExit();
            if ((createStartAndEnd(pathFile)) == false)
                return (false);
            return (true);
        }

        private Boolean createStartAndEnd(String pathToFile)
        {
            try
            {
                StreamReader rd = new StreamReader(pathToFile);
                Boolean first = true;
                String allFile = String.Empty;

                while (rd.Peek() >= 0)
                {
                    String line = rd.ReadLine();
                    if (first == true)
                    {
                        allFile += line.Replace('.', 's') + "\n";
                        first = false;
                    }
                    else if (rd.Peek() < 0)
                        allFile += line.Replace('.', 'e') + "\n";
                    else
                        allFile += line + "\n";
                }
                rd.Close();
                if ((updateFile(pathToFile, allFile)) == false)
                    return (false);
            }
            catch (Exception ex)
            {
                return (false);
            }
            return (true);
        }

        private Boolean updateFile(String path, String content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path);

                sw.Write(content);
                sw.Close();
            }
            catch (Exception ex)
            {
                return (false);
            }
            return (true);
        }
    }
}
