namespace Tetris
{
    internal static class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }

        public static Ground Ground { get; } = new Ground();
    }
}
