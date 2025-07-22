using TanksGameProject.Map;

namespace TanksGameProject.Entities
{
    public class EnemyTank : Tank
    {
        private readonly Random rnd = new();

        public EnemyTank(int x, int y) : base(x, y)
        {
            HP = 5;
        }

        public override void Update(DateTime now, Map.GameMap map, IList<Tank> targets)
        {
            if (!IsAlive) return;
            if (rnd.NextDouble() < 0.4)
            {
                var dir = (Direction)rnd.Next(4);
                Facing = dir;
                TryMove(dir);
            }

            var player = targets.FirstOrDefault(t => t is PlayerTank && t.IsAlive);
            if (player == null) return;
            if (player.X == X || player.Y == Y)
            {
                if (LineClear(player, map))
                {
                    Facing = player.X == X
                        ? (player.Y > Y
                        ? Direction.Down
                        : Direction.Up)
                        : (player.X > X
                        ? Direction.Right
                        : Direction.Left);
                    Fire(map);
                }
            }
        }

        private bool LineClear(Tank target, Map.GameMap map)
        {
            return map.ClearStraightPath((X, Y), (target.X, target.Y));
        }
    }
}