
using DBSvcs;
using DBSvcs.SubSvcs;
using Interfaces;

namespace Tests.IT.DBSvcs.TitleDBSvcs
{
    public abstract class ITTitleDBSvc : ITDBSvc
    {
        protected ITitleDBSvc? _dbSvc;

        protected override IDBSvc CreateIDBSvc(SqlConnectionSvc sqlConnectionSvc)
        {
            _dbSvc = new TitleDBSvc(sqlConnectionSvc);
            return _dbSvc;
        }
    }
}