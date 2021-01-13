using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    internal class GameItem : Control
    {
        internal PictureBox pBox;


        internal static int SizeOfSquare { get; set; }
        internal static int XPanelShift { get; set; }
        internal static int YPanelShift { get; set; }
        internal static int XFieldShift { get; set; }
        internal static int YFieldShift { get; set; }
        internal static int LineShift { get; set; }


        internal Point point;
        internal int XCoor { get; set; }
        internal int YCoor { get; set; }

        internal new Point Location
        {
            get
            {
                point.X = XCoor * SizeOfSquare + XPanelShift + XFieldShift + LineShift;
                point.Y = YCoor * SizeOfSquare + YPanelShift + YFieldShift + LineShift;
                return point; 
            }
            set
            {

            }
        }
        public GameItem()
        {
            point = new Point();

        }

    }
}
