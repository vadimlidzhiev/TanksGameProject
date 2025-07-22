namespace TanksGameProject.Map
{
    public class Cell(CellType type)
    {
        public CellType Type { get; set; } = type;
        public int Health { get; set; } = type == CellType.Wall ? 3 : 0;

        public char GetGlyph()
        {
            return Type switch
            {
                CellType.Empty => ' ',
                CellType.Wall => Health switch { 3 => '▓', 2 => '▒', _ => '░' },
                CellType.Water => '▒',
                _ => '?'
            };
        }
    }
}