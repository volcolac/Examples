using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
	public class DiggerWindow : Form
    {
        const int ElementSize = 32;
        Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        public List<CreatureAnimation> animations { get; set; }
        public Game game { get; set; }
        public string currentLevel { get; set; }
        public MainMenu menu { get; set; }

        public DiggerWindow(string fileName, MainMenu menu)
        {
            this.menu = menu;
            currentLevel = fileName;
            game = new Game(currentLevel);
            animations = new List<CreatureAnimation>();
            ClientSize = new Size(ElementSize * game.MapWidth, ElementSize * game.MapHeight + ElementSize);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Text = fileName.Substring(7, 6);
            DoubleBuffered = true;
    
            this.KeyDown += new KeyEventHandler(DiggerWindow_KeyDown);

            var imagesDirectory = new DirectoryInfo("Images");
            foreach(var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name]=(Bitmap)Bitmap.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();
        }


        private void DiggerWindow_KeyDown(object sender, KeyEventArgs e)
        {
            game.KeyPressed = e.KeyCode;
        }

        void Act(Type typeCreature) {
            for (int x = 0; x < game.MapWidth; x++)
                for (int y = 0; y < game.MapHeight; y++)
                {
                    var creature = game.Map[x, y];
                    if (creature == null) continue;
                    if (!(creature.GetType() == typeCreature)) continue;
                    var command = creature.Act(x, y, game);

                    animations.Add(new CreatureAnimation
                    {
                        Command = command,
                        Creature = creature,
                        Location = new Point(x * ElementSize, y * ElementSize)
                    });
                }
        }

        void Act()
        {
            animations.Clear();
            for (int x = 0; x < game.MapWidth; x++)
                for (int y = 0; y < game.MapHeight; y++)
                {
                    var creature = game.Map[x, y];
                    if (creature == null || creature is Boom || creature is Dynamite) continue;
                    var command = creature.Act(x,y, game);

                    if (creature is Monster)
                    {
                        ((Monster)creature).Delay = !((Monster)creature).Delay;
                        if (((Monster)creature).Delay)
                        {
                            command = creature.Act(x, y, game);
                        }
                        else
                            command = new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = game.Map[x, y] };
                    }

                    animations.Add(new CreatureAnimation
                    {
                        Command=command,
                        Creature = creature,
                        Location = new Point(x * ElementSize, y * ElementSize)
                    });
                }

            Act(typeof(Dynamite));

            Act(typeof(Boom));

            animations = animations.OrderByDescending(z => z.Creature.GetDrawingPriority).ToList();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, ElementSize);
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, ElementSize * game.MapWidth, ElementSize * game.MapHeight);
            foreach (var a in animations)
                e.Graphics.DrawImage(bitmaps[a.Creature.GetImageFileName], a.Location);
            e.Graphics.ResetTransform();
            var str = game.Scores.ToString() + " из " + game.WinCondition.ToString() + " Бомб: " + game.countDynamit.ToString();
            e.Graphics.DrawString(str, new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        int tickCount = 0;

        void TimerTick(object sender, EventArgs args)
        {
            if (game.WinCondition <= game.Scores) {
                menu.Set_OK(currentLevel);
                this.Close();
            }

            if (game.KeyPressed == Keys.Escape)
            {
                this.Close();      
            }
            if (game.KeyPressed == Keys.Space) {
                game = new Game(currentLevel);
                Act();
            }
            else
            {
                if (tickCount == 0) Act();
                foreach (var e in animations)
                    e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
                if (tickCount == 7)
                {
                    for (int x = 0; x < game.MapWidth; x++) for (int y = 0; y < game.MapHeight; y++) game.Map[x, y] = null;
                    foreach (var e in animations)
                    {
                        var x = e.Location.X / 32;
                        var y = e.Location.Y / 32;
                        var nextCreature = e.Command.TransformTo;

                        if (game.Map[x, y] == null || nextCreature == null) game.Map[x, y] = nextCreature;
                        else
                        {
                            bool newDead = nextCreature.DeadInConflict(game.Map[x, y], game);
                            bool oldDead = game.Map[x, y].DeadInConflict(nextCreature, game);
                            if (newDead && oldDead)
                                game.Map[x, y] = null;
                            else if (!newDead && oldDead)
                                game.Map[x, y] = nextCreature;
                            else if (!newDead && !oldDead)
                                throw new Exception(string.Format("Существа {0} и {1} претендуют на один и тот же участок карты", nextCreature.GetType().Name, game.Map[x, y].GetType().Name));
                        }

                    }
                }
            }
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DiggerWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "DiggerWindow";
            this.ResumeLayout(false);

        }
    }
}
