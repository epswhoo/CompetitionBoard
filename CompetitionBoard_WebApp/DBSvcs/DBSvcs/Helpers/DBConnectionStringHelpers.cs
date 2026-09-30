using Base.Models.DB;
using Microsoft.Data.SqlClient;

namespace DBSvcs.Helpers
{
    internal static class DBConnectionStringHelpers
    {
        private const int ConnectTimeoutSeconds = 5;

        internal static string GetConnectionString(this DBConnectionSettings dbConnectionSettings)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = dbConnectionSettings.Server,
                InitialCatalog = dbConnectionSettings.DB,
                UserID = dbConnectionSettings.Username,
                Password = dbConnectionSettings.Password,
                TrustServerCertificate = true,
                ConnectTimeout = ConnectTimeoutSeconds
            };
            return builder.ConnectionString;
        }
    }
}
