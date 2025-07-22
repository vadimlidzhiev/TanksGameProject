using TanksGameProject.Entities;
using TanksGameProject.Map;

namespace TanksGameProject.Game
{
    internal class LevelManager
    {
        readonly MazeGenerator gen = new();
        public Map.GameMap CurrentMap { get; private set; } = null!;
        public PlayerTank Player { get; private set; } = null!;
        public List<EnemyTank> Enemies { get; private set; } = new();

        public void LoadLevel(int lvl)
        {
            CurrentMap = new Map.GameMap(Settings.MapWidth, Settings.MapHeight); 
            gen.Generate(CurrentMap);
            Player = new PlayerTank(CurrentMap.PlayerSpawn.X, CurrentMap.PlayerSpawn.Y);
            CurrentMap.RegisterEntity(Player);
            Enemies.Clear(); var rnd = new Random();
            for (int i = 0; i < lvl; i++)
            {
                var (X, Y) = CurrentMap.EnemySpawns[rnd.Next(CurrentMap.EnemySpawns.Count)];
                var e = new EnemyTank(X, Y); Enemies.Add(e); CurrentMap.RegisterEntity(e);
            }
        }
    }
}