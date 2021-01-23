using System;
using System.Collections.Generic;

namespace Routing
{
    class PolygonGraph : IGraph
    {
        private IGraph src;
        private IPolygon zone;
        public PolygonGraph(IGraph g)
        {
            src = g;
        }
        public void RouteOn(IPolygon zone)
        {
            this.zone = zone;
        }
        public int Cols => src.Cols;

        public int Rows => src.Rows;

        public void Add(IGraph g)
        {
            src.Add(g);
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (int n in src.GetAdj(node))
                if (zone.InRange(src.GetRow(n), src.GetCol(n)))
                    yield return n;
        }

        public int GetCol(int node)
        {
            return src.GetCol(node);
        }

        public int GetN()
        {
            return src.GetN();
        }

        public int GetNodeLayer(int node)
        {
            return src.GetNodeLayer(node);
        }

        public int GetRow(int node)
        {
            return src.GetRow(node);
        }

        public bool IsComposite()
        {
            return src.IsComposite();
        }

        public bool IsMultilayer()
        {
            return src.IsMultilayer();
        }

        public bool IsObstacle(int row, int col, int layer)
        {
            return src.IsObstacle(row, col, layer) ;
        }

        public bool IsVia(int row, int col, int layer)
        {
            return src.IsVia(row, col, layer);
        }

        public void SetPrefferedDirection(bool direction, int layer)
        {
            throw new NotImplementedException();
        }

        public void SetVia(int row, int col, int layer)
        {
            src.SetVia(row, col, layer);
        }

        public int ToNum(int row, int col, int layer)
        {
            return src.ToNum(row, col, layer);
        }
    }
}
