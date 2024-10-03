using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.IDBSvc;
using CompetitionBoard_Net8.Tests.Base;

namespace CompetitionBoard_Net8.Tests.IT.DBSvcs
{
    public abstract class ITDBSvc : TestBase
    {
        protected readonly IDBSvc _dbSvc = new DBSvc.DBSvc();

        [TestInitialize]
        public void TestDBConnect()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "0000",
                TimeOut = 5
            };
            _ = _dbSvc.SetDBSettings(settings);
        }
    }
}