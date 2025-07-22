namespace TanksGameProject.Map
{
    public enum CellType { Empty, Wall, Water }

    public class Cell
    {
        public CellType Type { get; set; }
        public int Health { get; set; }

        public Cell(CellType type)
        {
            Type = type;
            Health = type == CellType.Wall ? 3 : 0;
        }

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