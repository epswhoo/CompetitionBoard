
using ViewModels.Svcs;

namespace Tests.UT.UTTitleRepo
{
    [TestClass]
    public class UTGaussNo_42_5
    {
        [TestMethod]
        public void TestLoad()
        {
            int result = GausNo.Get(42, 5);
            Assert.AreEqual(9, result);
        }
    }
}