using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    class Digger : ICreature
    {

        public Digger() {
            GetImageFileName = "Digger.png";
            GetDrawingPriority = 5;
        }

        public string GetImageFileName { get; set; }

        public int GetDrawingPriority { get; set; }

        public CreatureCommand Act(int x, int y, Game game)
        {
            int[] mas = new int[] { 0, 0 };
            if (game.KeyPressed == Keys.F && game.LastKeyPressed != null && game.LastKeyPressed != Keys.F && game.countDynamit > 0)
            {
                game.countDynamit--;
                game.KeyPressed = game.LastKeyPressed;
                if (game.KeyPressed == Keys.W || game.KeyPressed == Keys.Up)
                {
                    if(game.Map[x, y + 1] == null)      
                        game.Map[x, y + 1] = new Dynamite(6);
                }
                if (game.KeyPressed == Keys.A || game.KeyPressed == Keys.Left)
                {
                    if (game.Map[x + 1, y] == null)  
                        game.Map[x + 1, y] = new Dynamite(6);
                }
                if (game.KeyPressed == Keys.D || game.KeyPressed == Keys.Right)
                {
                    if (game.Map[x - 1, y] == null)  
                        game.Map[x - 1, y] = new Dynamite(6);
                }
                if (game.KeyPressed == Keys.S || game.KeyPressed == Keys.Down)
                {
                    if (game.Map[x, y - 1] == null)  
                        game.Map[x, y - 1] = new Dynamite(6);
                }
                if (game.KeyPressed != null)
                    game.LastKeyPressed = game.KeyPressed;
                game.KeyPressed = null;
                return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
            }
            if (game.KeyPressed == Keys.W || game.KeyPressed == Keys.Up)
                mas = new int[] { 0, -1 };
            if (game.KeyPressed == Keys.A || game.KeyPressed == Keys.Left)
                mas = new int[] { -1, 0 };
            if (game.KeyPressed == Keys.D || game.KeyPressed == Keys.Right)
                mas = new int[] { 1, 0 };
            if (game.KeyPressed == Keys.S || game.KeyPressed == Keys.Down)
                mas = new int[] { 0, 1 };
            if(game.KeyPressed != null)
                game.LastKeyPressed = game.KeyPressed;
            game.KeyPressed = null;
  
            var ans = new CreatureCommand { DeltaX = mas[0], DeltaY = mas[1], TransformTo = game.Map[x, y] };
            if (x + ans.DeltaX > 0 && x + ans.DeltaX < game.MapWidth - 1 && y + ans.DeltaY > 0 && y + ans.DeltaY < game.MapHeight - 1 &&
                !(game.Map[x + ans.DeltaX, y + ans.DeltaY] is Sack || game.Map[x + ans.DeltaX, y + ans.DeltaY] is Dynamite))
                return ans;
            else
                return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {

            if (conflictedObject is Sack || conflictedObject is Monster || conflictedObject is Boom)
            {
                return true;
            }
            else
                return false;
        }
    }
}
