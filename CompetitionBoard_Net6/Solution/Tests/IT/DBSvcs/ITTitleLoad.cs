

using Models.Results;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITTitleLoad : ITDBSvc
    {
        [TestMethod]
        public void TestLoad()
        {
            string title = "Prüfung L*";
            _ = _dbSvc?.TitleSave(title);
            Result<string>? result = _dbSvc?.TitleLoad();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(title, result.Content);
        }
    }
}