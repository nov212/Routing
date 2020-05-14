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
            List<Conductor> expected = new List<Conductor>();
            List<Conductor> actual = new List<Conductor>();
            s.FindPathOnSubgraph(simpleGraph, new List<int[]> { pins }, new int[] { 2, 3, 4 });
            expected.Add(new Conductor(32, 22));
            expected.Add(new Conductor(22, 23));
            expected.Add(new Conductor(23, 24));
            expected.Add(new Conductor(24, 25));
            expected.Add(new Conductor(25, 26));
            expected.Add(new Conductor(26, 27));
            expected.Add(new Conductor(32, 42));
            expected.Add(new Conductor(42, 52));
            expected.Add(new Conductor(52, 53));
            expected.Add(new Conductor(53, 54));
            expected.Add(new Conductor(54, 55));
            expected.Add(new Conductor(55, 65));
            expected.Add(new Conductor(65, 66));
            expected.Add(new Conductor(66, 67));
            expected.Add(new Conductor(67, 68));
            expected.Add(new Conductor(54, 64));
            expected.Add(new Conductor(64, 74));
            expected.Add(new Conductor(74, 84));
            foreach (List<Conductor> trace in s.GetTrace())
                foreach (Conductor cond in trace)
                    actual.Add(cond);
            Assert.IsTrue(AreEqualTraces(expected, actual));
        }

        [TestMethod]
        public void FindPathOnSubgraph_TwoCircuitsNoObstruct()
        {
            Graph simpleGraph = new Graph(10, 10);
            Solver s = new Solver(simpleGraph);
            int[] pins1 = { 32, 27, 55, 68, 84 };
            int[] pins2 = { 62, 87 };
            List<Conductor> expected = new List<Conductor>();
            List<Conductor> actual = new List<Conductor>();
            s.FindPathOnSubgraph(simpleGraph, new List<int[]> { pins1, pins2 }, new int[] { 2, 3, 4 });
            expected.Add(new Conductor(32, 22));
            expected.Add(new Conductor(22, 23));
            expected.Add(new Conductor(23, 24));
            expected.Add(new Conductor(24, 25));
            expected.Add(new Conductor(25, 26));
            expected.Add(new Conductor(26, 27));
            expected.Add(new Conductor(32, 42));
            expected.Add(new Conductor(42, 52));
            expected.Add(new Conductor(52, 53));
            expected.Add(new Conductor(53, 54));
            expected.Add(new Conductor(54, 55));
            expected.Add(new Conductor(55, 65));
            expected.Add(new Conductor(65, 66));
            expected.Add(new Conductor(66, 67));
            expected.Add(new Conductor(67, 68));
            expected.Add(new Conductor(54, 64));
            expected.Add(new Conductor(64, 74));
            expected.Add(new Conductor(74, 84));
            expected.Add(new Conductor(62, 72));
            expected.Add(new Conductor(72, 82));
            expected.Add(new Conductor(82, 92));
            expected.Add(new Conductor(92, 93));
            expected.Add(new Conductor(93, 94));
            expected.Add(new Conductor(94, 95));
            expected.Add(new Conductor(95, 85));
            expected.Add(new Conductor(85, 86));
            expected.Add(new Conductor(86, 87));
            foreach (List<Conductor> trace in s.GetTrace())
                foreach (Conductor cond in trace)
                    actual.Add(cond);
            Assert.IsTrue(AreEqualTraces(expected, actual));
        }

        [TestMethod]
        public void FindTrace_RouteTwoCircuits()
        {
            Graph simpleGraph = new Graph(10, 10);
            Solver s = new Solver(simpleGraph);
            int[] pins1 = { 32, 27, 55, 68, 84 };
            int[] pins2 = { 62, 87 };
            List<Conductor> expected = new List<Conductor>();
            List<Conductor> actual = new List<Conductor>();
            s.FindTrace(simpleGraph, new List<int[]> { pins1, pins2 });
            expected.Add(new Conductor(32, 22));
            expected.Add(new Conductor(22, 23));
            expected.Add(new Conductor(23, 24));
            expected.Add(new Conductor(24, 25));
            expected.Add(new Conductor(25, 26));
            expected.Add(new Conductor(26, 27));
            expected.Add(new Conductor(32, 42));
            expected.Add(new Conductor(42, 52));
            expected.Add(new Conductor(52, 53));
            expected.Add(new Conductor(53, 54));
            expected.Add(new Conductor(54, 55));
            expected.Add(new Conductor(55, 65));
            expected.Add(new Conductor(65, 66));
            expected.Add(new Conductor(66, 67));
            expected.Add(new Conductor(67, 68));
            expected.Add(new Conductor(54, 64));
            expected.Add(new Conductor(64, 74));
            expected.Add(new Conductor(74, 84));
            expected.Add(new Conductor(62, 72));
            expected.Add(new Conductor(72, 82));
            expected.Add(new Conductor(82, 92));
            expected.Add(new Conductor(92, 93));
            expected.Add(new Conductor(93, 94));
            expected.Add(new Conductor(94, 95));
            expected.Add(new Conductor(95, 85));
            expected.Add(new Conductor(85, 86));
            expected.Add(new Conductor(86, 87));
            foreach (List<Conductor> trace in s.GetTrace())
                foreach (Conductor cond in trace)
                    actual.Add(cond);
            Assert.IsTrue(AreEqualTraces(expected, actual));

        }

        public static bool AreEqualTraces(List<Conductor> expected, List<Conductor> actual)
        {
            if (expected.Count != actual.Count)
                return false;
            int count = expected.Count;
            for (int i = 0; i < count; i++)
                if (!expected[i].Equals(actual[i]))
                    return false;
            return true;
        }       
    }
}
