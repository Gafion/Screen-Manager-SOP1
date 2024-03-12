using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    internal class Button(int left, int top, int width, int height, string label) : ScreenObject(left, top, width, height)
    {
        public bool IsFocused { get; set; }
        public string Label { get; private set; } = label;
        public ConsoleColor TextColor { get; } = ConsoleColor.White;
        public ConsoleColor BackgroundColor { get; } = ConsoleColor.White;

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
