using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Monster : ICreature
    {
        
        public Monster() {
            GetImageFileName = "Monster.png";
            GetDrawingPriority = 4;
        }

        public bool Delay { get; set; }
        public int[] Move { get; set; }

        public string GetImageFileName { get; set; }

        public int GetDrawingPriority { get; set; }

        public int[] FindDigger(Game game)
        {
            var finishX = 1;
            var finishY = 1;
            for (int i = 0; i < game.MapWidth; i++)
                for (int j = 0; j < game.MapHeight; j++)
                    if (game.Map[i, j] != null && game.Map[i, j].GetType() == typeof(Digger))
                    {
                        finishX = i;
                        finishY = j;
                        break;
                    }
            return new int[] { finishX, finishY };
        }

        void UpdateQueue(Pathfinding path, Game game)
        {
            var posDigger = FindDigger(game);
            var finishX = posDigger[0];
            var finishY = posDigger[1];
            for (int i = 0; i < 4; i++)
            {
                var newX = path.queue[path.pointer][0] + path.step[i, 0];
                var newY = path.queue[path.pointer][1] + path.step[i, 1];
                if (!path.usd[newX, newY] && !(game.Map[newX, newY] is Terrain || game.Map[newX, newY] is Sack || game.Map[newX, newY] is Gold))
                {
                    path.usd[newX, newY] = true;
                    path.queue.Add(new int[] { newX, newY });
                    path.parent[0, newX, newY] = -path.step[i, 0];
                    path.parent[1, newX, newY] = -path.step[i, 1];
                    if (newX == finishX && newY == finishY)
                    {
                        path.stop = true;
                        break;
                    }
                }
            }
        }

        public int[] RecoveryPath(int startX, int startY, Pathfinding path, Game game)
        {
            var posDigger = FindDigger(game);
            var currentX = posDigger[0];
            var currentY = posDigger[1];
            while (true)
            {
                var nextX = currentX + path.parent[0, currentX, currentY];
                var nextY = currentY + path.parent[1, currentX, currentY];
                if (nextX == startX && nextY == startY)
                {
                    return new int[] { -path.parent[0, currentX, currentY], -path.parent[1, currentX, currentY] };
                }
                currentX = nextX;
                currentY = nextY;
            }
        }

        public int[] PathfindingToDigger(int startX, int startY, Pathfinding path, Game game)
        {
            var ans = new int[] { 0, 0 };
            path.queue.Add(new int[] { startX, startY });
            while (path.pointer < path.queue.Count())
            {
                UpdateQueue(path, game);
                path.pointer++;
                if (path.stop)
                    break;
            }

            if (!path.stop)
                return ans;

            return RecoveryPath(startX, startY, path, game);
        }

        public bool MonsterClose(int x, int y, Game game) {
            for (int i = x - 2; i <= x + 2; i++)
                for (int j = y - 2; j <= y + 2; j++)
                {
                    if (i < 0 || i >= game.MapWidth)
                        continue;
                    if (j < 0 || j >= game.MapHeight)
                        continue;
                    if (i == x && j == y)
                        continue;
                    if (!(game.Map[i, j] is Monster))
                        continue;
                    if (((Monster)game.Map[i, j]).Move == null)
                        continue;
                    if (i + ((Monster)game.Map[i, j]).Move[0] == x + Move[0] && j + ((Monster)game.Map[i, j]).Move[1] == y + Move[1])
                        return true;
                }

            return false;
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var path = new Pathfinding(game);
            Move = PathfindingToDigger(x, y, path, game);
            if (MonsterClose(x, y, game))
            {
                Move = new int[] { 0, 0 };
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
            }
            if (game.Map[x + Move[0], y + Move[1]] == null || game.Map[x + Move[0], y + Move[1]] is Digger)
            {
                var deltaX = Move[0];
                var deltaY = Move[1];
                return new CreatureCommand() { DeltaX = deltaX, DeltaY = deltaY, TransformTo = game.Map[x, y] };
            }
            else
            {
                Move = new int[] { 0, 0 };
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
            }
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Sack || conflictedObject is Boom)
            {
                game.Scores += 20;
                return true;
            }
            return false;
        }
    }
}
