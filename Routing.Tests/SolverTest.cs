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
        public void PinConnect_HaveTraceBetweenAllPinsWithObstruct()
        {
            Obstruct simpleGraph =new Obstruct(new Graph(5, 5));
            Solver s = new Solver(simpleGraph);
            int[] pins=new int[]{1,21,14};
            simpleGraph.SetObstructZone(10,17);
            s.PinConnect(simpleGraph, pins);
            List<Conductor> expected = new List<Conductor>();
            expected.Add(new Conductor(1,2));
            expected.Add(new Conductor(2,3));
            expected.Add(new Conductor(3,8));
            expected.Add(new Conductor(8,13));
            expected.Add(new Conductor(13,18));
            expected.Add(new Conductor(18,23));
            expected.Add(new Conductor(23,22));
            expected.Add(new Conductor(22,21));
            expected.Add(new Conductor(13,14));
            List<Conductor> actual = s.GetTrace()[0];
            Assert.IsTrue(AreEqualTraces(expected, actual));
       }
        [TestMethod]

        public void PinConnect_SomePinsAreDisable()
        {
            Obstruct simpleGraph = new Obstruct(new Graph(5, 5));
            Solver s = new Solver(simpleGraph);
            int[] pins1 = new int[] { 3,20 };
            int[] pins2 = new int[] { 4, 24, 11 };
            List<Conductor> expected = new List<Conductor>();
            expected.Add(new Conductor(3,8));
            expected.Add(new Conductor(8,13));
            expected.Add(new Conductor(13,18));
            expected.Add(new Conductor(18,23));
            expected.Add(new Conductor(23,22));
            expected.Add(new Conductor(22,21));
            expected.Add(new Conductor(21,20));
            expected.Add(new Conductor(4, 9));
            expected.Add(new Conductor(9,14));
            expected.Add(new Conductor(14,19));
            expected.Add(new Conductor(19,24));
            s.PinConnect(simpleGraph, pins1);
            s.PinConnect(simpleGraph, pins2);
            List<Conductor> actual = new List<Conductor>();
            foreach (List<Conductor> trace in s.GetTrace())
                foreach (Conductor cond in trace)
                    actual.Add(cond);
            Assert.IsTrue(AreEqualTraces(actual, expected));
        }

        [TestMethod]

        public void PinConnect_HaveTraceBetweenAllPinsWithoutObstruct()
        {
            Graph simpleGraph = new Graph(5, 5);
            Solver s = new Solver(simpleGraph);
            int[] pins1 = new int[] { 1,21,14 };
            s.PinConnect(simpleGraph, pins1);
            List<Conductor> expected = new List<Conductor>();
            expected.Add(new Conductor(1,6));
            expected.Add(new Conductor(6,11));
            expected.Add(new Conductor(11,16));
            expected.Add(new Conductor(16,21));
            expected.Add(new Conductor(11,12));
            expected.Add(new Conductor(12,13));
            expected.Add(new Conductor(13,14));
            List<Conductor> actual = s.GetTrace()[0];
            Assert.IsTrue(AreEqualTraces(expected, actual));
        }

        [TestMethod]
        public void PinConnect_RouteTwoCircuits()
        {
            Graph simpleGraph = new Graph(6, 6);
            Solver s = new Solver(simpleGraph);
            int[] pins1 = { 7,29 };
            int[] pins2 = { 30, 14, 10 };
            s.PinConnect(simpleGraph, pins1);
            s.PinConnect(simpleGraph, pins2);
            List<Conductor> expected = new List<Conductor>();
            List<Conductor> actual = new List<Conductor>();
            expected.Add(new Conductor(29,28));
            expected.Add(new Conductor(28,27));
            expected.Add(new Conductor(27,26));
            expected.Add(new Conductor(26,25));
            expected.Add(new Conductor(25,19));
            expected.Add(new Conductor(19,13));
            expected.Add(new Conductor(13, 7));
            expected.Add(new Conductor(14,8));
            expected.Add(new Conductor(8,2));
            expected.Add(new Conductor(2,1));
            expected.Add(new Conductor(1,0));
            expected.Add(new Conductor(0,6));
            expected.Add(new Conductor(6,12));
            expected.Add(new Conductor(12,18));
            expected.Add(new Conductor(18,24));
            expected.Add(new Conductor(24,30));
            expected.Add(new Conductor(10,9));
            expected.Add(new Conductor(9,8));
            foreach (List<Conductor> trace in s.GetTrace())
                foreach (Conductor cond in trace)
                    actual.Add(cond);
            Assert.IsTrue(AreEqualTraces(expected, actual));

        }

        [TestMethod]
        public void HeuristicTestForGridWithoutObstruct()
        {
            Graph simpleGraph = new Graph(5, 5);
            Solver s = new Solver(simpleGraph);
            s.Heuristic(simpleGraph, new int[] { 23,6 });
            List<Conductor> expected = new List<Conductor>();
            expected.Add(new Conductor(7, 6));
            expected.Add(new Conductor(8, 7));
            expected.Add(new Conductor(13, 8));
            expected.Add(new Conductor(18, 13));
            expected.Add(new Conductor(23, 18));
            List<Conductor> actual = s.GetTrace()[0];
            Assert.IsTrue(AreEqualTraces(expected, actual));
        }

        [TestMethod]
        public void HeuristicTestForGridWithObstruct()
        {
            Obstruct simpleGraph = new Obstruct(new Graph(6, 6));
            simpleGraph.SetObstructZone(12, 13);
            simpleGraph.SetObstructZone(15, 16);
            simpleGraph.SetObstructZone(16, 34);
            Solver s = new Solver(simpleGraph);
            s.Heuristic(simpleGraph, new int[] { 26, 11 });
            List<Conductor> expected = new List<Conductor>();
            expected.Add(new Conductor(11, 10));
            expected.Add(new Conductor(10, 9));
            expected.Add(new Conductor(9, 8));
            expected.Add(new Conductor(8, 14));
            expected.Add(new Conductor(14, 20));
            expected.Add(new Conductor(20, 26));
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
