using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Sack : ICreature
    {
        public Sack() {
            GetImageFileName = "Sack.png";
            GetDrawingPriority = 3;
        }

        public bool isFly { get; set; }

        public string GetImageFileName { get; set; }

        public int GetDrawingPriority { get; set; }

        public CreatureCommand Act(int x, int y, Game game)
        {
            if (!(game.Map[x, y + 1] is Terrain || game.Map[x, y + 1] is Sack || game.Map[x, y + 1] is Dynamite))
            {
                isFly = true;
                return new CreatureCommand { DeltaX = 0, DeltaY = 1, TransformTo = game.Map[x, y] };
            }
            else
                if (game.Map[x, y + 1] is Terrain && isFly)
                {
                    return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
                }
                else
                    return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Boom) {
                game.Scores += 10;
                return true;
            }
            return false;
        }
    }
}
