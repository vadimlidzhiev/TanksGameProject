namespace TanksGameProject.Renderer
{
    public static class ConsoleRenderer
    {
        public static void DrawSprite(int x, int y, ReadOnlySpan<string> sprite)
        {
            for (int row = 0; row < sprite.Length; row++)
            {
                Console.SetCursorPosition(x * Settings.CellPxW,
                                          y * Settings.CellPxH + row);
                Console.Write(sprite[row]);
            }
        }
    }
}
