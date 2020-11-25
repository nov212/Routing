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
        //static MultilayerGragh mlg;
        //[ClassInitialize]
        //public static void MlgInitilize()
        //{
        //    Graph g1 = new Graph(4, 4);
        //    Graph g2 = new Graph(4, 4);
        //    Obstruct g3 = new Obstruct(new Graph(4, 4));
        //    g3[6] = true;
        //    g3[9] = true;
        //    mlg = new MultilayerGragh(new IGraph[] { g1, g2, g3 });
        //}

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

        [TestMethod]
        public  void TestGetAdj()
        {
            MultilayerGragh mlg;
            Graph g1 = new Graph(4, 4);
            Graph g2 = new Graph(4, 4);
            Obstruct g3 = new Obstruct(new Graph(4, 4));
            g3[6] = true;
            g3[9] = true;
            mlg = new MultilayerGragh(new IGraph[] { g1, g2, g3 });
            int[] expected = { 25, 27, 22, 30 };
            int[] actual = mlg.GetAdj(26).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

       [TestMethod]
       public void TestSetVia()
        {
            MultilayerGragh mlg;
            Graph g1 = new Graph(4, 4);
            Graph g2 = new Graph(4, 4);
            Obstruct g3 = new Obstruct(new Graph(4, 4));
            g3[6] = true;
            g3[9] = true;
            mlg = new MultilayerGragh(new IGraph[] { g1, g2, g3 });

            mlg.SetVia(2, 2, 1, 2);
            int[] expected = { 25, 27, 22, 30, 42 };
            int[] actual =mlg.GetAdj(26).ToArray();
            CollectionAssert.AreEqual(expected, actual);

            mlg.SetVia(2, 2, 0, 2);
            int[] expected1 = { 25, 27, 22, 30, 42, 10 };
            int[] actual1 = mlg.GetAdj(26).ToArray();
            CollectionAssert.AreEqual(expected1, actual1);

            int[] expected2 = { 9, 11, 6, 14, 26 };
            int[] actual2 = mlg.GetAdj(10).ToArray();
            CollectionAssert.AreEqual(expected2, actual2);

            int[] expected3 = { 43,46,26 };
            int[] actual3 = mlg.GetAdj(42).ToArray();
            CollectionAssert.AreEqual(expected3, actual3);
        }

    }
}
