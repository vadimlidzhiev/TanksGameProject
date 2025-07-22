using TanksGameProject.Entities;
using TanksGameProject.Map;

namespace TanksGameProject.InputHandler
{
    public sealed class ConsoleInputHandler : IInputHandler
    {
        public void Poll(PlayerTank p, GameMap m)
        {
            while (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow: p.QueueMove(Direction.Up); break;
                    case ConsoleKey.DownArrow: p.QueueMove(Direction.Down); break;
                    case ConsoleKey.LeftArrow: p.QueueMove(Direction.Left); break;
                    case ConsoleKey.RightArrow: p.QueueMove(Direction.Right); break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter: p.Fire(m); break;
                }
            }
        }
    }
}
