using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    public class Box : ScreenObject
    {
        public ConsoleColor Color { get; private set; }
        public string Title { get; set; }

        public Box(int left, int top, int width, int height, ConsoleColor color = ConsoleColor.White, string title = "")
            : base(left, top, width, height)
        {
            Color = color;
            Title = title;
        }

        private void ClearArea()
        {
            for (int row = 0; row < Height; row++)
            {
                Console.SetCursorPosition(Left, Top + row);
                Console.Write(new string(' ', Width));
            }
        }

        public void Draw()
        {
            // Define all border characters
            char topLeft = '┌';
            char topRight = '┐';
            char bottomLeft = '└';
            char bottomRight = '┘';
            char horizontal = '─';
            char vertical = '│';
            char leftMiddle = '├';
            char rightMiddle = '┤';
            char topMiddle = '┬';
            char bottomMiddle = '┴';
            char cross = '┼';
            char middle = '│'; // Same as vertical, included for completeness

            ClearArea();

            // Draw top border
            ScreenObject.InsertAt(Left, Top, topLeft + new string(horizontal, Width - 2) + topRight, Color);

            // Draw the title text
            if (!string.IsNullOrEmpty(Title))
            {
                // Ensure the title fits within the box's width
                string trimmedTitle = Title.Length > Width - 2 ? Title.Substring(0, Width - 2) : Title;
                ScreenObject.InsertAt(Left + 2, Top + 1, trimmedTitle, Color); // Position the title inside the box, at the top left corner
            }

            // Draw middle sections
            for (int i = 1; i < Height - 1; i++)
            {
                ScreenObject.InsertAt(Left, Top + i, vertical.ToString(), Color);
                // If you want to include middle cross sections, you can modify this loop
                ScreenObject.InsertAt(Left + Width - 1, Top + i, vertical.ToString(), Color);
            }

            // Draw bottom border
            ScreenObject.InsertAt(Left, Top + Height - 1, bottomLeft + new string(horizontal, Width - 2) + bottomRight, Color);
        }
    }
}
