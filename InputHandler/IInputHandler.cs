using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksGameProject.Entities;
using TanksGameProject.Map;

namespace TanksGameProject.InputHandler
{
    public interface IInputHandler
    {
        void Poll(PlayerTank player, Map.GameMap map);
    }
}
