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
            int[] pins = { 32, 27, 55, 68,84 };
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
            expected.Add(55);
            expected.Add(65);
            expected.Add(66);
            expected.Add(67);
            expected.Add(68);
            expected.Add(54);
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
            List<int> firstCircuitExp=new List<int>();
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
            firstCircuitExp.Add(55);
            firstCircuitExp.Add(65);
            firstCircuitExp.Add(66);
            firstCircuitExp.Add(67);
            firstCircuitExp.Add(68);
            firstCircuitExp.Add(54);
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
            List<int> expected = new List<int>{8, 7, 13, 19, 25, 26, 27, 28, 29, 23, 17};
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
            expected.Add(55);
            expected.Add(65);
            expected.Add(66);
            expected.Add(67);
            expected.Add(68);
            expected.Add(54);
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
            firstCircuitExp.Add(55);
            firstCircuitExp.Add(65);
            firstCircuitExp.Add(66);
            firstCircuitExp.Add(67);
            firstCircuitExp.Add(68);
            firstCircuitExp.Add(54);
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
            Graph g = new Graph(5, 6);
            Obstruct obs = new Obstruct(g);
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
}
