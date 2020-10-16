﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Solver
    {
        private int[] inGroupTrace;                 //массив меток для каждого контакта 0-не трассирован, 1-принадлежит цепи 1
        private Dictionary<int, List<int>> fail;    //Key: номер цепи, Value: список цепей, которые не удалось соединить 

        public Solver(IGraph g)
        {
            inGroupTrace = new int[g.GetN()];
            for (int i = 0; i < inGroupTrace.Length; i++)
                inGroupTrace[i] = 0;
        }

        private int[] GetWave(IGraph obs, int src, int dest)
        {
            int currentNode = src;
            int[] wave = new int[obs.GetN()];
            for (int i = 0; i< wave.Length; i++)
                wave[i] = -1;
            Queue<int> pinsQueue=new Queue<int>();
            PerPut Graph_Config = new PerPut(obs.GetN());
            Graph_Config.MoveLeft(src);
            wave[src] = 0;
            int group = inGroupTrace[src];
            pinsQueue.Enqueue(src);
            while (pinsQueue.Count()!=0)
            {
                currentNode = pinsQueue.Dequeue();
                if (currentNode == dest)
                    break;
                foreach (int n in obs.GetAdj(currentNode))
                {
                    if ((inGroupTrace[n]==0 || inGroupTrace[n]==group) && Graph_Config.MoveLeft(n))
                    {
                        wave[n] = wave[currentNode] + 1;
                        pinsQueue.Enqueue(n);
                    }
                }
            }
            return wave;
        }

        private void InitCircuitGroups(IEnumerable<int[]> circuits)
        {
            int group = 0;
            for (int i = 0; i < inGroupTrace.Length; i++)
                inGroupTrace[i] = 0;
            fail = new Dictionary<int, List<int>>();
            foreach (var circ in circuits)
            {
                group++;
                fail.Add(group, null);
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
                sub.Add(circ);
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
                        wave = GetWave(sub, start, dest);
                        if (wave[dest] > -1)
                        {
                            PinToPinTrace = Route(sub, wave, start, dest);
                            break;
                        }
                    }

                    //если в результате всех расширений сетки найти путь не удалось,
                    //то применяем алгоритм на всей
                    if (wave[dest]==-1)
                    {
                        wave = GetWave(g, start, dest);
                        if (wave[dest] == -1)
                            FailReport(inGroupTrace[dest], dest);
                        else
                        PinToPinTrace = Route(g, wave, start, dest);
                    }
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
            int start = 0;
            int[] wave = null;
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
                    wave = GetWave(g, start, dest);
                    if (wave[dest] > -1)
                    {
                        PinToPinTrace = Route(g, wave, start, dest);
                        foreach (var cond in PinToPinTrace)
                            Circuit.Add(cond);
                    }
                    else
                        FailReport(inGroupTrace[dest], dest);
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
            Queue<int> vertQueue = new Queue<int>();
            Queue<int> horQueue = new Queue<int>();
            bool toggle = false;
            int currentNode = 0;
            int currentGroup = 0;
            int[] prev = new int[wave.Length];
            int[] cost = new int[wave.Length];
            if (src == dest)
            {
                trace.Add(src);
                return trace;
            }
            if (wave[dest] > -1)
            {
                Subgraph vert = new Subgraph(g);
                Subgraph hor = new Subgraph(g);
                for (int i = 0; vert.AddVertical(i) == true; i++) ;
                for (int i = 0; hor.AddHorizontal(i) == true; i++) ;
                currentGroup = inGroupTrace[dest];
                currentNode = dest;


                //инициализация массивов prev и cost
                for (int i = 0; i < prev.Length; i++)
                {
                    prev[i] = -1;
                    cost[i] = Int32.MaxValue;
                }

                cost[currentNode] = 0;
                foreach (int next in g.GetAdj(currentNode))
                    if (wave[next] == (wave[currentNode] - 1))
                    {
                        if (g.GetCol(next) == g.GetCol(currentNode))
                            vertQueue.Enqueue(next);
                        else
                            horQueue.Enqueue(next);
                        prev[next] = currentNode;
                        cost[next] = 0;
                    }
                if (horQueue.Count() > 0)
                {
                    toggle = true;
                    currentNode = horQueue.Peek();
                }
                else
                {
                    toggle = false;
                    currentNode = vertQueue.Peek();
                }

                while (inGroupTrace[currentNode] != currentGroup)
                {
                    if (toggle == true)
                    {
                        while (horQueue.Count() != 0)
                        {
                            currentNode = horQueue.Dequeue();
                            if (inGroupTrace[currentNode] == currentGroup)
                                break;
                            if (GetNext(hor, wave, currentNode).Count()>0)
                                foreach (int next in GetNext(hor, wave, currentNode))
                                {
                                    if (prev[next]==-1 || cost[next] > cost[currentNode])
                                    {
                                        prev[next] = currentNode;
                                        cost[next] = cost[currentNode];
                                        horQueue.Enqueue(next);
                                    }
                                }
                            else
                            {
                                cost[currentNode]++;
                                vertQueue.Enqueue(currentNode);
                            }
                        }
                        toggle = false;
                    }
                    else
                    {
                        while (vertQueue.Count() != 0)
                        {
                            currentNode = vertQueue.Dequeue();
                            if (inGroupTrace[currentNode] == currentGroup)
                                break;
                            if (GetNext(vert, wave, currentNode).Count() > 0)
                                foreach (int next in GetNext(vert, wave, currentNode))
                                {
                                    if (prev[next] == -1 || cost[next] > cost[currentNode])
                                    {
                                        prev[next] = currentNode;
                                        cost[next] = cost[currentNode];
                                        vertQueue.Enqueue(next);
                                    }
                                }
                            else
                            {
                                cost[currentNode]++;
                                horQueue.Enqueue(currentNode);
                            }
                        }
                        toggle = true;
                    }
                    
                }

                //построение трассы
                while (prev[currentNode] != -1)
                {
                    trace.Add(currentNode);
                    inGroupTrace[currentNode] = currentGroup;
                    inGroupTrace[prev[currentNode]] = currentGroup;
                    currentNode = prev[currentNode];
                }
                trace.Add(currentNode);
                vertQueue.Clear();
                horQueue.Clear();
            }
            return trace;
        }

        private void FailReport(int group, int pin)
        {
            if (fail[group] == null)
                fail[group] = new List<int>();
            fail[group].Add(pin);
        }

        public void ClearFailReport()
        {
            fail.Clear();
        }

        public Dictionary<int, List<int>> GetFailReport()
        {
            return fail;
        }

        private IEnumerable<int> GetNext(IGraph g, int[] wave, int node)
        {
            foreach (int n in g.GetAdj(node))
                if (wave[n] == wave[node] - 1)
                    yield return n;
        }
    }
}
