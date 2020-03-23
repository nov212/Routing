using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Routing
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            List<int[]> circuits = new List<int[]>();
            List<int> exist = new List<int>();
            List<int> obstr = new List<int>();
            int range = 100;
            Graph g = new Graph(100, 100);
            Obstruct obs = new Obstruct(g);
            Solver s = new Solver(obs);
            int point = 0;
            int obsPoints = (int)(100 * 100 * 0.1);
            Random rnd = new Random();
            for (int i = 0; i < obsPoints; i++)
            {
                point = rnd.Next(range * range);
                while (obs[point] == true)
                    point = rnd.Next(range * range);
                obs[point] = true;
                obstr.Add(point);
            }

            for (int i = 0; i < 10; i++)
            {
                int[] circuit = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    point = rnd.Next(obs.GetN());
                    while (obs[point] == true || exist.Contains(point))
                        point = rnd.Next(range * range);
                    exist.Add(point);
                    circuit[j] = point;
                }
                circuits.Add(circuit);
            }

            // Stopwatch sw = new Stopwatch();
            // sw.Start();
            foreach (int[] circ in circuits)
                s.PinConnect(obs, circ);
            frm_grid fg = new frm_grid();
            // sw.Stop();
            //System.Console.WriteLine("RUNTIME {0}", sw.ElapsedMilliseconds);
            Application.Run(fg);
        }

        public static  System.Windows.Forms.Form Test()
        {
            List <List<Conductor>> trace = new List<List<Conductor>>();
            List<Conductor> line1 = new List<Conductor>();
            line1.Add(new Conductor(40, 90));
            line1.Add(new Conductor(90, 140));
            line1.Add(new Conductor(140, 139));
            List<Conductor> line2 = new List<Conductor>();
            line2.Add(new Conductor(150,190));
            trace.Add(line1);
            trace.Add(line2);
            frm_grid fg = new frm_grid();
           // fg.SetObstruct(12, 25);
            //int range = 10;
            //Graph g = new Graph(range, range);
            //Obstruct obs = new Obstruct(g);
            //obs.SetObstructZone(14, 35);
            //Solver s = new Solver(obs);
            //s.PinConnect(obs, new int[] { 36, 23, 57, 76, 73 });
            //s.PinConnect(obs, new int[] { 28, 52, 54, 85 });
            //foreach (var trace in s.GetTrace())
            //    fg.DrawLines(trace);
            //fg.DrawObstruct(14, 35);
            return fg;
        }
    }
}
