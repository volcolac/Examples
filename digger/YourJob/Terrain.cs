using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Terrain : ICreature
    {
        public Terrain() {
            GetImageFileName = "Terrain.png";
            GetDrawingPriority = 6;
        }

        public string GetImageFileName { get; set; }

        public int GetDrawingPriority { get; set; }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Digger || conflictedObject is Boom)
            {
                return true;
            }
            else
                return false;
        }
    }
}
