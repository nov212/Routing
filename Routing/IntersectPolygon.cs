
namespace Routing
{
    public class IntersectPolygon :  CompositePolygon
    {
        override public bool InRange(int row, int col)
        {
            foreach (IPolygon p in polygons)
                if (!p.InRange(row, col))
                    return false;
            return true;
        }
    }
}

