using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Tests.Base;
using CompetitionBoard_Net8.ViewModels.Repos;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
{
    public abstract class UTRnHsRepos : TestBase
    {
        private readonly IDBSvc _dbSvc = new Sim.DBSvc();
        protected IRnHsRepo? _rnHsRepo;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _rnHsRepo = new RnHsRepo(_dbSvc);
        }
    }
}