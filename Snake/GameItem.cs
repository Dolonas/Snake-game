using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{

    enum Direction { Up, Right, Down, Left};
    
    internal class GameItem : PictureBox
    {
        private int xCoor;
        private int yCoor;

        internal static int SizeOfSquare { get; set; }
        internal static int GameItemSize { get; set; }
        internal static int XPanelShift { get; set; }
        internal static int YPanelShift { get; set; }
        internal static int XFieldShift { get; set; }
        internal static int YFieldShift { get; set; }
        internal static int LineShift { get; set; }
        internal static int XFieldSize { get; set; }
        internal static int YFieldSize { get; set; }
        internal int XCoor
        {
            get
            {
                return xCoor;
            }

            set
            {
                if (value < 0)
                    xCoor = 0;
                else if (value > XFieldSize)
                    xCoor = XFieldSize;
                else
                    xCoor = value;

            }
        }

        internal int YCoor
        {
            get
            {
                return yCoor;
            }

            set
            {
                if (value < 0)
                    yCoor = 0;
                else if (value > YFieldSize)
                    yCoor = YFieldSize;
                else
                    yCoor = value;

            }
        }
               

        internal Point LocationField
        {
            get
            {
                return new Point(XCoor, YCoor);
            }

            set
            {
                XCoor = value.X;
                YCoor = value.Y;
                
                this.Location = new Point (XCoor * SizeOfSquare + XPanelShift + XFieldShift + LineShift, YCoor * SizeOfSquare + YPanelShift + YFieldShift + LineShift);

            }

        }



        public GameItem()
        {
            SizeOfSquare = 40;
            GameItemSize = SizeOfSquare - 1;
            XPanelShift = 5;
            YPanelShift = 5;
            XFieldShift = 5;
            YFieldShift = 5;
            XFieldSize = 20;
            YFieldSize = 20;
            LineShift = 1;
            this.Size = new Size(GameItem.GameItemSize, GameItem.GameItemSize);

        }

        internal new void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    YCoor--;
                    break;
                case Direction.Right:
                    XCoor++;
                    break;
                case Direction.Down:
                    YCoor++;
                    break;
                case Direction.Left:
                    XCoor--;
                    break;
            }
        }
    }
}
