using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.Properties
{
    public class Solver : ISolver
    {
        public int[] GetPer(Graph g, int node)
        {
            int i = 0;
            int[] nodeLayers = new int[g.GetN()];
            for (int j = 0; j < g.GetN(); j++)
                nodeLayers[j] = -1;
           PerPut pp = new PerPut(g.GetN());
            pp.MoveLeft(node);
            nodeLayers[node] = 0;
            while (pp.GetRgt() < pp.GetN())
            {
                foreach (int n in g.GetAdj(node))
                {
                    if(pp.MoveLeft(n))
                        nodeLayers[n] = nodeLayers[node]+1;
                }
                i++;
                node = pp.GetNum(i);
            }
            for (int j = 0; j < pp.GetN(); j++)
                System.Console.Write("{0} ", pp.GetNum(j));
            System.Console.WriteLine();
            for (int j = 0; j < pp.GetN(); j++)
                System.Console.Write("{0} ", nodeLayers[pp.GetNum(j)]);
            return nodeLayers;
        }

        public void Route(Graph g, int start, int end)
        {
            int[] layerNode = GetPer(g, start);
            if (layerNode[end]==-1)
            {
                System.Console.WriteLine("No way");
                return;
            }
            int[] trace = new int[layerNode[end]];
            PrivateRoute(g, layerNode, trace, end);
        }

        private void PrivateRoute(Graph g, int[] ln, int[] trace, int end)
        {
            trace[ln[end]] = end;
            if (ln[end] == 0)
                return;
            IEnumerable<int> cand;
            
            foreach(int n in g.GetAdj(end))
                if (ln[n]==ln[end]-1)
        }

    }
}
