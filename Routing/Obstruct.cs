using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Obstruct: IGraph
    {
        private Graph sourceGraph;
        private bool[] obs;

        public Obstruct(Graph g)
        {
            this.sourceGraph = g;
            this.obs = new bool[g.GetN()];
        }

        public bool this[int node]
        {
            get { return obs[node]; }
            set { obs[node] = value; }     
        }

        public int GetN()
        {
            return sourceGraph.GetN();
        }

        public IEnumerable<int> GetAdj(int node)
        {
            foreach (var i in sourceGraph.GetAdj(node))
            {
                if (!obs[i])
                    yield return i;
            }
        }


        public void SetObstructZone(int upLeft, int downRight)
        {
            int cols = sourceGraph.Cols;
            int firstX = GetRow(upLeft);
            int firstY= GetCol(upLeft);
            int secondX = GetRow(downRight);
            int secondY = GetCol(downRight);
            try
            {
                for (int i = firstX; i <= secondX; i++)
                    for (int j = firstY; j <= secondY; j++)
                        obs[this.sourceGraph.ToNum(i,j)] = true;
            }
            catch (IndexOutOfRangeException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        public int Cols
        {
            get
            {
                return sourceGraph.Cols;
            }
        }

        public int Rows
        {
            get
            {
                return sourceGraph.Rows;
            }
        }

        public int GetRow(int node) { return sourceGraph.GetRow(node); }
        public int GetCol(int node) { return sourceGraph.GetCol(node); }

        public int ToNum(int row, int col)
        {
            return sourceGraph.ToNum(row, col);
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            foreach (var n in sourceGraph.GetAdj(direction, node))
                if (!obs[n])
                    yield return n;
        }
    }
}
