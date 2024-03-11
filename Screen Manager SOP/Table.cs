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
        private List<List<string>> rows;
        private readonly List<int> columnWidths;
        private readonly int marginLeft;
        private readonly int marginRight;
        private int activeRow; // Index of the currently active (focused) row
        private int activeColumn; // Index of the currently active (focused) column


        public Table(int top, List<string> headers, List<User> users, int marginTop, int marginBottom, int marginLeft, int marginRight)
        : base(marginLeft, top + marginTop, Console.WindowWidth - marginLeft - marginRight, 0) // Height will be calculated later
        {
            this.headers = headers;
            this.rows = ConvertUsersToRows(users);
            this.marginLeft = marginLeft;
            this.marginRight = marginRight;
            this.columnWidths = CalculateColumnWidths();
            // Calculate the height based on the number of rows to display, including empty rows
            int totalRowsToDisplay = Console.WindowHeight - top - marginTop - marginBottom;
            Height = totalRowsToDisplay; // This includes content rows and empty rows
            activeRow = 0; // Initialize the active row to the first row
            activeColumn = headers.IndexOf("Delete"); // Initialize activeColumn to point to "Delete" column
        }

        private List<int> CalculateColumnWidths()
        {
            // Calculate initial widths based on the maximum length of content in headers and rows
            var initialWidths = Enumerable.Range(0, headers.Count)
                .Select(colIndex => headers[colIndex].Length)
                .Select((headerWidth, colIndex) => Math.Max(headerWidth, rows.Any() ? rows.Max(row => row.ElementAtOrDefault(colIndex)?.Length ?? 0) : 0))
                .ToList();

            // Adjust the initial widths for specific columns as per the original logic
            int adjustments = 12; // Total adjustments for columns 0, 7, and 8
            var availableWidth = Console.WindowWidth - marginLeft - marginRight - (headers.Count + 1) - adjustments;
            var totalFixedWidth = initialWidths.Sum();
            var extraSpace = availableWidth - totalFixedWidth;

            // Distribute extra space across columns, excluding specific ones (0, 7, and 8)
            var columnsToAdjust = Enumerable.Range(1, headers.Count - 3).Where(i => i != 7 && i != 8).ToList();
            foreach (var colIndex in columnsToAdjust)
            {
                int addPerColumn = extraSpace / columnsToAdjust.Count;
                initialWidths[colIndex] += addPerColumn;
            }

            // Adjust remaining space due to integer division
            extraSpace -= columnsToAdjust.Count * (extraSpace / columnsToAdjust.Count);
            for (int i = 0; i < extraSpace; i++)
            {
                initialWidths[columnsToAdjust[i]] += 1;
            }

            // Apply fixed adjustments to specific columns (0, 7, and 8)
            initialWidths[0] += 4;
            if (initialWidths.Count > 7) initialWidths[7] += 4;
            if (initialWidths.Count > 8) initialWidths[8] += 4;

            return initialWidths;
        }

        private static List<List<string>> ConvertUsersToRows(List<User> users)
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
            InsertAt(Left, Top, topBorder, ConsoleColor.White);
        }

        private void DrawHeaders()
        {
            // Draw the headers with padding
            string headerLine = Box.Vertical + string.Join(Box.Middle, headers.Select((header, index) => header.PadRight(columnWidths[index]))) + Box.Vertical;
            InsertAt(Left, Top + 1, headerLine, ConsoleColor.White);
        }

        private void DrawHeadersBottom()
        {
            // Draw a border beneath the headers
            string headerBottomBorder = Box.LeftMiddle + string.Join(Box.Cross, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.RightMiddle;
            InsertAt(Left, Top + 2, headerBottomBorder, ConsoleColor.White); // Adjusted Top + 2 to draw below headers
        }

        private void DrawRows()
        {
            for (int rowIndex = 0; rowIndex < Height - 4; rowIndex++)
            {
                int cellLeftPosition = Left; // Start position for the first cell
                var row = rowIndex < rows.Count ? rows[rowIndex] : new List<string>(new string[headers.Count]);

                for (int colIndex = 0; colIndex < headers.Count; colIndex++)
                {
                    ConsoleColor cellColor = GetCellColor(rowIndex, colIndex);
                    string cellContent = GetCellContent(row, colIndex).PadRight(columnWidths[colIndex]);
                    InsertAt(cellLeftPosition + 1, Top + 3 + rowIndex, cellContent, cellColor);

                    // Draw the vertical separator for each cell
                    InsertAt(cellLeftPosition, Top + 3 + rowIndex, Box.Vertical.ToString(), ConsoleColor.White);

                    // Update the position for the next cell, accounting for the cell content and the vertical separator
                    cellLeftPosition += columnWidths[colIndex] + 1;
                }

                // Draw the vertical line at the end of the row, after the last cell
                InsertAt(cellLeftPosition, Top + 3 + rowIndex, Box.Vertical.ToString(), ConsoleColor.White);
            }
        }

        private ConsoleColor GetCellColor(int rowIndex, int colIndex)
        {
            // Check if the cell is the active cell
            if (IsFocused && rowIndex == activeRow && colIndex == activeColumn)
                return ConsoleColor.Red; // Active cell color

            // Check if the cell is in columns 7-8
            if (colIndex >= 7)
                return ConsoleColor.DarkGray; // Columns 7-8 color

            // Default color for columns 0-6
            return ConsoleColor.Gray;
        }

        private string GetCellContent(List<string> row, int colIndex)
        {
            return row.ElementAtOrDefault(colIndex) ?? "";
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
            InsertAt(cellLeft, cellTop, cellContent, color);
        }

        private void DrawBorderBottom()
        {
            // Draw the bottom border of the table at the correct position
            string bottomBorder = Box.BottomLeft + string.Join(Box.BottomMiddle, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.BottomRight;
            // The bottom border should be drawn after the last row
            InsertAt(Left, Top + Height - 1, bottomBorder, ConsoleColor.White);
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
            RedrawCell(activeRow, previousActiveColumn, ConsoleColor.DarkGray);

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
                RedrawCell(activeRow, activeColumn, ConsoleColor.DarkGray);

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
                RedrawCell(activeRow, activeColumn, ConsoleColor.DarkGray);

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
