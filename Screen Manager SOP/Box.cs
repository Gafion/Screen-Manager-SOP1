using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreenManager;

namespace Screen_Manager_SOP
{
    public class Box(int left, int top, int width, int height, ConsoleColor color = ConsoleColor.White) : ScreenObject(left, top, width, height)
    {
        public const char TopLeft = '┌';
        public const char TopRight = '┐';
        public const char BottomLeft = '└';
        public const char BottomRight = '┘';
        public const char Horizontal = '─';
        public const char Vertical = '│';
        public const char LeftMiddle = '├';
        public const char RightMiddle = '┤';
        public const char TopMiddle = '┬';
        public const char BottomMiddle = '┴';
        public const char Cross = '┼';
        public const char Middle = '│'; // Same as Vertical, added for completeness
        public ConsoleColor Color { get; private set; } = color;

        public void Draw()
        {
            ClearArea(Left, Top, Width, Height);
            InsertAt(Left, Top, $"{TopLeft}{new string(Horizontal, Width - 2)}{TopRight}", Color); // Draw the top border

            for (int i = 1; i < Height - 1; i++) // Draw middle sections
            {
                InsertAt(Left, Top + i, Vertical.ToString(), Color);
                InsertAt(Left + Width - 1, Top + i, Vertical.ToString(), Color);
            }

            InsertAt(Left, Top + Height - 1, $"{BottomLeft}{new string(Horizontal, Width - 2)}{BottomRight}", Color); // Draw bottom border
        }
    }
}
