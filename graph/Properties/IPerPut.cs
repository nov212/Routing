using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.Properties
{
    interface IPerPut
    {
        int GetN();
        int GetNum(int pos);    //вершина в позиции pos
        int GetPos(int num);    //позиция вершины num
        bool MoveLeft(int num); //сдвиг вершины влево
    }
}
