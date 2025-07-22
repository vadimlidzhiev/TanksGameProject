using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksGameProject.Entities;
using TanksGameProject.Map;

namespace TanksGameProject.InputHandler
{
    public sealed class ConsoleInputHandler : IInputHandler
    {
        public void Poll(PlayerTank p, Map.GameMap m)
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
