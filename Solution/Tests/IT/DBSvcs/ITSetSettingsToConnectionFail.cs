
using DBSvcs;
using DBSvcs.SubSvcs;
using Interfaces;
using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITSetSettingsToConnectionFail : ITDBSvc
    {
        private IDBSvc? _dbSvc;

        protected override IDBSvc CreateIDBSvc(SqlConnectionSvc sqlConnectionSvc)
        {
            _dbSvc = new TitleDBSvc(sqlConnectionSvc);
            return _dbSvc;
        }

        [TestMethod]
        public void TestSetSettingsToConnection()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS2",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            DBResult<string>? result = _dbSvc?.SetSettingsToConnection(settings);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorCode);
            Assert.IsNotNull(result.Exception);
        }
    }
}