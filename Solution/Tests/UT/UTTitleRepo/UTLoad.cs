using Models.Results;

namespace Tests.UT.UTTitleRepo
{
    [TestClass]
    public class UTLoad : UTTitleRepos
    {
        [TestMethod]
        public void TestLoad()
        {
            Result<string>? result = _titleRepo?.Load();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual("Prüfung A", result.Content);
        }
    }
}