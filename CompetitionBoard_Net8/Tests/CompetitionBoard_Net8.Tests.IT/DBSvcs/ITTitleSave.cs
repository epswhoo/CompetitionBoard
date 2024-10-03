

using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.IT.DBSvcs
{
    [TestClass]
    public class ITTitleSave : ITDBSvc
    {
        [TestMethod]
        public void TestSave()
        { 
            string title = "Prüfung ABCD";
            Result<string>? result = _dbSvc?.TitleSave(title);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(title, result.Content);
        }
    }
}