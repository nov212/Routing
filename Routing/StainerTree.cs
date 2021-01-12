using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{

    class StainerTree: IGraph 
    {
        private IGraph src;
        private bool[] edges;

        public int Cols => src.Cols;

        public int Rows => src.Rows;

        public StainerTree(IGraph src)
        {
            this.src = src;
            edges = new bool[src.GetN()];
        }

        public void AddVerticalEdge(int startRow, int endRow, int col, int layer)
        {
            for (int i = startRow; i <= endRow; i++)
                    edges[ToNum(i,col,layer)] = true;
        }

        public void AddHorisontalEdge(int startCol, int endCol, int row, int layer)
        {
            for (int i = startCol; i <= endCol; i++)
                edges[ToNum(row, i, layer)] = true;
        }

        public void DeleteVerticalEdge(int startRow, int endRow, int col, int layer)
        {
            for (int i = startRow; i <= endRow; i++)
                edges[ToNum(i,col, layer)] = false;
        }

        public void DeleteHorisontalEdge(int startCol, int endCol, int row, int layer)
        {
            for (int i = startCol; i <= endCol; i++)
                edges[ToNum(row,i, layer)] = false; 
        }
        public void Add(IGraph g)
        {
            return;
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (int n in src.GetAdj(node))
                if (edges[n])
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
            return 0;
        }

        public int GetRow(int node)
        {
            return src.GetRow(node);
        }

        public bool IsComposite()
        {
            return false;
        }

        public bool IsMultilayer()
        {
            return false;
        }

        public bool IsObstacle(int row, int col, int layer)
        {
            if (edges[ToNum(row, col, layer)])
                return src.IsObstacle(row, col, layer);
            return true;
        }

        public bool IsVia(int row, int col, int layer)
        {
            return src.IsVia(row, col, layer);
        }

        public void SetVia(int row, int col, int layer)
        {
            src.SetVia(row, col, layer);
        }

        public int ToNum(int row, int col, int layer)
        {
           return src.ToNum(row, col, layer);
        }

        public void SetPrefferedDirection(bool direction, int layer)
        {
            src.SetPrefferedDirection(direction, layer);
        }
    }
}
