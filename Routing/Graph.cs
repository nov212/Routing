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
        private bool direction = true;
        private bool[] via;

        public int ToNum(int row, int col, int layer)
        {
            int res = row * COLS + col;
            if (res >= 0 && res < GetN())
                return res;
            else
                throw new ArgumentOutOfRangeException();
        }
        public Graph(int _n, int _m)
        {
            ROWS = _n;
            COLS = _m;
            via = new bool[ROWS * COLS];
        }
        public int GetN()
        {
            return ROWS * COLS;
        }
        public IEnumerable<int> GetAdj(int node)
        {
            if (direction)
            {
                if ((GetCol(node) - 1) >= 0) yield return node - 1;
                if ((GetCol(node) + 1) < COLS) yield return node + 1;
                if ((GetRow(node) - 1) >= 0) yield return node - COLS;
                if ((GetRow(node) + 1) < ROWS) yield return node + COLS;
            }
            else
            {
                if ((GetRow(node) - 1) >= 0) yield return node - COLS;
                if ((GetRow(node) + 1) < ROWS) yield return node + COLS;
                if ((GetCol(node) - 1) >= 0) yield return node - 1;
                if ((GetCol(node) + 1) < COLS) yield return node + 1;
            }
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
            else throw new ArgumentOutOfRangeException();
        }
        public int GetCol(int node)
        {
            if (node < GetN())
                return node % COLS;
            else throw new ArgumentOutOfRangeException();
        }

        public int GetNodeLayer(int node)
        {
            if (node < 0 || node >= GetN())
                throw new ArgumentOutOfRangeException();
            return 0;
        }

        public void Add(IGraph g){}

        public bool IsMultilayer()
        {
            return false;
        }

        public bool IsComposite()
        {
            return false;
        }

        public bool IsObstacle(int row, int col, int layer)
        {
            return false;
        }

        public bool IsVia(int row, int col, int layer)
        {
            return via[ToNum(row, col, layer)];
        }

        public void SetVia(int row, int col, int layer)
        {
            via[this.ToNum(row, col, layer)]=true;
        }

        public void SetPrefferedDirection(bool direction, int layer)
        {
            this.direction=direction;
        }
    }
}
