using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.Properties
{
    public interface IGraph
    {
        int GetN();                             //число вершин
        IEnumerable<int> GetAdj(int node);      //соседи вершины
    }
}
