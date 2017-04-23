using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Dynamite : ICreature
    {
        public int timeToBoom { get; set; }

        public string GetImageFileName { get; set; }


        public int GetDrawingPriority { get; set; }

        public Dynamite(int timeToBoom)
        {
            this.timeToBoom = timeToBoom;
            GetDrawingPriority = 8;
            GetImageFileName = "Dynamite.png";
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            if (timeToBoom == 0)
            {
                for (int i = x - 1; i < x + 2; i++)
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        if (i > 0 && i < game.MapWidth - 1 && j > 0 && j < game.MapHeight - 1)
                        {
                            if (game.Map[i, j] is Monster)
                                game.Scores += 10;
                            game.Map[i, j] = new Boom();
                        }
                    }
                return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
            }
            else {
                timeToBoom--;
                return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Dynamite(timeToBoom) };
            }
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }
}
