using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Tests.Base;
using CompetitionBoard_Net8.ViewModels.Repos;

namespace CompetitionBoard_Net8.Tests.UT.UTTitleRepo
{
    public abstract class UTTitleRepos : TestBase
    {
        private readonly IDBSvc _dbSvc = new Sim.DBSvc();
        protected ITitleRepo? _titleRepo;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _titleRepo = new TitleRepo(_dbSvc);
        }
    }
}