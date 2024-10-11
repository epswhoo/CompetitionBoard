
using CompetitionBoard_Net8.Models.IDBSvc;

namespace CompetitionBoard_Net8.DBSvc.SqlConnectionExtensions
{
    public static class ConnectionStringDBSettingsExt
    {
        public static string GetConnectionString(this DBConnectionSettings dbConnectionSettings)
        {   
            string port = dbConnectionSettings.Port != 0 ? $",{dbConnectionSettings.Port}" : string.Empty;
            string cnnString = $"Data Source={dbConnectionSettings.Server}{port};" +
                $"Initial Catalog={dbConnectionSettings.DB};" +
                "TrustServerCertificate=True;" +
                $"User ID={dbConnectionSettings.Username};" +
                $"Password={dbConnectionSettings.Password};" +
                $"Connection Timeout={dbConnectionSettings.TimeOut}";
            return cnnString;
        }
    }
}