using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Solver
    {
        const int DISABLE = -1;
        const int NONE = 0;
        private int[] inGroupTrace;                 //массив меток для каждого контакта 0-не трассирован, 1-принадлежит цепи 1
        private Reporter failReporter;
        public Solver(IGraph g)
        {
            inGroupTrace = new int[g.GetN()];
            for (int i = 0; i < inGroupTrace.Length; i++)
                inGroupTrace[i] = NONE;
            failReporter = new Reporter();          
        }

        private void ResetWave(int[] wave)
        {
            for (int i = 0; i < wave.Length; i++)
                wave[i] = DISABLE;
        }

        private void GetWave(IGraph obs, int[] wave,int src, int dest)
        {
            int currentNode = src;
            ResetWave(wave);
            Queue<int> pinsQueue=new Queue<int>();
            PerPut Graph_Config = new PerPut(obs.GetN());
            Graph_Config.MoveLeft(src);
            wave[src] = 0;
            int group = inGroupTrace[src]; //номер рассматриваемой группы
            pinsQueue.Enqueue(src);
            while (pinsQueue.Count()!=0)
            {
                currentNode = pinsQueue.Dequeue();
                if (currentNode == dest)
                    break;
                foreach (int n in obs.GetAdj(currentNode))
                {

                    //если узел ещё не помечен или помечен номером текущей группы узлов и в обоих случаях узел ещё не рассматривался
                    if ((inGroupTrace[n]==NONE || inGroupTrace[n]==group) && Graph_Config.MoveLeft(n))
                    {
                        wave[n] = wave[currentNode] + 1;
                        pinsQueue.Enqueue(n);
                    }
                }
            }
        }

        private void InitCircuitGroups(IEnumerable<int[]> circuits)
        {
            int group = 0;
            for (int i = 0; i < inGroupTrace.Length; i++)
                inGroupTrace[i] = NONE;
            foreach (var circ in circuits)
            {
                group++;
                foreach (var pin in circ)
                    inGroupTrace[pin] = group;
            }
        }

        //волновой алгоритм для фрагмента сетки
        public List<List<int>> FindPathOnSubgraph(IGraph g, IEnumerable<int[]> circuits, int[] radius)
        {
            List<List<int>> solution = new List<List<int>>();  //построенные цепи
            List<int> PinToPinTrace = null; //соединение двух контактов
            List<int> Circuit = null;  //соединение для всей цепи
            int[] wave = null;
            int start = 0;
            int currentGroup = 0;
            int currentNode = 0;
            Subgraph sub = new Subgraph(g);

            //пометим контакты номером цепи
            InitCircuitGroups(circuits);

            foreach (var circ in circuits)
            {
                start = circ[0];
                currentGroup = inGroupTrace[start];
                currentNode = start;
                sub.AddPin(circ);
                Circuit = new List<int>();

                foreach (int dest in circ)
                {
                    if (dest == start)
                        continue;
                    foreach (int rad in radius)
                    {
                        sub.AddVerticalsAroundCol(sub.GetCol(start), rad, rad);
                        sub.AddHorisontalsAroundRow(sub.GetRow(start), rad, rad);
                        sub.AddVerticalsAroundCol(sub.GetCol(dest), rad, rad);
                        sub.AddHorisontalsAroundRow(sub.GetRow(dest), rad, rad);
                        GetWave(sub, wave,start, dest);
                        if (wave[dest] > -1)
                        {
                            PinToPinTrace = Route(sub, wave, start, dest);
                            break;
                        }
                    }

                    //если в результате всех расширений сетки найти путь не удалось,
                    //то применяем алгоритм на всей
                    GetWave(g, wave, start, dest);
                    PinToPinTrace = Route(g, wave, start, dest);                    
                    foreach (var pin in PinToPinTrace)
                        Circuit.Add(pin);
                }
 
                if (Circuit.Count() > 0)
                    solution.Add(Circuit);
            }
            return solution;
        }

        //волновой алгоритм 
        public List<List<int>> FindTrace(IGraph g, IEnumerable<int[]> circuits)
        {
            List<List<int>> solution = new List<List<int>>();  //построенные цепи
            List<int> PinToPinTrace = null; //соединение двух контактов
            List<int> Circuit = null;  //соединение для всей цепи

            failReporter.Clear();
            int start = 0;
            int[] wave=new int[g.GetN()];

            //пометим контакты номером цепи
            InitCircuitGroups(circuits);

            foreach (var trace in circuits)
            {
                Circuit = new List<int>();
                PinToPinTrace = new List<int>();      //построенная трасса для цепи
                start = trace[0];
                int currentGroup = inGroupTrace[start];     //номер обрабатываемой цепи
                foreach (int dest in trace)
                {
                    if (dest == start)
                        continue;

                   GetWave(g, wave,start, dest);    //пуск волны
                    PinToPinTrace = Route(g, wave, start, dest); //путь между двумя точками
                    if (PinToPinTrace.Count>0)
                        foreach (int node in PinToPinTrace)
                            Circuit.Add(node);                   
                }
                if (Circuit.Count() > 0)
                    solution.Add(Circuit);
            }
            return solution;
        }

        //возвращает путь между точками src и dest
        //private List<int> Route (IGraph g, int[] wave, int src, int dest)
        //{
        //    List<int> trace = new List<int>(); //соединение контактов src и dest
        //    Queue<int> pinsQueue = new Queue<int>();
        //    int currentNode = 0;
        //    int currentGroup = 0;
        //    int[] prev = new int[wave.Length];
        //    int[] cost = new int[wave.Length];
        //    int price = 0;
        //    if (src == dest)
        //    {
        //        trace.Add(src);
        //        return trace;
        //    }
        //    if (wave[dest] > -1)
        //    {
        //        /*
        //           * currentNode-обрабатываемый в текущий момент узел
        //           * prev[i]-предшественник узла i
        //           * cost[i]-стоимость пути до узла i
        //           * price-промежуточное значение, показывает стоимость пути до узла
        //          */
        //        currentGroup = inGroupTrace[dest];
        //        currentNode = dest;


        //        //инициализация массивов prev и cost
        //        for (int i = 0; i < prev.Length; i++)
        //        {
        //            prev[i] = -1;
        //            cost[i] = Int32.MaxValue;
        //        }

        //        cost[currentNode] = 0;
        //        foreach (int next in g.GetAdj(currentNode))
        //            if (wave[next] == (wave[currentNode] - 1))
        //            {
        //                pinsQueue.Enqueue(next);
        //                prev[next] = currentNode;
        //                cost[next] = 1;
        //            }

        //        while (pinsQueue.Count != 0)
        //        {
        //            currentNode = pinsQueue.Dequeue();

        //            //условие, при котором контакт соединён с цепью
        //            if (inGroupTrace[currentNode] == currentGroup)
        //                break;

        //            foreach (int next in g.GetAdj(currentNode))
        //            {
        //                if (wave[next] == (wave[currentNode] - 1))
        //                {
        //                    if (prev[next] == -1)
        //                    {
        //                        pinsQueue.Enqueue(next);
        //                        prev[next] = currentNode;
        //                    }
        //                    if (g.GetRow(prev[currentNode]) == g.GetRow(next) ||
        //                        g.GetCol(prev[currentNode]) == g.GetCol(next))
        //                        price = cost[currentNode] + 1;
        //                    else
        //                        price = cost[currentNode] + 2;
        //                    if (price < cost[next])
        //                    {
        //                        cost[next] = price;
        //                        prev[next] = currentNode;
        //                    }
        //                }
        //            }
        //        }

        //        //построение трассы
        //        while (prev[currentNode] != -1)
        //        {
        //            trace.Add(currentNode);
        //            inGroupTrace[currentNode] = currentGroup;
        //            inGroupTrace[prev[currentNode]] = currentGroup;
        //            currentNode = prev[currentNode];
        //        }
        //        trace.Add(currentNode);
        //        pinsQueue.Clear();
        //    }
        //    return trace;
        //}

        private List<int> Route(IGraph g, int[] wave, int src, int dest)
        {
            List<int> trace = new List<int>(); //соединение контактов src и dest

            //возвращаем пустой список, если пути нет и добавляем узел в список нетрассированных
            if (wave[dest] == DISABLE)
            {
                failReporter.AddNode(inGroupTrace[dest], dest);
                return trace;
            }

            //возвращает путь со стартовым узлом, если начальный и конечный узлы совпадают 
            if (src == dest)
            {
                trace.Add(src);
                return trace;
            }

            Queue<int> pinsQueue = new Queue<int>();
            int price;
            int currentNode = 0;
            int currentGroup = 0;
            int[] prev = new int[g.GetN()];
            int[] cost = new int[g.GetN()];


            //инициализация массивов prev и cost
            for (int i=0;i<g.GetN();i++)
            {
                prev[i] = DISABLE;
                cost[i] = Int32.MaxValue;
            }
                currentGroup = inGroupTrace[dest];
                currentNode = dest;
                cost[currentNode]=0;

            //заполнение очереди соседями стартового узла
            foreach (int n in g.GetAdj(currentNode))
            {
                if (wave[n] == wave[currentNode] - 1)
                {
                    prev[n] = currentNode;
                    cost[n] = 1;
                    pinsQueue.Enqueue(n);
                }
            }

            while (pinsQueue.Count() > 0)
            {
                currentNode = pinsQueue.Dequeue();
                //условие выхода, если текущая вершина принадлежит текущей группе цепи
                if (inGroupTrace[currentNode] == currentGroup && currentNode != dest)
                    break;

                //проверка, есть ли узлы, в которые можно перейти в заданном напавлении
                foreach (int next in g.GetAdj(currentNode))
                {
                    if (wave[next] == wave[currentNode] - 1)
                    {
                        price = GetPrice(g, prev, currentNode, next);
                        if (prev[next] == DISABLE || cost[next] > cost[currentNode] + price)
                        {
                            cost[next] = cost[currentNode] + price;
                            prev[next] = currentNode;
                            pinsQueue.Enqueue(next);
                        }
                    }
                }
            }

                //построение трассы
                while (prev[currentNode] != DISABLE)
                {
                    trace.Add(currentNode);
                    inGroupTrace[currentNode] = currentGroup;
                    currentNode = prev[currentNode];
                }
                inGroupTrace[currentNode] = currentGroup;
                trace.Add(currentNode);
                pinsQueue.Clear();
            return trace;
        }   

        private int GetPrice(IGraph g, int[] prev, int currentNode, int nextNode)
        {
            int price;
            if (g.GetNodeLayer(nextNode) != g.GetNodeLayer(currentNode))
                price = 10;
            else
            if (g.GetCol(nextNode) != g.GetCol(prev[currentNode]) && g.GetRow(nextNode) != g.GetRow(prev[currentNode]))
                price = 5;
            else
                price = 1;
            return price;
        }

        public Reporter GetFailReport()
        {
            return failReporter;
        }
    }
}
