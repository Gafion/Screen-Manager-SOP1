using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    internal class Button : ScreenObject
    {
        public bool IsFocused { get; set; }
        public string Label { get; private set; }
        public ConsoleColor TextColor { get; } = ConsoleColor.White;
        public ConsoleColor BackgroundColor { get; } = ConsoleColor.White;

        // Constructor that positions the button with a top margin of 1 and a right side margin of 2
        public Button(int left, int top, int width, int height, string label)
            : base(left, top, width, height)
        {
            Label = label;
        }

        public void Draw()
        {
            // Draw the button's box
            new Box(Left, Top, Width, Height, BackgroundColor).Draw();            

            // Calculate the position to center the label text within the button
            int centeredLeft = Left + (Width - Label.Length) / 2;
            int centeredTop = Top + Height / 2;

            // Change text color if the button is focused
            ConsoleColor currentColor = IsFocused ? ConsoleColor.Red : TextColor;

            // Draw the label text
            InsertAt(centeredLeft, centeredTop, Label, currentColor);
        }
    }
}
