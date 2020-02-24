using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.Properties
{
    class PerPut:IPerPut
    {
        private readonly int amount;     //количество вершин
        private readonly int[] num;      //номера вершин
        private readonly int[] pos;      //позиция вершины
        private int RGT;                //разделитель на правую и левую части массива num
        private bool ContainLeft(int num) { return (pos[num] >= RGT); }
        private void Flip(int pos1, int pos2)
        {
            int temp = num[pos1];
            num[pos1] = num[pos2];
            num[pos2] = temp;
            pos[num[pos1]] = pos1;
            pos[num[pos2]] = pos2;
        }

        public PerPut(int amount=4)
        {
            RGT = 0;
            this.amount = amount;
            num = new int[amount];
            pos = new int[amount];
            for (int i=0;i<this.amount;i++)
            {
                num[i] = i;
                pos[i] = i;
            }
        }

        public int GetN() { return amount; }
        public int GetNum(int pos) {return num[pos];}
        public int GetPos(int node) { return pos[node]; }
        public int GetRgt() { return RGT; }
       
        public bool MoveLeft(int node)
        {
            if (ContainLeft(node))
            {
                Flip(RGT, pos[node]);
                RGT++;
                return true;
            }
            return false;
        }

        //public void Test(Graph g, int node)
        //{
        //    Console.WriteLine("Число вершин: {0}", g.GetN());
        //    Console.WriteLine("Исходный узел: {0}", node);
        //    MoveLeft(node);
        //    int i = 0;
        //    while (RGT < amount)
        //    {
        //        Console.Write("Соседи {0}: ", node);
        //        foreach (int j in g.GetAdj(node))
        //            Console.Write("{0} ", j);
        //        Console.WriteLine();
        //        foreach (int n in g.GetAdj(node))
        //            MoveLeft(n);
        //        i++;
        //        node = num[i];
        //        Console.Write("num ");
        //        for (int j = 0; j < amount; j++)
        //        {
        //            if (j == RGT) Console.Write("| ");
        //            Console.Write($"{num[j]} ");
        //        }
        //        Console.WriteLine();
        //        Console.Write("pos ");
        //        for (int j = 0; j < amount; j++)
        //        {
        //            if (j == RGT) Console.Write("| ");
        //            Console.Write($"{pos[j]} ");
        //        }
        //        Console.WriteLine();
        //        Console.WriteLine("--------------------------------");
        //    }
        //    foreach (int j in num)
        //        Console.Write("{0} ", j);
        //}
    }
}
