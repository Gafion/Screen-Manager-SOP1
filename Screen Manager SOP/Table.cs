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
        private List<string> headers;
        private List<List<string>> rows;
        private List<int> columnWidths;
        private int marginLeft = 4;
        private int marginRight = 4;
        private int activeRow; // Index of the currently active (focused) row
        private int activeColumn; // Index of the currently active (focused) column


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

            // Initial calculation based on content for all columns
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

            // Subtract the additional width for columns 0, 7, and 8 from the available width
            availableWidth -= 12; // 4 units for each of the three columns

            // Adjust widths to fill the available space
            int extraSpace = availableWidth - totalFixedWidth;
            int columnsToAdjust = 6; // Columns 2 to 7
            int addPerColumn = extraSpace / columnsToAdjust;
            int remainder = extraSpace % columnsToAdjust;

            // Distribute extra space to columns 2 to 7
            for (int i = 1; i <= 6; i++) // Assuming columns are 0-indexed, adjust for your case
            {
                widths[i] += addPerColumn + (i - 1 < remainder ? 1 : 0); // Adjust for 0-index
            }

            // Add +4 to the widths of columns 0, 7, and 8
            widths[0] += 4; // Column 1 (0-indexed)
            if (widths.Count > 7) widths[7] += 4; // Column 8 (0-indexed)
            if (widths.Count > 8) widths[8] += 4; // Column 9 (0-indexed)

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
            string topBorder = Box.TopLeft + string.Join(Box.TopMiddle, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.TopRight;
            ScreenObject.InsertAt(Left, Top, topBorder, ConsoleColor.White);
        }

        private void DrawHeaders()
        {
            // Draw the headers with padding
            string headerLine = Box.Vertical + string.Join(Box.Middle, headers.Select((header, index) => header.PadRight(columnWidths[index]))) + Box.Vertical;
            ScreenObject.InsertAt(Left, Top + 1, headerLine, ConsoleColor.White);
        }

        private void DrawHeadersBottom()
        {
            // Draw a border beneath the headers
            string headerBottomBorder = Box.LeftMiddle + string.Join(Box.Cross, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.RightMiddle;
            ScreenObject.InsertAt(Left, Top + 2, headerBottomBorder, ConsoleColor.White); // Adjusted Top + 2 to draw below headers
        }

        private void DrawRows()
        {
            for (int rowIndex = 0; rowIndex < Height - 4; rowIndex++)
            {
                int cellLeftPosition = Left; // Start position for the first cell

                ScreenObject.InsertAt(cellLeftPosition, Top + 3 + rowIndex, Box.Middle.ToString(), ConsoleColor.Gray);
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
                        ScreenObject.InsertAt(cellLeftPosition - 1, Top + 3 + rowIndex, Box.Middle.ToString(), ConsoleColor.Gray);
                    }
                }

                // Draw the vertical line at the end of the row
                ScreenObject.InsertAt(cellLeftPosition - 1, Top + 3 + rowIndex, Box.Middle.ToString(), ConsoleColor.Gray);
            }
        }

        private void RedrawCell(int rowIndex, int colIndex, ConsoleColor color)
        {
            // Calculate the left position of the cell
            int cellLeft = Left + columnWidths.Take(colIndex).Sum() + colIndex + 1;
            // Calculate the top position of the cell
            int cellTop = Top + 3 + rowIndex;
            // Get the width of the cell
            int cellWidth = columnWidths[colIndex];
            // Assuming each cell is one line high
            int cellHeight = 1;

            // Clear the cell area
            ClearArea(cellLeft, cellTop, cellWidth, cellHeight);

            // Retrieve the content of the cell
            string cellContent = rows[rowIndex][colIndex].PadRight(cellWidth);

            // Redraw the cell with the new color
            ScreenObject.InsertAt(cellLeft, cellTop, cellContent, color);
        }

        private void DrawBorderBottom()
        {
            // Draw the bottom border of the table at the correct position
            string bottomBorder = Box.BottomLeft + string.Join(Box.BottomMiddle, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.BottomRight;
            // The bottom border should be drawn after the last row
            ScreenObject.InsertAt(Left, Top + Height - 1, bottomBorder, ConsoleColor.White);
        }

        public void ToggleActiveColumn()
        {
            // Save the previous active column
            int previousActiveColumn = activeColumn;

            // Toggle the active column
            if (activeColumn == headers.Count - 2) // If "Delete" is active
            {
                activeColumn = headers.Count - 1; // Make "Edit" active
            }
            else if (activeColumn == headers.Count - 1) // If "Edit" is active
            {
                activeColumn = headers.Count - 2; // Make "Delete" active
            }

            // Redraw the previously active cell with the default color
            RedrawCell(activeRow, previousActiveColumn, ConsoleColor.Gray);

            // Redraw the newly active cell with the highlight color
            RedrawCell(activeRow, activeColumn, ConsoleColor.Red);
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
                // Redraw the current active cell with the default color
                RedrawCell(activeRow, activeColumn, ConsoleColor.Gray);

                // Update the active row index
                activeRow--;

                // Redraw the new active cell with the highlight color
                RedrawCell(activeRow, activeColumn, ConsoleColor.Red);
            }
        }

        // Method to move the active row down
        public void MoveActiveRowDown()
        {
            if (activeRow < rows.Count - 1)
            {
                // Redraw the current active cell with the default color
                RedrawCell(activeRow, activeColumn, ConsoleColor.Gray);

                // Update the active row index
                activeRow++;

                // Redraw the new active cell with the highlight color
                RedrawCell(activeRow, activeColumn, ConsoleColor.Red);
            }
        }

        public void UpdateDataSource(List<User> newUsers)
        {
            this.rows = ConvertUsersToRows(newUsers); // Update the rows with the new user data
                                                      
        }

    }
}
