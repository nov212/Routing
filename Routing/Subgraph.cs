using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    class Subgraph: IGraph
    {
        private IGraph sourceGraph;
        private readonly bool[] vertical;
        private readonly bool[] horizontal;

        public Subgraph(IGraph graph, int[] pins)
        {
            sourceGraph = graph;
            int Cols=graph.GetCol(graph.GetN()-1)+1;
            int Rows=graph.GetRow(graph.GetN()-1)+1;
            vertical= new bool[Cols];
            horizontal = new bool[Rows];
            foreach (int pin in pins)
            {
                vertical[graph.GetCol(pin)] = true;
                horizontal[graph.GetRow(pin)] = true;
            }
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
