using System.Text;
using Screen_Manager_SOP;

namespace ScreenManager
{
    internal class TextField(int left, int top, int width, string text, ConsoleColor textColor = ConsoleColor.White) : ScreenObject(left, top, width, 1)
    {
        public string Text { get; private set; } = text;
        public ConsoleColor TextColor { get; private set; } = textColor;

        public void CaptureInput()
        {
            Console.SetCursorPosition(Left + 1, Top);
            Text = ReadInput("N/A");
        }

        public void Draw()
        {
            ClearArea(Left, Top, Width, Height); 
            InsertAt(Left, Top, Text);

        }

        private static string ReadInput(string defaultValue)
        {
            var input = Console.ReadLine();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }
    }
}