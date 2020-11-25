using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public interface IGraph
    {
        int GetN();                             //число вершин
        IEnumerable<int> GetAdj(int node);      //соседи вершины
        int GetRow(int node);                   //возвращает строку, в которой находится узел node
        int GetCol(int node);                   //возвращает столбец, в которой находится узел node
        int ToNum(int row, int col);
        IEnumerable<int> GetAdj(bool direction, int node); //true- по горизонтали false по вертикали
    }
}
