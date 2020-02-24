using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.Properties
{
    interface ISolver
    {
       int[] GetPer(Graph g, int node);
    }
}
