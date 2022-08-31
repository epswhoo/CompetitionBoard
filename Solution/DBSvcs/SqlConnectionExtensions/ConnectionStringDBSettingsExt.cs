
using Models.IDBSvc;

namespace DBSvcs.SqlConnectionExtensions
{
    public static class ConnectionStringDBSettingsExt
    {
        public static string GetConnectionString(this DBConnectionSettings dbConnectionSettings)
        {
            string cnnString = $"Data Source={dbConnectionSettings.Server};" +
                $"Initial Catalog={dbConnectionSettings.DB};" +
                "TrustServerCertificate=True;" +
                $"User ID={dbConnectionSettings.Username};" +
                $"Password={dbConnectionSettings.Password}";
            return cnnString;
        }
    }
}