using WebApp.Services.Board;

namespace WebApp.Tests.Services.Board
{
    [TestClass]
    public class GausNoTests
    {
        [TestMethod]
        [DataRow(0, 4, 0)]
        [DataRow(1, 4, 1)]
        [DataRow(4, 4, 1)]
        [DataRow(5, 4, 2)]
        [DataRow(20, 4, 5)]
        [DataRow(21, 5, 5)]
        [DataRow(26, 5, 6)]
        public void Get_RoundsUp(int dividend, int divisor, int expected)
        {
            Assert.AreEqual(expected, GausNo.Get(dividend, divisor));
        }
    }
}
