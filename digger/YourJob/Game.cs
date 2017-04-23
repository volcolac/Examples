using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class Game  
    {
        public ICreature[,] Map { get; private set; }
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int Scores { get; set; }
        public Keys? KeyPressed { get; set; }
        public Keys? LastKeyPressed { get; set; }
        public int[] Move { get; set; }
        public int WinCondition { get; set; }
        public int countDynamit { get; set; }

        public Game(string fileName) {
            CreateMap(fileName);
            countDynamit = 1;
        }

        void CreateMap(string fileName) {
            string[] lines = File.ReadAllLines(fileName);
            var sizes = lines[0].Split(' ');
            MapWidth = int.Parse(sizes[1]);
            MapHeight = int.Parse(sizes[0]);
            WinCondition = int.Parse(sizes[2]);
            Map = new ICreature[MapWidth, MapHeight];
            for (int i = 0; i < MapWidth; i++)
                for (int j = 0; j < MapHeight; j++) {
                    if (lines[j + 1][i] == '*')
                        Map[i, j] = new Terrain();
                    if (lines[j + 1][i] == 'D')
                        Map[i, j] = new Digger();
                    if (lines[j + 1][i] == 'S')
                        Map[i, j] = new Sack();
                    if (lines[j + 1][i] == 'M')
                        Map[i, j] = new Monster();
                    if (lines[j + 1][i] == 'G')
                        Map[i, j] = new Gold();
                }
        }

    }
}
