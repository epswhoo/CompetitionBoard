using CompetitionBoard_Net8.Models.IDBSvc;

namespace CompetitionBoard_Net8.WpfApp.Configs
{
    internal class Config
    {
        public DBConnectionSettings Db {  get; set; }

        public UIConfig UI { get; set; }
    }
}
