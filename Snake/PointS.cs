using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    internal class PointS
    {
        internal Point newPoint { get; set; }

        internal int sizeOfSquare { get; set; }
        internal int xPanelShift { get; set; }
        internal int yPanelShift { get; set; }
        internal int xFieldShift { get; set; }
        internal int yFieldShift { get; set; }
        internal int lineShift { get; set; }


        public PointS(int sqSize, int x1, int y1, int x2, int y2, int lShift1)
        {
            sizeOfSquare = sqSize;
            xPanelShift = x1;
            yPanelShift = y1;
            xFieldShift = x2;
            yFieldShift = y2;
            lineShift = lShift1;
            newPoint = new Point();
        }

        internal Point GetNewPoint(int x, int y)
        {
            
            
            return newPoint;
        }

    }
}
