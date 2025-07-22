namespace TanksGameProject.Entities
{
    public static class DirectionExtensions
    {
        public static (int dx, int dy) ToDelta(this Direction d) => d switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => (0, 0)
        };
    }
}
