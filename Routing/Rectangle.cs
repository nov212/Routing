using System;
namespace Routing
{
   public class Rectangle : IPolygon
    {
        private int[] rect;
        public Rectangle(int startRow, int startCol, int endRow, int endCol)
        {
            if (startRow < 0 || startCol < 0 || endRow < 0 || endCol < 0)
                throw new ArgumentException("Negative Argument");
            rect =new int[]{ startRow, startCol, endRow, endCol};
        }
        public IPolygon Add(IPolygon p)
        {
            return this;
        }

        public bool InRange(int row, int col)
        {
            if (row >= rect[0] && row <= rect[2] && col >= rect[1] && col <= rect[3])
                return true;
            return false;
        }
    }
}
