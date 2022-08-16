using Interfaces;
using Models.Common;
using Models.IDBSvc;
using DBSvcs.Initialize;
using Microsoft.Data.SqlClient;

namespace DBSvcs
{
    // All the code in this file is included in all platforms.
    public class DBSvc : IDBSvc
    {
        private SqlConnection _sqlConnection = new SqlConnection();

        public DBResult<string> ClearTitle(string title)
        {
            throw new NotImplementedException();
        }

        public DBResult<string> Initialize()
        {

            return _sqlConnection.Initialize();
        }

        public DBResult<RnH> DeleteRnH(RnH RnH)
        {
            throw new NotImplementedException();
        }

        public DBResult<RnH> InsertOnPosRnH(RnH RnH, int Pos)
        {
            throw new NotImplementedException();
        }

        public DBResult<RnH> InsertRnH(RnH RnH)
        {
            throw new NotImplementedException();
        }

        public DBResult<RnH> SaveRnH(RnH RnH)
        {
            throw new NotImplementedException();
        }

        public DBResult<string> SaveTitle(string title)
        {
            throw new NotImplementedException();
        }
    }
}