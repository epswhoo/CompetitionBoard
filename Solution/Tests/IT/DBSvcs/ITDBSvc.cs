using Interfaces;
using DBSvcs.SubSvcs;
using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    public abstract class ITDBSvc
    {
        [TestInitialize]
        public void TestDBConnect()
        {
            SqlConnectionSvc sqlConnectionSvc = new SqlConnectionSvc();
            IDBSvc dbSvc = CreateIDBSvc(sqlConnectionSvc);
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            _ = dbSvc.SetSettingsToConnection(settings);
        }

        protected abstract IDBSvc CreateIDBSvc(SqlConnectionSvc sqlConnectionSvc);
    }
}