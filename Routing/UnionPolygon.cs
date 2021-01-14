
namespace Routing
{
   public class UnionPolygon : CompositePolygon
    {
        override public bool InRange(int row, int col)
        {
            foreach (IPolygon p in polygons)
                if (p.InRange(row, col))
                    return true;
            return false;
        }
    }
}
