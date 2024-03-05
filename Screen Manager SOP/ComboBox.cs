using System;
using System.Collections.Generic;

namespace Screen_Manager_SOP
{
    internal class ComboBox : ScreenObject
    {
        private readonly List<string> options;
        private int activeIndex = 0;
        private Box dropdownBorder;

        public string SelectedOption => options[activeIndex];

        public ComboBox(int left, int top, int width, List<string> options)
            : base(left, top, width, options.Count + 2) // +2 for the top and bottom border
        {
            this.options = options ?? new List<string>();
            // Create the Box instance for the dropdown border
            dropdownBorder = new Box(left, top, width, options.Count + 2, ConsoleColor.White);
        }

        public void Draw()
        {
            // Draw the border using the Box class
            dropdownBorder.Draw();

            // Draw the options inside the border
            for (int i = 0; i < options.Count; i++)
            {
                Console.SetCursorPosition(Left + 1, Top + 1 + i); // +1 to account for the border
                if (i == activeIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.Write(options[i].PadRight(Width - 2)); // -2 to account for the border
                Console.ResetColor();
            }
        }

        public void CaptureInput()
        {
            ConsoleKey key;
            do
            {
                Draw();
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        activeIndex = (activeIndex - 1 + options.Count) % options.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        activeIndex = (activeIndex + 1) % options.Count;
                        break;
                }
            } while (key != ConsoleKey.Enter);
        }
    }
}