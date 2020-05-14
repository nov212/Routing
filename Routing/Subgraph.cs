using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Subgraph: IGraph
    {
        private IGraph sourceGraph;
        private readonly bool[] vertical;
        private readonly bool[] horizontal;

        public Subgraph(IGraph graph)
        {
            sourceGraph = graph;
            int Cols=graph.GetCol(graph.GetN()-1)+1;
            int Rows=graph.GetRow(graph.GetN()-1)+1;
            vertical= new bool[Cols];
            horizontal = new bool[Rows];
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
            if (col < 0 || col >= vertical.Length)
                return false;
            vertical[col] = true;
            return true;
        }
        public bool AddHorizontal(int row)
        {
            if (row < 0 || row >= horizontal.Length)
                return false;
            horizontal[row] = true;
            return true;
        }

        public void Extend(int pin, int radius)
        {
            if (pin < 0 || pin >= GetN())
                return;
                int col = GetCol(pin);
                int row = GetRow(pin);
                for (int i = 0; i <= radius; i++)
                {
                    AddVertical(col + i);
                    AddVertical(col - i);
                    AddHorizontal(row + i);
                    AddHorizontal(row - i);
                }
            
        }
        public void Extend(IEnumerable<int> pins, int radius)
        {
            foreach (int pin in pins)
                Extend(pin, radius);
        }

        public int GetN()
        {
            return sourceGraph.GetN();
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (int adj in sourceGraph.GetAdj(node))
                if (vertical[this.GetCol(adj)] == true || horizontal[this.GetRow(adj)]==true)
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
    }
}
