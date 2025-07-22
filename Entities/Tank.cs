using TanksGameProject.Map;
namespace TanksGameProject.Entities
{
    public abstract class Tank
    {
        protected readonly int MoveCd
            = Settings.TankMoveCd, FireCd
            = Settings.TankFireCd;
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int HP { get; protected set; } = 10;
        public Direction Facing { get; protected set; } = Direction.Up;
        protected DateTime lastMove = DateTime.MinValue, lastFire = DateTime.MinValue;
        public bool IsAlive => HP > 0;
        public Map.GameMap Map { get; set; } = null!;
        protected Tank(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected bool TryMove(Direction d)
        {
            var now = DateTime.Now;
            if ((now - lastMove).TotalMilliseconds < MoveCd)
                return false;
            (int nx, int ny) = d switch
            {
                Direction.Up => (X, Y - 1),
                Direction.Down => (X, Y + 1),
                Direction.Left => (X - 1, Y),
                Direction.Right => (X + 1, Y),
                _ => (X, Y)
            };
            if (Map.IsWalkable(nx, ny))
            {
                X = nx;
                Y = ny;
                lastMove = now;
                return true;
            }
            return false;
        }
        public void QueueMove(Direction dir)
        {
            Facing = dir;
            TryMove(dir);
        }
        public void Fire(Map.GameMap map)
        {
            var now = DateTime.Now;
            if ((now - lastFire).TotalMilliseconds < FireCd)
                return;
            map.AddBullet(new Bullet(X, Y, Facing, this, map));
            lastFire = now;
        }

        public virtual void Update(DateTime now, Map.GameMap m, IList<Tank> targets)
        {
        }

        public void Damage(int dmg)
        {
            HP -= dmg;
        }

        protected static readonly Dictionary<Direction, string[]> Glyph = new()
        {
            [Direction.Up] = ["▟▙", "▙▟"],
            [Direction.Right] = ["▛▚", "▙▞"],
            [Direction.Left] = ["▞▜", "▚▟"],
            [Direction.Down] = ["▛▜", "▚▞"]
        };

        protected virtual string[] GetGlyph() => Glyph[Facing];

        public void Draw()
        {
            if (!IsAlive) return;
            Console.ForegroundColor = this is PlayerTank
                ? ConsoleColor.Green
                : ConsoleColor.Red;

            var glyph = GetGlyph();
            for (int row = 0; row < Settings.CellPxH; row++)
            {
                Console.SetCursorPosition(X * Settings.CellPxW,
                                          Y * Settings.CellPxH + row);
                Console.Write(glyph[row]);
            }
            Console.ResetColor();
        }
    }
}