using System;
using System.Collections.Generic;
using System.Drawing;
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
            this.FlatStyle = FlatStyle.System;
            this.AutoSize = false;
            this.Size = new Size(120, 40);
            this.ForeColor = Color.White;
            this.BackColor = Color.FromArgb(50, 50, 50);
            this.Font = new Font(this.Font.FontFamily, 12, this.Font.Style);
        }

        
           
    }
}
