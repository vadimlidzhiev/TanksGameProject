using TanksGameProject.Map;
using TanksGameProject.Renderer;
namespace TanksGameProject.Entities
{
    public abstract class Tank(int x, int y)
    {
        protected readonly int MoveCd
            = Settings.TankMoveCd, FireCd
            = Settings.TankFireCd;
        public int X { get; protected set; } = x;
        public int Y { get; protected set; } = y;
        public int HP { get; protected set; } = 10;
        public Direction Facing { get; protected set; } = Direction.Up;
        protected DateTime lastMove = DateTime.MinValue, lastFire = DateTime.MinValue;
        public bool IsAlive => HP > 0;
        public GameMap Map { get; set; } = null!;

        protected bool TryMove(Direction d)
        {
            var now = DateTime.Now;
            if ((now - lastMove).TotalMilliseconds < MoveCd)
                return false;
            var (dx, dy) = d.ToDelta();
            int nx = X + dx, ny = Y + dy;
            bool free = Map.Tanks.All(t => !t.IsAlive
            || t == this
            || t.X != nx
            || t.Y != ny);
            if (Map.IsWalkable(nx, ny) && free)
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
        public void Fire(GameMap map)
        {
            var now = DateTime.Now;
            if ((now - lastFire).TotalMilliseconds < FireCd)
                return;
            map.AddBullet(new Bullet(X, Y, Facing, this, map));
            lastFire = now;
        }

        public virtual void Update(DateTime now, GameMap m, IList<Tank> targets)
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

            ConsoleRenderer.DrawSprite(X, Y, GetGlyph());
            Console.ResetColor();
        }
    }
}