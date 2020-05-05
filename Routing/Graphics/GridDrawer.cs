using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Routing
{
    class GridDrawer
    {
        private Scaler scaler;
        private Window view;
        private Drawer drawer;
        public GridDrawer(int Width, int Height, Drawer d)
        {
            drawer = d;
            view = new Window(new System.Drawing.Size(Width,Height), d);
            scaler = new Scaler(view);
        }

        public int Scale
        {
            get => scaler.Scale;
            set => scaler.Scale = value; 
        }

        public float PenWidth
        {
            set =>drawer.pen.Width = value; 
        }

        public void  DrawLine(Color color, int x1, int y1, int x2, int y2)
        {
            scaler.DrawLine(color, x1, y1, x2, y2);
        }

        public void DrawString(string text, System.Drawing.Font font, System.Drawing.SolidBrush drawBrush, int x, int y)
        {
            scaler.DrawString(text, font, drawBrush, x, y);
        }

        public void Move(int x, int y)
        {
            scaler.Move(x, y);
        }

        public Point Offset { get { return scaler.Offset; } }

        public void Clear(Color color)
        {
            drawer.Clear(color);
        }
    }
}
