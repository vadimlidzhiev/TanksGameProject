using TanksGameProject.Map;
using TanksGameProject.Renderer;
namespace TanksGameProject.Entities
{
    public class Bullet
    {
        readonly Direction dir;
        readonly Tank owner;
        readonly GameMap map;
        readonly int speed = Settings.BulletStep;
        DateTime lastMove = DateTime.MinValue;

        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Active { get; private set; } = true;

        public Bullet(int x, int y, Direction d, Tank o, GameMap m)
        {
            dir = d; owner = o; map = m;
            X = x; Y = y;
            StepForward();
            lastMove = DateTime.Now;
        }

        public void Update(DateTime now)
        {
            if (!Active || (now - lastMove).TotalMilliseconds < speed) return;
            lastMove = now;

            if (!map.InBounds(X, Y))
            {
                Active = false;
                return;
            }
            var cell = map[X, Y];

            if (cell.Type == CellType.Wall)
            {
                map.TryDamageCell(X, Y);
                Active = false;
                return;
            }
            foreach (var t in map.Tanks)
                if (t != owner && t.IsAlive && t.X == X && t.Y == Y)
                {
                    t.Damage(1);
                    Active = false;
                    return;
                }

            StepForward();
            if (!map.InBounds(X, Y))
                Active = false;
        }

        public void Draw()
        {
            if (!Active) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            ConsoleRenderer.DrawSprite(X, Y, Enumerable.Repeat
                (new string('•', Settings.CellPxW), Settings.CellPxH).ToArray());
        }

        private void StepForward()
        {
            var (dx, dy) = dir.ToDelta();
            X += dx;
            Y += dy;
        }
    }
}