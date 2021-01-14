using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Routing.Tests
{

    [TestClass]
    public class PolygonTest
    {
        /*
         * Test Stand
        0 0 0 0 0 0 0
        0 0 1 1 1 0 0
        0 1 2 2 1 0 0
        0 1 2 2 1 0 0
        0 1 1 1 0 0 0
        0 1 1 1 0 1 1
        0 1 1 1 0 0 0
        */
        [TestMethod]
        public void RectangleInRangeTest()
        {
            IPolygon rect = new Rectangle(2, 2, 3, 3);
            Assert.IsTrue(rect.InRange(2, 3));
            Assert.IsFalse(rect.InRange(1, 4));
        }

        [TestMethod]
        public void IntersectPolygonTest()
        {
            IPolygon intersect = new IntersectPolygon();
            intersect.Add(new Rectangle(1, 2, 3, 4)).Add(new Rectangle(2, 1, 5, 3));
            Assert.IsTrue(intersect.InRange(3, 3), "не в пересечении");
            Assert.IsFalse(intersect.InRange(1, 4), "точка в первой области, но не в пересечении");
            Assert.IsFalse(intersect.InRange(1, 4), "точка во второй области, но не в пересечении");
        }

        [TestMethod]
        public void UnionPolygonTest()
        {
            IPolygon intersect = new IntersectPolygon();
            IPolygon union = new UnionPolygon();
            intersect.Add(new Rectangle(1, 2, 3, 4)).Add(new Rectangle(2, 1, 5, 3));
            union.Add(intersect);
            union.Add(new Rectangle(5, 5, 5, 6));
            Assert.IsTrue(union.InRange(3, 3));
            Assert.IsTrue(union.InRange(5, 5));
            Assert.IsFalse(union.InRange(6, 0));
        }
    }
}
