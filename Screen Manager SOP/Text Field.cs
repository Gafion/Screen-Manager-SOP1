using System.Text;
using Screen_Manager_SOP;

namespace ScreenManager
{
    internal class TextField : ScreenObject
    {
        public string Text { get; private set; }
        public ConsoleColor TextColor { get; private set; }

        public TextField(int left, int top, int width, string text, ConsoleColor textColor = ConsoleColor.White)
            : base(left, top, width, 1) // Height is 1 for a single line of text
        {
            Text = text;
            TextColor = textColor;
        }

        // Method to capture user input
        public void CaptureInput()
        {
            Console.SetCursorPosition(Left + 1, Top);
            Text = Console.ReadLine();
        }

        public void Draw()
        {
            // Clear the current text field area
            Console.SetCursorPosition(Left, Top);
            Console.Write(new string(' ', Width));

            // Draw the text within the text field area
            Console.SetCursorPosition(Left, Top);
            Console.ForegroundColor = TextColor;
            Console.Write(Text.PadRight(Width)); // Pad the text to fill the text field
            Console.ResetColor();
        }

        // Additional methods to handle user input and update the text field...
    }
}