﻿using System;
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
            int range = 10000;
            Graph g = new Graph(range, range);
            Obstruct obs = new Obstruct(g);
            Solver s = new Solver(obs);
            int point = 0;
            int obsPoints =(int)(range * range *0.1);
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                point = rnd.Next(range * range);
                while (obs[point] == true)
                    point = rnd.Next(range * range);
                obs[point] = true;
            }
            int start = rnd.Next(range * range);
            System.Threading.Thread.Sleep(1000);
            int end = rnd.Next(range * range);
            while (start == end || obs[start] == true || obs[end] == true)
            {
                start = rnd.Next(range * range);
                end = rnd.Next(range * range);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            {
                s.PinConnect(obs, new int[] { start, end });
                //s.Heuristic(obs, new int[] { start, end });
            }
            sw.Stop();
            System.Console.WriteLine("RUNTIME {0}", sw.ElapsedMilliseconds);
            Application.Run(Test());
        }

        public static  System.Windows.Forms.Form Test()
        {
            frm_grid fg = new frm_grid(10, 10, 55);
            int range = 10;
            Graph g = new Graph(range, range);
            Obstruct obs = new Obstruct(g);
            return fg;
        }
    }
}
