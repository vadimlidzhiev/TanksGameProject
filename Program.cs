using TanksGameProject.Game;

namespace TanksGameProject
{
    internal static class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            new TanksGame().Run();
        }
    }
}