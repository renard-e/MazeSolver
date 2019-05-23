using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class genAntoine : IGenerator
    {
        private int width;
        private int height;
        private List<String> Map;
        private Random rand = new Random();

        public Boolean makeMaze(int sizeX, int sizeY, String pathToFile)
        {
            init(sizeX, sizeY);
            List<String> map = generate();

            try
            {
                StreamWriter sw = new StreamWriter(pathToFile);

                foreach (String line in map)
                    sw.WriteLine(line);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (false);
            }
            return (true);
        }

        private void ChangeChar(int y, int x, char letter)
        {
            this.Map[y] = this.Map[y].Remove(x, 1);
            this.Map[y] = this.Map[y].Insert(x, letter.ToString());
        }

        public void init(int width, int height)
        {
            int x;
            int y;

            x = 0;
            y = 0;
            this.width = width * 2 + 1;
            this.height = height * 2 + 1;
            this.Map = new List<String>(width * 2 + 1);
            while (y != this.height)
            {
                Map.Add(new String('x', this.width));
                y++;
            }
        }

        public List<String> generate()
        {

            this.ChangeChar(1, 1, '.');
            if (rand.Next() % 2 == 1)
            {
                this.ChangeChar(1, 2, '.');
                this.generate(0, 1);
            }
            else
            {
                this.ChangeChar(2, 1, '.');
                this.generate(1, 0);
            }
            ChangeChar(0, 1, 's');
            ChangeChar(this.height - 1, this.width - 2, 'e');
            return (Map);
        }
        public void generate(int y, int x)
        {
            int direction;

            if (this.Map[y * 2 + 1][x * 2 + 1] != 'x')
            {
                return;
            }
            this.ChangeChar(y * 2 + 1, x * 2 + 1, '.');
            List<int> possibilities = new List<int>();
            if (y * 2 + 1 > 1 && Map[y * 2 + 1 - 2][x * 2 + 1] == 'x')
                possibilities.Add(1);
            if ((x + 1) * 2 + 1 < width && Map[y * 2 + 1][(x + 1) * 2 + 1] == 'x')
                possibilities.Add(2);
            if (y * 2 + 1 + 2 < height && Map[y * 2 + 1 + 2][x * 2 + 1] == 'x')
                possibilities.Add(3);
            if (x * 2 + 1 > 1 && Map[y * 2 + 1][x * 2 + 1 - 2] == 'x')
                possibilities.Add(4);
            if (possibilities.Count == 0)
                return;

            direction = possibilities[rand.Next() % possibilities.Count];
            if (direction % 2 == 1)
            {
                this.ChangeChar((y * 2 + 1 + direction - 2), x * 2 + 1, '.');
                this.generate(y + direction - 2, x);
            }
            else
            {
                this.ChangeChar(y * 2 + 1, (x * 2 + 1 - direction + 3), '.');
                this.generate(y, x - direction + 3);
            }
            foreach (int i in possibilities)
            {
                if (i % 2 == 1 && this.Map[y * 2 + 1 + (i - 2) * 2][x * 2 + 1] == 'x')
                {

                    this.ChangeChar((y * 2 + 1 + i - 2), x * 2 + 1, '.');
                    this.generate(y + i - 2, x);
                }
                else if (i % 2 == 0 && this.Map[y * 2 + 1][x * 2 + 1 + (-i + 3) * 2] == 'x')
                {
                    this.ChangeChar(y * 2 + 1, (x * 2 + 1 - i + 3), '.');
                    this.generate(y, x - i + 3);
                }
            }
        }
    }
}
