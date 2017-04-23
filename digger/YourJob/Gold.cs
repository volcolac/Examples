using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Gold : ICreature
    {
        
        public Gold() {
            GetImageFileName = "Gold.png";
            GetDrawingPriority = 2;
        }

        public string GetImageFileName { get; set; }

        public int GetDrawingPriority { get; set; }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Digger)
            {
                game.Scores += 10;
                return true;
            }
            if (conflictedObject is Boom)
            {
                return true;
            }
            return false;
        }
    }
}
