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
        public HGraphComposite()
        {
            graphs = new List<IGraph> ();
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
            if ((GetCol(node) - 1) >= 0 && !IsObstacle(GetRow(node),GetCol(node)-1, 0 )) yield return node - 1;
            if ((GetCol(node) + 1) < COLS && !IsObstacle(GetRow(node), GetCol(node)+1, 0)) yield return node + 1;
            if ((GetRow(node)-1) >= 0 && !IsObstacle(GetRow(node)-1, GetCol(node), 0)) yield return node - COLS;
            if ((GetRow(node)+1) < Rows && !IsObstacle(GetRow(node) + 1, GetCol(node), 0)) yield return node + COLS;
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

        public bool IsObstacle(int row, int col, int layer)
        {

            //определяем в каком графе находится node
            foreach(var g in graphs)
            {
                //условие принадлежности 
                if (col < g.Cols)
                {
                    if (row >= g.Rows)
                        return true;
                    return g.IsObstacle(row, col, 0);
                }
                else
                    col -= g.Cols;
            }
            return true;
        }

        public bool IsVia(int row, int col, int layer)
        {
            foreach (var g in graphs)
            {
                //условие принадлежности 
                if (col < g.Cols)
                    return g.IsVia(row, col, 0);
                    col -= g.Cols;
            }
            return true;
        }

        public void SetVia(int row, int col, int layer)
        {
            foreach (var g in graphs)
            {
                //условие принадлежности 
                if (col < g.Cols)
                {
                    g.SetVia(row, col, 0);
                    return;
                }
                col -= g.Cols;
            }
        }

        public int ToNum(int row, int col, int layer)
        {
            return row*Cols+col;
        }
    }
}
