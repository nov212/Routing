using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Solver
    {
        private int[] inGroupTrace;                 //массив меток для каждого контакта 0-не трассирован, 1-принадлежит цепи 1
        private List<List<Conductor>> CondTrace;    //список трасс
        private List<int> fail;                     //список контактов, которые не удалось соединить

        public Solver(IGraph g)
        {
            inGroupTrace = new int[g.GetN()];
            CondTrace = new List<List<Conductor>>();
            for (int i = 0; i < inGroupTrace.Length; i++)
                inGroupTrace[i] = 0;
            fail = new List<int>();
        }

        private int[] GetWave(IGraph obs, int node)
        {
            int[] wave = new int[obs.GetN()];
            for (int i = 0; i< wave.Length; i++)
                wave[i] = -1;
            Queue<int> pinsQueue=new Queue<int>();
            PerPut Graph_Config = new PerPut(obs.GetN());
            Graph_Config.MoveLeft(node);
            wave[node] = 0;
            int group = inGroupTrace[node];
            pinsQueue.Enqueue(node);
            while (pinsQueue.Count()!=0)
            {
                node = pinsQueue.Dequeue();
                foreach (int n in obs.GetAdj(node))
                {
                    if ((inGroupTrace[n]==0 || inGroupTrace[n]==group) && Graph_Config.MoveLeft(n))
                    {
                        wave[n] = wave[node] + 1;
                        pinsQueue.Enqueue(n);
                    }
                }
            }
            return wave;
        }

        public void FindPathOnSubgraph(IGraph g, IEnumerable<int[]> circuits, int[] radius)
        {
            int idx;
            int rad;
            int failcount;
            Queue<int> pinsQueue = new Queue<int>();
            int[] prev = new int[g.GetN()];
            int[] cost = new int[g.GetN()];
            int group = 0;  //номер цепи 

            //пометим контакты номером цепи
            foreach (var circ in circuits)
            {
                group++;
                foreach (var pin in circ)
                    inGroupTrace[pin] = group;
            }

            foreach (var circ in circuits)
            {
                idx = 0;
                int start = circ[0];
                int currentGroup = inGroupTrace[start];
                int currentNode = start;
                Subgraph sub=new Subgraph(g);
                int price;
                sub.Add(circ);
                List<Conductor> PinToPinTrace = new List<Conductor>();
                do
                {
                    failcount = -1;
                    int[] wave = GetWave(sub, start);
                        rad = radius[idx++];
                        sub.Extend(circ, rad);
 
                    foreach (int n in circ)
                    {
                        if (wave[n] > 0)  //существует путь до узла n
                        {
                            currentNode = n;
                            for (int i = 0; i < prev.Length; i++)
                            {
                                prev[i] = -1;
                                cost[i] = -1;
                            }

                            cost[currentNode] = 0;
                            foreach (int next in sub.GetAdj(currentNode))
                                if (wave[next] == (wave[currentNode] - 1))
                                {
                                    pinsQueue.Enqueue(next);
                                    prev[next] = currentNode;
                                    cost[next] = 1;
                                }

                            while (pinsQueue.Count != 0)
                            {
                                currentNode = pinsQueue.Dequeue();
                                if (inGroupTrace[currentNode] == currentGroup)
                                    break;
                                foreach (int next in sub.GetAdj(currentNode))
                                    if (wave[next] == (wave[currentNode] - 1))
                                    {
                                        if (prev[next] == -1)
                                        {
                                            pinsQueue.Enqueue(next);
                                            prev[next] = currentNode;
                                        }
                                        if (Math.Abs(prev[currentNode] - currentNode) == Math.Abs(next - currentNode))
                                            price = cost[currentNode] + 1;
                                        else
                                            price = cost[currentNode] + 2;
                                        if (price < cost[next] || cost[next] == -1)
                                        {
                                            cost[next] = price;
                                            prev[next] = currentNode;
                                        }
                                    }
                            }
                            while (prev[currentNode] != -1)
                            {
                                PinToPinTrace.Add(new Conductor(currentNode, prev[currentNode]));
                                inGroupTrace[currentNode] = currentGroup;
                                inGroupTrace[prev[currentNode]] = currentGroup;
                                currentNode = prev[currentNode];
                            }
                            pinsQueue.Clear();
                        }
                        else failcount++;
                    }
                } while (failcount > 0 && idx < radius.Length);
                if (PinToPinTrace.Count()>0)
                    CondTrace.Add(PinToPinTrace);
            }
        }

        public void FindTrace(IGraph g, IEnumerable<int[]> pins)
        {
            int group = 0;  //номер цепи 

            //пометим контакты номером цепи
            foreach (var trace in pins)
            {
                group++;
                foreach (var pin in trace)
                    inGroupTrace[pin] = group;
            }

            foreach (var trace in pins)
            {
                List<Conductor> PinToPinTrace = new List<Conductor>();
                int start = trace[0];
                int currentGroup = inGroupTrace[start];     //номер обрабатываемой цепи
                int[] wave = new int[g.GetN()];
                wave = GetWave(g, start);
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
                for (int k = 1; k < trace.Length; k++)
                {
                    if (wave[trace[k]] > 0) //условие существования пути до узла trace[k]
                    {
                        currentNode = trace[k];
                        //инициализация массивов prev и cost
                        for (int i = 0; i < prev.Length; i++)
                        {
                            prev[i] = -1;
                            cost[i] = -1;
                        }

                        cost[currentNode] = 0;
                        foreach (int next in g.GetAdj(currentNode))
                            if (wave[next] == (wave[currentNode] - 1))
                            {
                                pinsQueue.Enqueue(next);
                                prev[next] = currentNode;
                                cost[next] = 1;
                            }

                        while (pinsQueue.Count != 0)
                        {
                            currentNode = pinsQueue.Dequeue();
                            if (inGroupTrace[currentNode] == currentGroup)
                                break;
                            foreach (int next in g.GetAdj(currentNode))
                            {
                                if (wave[next] == (wave[currentNode] - 1))
                                {
                                    if (prev[next] == -1)
                                    {
                                        pinsQueue.Enqueue(next);
                                        prev[next] = currentNode;
                                    }
                                    if (Math.Abs(prev[currentNode] - currentNode) == Math.Abs(next - currentNode))
                                        price = cost[currentNode] + 1;
                                    else
                                        price = cost[currentNode] + 2;
                                    if (price < cost[next] || cost[next] == -1)
                                    {
                                        cost[next] = price;
                                        prev[next] = currentNode;
                                    }
                                }
                            }
                        }

                        //построение трассы
                        while (prev[currentNode] != -1)
                        {
                            PinToPinTrace.Add(new Conductor(currentNode, prev[currentNode]));
                            inGroupTrace[currentNode] = currentGroup;
                            inGroupTrace[prev[currentNode]] = currentGroup;
                            currentNode = prev[currentNode];
                        }
                        pinsQueue.Clear();
                    }
                    else fail.Add(trace[k]);
                }
                if (PinToPinTrace.Count()>0)
                        CondTrace.Add(PinToPinTrace);                     
            }
        }

        public List<List<Conductor>> GetTrace()
        {
            return CondTrace;
        }
    }
}
