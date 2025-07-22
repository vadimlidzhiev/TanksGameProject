using TanksGameProject.Entities;
using TanksGameProject.Map;

namespace TanksGameProject.InputHandler
{
    public interface IInputHandler
    {
        void Poll(PlayerTank player, GameMap map);
    }
}
