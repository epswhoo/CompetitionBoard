
using DBSvcs.SqlConnectionExtensions;
using Microsoft.Data.SqlClient;
using Models.IDBSvc;

namespace DBSvcs.Initialize
{
    // All the code in this file is included in all platforms.
    public static class InitSqlConnectionExt
    {
        private const string CnnString = "Data Source=NB-563\\SQLEXPRESS;Initial Catalog=CompetitionBoardDB;TrustServerCertificate=True;User ID=CompetitionBoard;Password=Tafeltafel0#";
        
        public static DBResult<string> Initialize(this SqlConnection cnn)
        {
            cnn.ConnectionString = CnnString;
            DBResult<string> result = cnn.Open<string>();
            return result;
        }
    }
}