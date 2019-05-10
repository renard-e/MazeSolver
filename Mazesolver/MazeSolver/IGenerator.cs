using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public interface IGenerator
    {
        Boolean makeMaze(int sizeX, int sizeY, String pathFile);
    }
}
