using TanksGameProject.Entities;
using TanksGameProject.InputHandler;
using TanksGameProject.Map;

namespace TanksGameProject.Game
{
    public class TanksGame
    {
        readonly LevelManager lvlMgr = new();
        readonly int frameDelay = Settings.FrameDelay;
        DateTime lastFrame = DateTime.MinValue;
        private readonly IInputHandler _input = new ConsoleInputHandler();

        public void Run()
        {
            for (int lvl = 1; lvl <= 10; lvl++)
            {
                lvlMgr.LoadLevel(lvl);
                if (!PlayLevel(lvl)) return;
            }
            Console.Clear();
            Console.WriteLine("Поздравляем! Вы прошли все уровни!");
            Console.ReadKey(true);
        }

        bool PlayLevel(int lvl)
        {
            Console.Clear(); Console.WriteLine($"Level {lvl}");
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
            var map = lvlMgr.CurrentMap;
            EnsureConsoleSize(map);
            var player = lvlMgr.Player;
            var enemies = lvlMgr.Enemies;

            while (player.IsAlive && enemies.Any(e => e.IsAlive))
            {
                var now = DateTime.Now;
                if ((now - lastFrame).TotalMilliseconds < frameDelay) continue;
                lastFrame = now;

                _input.Poll(player, map);
                player.Update(now, map, enemies.Cast<Tank>().ToList());
                foreach (var e in enemies) e.Update(now, map, new List<Tank> { player });
                map.UpdateBullets(now);
                map.CleanupProjectiles();
                Draw(map, player, enemies, lvl);
                Thread.Sleep(Settings.IdleSleep);
            }
            if (!player.IsAlive) 
            { 
                Console.Clear(); 
                Console.WriteLine("Вы проиграли!");
                Console.ReadKey(true);
                return false; }
            return true;
        }


        void EnsureConsoleSize(Map.GameMap map)
        {
            int needWidth = map.Width * Settings.CellPxW + 2;   
            int needHeight = map.PixelHeight + 4;              
            if (Console.BufferWidth < needWidth || Console.BufferHeight < needHeight)
            {
                Console.SetBufferSize(
                    Math.Max(needWidth, Console.BufferWidth),
                    Math.Max(needHeight, Console.BufferHeight));
            }
            int winW = Math.Min(needWidth, Console.LargestWindowWidth);
            int winH = Math.Min(needHeight, Console.LargestWindowHeight);
            Console.SetWindowSize(winW, winH);
        }

        void Draw(Map.GameMap map, PlayerTank player, List<EnemyTank> enemies, int lvl)
        {
            map.Draw(); 
            player.Draw();
            enemies.ForEach(e => e.Draw());
            Console.SetCursorPosition(0, map.PixelHeight); 
            Console.Write(
                $"HP: {player.HP}  " +
                $" Уровень: {lvl}   " +
                $"Количество врагов: {enemies.Count(e => e.IsAlive)}   ");
        }
    }
}