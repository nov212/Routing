using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class MultilayerGragh : IGraph
    {
        private List<IGraph> layers;    //слои графа
        private int factor;               //множитель, использующийся для нумерации узлов

        public int Cols => layers[0].Cols;

        public int Rows => layers[0].Rows;

        public MultilayerGragh()
        {
            layers = new List<IGraph>();
            factor = 0;
        }
        public IEnumerable<int> GetAdj(int node)
        {
            int actualLayer = GetNodeLayer(node);
            int actualNode = ActualNumeration(node);
            IGraph current = layers[actualLayer];
            //соседи на одном слое с node
            foreach (int n in current.GetAdj(actualNode))
                yield return factor*actualLayer+n;
            //сосед сверху node
            if (actualLayer+1<layers.Count())
                //проверка возможности перехода наверх:
                //если в данном узле на текущей сетке есть переход
                //и есть переход на сетке сверху\снизу и нет препятствий,
                //то можно сделать переход
                if (current.IsVia(GetRow(actualNode), GetCol(actualNode), 0)&& 
                    layers[actualLayer + 1].IsVia(GetRow(actualNode), GetCol(actualNode), 0))
                        if (!IsObstacle(GetRow(actualNode), GetCol(actualNode), actualLayer+1))
                        yield return factor * (actualLayer + 1) + actualNode;
            //сосед снизу node
            if (actualLayer - 1 >=0)
                if (current.IsVia(GetRow(actualNode), GetCol(actualNode), 0) &&
                     layers[actualLayer - 1].IsVia(GetRow(actualNode), GetCol(actualNode), 0))
                        if (!IsObstacle(GetRow(actualNode), GetCol(actualNode), actualLayer - 1))
                        yield return factor * (actualLayer - 1) + actualNode;
        }

        public int GetN()
        {
            return layers.Sum(g=>g.GetN());
        }

        public int GetCol(int node)
        {
            if (node>=0 && node<GetN())
                return layers[GetNodeLayer(node)].GetCol(ActualNumeration(node));
            else return -1;
        }

        public int GetRow(int node)
        {
            if (node >= 0 && node < GetN())
                return layers[GetNodeLayer(node)].GetRow(ActualNumeration(node));
            else return -1;
        }

        public int ToNum(int row, int col, int layer)
        {
            return layers[layer].ToNum(row, col, 0)+layer*factor;
        }

        public void SetVia(int row, int col, int layer)
        {
            layers[layer].SetVia(row, col, 0);
        }

        public bool IsVia(int row, int col, int layer)
        {
            return layers[layer].IsVia(row, col, 0);
        }


        private int ActualNumeration(int n)
        {
            return n % factor;
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            throw new NotImplementedException();
        }


        public int GetNodeLayer(int node)
        {
           return node/factor;
        }

        public void Add(IGraph g)
        {
            layers.Add(g);
            factor = g.GetN();
        }

        public bool IsMultilayer()
        {
           return true;
        }

        public bool IsComposite()
        {
           return true;
        }

        public bool IsObstacle(int row, int col, int layer)
        {
            return layers[layer].IsObstacle(row, col, 0);
        }
    }
}