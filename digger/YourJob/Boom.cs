using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class Boom : ICreature
    {
        public string GetImageFileName { get; set; }

        public int GetDrawingPriority { get; set; }

        public Boom() {
            GetImageFileName = "Boom.png";
            GetDrawingPriority = 1;
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null};
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }
}
