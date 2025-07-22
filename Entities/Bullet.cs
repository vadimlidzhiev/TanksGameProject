using TanksGameProject.Map;
namespace TanksGameProject.Entities
{
    public class Bullet
    {
        readonly Direction dir;
        readonly Tank owner;
        readonly Map.GameMap map;
        readonly int speed = Settings.BulletStep;
        DateTime lastMove = DateTime.MinValue;
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Active { get; private set; } = true;

        public Bullet(int x, int y, Direction d, Tank o, Map.GameMap m)
        {
            dir = d; owner = o; map = m;
            (X, Y) = d switch
            {
                Direction.Up => (x, y - 1),
                Direction.Down => (x, y + 1),
                Direction.Left => (x - 1, y),
                Direction.Right => (x + 1, y),
                _ => (x, y)
            };
        }

        public void Update(DateTime now)
        {
            if (!Active || (now - lastMove).TotalMilliseconds < speed)
                return;
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
            {
                if (t != owner && t.IsAlive && t.X == X && t.Y == Y)
                {
                    t.Damage(1);
                    Active = false;
                    return;
                }
            }
            (X, Y) = dir switch
            {
                Direction.Up => (X, Y - 1),
                Direction.Down => (X, Y + 1),
                Direction.Left => (X - 1, Y),
                Direction.Right => (X + 1, Y),
                _ => (X, Y)
            };
            if (!map.InBounds(X, Y)) Active = false;
        }
        public void Draw()
        {
            if (!Active) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int dy = 0; dy < Settings.CellPxH; dy++)
            {
                Console.SetCursorPosition(X * Settings.CellPxW,
                                          Y * Settings.CellPxH + dy);
                Console.Write(new string('•', Settings.CellPxW));
            }
        }
    }
}