using Interfaces;
using DBSvcs;
using DBSvcs.SubSvcs;
using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    public abstract class ITDBSvc
    {
        protected IDBSvc? _dbSvc;

        [TestInitialize]
        public void TestDBConnect()
        {
            SqlConnectionSvc sqlConnectionSvc = new SqlConnectionSvc();
            _dbSvc = new DBSvc(sqlConnectionSvc);
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            _ = _dbSvc.SetSettingsToConnection(settings);
        }
    }
}