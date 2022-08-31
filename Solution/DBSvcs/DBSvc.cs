using Interfaces;
using Models.Common;
using Models.IDBSvc;
using Microsoft.Data.SqlClient;
using DBSvcs.SubSvcs;

namespace DBSvcs
{
    // All the code in this file is included in all platforms.
    public class DBSvc : IDBSvc
    {
        private readonly SqlConnection _sqlConnection = new SqlConnection();
        private SqlConnectionSvc _sqlConnectionSvc;

        public DBSvc(SqlConnectionSvc sqlConnectionSvc)
        {
            _sqlConnectionSvc = sqlConnectionSvc;
        }

        public DBResult<string> SetSettingsToConnection(DBConnectionSettings dBConnectionSettings)
        {
            return _sqlConnectionSvc.SetConnectionString(_sqlConnection, dBConnectionSettings);
        }

        //public DBResult<string> ClearTitle(string title)
        //{
        //    throw new NotImplementedException();
        //}

        //public DBResult<RnH> DeleteRnH(RnH RnH)
        //{
        //    throw new NotImplementedException();
        //}

        //public DBResult<RnH> InsertOnPosRnH(RnH RnH, int Pos)
        //{
        //    throw new NotImplementedException();
        //}

        //public DBResult<RnH> InsertRnH(RnH RnH)
        //{
        //    throw new NotImplementedException();
        //}

        //public DBResult<RnH> SaveRnH(RnH RnH)
        //{
        //    throw new NotImplementedException();
        //}

        //public DBResult<string> SaveTitle(string title)
        //{
        //    throw new NotImplementedException();
        //}
    }
}