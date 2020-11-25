using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Graph : IGraph
    {
        private readonly int ROWS;
        private readonly int COLS;

        public int ToNum(int row, int col)
        {
            int res = row * COLS + col;
            if (res >= 0 && res < GetN())
                return res;
            else return -1;
        }
        public Graph(int _n, int _m)
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
            if ((node + COLS) < ROWS * COLS) yield return node + COLS;
        }

        public int Rows
        {
            get
            {
                return ROWS;
            }
        }

        public int Cols
        {
            get
            {
                return COLS;
            }
        }


        public int GetRow(int node)
        {
            if (node < GetN())
                return node / COLS;
            else throw new IndexOutOfRangeException();
        }
        public int GetCol(int node)
        {
            if (node < GetN())
                return node % COLS;
            else throw new IndexOutOfRangeException();
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            foreach (int n in GetAdj(node))
            {
                if (direction && GetRow(n) == GetRow(node))
                    yield return n;
                if (!direction && GetCol(n) == GetCol(node))
                    yield return n;
            }
        }
    }
}
