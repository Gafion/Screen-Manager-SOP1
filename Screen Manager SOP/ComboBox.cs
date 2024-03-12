using System;
using System.Collections.Generic;

namespace Screen_Manager_SOP
{
    internal class ComboBox(int left, int top, int width, List<string> options) : ScreenObject(left, top, width, options.Count)
    {
        private readonly List<string> options = options;
        private int activeIndex = 0;
        private readonly Box dropdownBorder = new(left, top, width, options.Count + 2, ConsoleColor.White);

        public string SelectedOption => options[activeIndex];

        public void Draw()
        {
            ClearArea(Left, Top, Width, options.Count);
            dropdownBorder.Draw();

            for (int i = 0; i < options.Count; i++)
            {
                string text = options[i].PadRight(Width - 2); // -2 to account for the border
                if (i == activeIndex)
                {
                    // Use InsertAt method with custom colors for the active option
                    InsertAt(Left + 1, Top + 1 + i, text, ConsoleColor.Black, ConsoleColor.White);
                }
                else
                {
                    // Use InsertAt method with default colors for other options
                    InsertAt(Left + 1, Top + 1 + i, text);
                }
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