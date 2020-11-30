using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Routing;

namespace Routing.Tests
{
    /// <summary>
    /// Сводное описание для HGraphComponent
    /// </summary>
    [TestClass]
    public class HGraphCompositeTest
    {
        IGraph composite;
        public HGraphCompositeTest()
        {
            composite = new HGraphComposite(new IGraph[]
            {
                new Graph(5,4),
                new Graph(3,3)
            });
        }


        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
 

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
        public void ColsRowsTest()
        {
            int actualCols = composite.Cols;
            int expectedCols = 7;
            int actualRows = composite.Rows;
            int expectedRows = 5;
            Assert.AreEqual(expectedCols, actualCols, "неправильное количество столбцов, ожидалось {0}, получилось {1}",
                expectedCols, actualCols);
            Assert.AreEqual(expectedRows, actualRows, "неправильное количество сток, ожидалось {0}, получилось {1}",
                expectedRows, actualRows);
        }

        [TestMethod]
        public void GetAdjTest()
        {
            int[] expected1 = { 15, 17, 9, 23 };
            int[] actual1 = composite.GetAdj(16).ToArray();
            CollectionAssert.AreEqual(expected1, actual1);
            int[] expected2 = { 23, 17, 31 };
            int[] actual2 = composite.GetAdj(24).ToArray();
            CollectionAssert.AreEqual(expected2, actual2);
        }

        [TestMethod]
        public void GetAdjButSomeNodesAreDisable()
        {
            Obstruct compositeObs = new Obstruct(composite);
            compositeObs.SetObstructZone(3, 4, 4, 6);
            int[] expected = { 23, 17, 31 };
            int[] actual = compositeObs.GetAdj(24).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAdjButSomeGraphsAreObstruct()
        {
            IGraph g1 = new Graph(5, 4);
            Obstruct g2 = new Obstruct(new Graph(3,3));
            g2.SetObstructZone(0, 0, 2, 0);
            IGraph comp = new HGraphComposite(new IGraph[] { g1, g2 });
            int[] expected = { 9,3, 17 };
            int[] actual = comp.GetAdj(10).ToArray();
            CollectionAssert.AreEqual(expected, actual, "неправильное число соседей: ожидалось {0}, получилось {1}",
                3, actual.Length);
        }
    }
}
