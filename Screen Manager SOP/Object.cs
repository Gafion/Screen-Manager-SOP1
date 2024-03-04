using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    public class ScreenObject
    {
        private int left;
        private int top;
        private int width;
        private int height;

        public int Left
        {
            get => left;
            set => left = Math.Max(0, value); // Ensure Left is non-negative
        }

        public int Top
        {
            get => top;
            set => top = Math.Max(0, value); // Ensure Top is non-negative
        }

        public int Width
        {
            get => width;
            protected set => width = Math.Max(1, value); // Ensure Width is positive
        }

        public int Height
        {
            get => height;
            protected set => height = Math.Max(1, value); // Ensure Height is positive
        }

        public ScreenObject(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
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
