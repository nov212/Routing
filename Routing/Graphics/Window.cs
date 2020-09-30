using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    class Window: IDraw
    {
        private System.Drawing.Size size;
        private IDraw drawer;
        public Window(System.Drawing.Size size, IDraw d)
        {
            drawer = d;
            this.size = size;
        }

        //public bool InRange(int x1, int y1, int x2, int y2)
        //{
        //    if ((x1 < 0 && x2>size.Width) || (y1 <0 && y2>size.Height))
        //        return false;
        //    return true;
        //}

        public bool InRange(int x, int y)
        {
            if (x>=0 && x<=size.Width && y>=0 && y<=size.Height)
                return true;
            return false;
        }

        public void DrawString(string text, System.Drawing.Font font, System.Drawing.SolidBrush drawBrush, int x, int y)
        {
            if (InRange(x,y))
                drawer.DrawString(text, font, drawBrush, x, y);
        }

        public void DrawLine(System.Drawing.Pen pen, int x1, int y1, int x2, int y2)
        {
            if (InRange(x1, y1) || InRange(x2, y2))
                drawer.DrawLine(pen, x1, y1, x2, y2);
        }

        public System.Drawing.Size Size
        {
            get => size;
            set =>size=value;
        }
    }
}
