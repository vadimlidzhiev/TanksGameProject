namespace TanksGameProject
{
    public static class Settings
    {
        public const int MapWidth = 13;
        public const int MapHeight = 13;

        public const int CellPxW = 2;
        public const int CellPxH = 2;

        public const int TankMoveCd = 200;
        public const int TankFireCd = 300;
        public const int BulletStep = 40;
        public const int FrameDelay = 16;
        public const int IdleSleep = 2;

        // Maze generator
        public const double WaterChance = 0.05;
    }
}