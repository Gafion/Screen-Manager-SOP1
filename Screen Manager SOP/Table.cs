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
        public readonly List<string> headers;
        private List<List<string>> rows;
        private readonly List<int> columnWidths;
        private readonly int marginLeft;
        private readonly int marginRight;
        public int activeRow; 
        public int activeColumn; 


        public Table(int top, List<string> headers, List<User> users, int marginTop, int marginBottom, int marginLeft, int marginRight)
        : base(marginLeft, top + marginTop, Console.WindowWidth - marginLeft - marginRight, 0)
        {
            this.headers = headers;
            this.rows = ConvertUsersToRows(users);
            this.marginLeft = marginLeft;
            this.marginRight = marginRight;
            this.columnWidths = CalculateColumnWidths();

            // Calculate the height based on the number of rows to display, including empty rows
            int totalRowsToDisplay = Console.WindowHeight - top - marginTop - marginBottom;
            Height = totalRowsToDisplay;

            // Initialize the active row and column
            activeRow = 0;
            activeColumn = headers.IndexOf("Delete");
        }

        private List<int> CalculateColumnWidths()
        {
            // Calculate initial widths based on the maximum length of content in headers and rows
            var initialWidths = Enumerable.Range(0, headers.Count)
                .Select(colIndex => headers[colIndex].Length)
                .Select((headerWidth, colIndex) => Math.Max(headerWidth, rows.Count != 0 ? rows.Max(row => row.ElementAtOrDefault(colIndex)?.Length ?? 0) : 0))
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
            string topBorder = Box.TopLeft + string.Join(Box.TopMiddle, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.TopRight;
            InsertAt(Left, Top, topBorder, ConsoleColor.White);
        }

        private void DrawHeaders()
        {
            string headerLine = Box.Vertical + string.Join(Box.Middle, headers.Select((header, index) => header.PadRight(columnWidths[index]))) + Box.Vertical;
            InsertAt(Left, Top + 1, headerLine, ConsoleColor.White);
        }

        private void DrawHeadersBottom()
        {
            string headerBottomBorder = Box.LeftMiddle + string.Join(Box.Cross, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.RightMiddle;
            InsertAt(Left, Top + 2, headerBottomBorder, ConsoleColor.White);
        }

        private void DrawBorderBottom()
        {
            string bottomBorder = Box.BottomLeft + string.Join(Box.BottomMiddle, columnWidths.Select(w => new string(Box.Horizontal, w))) + Box.BottomRight;
            InsertAt(Left, Top + Height - 1, bottomBorder, ConsoleColor.White);
        }

        private void DrawRows()
        {
            for (int rowIndex = 0; rowIndex < Height - 4; rowIndex++)
            {
                int cellLeftPosition = Left;
                var row = rowIndex < rows.Count ? rows[rowIndex] : new List<string>(new string[headers.Count]);

                for (int colIndex = 0; colIndex < headers.Count; colIndex++)
                {
                    ConsoleColor cellColor = GetCellColor(rowIndex, colIndex);
                    string cellContent = GetCellContent(row, colIndex).PadRight(columnWidths[colIndex]);
                    InsertAt(cellLeftPosition + 1, Top + 3 + rowIndex, cellContent, cellColor);
                    InsertAt(cellLeftPosition, Top + 3 + rowIndex, Box.Vertical.ToString(), ConsoleColor.White);
                    cellLeftPosition += columnWidths[colIndex] + 1;
                }
                InsertAt(cellLeftPosition, Top + 3 + rowIndex, Box.Vertical.ToString(), ConsoleColor.White);
            }
        }

        private ConsoleColor GetCellColor(int rowIndex, int colIndex)
        {
            if (IsFocused && rowIndex == activeRow && colIndex == activeColumn)
                return ConsoleColor.Red;

            if (colIndex >= 7)
                return ConsoleColor.DarkGray;

            return ConsoleColor.Gray;
        }

        private static string GetCellContent(List<string> row, int colIndex)
        {
            return row.ElementAtOrDefault(colIndex) ?? "";
        }

        private void RedrawCell(int rowIndex, int colIndex, ConsoleColor color)
        {
            int cellLeft = Left + Enumerable.Range(0, colIndex).Sum(i => columnWidths[i] + 1) + 1;
            int cellTop = Top + 3 + rowIndex;
            string cellContent = rows[rowIndex][colIndex].PadRight(columnWidths[colIndex]);
            ClearArea(cellLeft, cellTop, columnWidths[colIndex], 1);
            InsertAt(cellLeft, cellTop, cellContent, color);
        }

        

        public void ToggleActiveColumn()
        {
            int previousActiveColumn = activeColumn;
            activeColumn = (activeColumn == headers.Count - 2) ? headers.Count - 1 : headers.Count - 2;
            RedrawCell(activeRow, previousActiveColumn, ConsoleColor.DarkGray);
            RedrawCell(activeRow, activeColumn, ConsoleColor.Red);
        }

        public void SetActiveRow(int index)
        {
            activeRow = index;
            activeRow = Math.Max(0, Math.Min(activeRow, rows.Count - 1));
        }

        private void UpdateActiveRow(int newRow)
        {
            RedrawCell(activeRow, activeColumn, ConsoleColor.DarkGray);
            activeRow = newRow;
            RedrawCell(activeRow, activeColumn, ConsoleColor.Red);
        }

        public void MoveActiveRowUp()
        {
            if (activeRow > 0) UpdateActiveRow(activeRow - 1);
        }

        public void MoveActiveRowDown()
        {
            if (activeRow < rows.Count - 1) UpdateActiveRow(activeRow + 1);
        }

        public void AdjustActiveRowAfterDeletion()
        {
            if (activeRow >= rows.Count) activeRow = Math.Max(0, rows.Count - 1);
        }

        public void UpdateDataSource(List<User> newUsers)
        {
            this.rows = ConvertUsersToRows(newUsers); 
        }


    }
}
