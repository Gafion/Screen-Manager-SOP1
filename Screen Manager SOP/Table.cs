using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    internal class Table : ScreenObject
    {
        public bool IsFocused { get; set; }
        private readonly List<string> headers;
        private readonly List<List<string>> rows;
        private readonly List<int> columnWidths;
        private readonly int marginLeft = 4;
        private readonly int marginRight = 4;
        private readonly int marginTop = 5;
        private readonly int marginBottom = 6;
        private int activeRow; // Index of the currently active (focused) row
        private int activeColumn; // Index of the currently active (focused) column
        private readonly int deleteColumnIndex;
        private readonly int editColumnIndex;


        public Table(int top, List<string> headers, List<User> users, int marginTop, int marginBottom, int marginLeft, int marginRight)
        : base(marginLeft, top + marginTop, Console.WindowWidth - marginLeft - marginRight, 0) // Height will be calculated later
        {
            this.headers = headers;
            this.rows = ConvertUsersToRows(users);
            this.columnWidths = CalculateColumnWidths();
            // Calculate the height based on the number of rows to display, including empty rows
            int totalRowsToDisplay = Console.WindowHeight - top - marginTop - marginBottom;
            Height = totalRowsToDisplay; // This includes content rows and empty rows
            activeRow = 0; // Initialize the active row to the first row
            activeColumn = headers.IndexOf("Delete"); // Initialize activeColumn to point to "Delete" column
        }

        private List<int> CalculateColumnWidths()
        {
            var availableWidth = Console.WindowWidth - marginLeft - marginRight - (headers.Count + 1); // Account for vertical separators and margins
            var widths = new List<int>();
            var totalFixedWidth = 0;

            // Initial calculation based on content
            for (int i = 0; i < headers.Count; i++)
            {
                int maxWidth = headers[i].Length;
                foreach (var row in rows)
                {
                    if (i < row.Count) // Ensure the row has enough columns
                    {
                        maxWidth = Math.Max(maxWidth, row[i].Length);
                    }
                }
                widths.Add(maxWidth);
                totalFixedWidth += maxWidth;
            }

            // Adjust widths to fill the available space
            int extraSpace = availableWidth - totalFixedWidth;
            int addPerColumn = extraSpace / headers.Count;
            int remainder = extraSpace % headers.Count;

            for (int i = 0; i < widths.Count; i++)
            {
                widths[i] += addPerColumn + (i < remainder ? 1 : 0);
            }

            return widths;
        }

        private List<List<string>> ConvertUsersToRows(List<User> users)
        {
            return users.Select(user => new List<string>
            {
                user.Id.ToString(),
                user.FirstName,
                user.LastName,
                user.EmailAddress,
                user.PhoneNumber,
                user.Address,
                user.Title,
                "Delete", // Placeholder for delete action
                "Edit"    // Placeholder for edit action
            }).ToList();
        }

        public void Draw()
        {
            DrawBorderTop();
            DrawHeaders();
            DrawHeadersBottom();
            DrawRows();
            DrawBorderBottom();
        }

        private void DrawBorderTop()
        {
            // Draw the top border of the table
            string topBorder = "┌" + string.Join("┬", columnWidths.Select(w => new string('─', w))) + "┐";
            ScreenObject.InsertAt(Left, Top, topBorder, ConsoleColor.White);
        }

        private void DrawHeaders()
        {
            // Draw the headers with padding
            string headerLine = "│" + string.Join("│", headers.Select((header, index) => header.PadRight(columnWidths[index]))) + "│";
            ScreenObject.InsertAt(Left, Top + 1, headerLine, ConsoleColor.White);
        }

        private void DrawHeadersBottom()
        {
            // Draw a border beneath the headers
            string headerBottomBorder = "├" + string.Join("┼", columnWidths.Select(w => new string('─', w))) + "┤";
            ScreenObject.InsertAt(Left, Top + 2, headerBottomBorder, ConsoleColor.White); // Adjusted Top + 2 to draw below headers
        }

        private void DrawRows()
        {
            for (int rowIndex = 0; rowIndex < Height - 4; rowIndex++)
            {
                int cellLeftPosition = Left; // Start position for the first cell

                ScreenObject.InsertAt(cellLeftPosition, Top + 3 + rowIndex, "│", ConsoleColor.Gray);
                cellLeftPosition += 1; // Move past the separator

                var row = rowIndex < rows.Count ? rows[rowIndex] : new List<string>(new string[headers.Count]);
                for (int colIndex = 0; colIndex < headers.Count; colIndex++)
                {
                    // Determine cell color
                    ConsoleColor cellColor = ConsoleColor.Gray; // Default color
                    if (IsFocused && rowIndex == activeRow && colIndex == activeColumn)
                    {
                        cellColor = ConsoleColor.Red; // Highlight color for active cell
                    }

                    // Write cell content
                    string cellContent = (row.ElementAtOrDefault(colIndex) ?? "").PadRight(columnWidths[colIndex]);
                    ScreenObject.InsertAt(cellLeftPosition, Top + 3 + rowIndex, cellContent, cellColor);

                    // Update the position for the next cell, accounting for the cell content and the vertical separator
                    cellLeftPosition += columnWidths[colIndex] + 1;

                    // Draw the vertical separator after the cell content, except after the last cell
                    if (colIndex < headers.Count - 1)
                    {
                        ScreenObject.InsertAt(cellLeftPosition - 1, Top + 3 + rowIndex, "│", ConsoleColor.Gray);
                    }
                }

                // Draw the vertical line at the end of the row
                ScreenObject.InsertAt(cellLeftPosition - 1, Top + 3 + rowIndex, "│", ConsoleColor.Gray);
            }
        }

        private void DrawBorderBottom()
        {
            // Draw the bottom border of the table at the correct position
            string bottomBorder = "└" + string.Join("┴", columnWidths.Select(w => new string('─', w))) + "┘";
            // The bottom border should be drawn after the last row
            ScreenObject.InsertAt(Left, Top + Height - 1, bottomBorder, ConsoleColor.White);
        }

        public void ToggleActiveColumn()
        {
            // Assuming "Delete" is the second last and "Edit" is the last column
            if (activeColumn == headers.Count - 2) // If "Delete" is active
            {
                activeColumn = headers.Count - 1; // Make "Edit" active
            }
            else if (activeColumn == headers.Count - 1) // If "Edit" is active
            {
                activeColumn = headers.Count - 2; // Make "Delete" active
            }
        }

        public void SetActiveRow(int index)
        {
            activeRow = index;
            // Ensure the index is within the bounds of the rows list
            activeRow = Math.Max(0, Math.Min(activeRow, rows.Count - 1));
        }

        // Method to move the active row up
        public void MoveActiveRowUp()
        {
            if (activeRow > 0)
            {
                activeRow--;
            }
        }

        // Method to move the active row down
        public void MoveActiveRowDown()
        {
            if (activeRow < rows.Count - 1)
            {
                activeRow++;
            }
        }
               
        
        // Method to draw the table based on the list of users
        void DrawTable(List<User> users)
        {
            // Clear the previous table or refresh the screen if necessary
            Console.Clear();

            // Logic to draw the table headers

            // Logic to draw each user in the list
            foreach (var user in users)
            {
                // Draw the user details in the table
            }

            // Any additional logic to finalize the table drawing
        }
    }
}
