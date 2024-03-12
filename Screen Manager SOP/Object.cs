using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    public class ScreenObject(int left, int top, int width, int height)
    {
        public int Left { get; set; } = Math.Max(0, left);
        public int Top { get; set; } = Math.Max(0, top);
        public int Width { get; protected set; } = Math.Max(1, width);
        public int Height { get; protected set; } = Math.Max(1, height);

        public static void ClearArea(int left, int top, int width, int height)
        {
            // Store the original cursor position
            int originalLeft = Console.CursorLeft;
            int originalTop = Console.CursorTop;
            string blankLine = new(' ', width);

            // Loop through each line of the area
            for (int row = 0; row < height; row++)
            {
                Console.SetCursorPosition(left, top + row);
                Console.Write(blankLine);
            }

            Console.SetCursorPosition(originalLeft, originalTop);
        }

        public static void InsertAt(int left, int top, string text, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ResetColor(); // Reset to default color after writing
        }
    }
    
    
}
