using TanksGameProject;
namespace TanksGameProject.Map
{
    public class MazeGenerator
    {
        private readonly Random rnd = new();
        public void Generate(GameMap map)
        {
            int w = map.Width, h = map.Height;
            var st = new Stack<(int x, int y)>();
            map[1, 1].Type = CellType.Empty;
            st.Push((1, 1));
            int[] dx = { 0, 0, -2, 2 }, dy = { -2, 2, 0, 0 };

            while (st.Count > 0)
            {
                var (x, y) = st.Peek(); 
                var dirs = new List<int>();
                for (int d = 0; d < 4; d++)
                {
                    int nx = x + dx[d], ny = y + dy[d];
                    if (nx > 0 && ny > 0 && nx < w - 1 && ny < h - 1 
                        && map[nx, ny].Type
                        == CellType.Wall) 
                        dirs.Add(d);
                }
                if (dirs.Count == 0) 
                { 
                    st.Pop();
                    continue; 
                }
                int dir = dirs[rnd.Next(dirs.Count)];
                int wx = x + dx[dir] 
                    / 2, wy = y + dy[dir]
                    / 2, nx2 = x + dx[dir], 
                    ny2 = y + dy[dir];
                map[wx, wy].Type = CellType.Empty;
                map[nx2, ny2].Type = CellType.Empty;
                st.Push((nx2, ny2));
            }

            for (int y = 1; y < h - 1; y++)
                for (int x = 1; x < w - 1; x++)
                    if (map[x, y].Type
                        == CellType.Empty 
                        && rnd.NextDouble() 
                        < Settings.WaterChance) 
                        map[x, y].Type = CellType.Water;
        }
    }
}