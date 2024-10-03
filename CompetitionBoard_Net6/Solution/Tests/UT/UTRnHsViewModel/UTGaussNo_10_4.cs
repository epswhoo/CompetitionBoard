
using ViewModels.Svcs;

namespace Tests.UT.UTTitleRepo
{
    [TestClass]
    public class UTGaussNo
    {
        [TestMethod]
        public void TestLoad()
        {
            int result = GausNo.Get(10, 4);
            Assert.AreEqual(3, result);
        }
    }
}