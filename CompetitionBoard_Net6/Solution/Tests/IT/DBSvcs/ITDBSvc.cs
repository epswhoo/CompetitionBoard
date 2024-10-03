using Interfaces;
using Models.IDBSvc;
using DBSvcs;

namespace Tests.IT.DBSvcs
{
    public abstract class ITDBSvc : TestBase
    {
        protected readonly IDBSvc _dbSvc = new DBSvc();

        [TestInitialize]
        public void TestDBConnect()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            _ = _dbSvc.SetDBSettings(settings);
        }
    }
}