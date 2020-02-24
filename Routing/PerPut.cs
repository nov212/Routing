using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class PerPut
    {
        private readonly int amount;     //количество вершин
        private readonly int[] num;      //номера вершин
        private readonly int[] pos;      //позиция вершины
        private readonly int[] distance;    //уровень вершины i
        private int RGT;                //разделитель на правую и левую части массива num

        private bool ContainLeft(int num) { return (pos[num] < RGT); }
        public void Flip(int pos1, int pos2)
        {
            int temp = num[pos1];
            num[pos1] = num[pos2];
            num[pos2] = temp;
            pos[num[pos1]] = pos1;
            pos[num[pos2]] = pos2;
        }

        public PerPut(int amount)
        {
            RGT = 0;
            this.amount = amount;
            num = new int[amount];
            pos = new int[amount];
            distance = new int[amount];
            for (int i = 0; i < this.amount; i++)
            {
                num[i] = i;
                pos[i] = i;
                distance[i] = -1;
            }

        }
        public int GetN() { return amount; }                //возвращает число элементов в перестановке
        public int GetNum(int pos) { return num[pos]; }       //возвращает номер вершины в перестановке
        public int GetPos(int node) { return pos[node]; }   //позиция вершины в перестановке
        public int GetRgt() { return RGT; }                 //позиция рахделителя
        public void SetDistance(int node, int _dist) { distance[node] = _dist; }      //устанавливает уровень вершины
        public int GetDistance(int node) { return distance[node]; }                   //уровень вершины node

        public bool MoveLeft(int node)
        {
            if (!ContainLeft(node))
            {
                Flip(RGT, pos[node]);
                RGT++;
                return true;
            }
            return false;
        }
    }
}
