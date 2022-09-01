
using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITSetSettingsToConnection : ITDBSvc
    {
        [TestMethod]
        public void TestSetSettingsToConnection()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            DBResult<bool>? result = _dbSvc?.SetDBSettings(settings);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsTrue(result.Content);
        }
    }
}