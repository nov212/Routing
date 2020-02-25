using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    class Subgraph: IGraph
    {
        IGraph sourceGraph;
        bool[] track;           
        //track предназначен для обозначения узлов, входящих в подграф
        //track[n] принимает значение true, если узел n принадлежит подграфу

        public Subgraph(IGraph graph, int[] pins)
        {
            sourceGraph = graph;
            track = new bool[graph.GetN()];
            int Cols=GetCol(graph.GetN()-1)+1;
            int Rows=GetRow(graph.GetN()-1)+1;
            foreach (int pin in pins)
            {
                track[pin] = true;
                for (int i = GetCol(pin); i < GetN(); i += Cols)
                    track[i] = true;
                for (int i = 0; i < Cols; i++)
                    track[GetRow(pin) * Cols + i] = true;
            }
        }
        public int GetN()
        {
            return sourceGraph.GetN();
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (int adj in sourceGraph.GetAdj(node))
                if (track[adj] == true)
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
