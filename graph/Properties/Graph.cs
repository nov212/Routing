using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.Properties
{
    public class Graph: IGraph
    {
        private readonly int ROWS;
        private readonly int COLS;
        public Graph(int _n = 2, int _m = 2)
        {
            ROWS = _n;
            COLS = _m;
        }
        public int GetN()
        {
            return ROWS * COLS;
        }
        public IEnumerable<int> GetAdj(int node)
        {
            if ((node % COLS - 1) >= 0) yield return node - 1;
            if ((node % COLS + 1) < COLS) yield return node + 1;
            if ((node - COLS) >= 0) yield return node - COLS;
            if ((node + COLS) < ROWS*COLS) yield return node + COLS;
        }
    }
}
