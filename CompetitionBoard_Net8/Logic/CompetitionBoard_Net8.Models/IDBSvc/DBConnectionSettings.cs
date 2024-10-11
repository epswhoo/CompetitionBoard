namespace CompetitionBoard_Net8.Models.IDBSvc
{
    public class DBConnectionSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string DB { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int TimeOut { get; set; }
    }
}