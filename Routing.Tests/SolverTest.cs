using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing;

namespace Routing.Tests
{
    [TestClass]
    public class SolverTest
    {
        [TestMethod]
        public void FindPathOnSubgraph_OneCircuitNoObstruct()
        {
            Graph simpleGraph = new Graph(10, 10);
            Solver s = new Solver(simpleGraph);
            int[] pins = { 32, 27, 55, 68, 84 };
            List<int> expected = new List<int>();
            List<List<int>> actual = s.FindPathOnSubgraph(simpleGraph, new List<int[]> { pins }, new int[] { 2, 3, 4 });
            expected.Add(32);
            expected.Add(22);
            expected.Add(23);
            expected.Add(24);
            expected.Add(25);
            expected.Add(26);
            expected.Add(27);
            expected.Add(32);
            expected.Add(42);
            expected.Add(52);
            expected.Add(53);
            expected.Add(54);
            expected.Add(55);
            expected.Add(52);
            expected.Add(62);
            expected.Add(63);
            expected.Add(64);
            expected.Add(65);
            expected.Add(66);
            expected.Add(67);
            expected.Add(68);
            expected.Add(64);
            expected.Add(74);
            expected.Add(84);
            CollectionAssert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void FindPathOnSubgraph_TwoCircuitsNoObstruct()
        {
            Graph simpleGraph = new Graph(10, 10);
            Solver s = new Solver(simpleGraph);
            int[] pins1 = { 32, 27, 55, 68, 84 };
            int[] pins2 = { 62, 87 };

            List<List<int>> actual = s.FindPathOnSubgraph(simpleGraph, new List<int[]> { pins1, pins2 }, new int[] { 2, 3, 4 });
            List<int> firstCircuitExp = new List<int>();
            List<int> secondCircuitExp = new List<int>();
            firstCircuitExp.Add(32);
            firstCircuitExp.Add(22);
            firstCircuitExp.Add(23);
            firstCircuitExp.Add(24);
            firstCircuitExp.Add(25);
            firstCircuitExp.Add(26);
            firstCircuitExp.Add(27);
            firstCircuitExp.Add(32);
            firstCircuitExp.Add(42);
            firstCircuitExp.Add(52);
            firstCircuitExp.Add(53);
            firstCircuitExp.Add(54);
            firstCircuitExp.Add(55);
            firstCircuitExp.Add(53);
            firstCircuitExp.Add(63);
            firstCircuitExp.Add(64);
            firstCircuitExp.Add(65);
            firstCircuitExp.Add(66);
            firstCircuitExp.Add(67);
            firstCircuitExp.Add(68);
            firstCircuitExp.Add(64);
            firstCircuitExp.Add(74);
            firstCircuitExp.Add(84);

            secondCircuitExp.Add(62);
            secondCircuitExp.Add(72);
            secondCircuitExp.Add(82);
            secondCircuitExp.Add(92);
            secondCircuitExp.Add(93);
            secondCircuitExp.Add(94);
            secondCircuitExp.Add(95);
            secondCircuitExp.Add(96);
            secondCircuitExp.Add(97);
            secondCircuitExp.Add(87);
            CollectionAssert.AreEqual(firstCircuitExp, actual[0]);
            CollectionAssert.AreEqual(secondCircuitExp, actual[1]);
        }

        [TestMethod]
        public void FindPathOnSubgraph_OneCircuitWithObstruct()
        {
            Graph g = new Graph(5, 6);
            Obstruct obs = new Obstruct(g);
            obs[20] = true;
            obs[21] = true;
            obs[15] = true;
            obs[9] = true;
            obs[3] = true;
            obs[16] = true;
            Solver solver = new Solver(obs);
            List<List<int>> actual = solver.FindPathOnSubgraph(obs, new List<int[]> { new int[] { 8, 17 } },
                new int[] { 3 });
            List<int> expected = new List<int> { 8, 7, 13, 19, 25, 26, 27, 28, 29, 23, 17 };
            CollectionAssert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void FindTrace_OneCircuitNoObstruct()
        {
            Graph simpleGraph = new Graph(10, 10);
            Solver s = new Solver(simpleGraph);
            int[] pins = { 32, 27, 55, 68, 84 };
            List<int> expected = new List<int>();
            List<List<int>> actual = s.FindTrace(simpleGraph, new List<int[]> { pins });
            expected.Add(32);
            expected.Add(22);
            expected.Add(23);
            expected.Add(24);
            expected.Add(25);
            expected.Add(26);
            expected.Add(27);
            expected.Add(32);
            expected.Add(42);
            expected.Add(52);
            expected.Add(53);
            expected.Add(54);
            expected.Add(55);
            expected.Add(52);
            expected.Add(62);
            expected.Add(63);
            expected.Add(64);
            expected.Add(65);
            expected.Add(66);
            expected.Add(67);
            expected.Add(68);
            expected.Add(64);
            expected.Add(74);
            expected.Add(84);
            CollectionAssert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void FindTrace_TwoCircuitsNoObstruct()
        {
            Graph simpleGraph = new Graph(10, 10);
            Solver s = new Solver(simpleGraph);
            int[] pins1 = { 32, 27, 55, 68, 84 };
            int[] pins2 = { 62, 87 };

            List<List<int>> actual = s.FindTrace(simpleGraph, new List<int[]> { pins1, pins2 });
            List<int> firstCircuitExp = new List<int>();
            List<int> secondCircuitExp = new List<int>();
            firstCircuitExp.Add(32);
            firstCircuitExp.Add(22);
            firstCircuitExp.Add(23);
            firstCircuitExp.Add(24);
            firstCircuitExp.Add(25);
            firstCircuitExp.Add(26);
            firstCircuitExp.Add(27);
            firstCircuitExp.Add(32);
            firstCircuitExp.Add(42);
            firstCircuitExp.Add(52);
            firstCircuitExp.Add(53);
            firstCircuitExp.Add(54);
            firstCircuitExp.Add(55);
            firstCircuitExp.Add(53);
            firstCircuitExp.Add(63);
            firstCircuitExp.Add(64);
            firstCircuitExp.Add(65);
            firstCircuitExp.Add(66);
            firstCircuitExp.Add(67);
            firstCircuitExp.Add(68);
            firstCircuitExp.Add(64);
            firstCircuitExp.Add(74);
            firstCircuitExp.Add(84);

            secondCircuitExp.Add(62);
            secondCircuitExp.Add(72);
            secondCircuitExp.Add(82);
            secondCircuitExp.Add(92);
            secondCircuitExp.Add(93);
            secondCircuitExp.Add(94);
            secondCircuitExp.Add(95);
            secondCircuitExp.Add(96);
            secondCircuitExp.Add(97);
            secondCircuitExp.Add(87);
            CollectionAssert.AreEqual(firstCircuitExp, actual[0]);
            CollectionAssert.AreEqual(secondCircuitExp, actual[1]);
        }

        [TestMethod]
        public void FindTrace_OneCircuitWithObstruct()
        {
            Obstruct obs = new Obstruct(new Graph(5, 6));
            obs.SetPrefferedDirection(false,0);
            obs[20] = true;
            obs[21] = true;
            obs[15] = true;
            obs[9] = true;
            obs[3] = true;
            obs[16] = true;
            Solver solver = new Solver(obs);
            List<List<int>> actual = solver.FindTrace(obs, new List<int[]> { new int[] { 8, 17 } });
            List<int> expected = new List<int> { 8, 7, 13, 19, 25, 26, 27, 28, 29, 23, 17 };
            CollectionAssert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void FindTrace_OneCircuitOnePinIsDisable()
        {
            Graph g = new Graph(5, 6);
            Obstruct obs = new Obstruct(g);
            obs[13] = true;
            obs[14] = true;
            obs[15] = true;
            obs[16] = true;
            obs[19] = true;
            obs[25] = true;
            obs[22] = true;
            obs[28] = true;
            Solver solver = new Solver(obs);
            List<List<int>> actual = solver.FindTrace(obs, new List<int[]> { new int[] { 0, 5, 29, 20 },
                new int[] { 6, 24, 27 } });
            Dictionary<int, List<int>> expectedError = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 20 } },
                {2, new List<int>{ 27 } }
            };
            CollectionAssert.AreEqual(expectedError.Keys, solver.GetFailReport().Keys);
            CollectionAssert.AreEqual(expectedError.Values, solver.GetFailReport().Values);

        }

        [TestMethod]
        public void FindPathOnSubgraph_OneCircuitOnePinIsDisable()
        {
            Graph g = new Graph(5, 6);
            Obstruct obs = new Obstruct(g);
            obs[13] = true;
            obs[14] = true;
            obs[15] = true;
            obs[16] = true;
            obs[19] = true;
            obs[25] = true;
            obs[22] = true;
            obs[28] = true;
            Solver solver = new Solver(obs);
            List<List<int>> actual = solver.FindPathOnSubgraph(obs, new List<int[]> { new int[] { 0, 5, 29, 20 },
                new int[] { 6, 24, 27 } }, new int[] { 1 });
            Dictionary<int, List<int>> expectedError = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 20 } },
                {2, new List<int>{ 27 } }
            };
            CollectionAssert.AreEqual(expectedError.Keys, solver.GetFailReport().Keys);
            CollectionAssert.AreEqual(expectedError.Values, solver.GetFailReport().Values);
        }

        [TestMethod]
        public void MultilayerRouting()
        {
            Obstruct layer1 = new Obstruct(new Graph(6, 6));
            Obstruct layer2 = new Obstruct(new Graph(6, 6));
            MultilayerGragh mg = new MultilayerGragh();
            layer1.SetObstructZone(1, 3, 3, 3,0);
            layer2.SetObstructZone(2, 3, 3, 4, 0);
            layer1.SetVia(4, 2, 0);
            layer2.SetVia(4, 2, 0);
            mg.Add(layer1);
            mg.Add(layer2);
            mg.SetPrefferedDirection(false, 0);
            mg.SetPrefferedDirection(true, 1);
            Solver s = new Solver(mg);
            List<List<int>> actual=s.FindTrace(mg, new List<int[]> { new int[] { 7, 47 } });
            List<int> expected = new List<int> { 7, 8, 14, 20, 26, 62, 63, 64, 65, 59, 53, 47 };
            CollectionAssert.AreEqual(expected, actual[0]);
        }
    }
}
