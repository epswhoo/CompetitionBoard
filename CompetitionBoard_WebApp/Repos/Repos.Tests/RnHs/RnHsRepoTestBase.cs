using Repos.RnHs;
using Repos.Tests.Fakes;

namespace Repos.Tests.RnHs
{
    public abstract class RnHsRepoTestBase
    {
        internal InMemoryDBSvc DBSvc { get; private set; }

        protected RnHsRepo Repo { get; private set; }

        protected List<string> Errors { get; private set; }

        [TestInitialize]
        public void InitRepo()
        {
            DBSvc = new InMemoryDBSvc();
            Repo = new RnHsRepo(DBSvc);
            Errors = new List<string>();
            Repo.ErrorRised += (_, message) => Errors.Add(message);
        }

        protected List<int> StoredHorseNos()
        {
            return DBSvc.Stored.Select(r => r.HorseNo).ToList();
        }

        protected List<int> StoredOrders()
        {
            return DBSvc.Stored.Select(r => r.Order).ToList();
        }
    }
}
