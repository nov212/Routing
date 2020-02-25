using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Solver
    {
        public PerPut Graph_Config;
        private int[] inGroupTrace;
        private static int groupNum = 0;
        private List<List<Conductor>> CondTrace;

        public Solver(IGraph g)
        {
            inGroupTrace = new int[g.GetN()];
            CondTrace = new List<List<Conductor>>();
            for (int i = 0; i < inGroupTrace.Length; i++)
                inGroupTrace[i] = 0;
        }
        private void GetPer(IGraph obs, int node)
        {
            Queue<int> pinsQueue=new Queue<int>();
            Graph_Config = new PerPut(obs.GetN());
            Graph_Config.MoveLeft(node);
            Graph_Config.SetDistance(node, 0);
            pinsQueue.Enqueue(node);
            while (pinsQueue.Count()!=0)
            {
                node = pinsQueue.Dequeue();
                foreach (int n in obs.GetAdj(node))
                {
                    if (inGroupTrace[n]==0 && Graph_Config.MoveLeft(n))
                    {
                        Graph_Config.SetDistance(n, Graph_Config.GetDistance(node) + 1);
                        pinsQueue.Enqueue(n);
                    }
                }
            }
        }

        //private IEnumerable<int> FreeAdj(IGraph obs,int node)
        //{
        //    foreach (int n in obs.GetAdj(node))
        //        if (inGroupTrace[n] == 0)
        //            yield return n;
        //}


        public void PinConnect(IGraph g,int[] pins)
        {
            //Список проводников для соединения группы пинов
            List<Conductor> PinToPinTrace = new List<Conductor>();
            //Subgraph subgraph = new Subgraph(g, pins);
                groupNum++;

            //распространение волны от узла start
                int start = pins[0];
                GetPer(g, start);
                    /*
                     * currentNode-обрабатываемый в текущий момент узел
                     * prev[i]-предшественник узла i
                     * cost[i]-стоимость пути до узла i
                     * price-промежуточное значение, показывает стоимость пути до узла
                    */
            int currentNode;
            int price;
            Queue<int> pinsQueue = new Queue<int>();
            int[] prev = new int[g.GetN()];
            int[] cost = new int[g.GetN()];
            for (int k = 1; k < pins.Length; k++)
            {
                if (Graph_Config.GetDistance(pins[k]) > 0)
                {
                    currentNode = pins[k];
                    pinsQueue.Enqueue(currentNode);

                    //инициализация массивов prev и cost
                    for (int i = 0; i < prev.Length; i++)
                    {
                        prev[i] = -1;
                        cost[i] = -1;
                    }

                    cost[currentNode] = 0;
                    while (pinsQueue.Count!=0)
                    {
                        currentNode = pinsQueue.Dequeue();
                        if (inGroupTrace[currentNode] == groupNum || currentNode == start)
                            break;
                        foreach (int next in NextNode(currentNode, g))
                        {
                            if (prev[next] == -1)
                            {
                                pinsQueue.Enqueue(next);
                                prev[next] = currentNode;
                            }
                            if (Math.Abs(prev[currentNode] - currentNode) == Math.Abs(next - currentNode))
                                price=cost[currentNode]+1;
                            else
                                price=cost[currentNode]+2;
                            if (price<cost[next] || cost[next]==-1)
                            {
                                cost[next] = price;
                                prev[next] = currentNode;
                            }
                        }
                    }

                    //построение трассы
                    while (prev[currentNode] != -1)
                    {
                        PinToPinTrace.Add(new Conductor(currentNode, prev[currentNode]));
                        inGroupTrace[currentNode] = groupNum;
                        inGroupTrace[prev[currentNode]] = groupNum;
                        currentNode = prev[currentNode];
                    }
                    CondTrace.Add(PinToPinTrace);
                    pinsQueue.Clear();
                }
            }
        }

        public void Heuristic(IGraph grid, int[] pins)
        {
            groupNum++;
            int end = pins[0];
            PriorityQueue queue = new PriorityQueue();
            int[] prev = new int[grid.GetN()];
            int currentNode;
            int priority;
            int start;
           
            for (int i = 1; i < pins.Length; i++)
            {
                start = pins[i];
                currentNode = start;
                for (int j = 0; j < grid.GetN(); j++) { prev[j] = -1; }
                prev[start] = start;
                queue.Enqueue(GetDistance(grid, start, end), currentNode);
                while (queue.Count != 0)
                {
                    currentNode = queue.Deque();
                    if (currentNode == end || inGroupTrace[currentNode]==groupNum)
                        break;
                    foreach (int next in grid.GetAdj(currentNode))
                    {
                        if (inGroupTrace[next] == groupNum)
                            priority = 0;
                        else
                        priority = GetDistance(grid, end, next);
                        if (prev[next] == -1)
                        {
                            prev[next] = currentNode;
                            queue.Enqueue(priority, next);
                        }
                    }
                }

                //построение трассы
                if (currentNode == end || inGroupTrace[currentNode] == groupNum)
                {
                    List<Conductor> trace = new List<Conductor>();
                    while (currentNode != start)
                    {
                        inGroupTrace[currentNode] = groupNum;
                        trace.Add(new Conductor(currentNode, prev[currentNode]));
                        currentNode = prev[currentNode];
                    }
                    inGroupTrace[currentNode] = groupNum;
                    CondTrace.Add(trace);
                }
                queue.Clear();
            }
        }

        private int GetDistance(IGraph grid,int start, int end)
        {
            return Math.Abs(grid.GetRow(start) - grid.GetRow(end)) + Math.Abs(grid.GetCol(start) - grid.GetCol(end));
        }
        //public List<Conductor> FindAllWaysBetweenPins(IGraph g, int start, int end)
        //{
        //    groupNum++;
        //    List<int>[] prev=new List<int>[g.GetN()];
        //    List<Conductor> ways = new List<Conductor>();
        //    Queue<int> pinsQueue = new Queue<int>();
        //    for (int i = 0; i < g.GetN(); i++)
        //        prev[i] = null;
        //    GetPer(g, start);
        //    int currentNode = end;
        //    pinsQueue.Enqueue(end);
        //    while(currentNode!=start)
        //    {
        //        currentNode = pinsQueue.Peek();
        //        foreach(int next in NextNode(currentNode, g))
        //        {
        //            if (prev[next] == null)
        //            {
        //                prev[next] = new List<int>();
        //                pinsQueue.Enqueue(next);
        //            }
        //            prev[next].Add(currentNode);
        //        }
        //        pinsQueue.Dequeue();
        //    }
        //    for (int i = 0; i < g.GetN(); i++)
        //        if (prev[i] != null)
        //            foreach (int n in prev[i])
        //                ways.Add(new Conductor(i, n));
        //    return ways;
        //}

        private IEnumerable<int> NextNode(int v, IGraph g)
        {
            foreach (int n in g.GetAdj(v))
            {
                if ((Graph_Config.GetDistance(n) == Graph_Config.GetDistance(v) - 1) || inGroupTrace[n] == groupNum)
                        yield return n;      
            }
        }

        public List<List<Conductor>> GetTrace()
        {
            return CondTrace;
        }
    }
}
