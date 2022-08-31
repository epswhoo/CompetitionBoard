using Microsoft.Data.SqlClient;
using Models.IDBSvc;
using DBSvcs.SqlConnectionExtensions;

namespace DBSvcs.SubSvcs
{
    // All the code in this file is included in all platforms.
    public class SqlConnectionSvc
    {
        public DBResult<string> SetConnectionString(SqlConnection cnn, DBConnectionSettings dbConnectionSettings)
        {
            string cnnString = $"Data Source={dbConnectionSettings.Server};" +
                $"Initial Catalog={dbConnectionSettings.DB};" +
                "TrustServerCertificate=True;" +
                $"User ID={dbConnectionSettings.Username};" +
                $"Password={dbConnectionSettings.Password}"; 
            cnn.ConnectionString = cnnString;
            DBResult<string> result = cnn.Open<string>();
            return result;
        }
    }
}