using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    internal class DialogBox : Box
    {
        public DialogBox(int left, int top, int width, int height, ConsoleColor color)
            : base(left, top, width, height, color)
        {
            Draw();
        }

    }
}
