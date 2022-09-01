

using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITTitleSave : ITDBSvc
    {
        [TestMethod]
        public void TestSetSettingsToConnection()
        { 
            string title = "Prüfung ABC";
            DBResult<string>? result = _dbSvc?.TitleSave(title);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(title, result.Content);
        }
    }
}