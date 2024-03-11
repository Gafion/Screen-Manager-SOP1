using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    public class ScreenObject
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public ScreenObject(int left, int top, int width, int height)
        {
            Left = Math.Max(0, left);
            Top = Math.Max(0, top);
            Width = Math.Max(1, width);
            Height = Math.Max(1, height);
        }

        public static void ClearArea(int left, int top, int width, int height)
        {
            // Store the original cursor position
            int originalLeft = Console.CursorLeft;
            int originalTop = Console.CursorTop;

            // Create a string of spaces that is the width of the area to clear
            string blankLine = new string(' ', width);

            // Loop through each line of the area
            for (int row = 0; row < height; row++)
            {
                // Set the cursor position to the beginning of the line to clear
                Console.SetCursorPosition(left, top + row);

                // Write the blank line to clear the content
                Console.Write(blankLine);
            }

            // Restore the original cursor position
            Console.SetCursorPosition(originalLeft, originalTop);
        }

        public static void InsertAt(int left, int top, string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor(); // Reset to default color after writing
        }
    }
    
    
}
