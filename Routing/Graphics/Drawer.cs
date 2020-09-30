using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Routing
{
    class Drawer:IDraw
    {
        private Graphics gr;
        public Pen pen;
        public Drawer(Graphics g)
        {
            gr = g;
            pen = new Pen(System.Drawing.Color.Gray, 1);
        }
        public void DrawLine(System.Drawing.Pen pen, int x1, int y1, int x2, int y2)
        {
            gr.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawString(string text, System.Drawing.Font font, System.Drawing.SolidBrush drawBrush, int x, int y)
        {
            gr.DrawString(text, font, drawBrush, x, y);
        }

        public void Clear(Color color) { gr.Clear(color); }
    }
}
