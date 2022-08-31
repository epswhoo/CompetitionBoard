using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using Models.IDBSvc;

namespace DBSvcs.SqlConnectionExtensions
{
    // All the code in this file is included in all platforms.
    public static class ExecuteNonQuerySqlConnectionExt
    {
        public static DBResult<string> Execute(this SqlConnection cnn, string cmd)
            => cnn.Open(cnn => {
                SqlCommand sqlCmd = new SqlCommand(cmd, cnn);
                sqlCmd.ExecuteNonQuery();
                return new DBResult<string>();
                });
    }
}