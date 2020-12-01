using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Subgraph : IGraph
    {
        private IGraph sourceGraph;
        private readonly bool[] vertical;
        private readonly bool[] horizontal;

        public int Cols => sourceGraph.Cols;

        public int Rows => sourceGraph.Rows;

        public Subgraph(IGraph graph)
        {
            sourceGraph = graph;
            vertical = new bool[this.Cols];
            horizontal = new bool[this.Rows];
        }

        public void Add(int[] pins)
        {
            foreach (int pin in pins)
                Add(pin);
        }

        public void Add(int pin)
        {
            vertical[sourceGraph.GetCol(pin)] = true;
            horizontal[sourceGraph.GetRow(pin)] = true;
        }

        public bool AddVertical(int col)
        {
            if (col >= 0 && col < vertical.Length)
            {
                vertical[col] = true;
                return true;
            }
            return false;
        }
        public bool AddHorizontal(int row)
        {
            if (row >= 0 && row < horizontal.Length)
            {
                horizontal[row] = true;
                return true;
            }
            return false;
        }

        public void AddVerticalsAroundCol(int col, int left, int right)
        {
            for (int i = 1; i <= left; i++)
                if (AddVertical(col - i) == false)
                    break;
            for (int i = 1; i <= right; i++)
                if (AddVertical(col + i) == false)
                    break;
        }

        public void AddHorisontalsAroundRow(int row, int down, int up)
        {
            for (int i = 1; i <= down; i++)
                if (AddHorizontal(row + i) == false)
                    break;
            for (int i = 1; i <= up; i++)
                if (AddHorizontal(row - i) == false)
                    break;
        }

        public bool DisableHorizontal(int num)
        {
            if (num >= 0 && num < horizontal.Length)
            {
                horizontal[num] = false;
                return true;
            }
            return false;
        }

        public bool DisableVertical(int num)
        {
            if (num >= 0 && num < vertical.Length)
            {
                vertical[num] = false;
                return true;
            }
            return false;
        }

        public void DisableHorisontalAroundRow(int row, int down, int up)
        {
            for (int i = 1; i <= down; i++)
                if (DisableHorizontal(row + i) == false)
                    break;
            for (int i = 1; i <= up; i++)
                if (DisableHorizontal(row - i) == false)
                    break;
        }

        public void DisableVerticalAroundCol(int col, int left, int right)
        {
            for (int i = 1; i <= left; i++)
                if (DisableVertical(col - i) == false)
                    break;
            for (int i = 1; i <= right; i++)
                if (DisableVertical(col + i) == false)
                    break;
        }

        public int GetN()
        {
            return sourceGraph.GetN();
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (int adj in sourceGraph.GetAdj(node))
                if ((this.GetCol(adj) == this.GetCol(node) && vertical[this.GetCol(adj)] == true)
                    || (this.GetRow(adj) == this.GetRow(node) && horizontal[this.GetRow(adj)]==true))
                    yield return adj;
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            foreach (int adj in sourceGraph.GetAdj(direction, node))
                if ((this.GetCol(adj) == this.GetCol(node) && vertical[this.GetCol(adj)] == true)
                    || (this.GetRow(adj) == this.GetRow(node) && horizontal[this.GetRow(adj)] == true))
                    yield return adj;
        }

        public int GetRow(int node)
        {
            return sourceGraph.GetRow(node);
        }

        public int GetCol(int node)
        {
            return sourceGraph.GetCol(node);
        }

        public int ToNum(int row, int col, int layer)
        {
            return sourceGraph.ToNum(row, col, layer);
        }

        public int GetNodeLayer(int node)
        {
            return sourceGraph.GetNodeLayer(node);
        }

        public void Add(IGraph g)
        {
            sourceGraph.Add(g);
        }

        public bool IsMultilayer()
        {
            return sourceGraph.IsMultilayer();
        }

        public bool IsComposite()
        {
            return sourceGraph.IsComposite();
        }

        public bool IsObstacle(int row, int col, int layer)
        {
            if (vertical[col] || horizontal[row])
                return sourceGraph.IsObstacle(row, col, layer) ;
            return true;
        }

        public bool IsVia(int row, int col, int layer)
        {
            return sourceGraph.IsVia(row, col, layer);
        }

        public void SetVia(int row, int col, int layer)
        {
            sourceGraph.SetVia(row, col, layer);
        }
    }
}
