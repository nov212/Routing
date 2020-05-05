using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Routing
{
    class Scaler: IDraw
    {
        private Point offset;
        private IDraw drawer;
        private int scale;
        public Scaler(IDraw d)
        {
            drawer = d;
            scale = 10;
            offset = new Point(0, 0);
        }
        public void Move(int x, int y)
        {
            offset.X = x;
            offset.Y = y;
        }

        public Point Offset {get =>offset;}

        public void DrawLine(Color color, int x1, int y1, int x2, int y2)
        {
            drawer.DrawLine(color, (x1) * scale + offset.X, (y1) * scale + offset.Y, (x2) * scale + offset.X, (y2) * scale + offset.Y);
        }

       public void DrawString(string text, System.Drawing.Font font, System.Drawing.SolidBrush drawBrush, int x, int y)
        {
            drawer.DrawString(text, font, drawBrush, (x) * scale + offset.X, (y) * scale + offset.Y);
        }

        public int Scale
        {
            get =>scale;
            set
            {
                if (value > 70)
                    value = 70;
                if (value < 10)
                    value = 10;
                else
                scale = value;
            }
        }
    }
}
