using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing;

namespace Routing.Tests
{
    /// <summary>
    /// Сводное описание для MultilayerGraphTest
    /// </summary>
    [TestClass]
    public class MultilayerGraphTest
    {
        MultilayerGragh mlg;
        public MultilayerGraphTest()
        {
            Graph g1 = new Graph(4, 4);
            Graph g2 = new Graph(4, 4);
            Obstruct g3 = new Obstruct(new Graph(4, 4));
            g3[6] = true;
            g3[9] = true;
            mlg = new MultilayerGragh();
            mlg.Add(g1);
            mlg.Add(g2);
            mlg.Add(g3);
            
        }

        #region Дополнительные атрибуты тестирования
        //
        // При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        // ClassInitialize используется для выполнения кода до запуска первого теста в классе
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // TestInitialize используется для выполнения кода перед запуском каждого теста 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // TestCleanup используется для выполнения кода после завершения каждого теста
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

            // TEST STAND
            // LAYER 0          LAYER 1         LAYER 2
            // 0  1  2  3       16 17 18 19     32 33 34 35
            // 4  5  6  7       20 21 22 23     36 37 38 39
            // 8  9  V  11      24 25 V  27     40 41 V  43
            // 12 13 14 15      28 29 30 31     44 45 46 47

        [TestMethod]
        public  void TestGetAdj()
        {
            int[] expected = { 25, 27, 22, 30 };
            int[] actual = mlg.GetAdj(26).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAdjOneLayerUp()
        {
            mlg.SetVia(2, 2, 1);
            mlg.SetVia(2, 2, 2);
            int[] expected = { 25, 27, 22, 30, 42 };
            int[] actual = mlg.GetAdj(26).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAdjCurrentUpDown()
        {
            mlg.SetVia(2, 2, 1);
            mlg.SetVia(2, 2, 2);
            mlg.SetVia(2, 2, 0);
            int[] expected1 = { 25, 27, 22, 30, 42, 10 };
            int[] actual1 = mlg.GetAdj(26).ToArray();
            CollectionAssert.AreEqual(expected1, actual1);
        }
        [TestMethod]
        public void ToNumTest()
        {
            int expected = 22;
            int actual = mlg.ToNum(1, 2, 1);
            Assert.AreEqual(expected, actual, "ожидалось: {0}, получилось: {1}", expected, actual);
            expected = 15;
            actual = mlg.ToNum(3, 3, 0);
            Assert.AreEqual(expected, actual, "ожидалось: {0}, получилось: {1}", expected, actual);
        }

        [TestMethod]
        public void GetColTest()
        {
            int expected = 2;
            int actual = mlg.GetCol(22);
            Assert.AreEqual(expected, actual, "ожидалось: {0}, получилось: {1}", expected, actual);
            expected = 0;
            actual = mlg.GetCol(32);
            Assert.AreEqual(expected, actual, "ожидалось: {0}, получилось: {1}", expected, actual);
        }

        [TestMethod]
        public void GetRowTest()
        {
            int expected = 1;
            int actual = mlg.GetRow(22);
            Assert.AreEqual(expected, actual, "ожидалось: {0}, получилось: {1}", expected, actual);
            expected = 0;
            actual = mlg.GetRow(32);
            Assert.AreEqual(expected, actual, "ожидалось: {0}, получилось: {1}", expected, actual);
        }
    }
}

