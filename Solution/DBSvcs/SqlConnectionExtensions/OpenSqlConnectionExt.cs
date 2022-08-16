using Microsoft.Data.SqlClient;
using Models.IDBSvc;

namespace DBSvcs.SqlConnectionExtensions
{
    // All the code in this file is included in all platforms.
    public static class OpenSqlConnectionExt
    {
        public static DBResult<T> Open<T>(this SqlConnection cnn, Func<SqlConnection, DBResult<T>> toDo = null)
        {
            DBResult<T> result;
            try
            {
                cnn.Open();
                result = toDo == null ? new DBResult<T>() : toDo.Invoke(cnn);
                cnn.Close();
            }
            catch (Exception ex)
            {
                result = new DBResult<T>
                {
                    ErrorCode = 1,
                    Exception = ex
                };
            }
            return result;
        }
    }
}