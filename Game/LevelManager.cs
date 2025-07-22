using TanksGameProject.Entities;
using TanksGameProject.Map;

namespace TanksGameProject.Game
{
    internal class LevelManager
    {
        readonly MazeGenerator gen = new();
        public GameMap CurrentMap { get; private set; } = null!;
        public PlayerTank Player { get; private set; } = null!;
        public List<EnemyTank> Enemies { get; private set; } = [];

        public void LoadLevel(int lvl)
        {
            CurrentMap = new GameMap(Settings.MapWidth, Settings.MapHeight);
            gen.Generate(CurrentMap);
            Player = new PlayerTank(GameMap.PlayerSpawn.X, GameMap.PlayerSpawn.Y);
            CurrentMap.RegisterEntity(Player);
            CurrentMap[Player.X, Player.Y].Type = CellType.Empty;
            foreach (var (X, Y) in CurrentMap.EnemySpawns)
                CurrentMap[X, Y].Type = CellType.Empty;

            Enemies.Clear(); var rnd = new Random();
            for (int i = 0; i < lvl; i++)
            {
                (int X, int Y) pos;
                int attempts = CurrentMap.EnemySpawns.Count;
                do
                {
                    pos = CurrentMap.EnemySpawns[rnd.Next(CurrentMap.EnemySpawns.Count)];
                    attempts--;
                }
                while (attempts > 0
                && CurrentMap.Tanks.Any(
                    t => t.IsAlive
                    && t.X == pos.X
                    && t.Y == pos.Y)
                );
                var e = new EnemyTank(pos.X, pos.Y);
                Enemies.Add(e);
                CurrentMap.RegisterEntity(e);
            }
        }
    }
}