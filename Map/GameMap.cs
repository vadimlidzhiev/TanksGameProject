using TanksGameProject.Entities;

namespace TanksGameProject.Map
{
    public class GameMap
    {
        public readonly int Width;
        public readonly int Height;
        private readonly Cell[,] _cells;
        private readonly List<Bullet> _bullets = new();
        private readonly List<Tank> _tanks = new();

        public (int X, int Y) PlayerSpawn => (1, 1);
        public List<(int X, int Y)> EnemySpawns => new()
        { 
            (Width - 2, Height - 2),
            (Width - 2, 1),
            (1, Height - 2) 
        };

        public int PixelWidth => Width * Settings.CellPxW;
        public int PixelHeight => Height * Settings.CellPxH;

        public GameMap(int w, int h)
        {
            Width = w; Height = h;
            _cells = new Cell[w, h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    _cells[x, y] = new Cell(CellType.Wall);
        }

        public Cell this[int x, int y] => _cells[x, y];
        public bool InBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
        public bool IsWalkable(int x, int y) => InBounds(x, y) && _cells[x, y].Type == CellType.Empty;

        public void RegisterEntity(Tank t) 
        {
            t.Map = this;
            _tanks.Add(t);
        }
        public bool ClearStraightPath((int X, int Y) from, (int X, int Y) to)
        {
            if (from.X == to.X)
            {
                int step = to.Y > from.Y ? 1 : -1;
                for (int y = from.Y + step; y != to.Y; y += step)
                    if (_cells[from.X, y].Type == CellType.Wall) return false;
                return true;
            }
            if (from.Y == to.Y)
            {
                int step = to.X > from.X ? 1 : -1;
                for (int x = from.X + step; x != to.X; x += step)
                    if (_cells[x, from.Y].Type == CellType.Wall) return false;
                return true;
            }
            return false;
        }

        public IEnumerable<Tank> Tanks => _tanks;
        public void AddBullet(Bullet b) => _bullets.Add(b);
        public void CleanupProjectiles() => _bullets.RemoveAll(b => !b.Active);
        public void UpdateBullets(DateTime now)
        {
            foreach (var b in _bullets.ToList())
                b.Update(now);
        }

        public bool TryDamageCell(int x, int y)
        {
            if (!InBounds(x, y)) return false;
            var c = _cells[x, y];
            if (c.Type != CellType.Wall) return false;
            c.Health--;
            if (c.Health <= 0) c.Type = CellType.Empty;
            return true;
        }

        public void Draw()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int row = 0; row < Settings.CellPxH; row++)
                {
                    int consoleY = y * Settings.CellPxH + row;
                    Console.SetCursorPosition(0, consoleY);

                    for (int x = 0; x < Width; x++)
                    {
                        var cell = _cells[x, y];

                        Console.ForegroundColor = cell.Type switch
                        {
                            CellType.Wall => ConsoleColor.DarkRed,
                            CellType.Water => ConsoleColor.DarkBlue,
                            _ => ConsoleColor.Gray
                        };

                        char g = cell.GetGlyph();
                        Console.Write($"{g}{g}");
                    }
                }
            }

            Console.ResetColor();

            foreach (var b in _bullets)
                b.Draw();
        }
    }
}