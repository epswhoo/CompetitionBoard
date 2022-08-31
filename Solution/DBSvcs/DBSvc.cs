using Interfaces;
using Models.Common;
using Models.IDBSvc;
using Microsoft.Data.SqlClient;
using DBSvcs.SubSvcs;

namespace DBSvcs
{
    // All the code in this file is included in all platforms.
    public abstract class DBSvc : IDBSvc
    {
        protected readonly SqlConnection _sqlConnection = new SqlConnection();
        private SqlConnectionSvc _sqlConnectionSvc;

        public DBSvc(SqlConnectionSvc sqlConnectionSvc)
        {
            _sqlConnectionSvc = sqlConnectionSvc;
        }

        public DBResult<string> SetSettingsToConnection(DBConnectionSettings dBConnectionSettings)
        {
            return _sqlConnectionSvc.SetConnectionString(_sqlConnection, dBConnectionSettings);
        }
    }
}