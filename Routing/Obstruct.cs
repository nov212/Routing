using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Obstruct: IGraph
    {
        private IGraph sourceGraph;
        private bool[] obs;

        public Obstruct(IGraph g)
        {
            this.sourceGraph = g;
            this.obs = new bool[g.GetN()];
        }

        public bool this[int node]
        {
            get { return obs[node]; }
            set { obs[node] = value; }     
        }

        public int GetN()
        {
            return sourceGraph.GetN();
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (var i in sourceGraph.GetAdj(node))
            {
                if (!obs[i])
                    yield return i;
            }
        }


        public void SetObstructZone(int row1, int col1, int row2, int col2, int layer)
        {
            for (int i = row1; i <= row2; i++)
                for (int j = col1; j <= col2; j++)
                    this[this.ToNum(i, j, layer)] = true;
        }

        public int Cols
        {
            get
            {
                return sourceGraph.Cols;
            }
        }

        public int Rows
        {
            get
            {
                return sourceGraph.Rows;
            }
        }

        public int GetRow(int node) { return sourceGraph.GetRow(node); }
        public int GetCol(int node) { return sourceGraph.GetCol(node); }

        public int ToNum(int row, int col, int layer)
        {
            return sourceGraph.ToNum(row, col, layer);
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            foreach (var n in sourceGraph.GetAdj(direction, node))
                if (!obs[n])
                    yield return n;
        }

        public int GetNodeLayer(int node)
        {
           return sourceGraph.GetNodeLayer(node);
        }

        public void Add(IGraph g)
        {
            throw new NotImplementedException();
        }

        public bool IsMultilayer()
        {
            return false;
        }

        public bool IsComposite()
        {
            return sourceGraph.IsComposite();
        }

        public bool IsObstacle(int node)
        {
            if (node < 0 || node >= GetN())
                throw new ArgumentOutOfRangeException();
            return obs[node];
        }
    }
}
