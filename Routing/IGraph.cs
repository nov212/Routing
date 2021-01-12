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
        int ToNum(int row, int col, int layer);            //возвращает номер узла по координатам rows, cols
        int GetNodeLayer(int node);             //возвращает номер слоя, в котором расположен узел node
        int Cols { get; }                       //кол-во столбцов
        int Rows { get; }                       //кол-во строк
        void Add(IGraph g);                     
        bool IsMultilayer();
        bool IsComposite();
        bool IsObstacle(int row, int col, int layer);
        bool IsVia(int row, int col, int layer);
        void SetVia(int row, int col, int layer);
        //устанавливает предпочтительное направление
        //true-по-горизонтали (по умолчанию)
        //false-по-вертикали
        void SetPrefferedDirection(bool direction, int layer);
                                
    }
}
