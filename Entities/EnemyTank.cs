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

        public override void Update(DateTime now, GameMap map, IList<Tank> targets)
        {
            if (!IsAlive) return;
            if (!TryMove(Facing))
            {
                for (int i = 0; i < 4; i++)
                {
                    Direction dir = (Direction)rnd.Next(4);
                    if (dir == Facing) continue;
                    if (TryMove(dir))
                    {
                        Facing = dir;
                        break;
                    }
                }
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

        private bool LineClear(Tank target, GameMap map)
        {
            return map.ClearStraightPath((X, Y), (target.X, target.Y));
        }
    }
}