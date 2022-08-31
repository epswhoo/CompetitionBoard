
using Models.IDBSvc;

namespace Tests.IT.DBSvcs.TitleDBSvcs
{
    [TestClass]
    public class ITSaveTitle : ITTitleDBSvc
    {
        [TestMethod]
        public void TestSaveTitle()
        {
            DBResult<string>? result = _dbSvc?.SaveTitle("Prüfung ABC");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
        }
    }
}