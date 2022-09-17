using Interfaces;
using Simulations;
using Tests.IT.DBSvcs;
using ViewModels.Repos;

namespace Tests.UT.UTTitleRepo
{
    public abstract class UTTitleRepos : TestBase
    {
        private readonly IDBSvc _dbSvc = new DBSvc();
        protected ITitleRepo? _titleRepo;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _titleRepo = new TitleRepo(_dbSvc);
        }
    }
}