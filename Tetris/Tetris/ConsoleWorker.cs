using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class ConsoleWorker
    {
        #region disabling closing
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        public int Height => Settings.Height;
        public int Width => Settings.Width;


        private string[,] gameData;
        private (string Filler, string Emptied) Content;
        private Ground Ground => Settings.Ground;
        private Tetramino MovableObject { get; set; }
        public ConsoleWorker(int height, int width, Tetramino movable)
        {
            Settings.Width = width;
            Settings.Height = height;
            gameData = new string[height, width];
            SetConsoleSize(height, width);
            MovableObject = movable;
        }
        public void MoveToNextFrame()
        {
            StringBuilder output = new StringBuilder("");
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Coordinate currCoord = new Coordinate((col, row));
                    gameData[row, col] = Ground.GetCoordinates().Contains(currCoord) || MovableObject.AllCoordinates.Contains(currCoord)
                        ? Content.Filler
                        : Content.Emptied;
                    output.Append($"{gameData[row, col]} ");
                }
                output.AppendLine();
            }
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Console.WriteLine(output.ToString());
        }
        public void SetContent(string filler, string emptier)
        {
            Content = (filler, emptier);
        }
        private void SetConsoleSize(int height, int width)
        {
            Console.WindowHeight = height;
            Console.WindowWidth = width * 2;
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }
        private void SetColor(ConsoleColor bg, ConsoleColor fg)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Clear();
        }

    }
}
