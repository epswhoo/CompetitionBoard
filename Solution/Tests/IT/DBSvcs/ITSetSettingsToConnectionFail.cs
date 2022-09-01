
using DBSvcs;
using Interfaces;
using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITSetSettingsToConnectionFail : ITDBSvc
    {
        [TestMethod]
        [Ignore]
        public void TestSetSettingsToConnection()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS2",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            DBResult<bool>? result = _dbSvc?.SetDBSettings(settings); 
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsFalse(result.Content);
        }
    }
}