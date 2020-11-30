using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
   public class HGraphComposite : IGraph
    {
        private List<IGraph> graphs;
        public HGraphComposite(IGraph g)
        {
            graphs = new List<IGraph> { g };
        }
        public HGraphComposite(IEnumerable<IGraph> g)
        {
            graphs = new List<IGraph>();
            foreach (var graph in g)
                graphs.Add(graph);
        }

        public int Cols => graphs.Sum(g=>g.Cols);

        public int Rows => graphs.Max(g=>g.Rows);

        public void Add(IGraph g)
        {
            graphs.Add(g);
        }

        public IEnumerable<int> GetAdj(int node)
        {
            int COLS = this.Cols;
            int ROWS = this.Rows;
            if ((GetCol(node) - 1) >= 0 && !IsObstacle(node-1) ) yield return node - 1;
            if ((GetCol(node) + 1) < COLS && !IsObstacle(node+1)) yield return node + 1;
            if ((GetRow(node)-1) >= 0 && !IsObstacle(node-COLS)) yield return node - COLS;
            if ((GetRow(node)+1) < Rows && !IsObstacle(node+COLS)) yield return node + COLS;
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            throw new NotImplementedException();
        }

        public int GetCol(int node)
        {
            if (node < 0 || node >= GetN())
                throw new ArgumentOutOfRangeException();
            return node % this.Cols;
        }

        public int GetN()
        {
            return Rows*Cols;
        }

        public int GetNodeLayer(int node)
        {
           return 0;
        }

        public int GetRow(int node)
        {
            if (node < 0 || node >= GetN())
                throw new ArgumentOutOfRangeException();
            return node / this.Cols;
        }

        public bool IsComposite()
        {
           return true;
        }

        public bool IsMultilayer()
        {
            return false;
        }

        public bool IsObstacle(int node)
        {
            if (node < 0 || node >= GetN())
                throw new ArgumentOutOfRangeException();
            int col = GetCol(node);

            //определяем в каком графе находится node
            foreach(var g in graphs)
            {
                //условие принадлежности 
                if (col < g.Cols)
                {
                    if (GetRow(node) >= g.Rows)
                        return true;
                    return g.IsObstacle(g.ToNum(GetRow(node), col));
                }
                else
                    col -= g.Cols;
            }
            return true;
        }

        public int ToNum(int row, int col)
        {
            return row*Cols+col;
        }
    }
}
