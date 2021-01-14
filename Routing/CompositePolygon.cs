using System.Collections.Generic;

namespace Routing
{
  public abstract class CompositePolygon : IPolygon
    {
        protected List<IPolygon> polygons;
        public CompositePolygon()
        {
            polygons = new List<IPolygon>();
        }
        public IPolygon Add(IPolygon p)
        {
            polygons.Add(p);
            return this;
        }
        public abstract bool InRange(int row, int col);
    }
}
