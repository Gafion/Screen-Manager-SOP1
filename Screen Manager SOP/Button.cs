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
        public ConsoleColor TextColor { get; private set; }
        public ConsoleColor BackgroundColor { get; private set; }

        // Constructor that positions the button with a top margin of 1 and a right side margin of 2
        public Button(int width, int height, string label,
                      ConsoleColor textColor = ConsoleColor.White,
                      ConsoleColor backgroundColor = ConsoleColor.White,
                      int topMargin = 1, int rightMargin = 2) // Separate margins for top and right
            : base(Console.WindowWidth - width - rightMargin, topMargin, width, height)
        {
            Label = label;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
        }

        public void Draw()
        {
            ConsoleColor originalColor = TextColor;
            TextColor = IsFocused ? ConsoleColor.Red : originalColor; // Change color if focused

            // Draw the button's box (optional, if you want a border around the button)
            Box buttonBox = new Box(Left, Top, Width, Height, BackgroundColor);
            buttonBox.Draw();

            // Calculate the position to center the label text within the button
            int centeredLeft = Left + (Width - Label.Length) / 2;
            int centeredTop = Top + Height / 2;

            // Draw the label text
            ScreenObject.InsertAt(centeredLeft, centeredTop, Label, TextColor);

            TextColor = originalColor; // Reset color after drawing
        }

        
    }
}
