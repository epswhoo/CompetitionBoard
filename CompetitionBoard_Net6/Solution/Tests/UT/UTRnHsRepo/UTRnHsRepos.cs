using Interfaces;
using Simulations;
using Tests.IT.DBSvcs;
using ViewModels.Repos;

namespace Tests.UT.UTRnHsRepo
{
    public abstract class UTRnHsRepos : TestBase
    {
        private readonly IDBSvc _dbSvc = new DBSvc();
        protected IRnHsRepo? _rnHsRepo;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _rnHsRepo = new RnHsRepo(_dbSvc);
        }
    }
}