using Interfaces;
using Models.IDBSvc;
using DBSvcs.SubSvcs;
using DBSvcs.SqlConnectionExtensions;

namespace DBSvcs
{
    // All the code in this file is included in all platforms.
    public class TitleDBSvc : DBSvc, ITitleDBSvc
    {
        public TitleDBSvc(SqlConnectionSvc sqlConnectionSvc) : base(sqlConnectionSvc)
        {
            
        }

        public DBResult<string> ClearTitle(string title)
        {
            throw new NotImplementedException();
        }

        public DBResult<string> SaveTitle(string title)
        {
            string cmd = $"UPDATE dbo.TitleTable SET Title = '{title}';";
            return _sqlConnection.Execute(cmd);
        }
    }
}