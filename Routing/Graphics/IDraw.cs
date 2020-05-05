using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Routing
{
    interface IDraw
    {
        void DrawLine(System.Drawing.Color color, int x1, int y1, int x2, int y2);
        void DrawString(string text, System.Drawing.Font font, System.Drawing.SolidBrush drawBrush, int x, int y);
    }
}
