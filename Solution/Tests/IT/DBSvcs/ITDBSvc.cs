using Interfaces;
using DBSvcs;

namespace Tests.IT.DBSvcs
{
    public abstract class ITDBSvc
    {
        protected IDBSvc _dbSvc = new DBSvc();
    }
}