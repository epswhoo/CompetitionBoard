using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTTitleRepo
{
    [TestClass]
    public class UTClear : UTTitleRepos
    {
        [TestMethod]
        public void TestClear()
        {
            Result<string>? result = _titleRepo?.Clear();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(string.Empty, result.Content);
        }
    }
}