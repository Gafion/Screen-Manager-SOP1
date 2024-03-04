using System.Reflection.PortableExecutable;
using Screen_Manager_SOP;
using ScreenManager;

int cWidth = Console.WindowWidth, cHeight = Console.WindowHeight;


Box box = new Box(0, 0, cWidth, cHeight, ConsoleColor.White, "CRUapp");
box.Draw();

Button button = new Button(15, 3, "New User");
button.IsFocused = false; // Button is not focused initially
button.Draw();


UserRepository userRepository = new UserRepository();
List<User> users = userRepository.GetAllUsers();
List<string> headers = new List<string>
{
    "ID", "First Name", "Last Name", "Email Adr", "Phone Nr", "Address", "Title", "Delete", "Edit"
};
Table table = new Table(5, headers, users, 0, 6, 4, 4); // Pass the list of users to the table
table.IsFocused = true; // Start with the table focused
table.Draw();

// ... existing code to set up the UI ...

bool isRunning = true;
while (isRunning)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(intercept: true).Key; // Don't display the pressed key
        switch (key)
        {
            case ConsoleKey.LeftArrow:
            case ConsoleKey.RightArrow:
                if (table.IsFocused)
                {
                    table.ToggleActiveColumn();
                }
                break;
            case ConsoleKey.UpArrow:
                if (table.IsFocused) table.MoveActiveRowUp();
                break;
            case ConsoleKey.DownArrow:
                if (table.IsFocused) table.MoveActiveRowDown();
                break;
            case ConsoleKey.Tab:
                if (table.IsFocused)
                {
                    table.IsFocused = false;
                    button.IsFocused = true;
                }
                else
                {
                    table.IsFocused = true;
                    button.IsFocused = false;
                }
                break;
            case ConsoleKey.Enter:
                if (button.IsFocused)
                {
                    int boxWidth = 55; // Adjust width for newUserBox
                    int boxHeight = Math.Min(25, Console.WindowHeight); // Ensure box height does not exceed console height
                    int boxLeft = (Console.WindowWidth - boxWidth) / 2;
                    int boxTop = (Console.WindowHeight - boxHeight) / 2;

                    Box newUserBox = new Box(boxLeft, boxTop, boxWidth, boxHeight, ConsoleColor.White, "New User");
                    newUserBox.Draw(); // Draw newUserBox first

                    // Define the starting position for the TextFields inside newUserBox
                    int textFieldWidth = newUserBox.Width / 2 - 4; // Half the width of newUserBox minus padding
                    int textFieldHeight = 3; // Height of each TextField including border lines
                    int leftHalfWidth = newUserBox.Width / 2; // Width of the left half of the newUserBox
                    int textFieldLeft = newUserBox.Left + (leftHalfWidth - textFieldWidth) / 2;
                    int textFieldTop = newUserBox.Top + 5; // Start 5 characters below the top border of newUserBox

                    // Create and draw the TextFields
                    string[] labels = { "First Name", "Last Name", "Email Adr", "Phone Nr", "Address" };
                    TextField[] textFields = new TextField[labels.Length];

                    for (int i = 0; i < labels.Length; i++)
                    {
                        // Calculate the top position for the current TextField
                        int currentTextFieldTop = textFieldTop + i * (textFieldHeight + 1); // +1 for spacing between TextFields

                        // Create the TextField with the label as initial text
                        TextField textField = new TextField(textFieldLeft, currentTextFieldTop, textFieldWidth, labels[i]);
                        textField.Draw(); // Draw the TextField
                    }

                    // Adjustments for small boxes to be within newUserBox
                    int smallBoxWidth = 22;
                    int smallBoxHeight = 3;

                    // Position small boxes within the right half of newUserBox
                    int smallBoxLeft = newUserBox.Left + 3 * boxWidth / 4 - smallBoxWidth / 2;

                    // Calculate vertical spacing for small boxes
                    int spaceAvailable = boxHeight - (5 * smallBoxHeight); // Use actual newUserBox height
                    int spaceBetweenBoxes = spaceAvailable / 6;
                    int smallBoxTop = newUserBox.Top + spaceBetweenBoxes +3;

                    // Draw the 5 smaller boxes
                    for (int i = 0; i < 5; i++)
                    {
                        int currentBoxTop = smallBoxTop + i * (smallBoxHeight + spaceBetweenBoxes);
                        Box smallBox = new Box(smallBoxLeft, currentBoxTop, smallBoxWidth, smallBoxHeight, ConsoleColor.White);
                        smallBox.Draw();
                    }

                    Console.ReadKey(true); // Wait for any key to close newUserBox
                }
                break;
            case ConsoleKey.Escape:
                isRunning = false; // Exit the loop and close the application
                break;
        }

        // Redraw the UI elements to reflect the updated state
        Console.Clear(); // Clear the console to redraw the UI
        box.Draw();
        button.Draw();
        table.Draw();
    }
}



