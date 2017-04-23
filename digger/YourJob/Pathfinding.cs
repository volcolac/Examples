using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Pathfinding
    {
        public bool[,] usd;
        public int[,,] parent;
        public List<int[]> queue;
        public int pointer;
        public int[,] step;
        public bool stop;

        public Pathfinding(Game game) {
            usd = new bool[game.MapWidth, game.MapHeight];
            parent = new int[2, game.MapWidth, game.MapHeight];
            queue = new List<int[]>();
            pointer = 0;
            step = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            stop = false; 
        }
    }
}
