using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    internal class MyButton: Button
    {
        public MyButton()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
}
