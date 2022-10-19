using ViewModels.Svcs;

namespace Tests.UT.UTTitleRepo
{
    [TestClass]
    public class UTGaussNo_20_4
    {
        [TestMethod]
        public void TestLoad()
        {
            int result = GausNo.Get(20, 4);
            Assert.AreEqual(5, result);
        }
    }
}