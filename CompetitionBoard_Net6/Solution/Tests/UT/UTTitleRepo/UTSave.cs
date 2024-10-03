using Models.Results;

namespace Tests.UT.UTTitleRepo
{
    [TestClass]
    public class UTSave : UTTitleRepos
    {
        [TestMethod]
        public void TestSave()
        {
            string title = "Prüfung L";
            Result<string>? result = _titleRepo?.Save(title);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(title, result.Content);
        }
    }
}