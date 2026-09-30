using Repos.Tests.Fakes;
using Repos.Title;

namespace Repos.Tests.Title
{
    [TestClass]
    public class TitleRepoTests
    {
        private InMemoryDBSvc _dbSvc;
        private TitleRepo _repo;

        [TestInitialize]
        public void Init()
        {
            _dbSvc = new InMemoryDBSvc();
            _repo = new TitleRepo(_dbSvc);
        }

        [TestMethod]
        public void Save_StoresTitle()
        {
            Assert.AreEqual("Dressur A", _repo.Save("Dressur A"));

            Assert.AreEqual("Dressur A", _dbSvc.TitleLoad());
        }

        [TestMethod]
        public void Load_ReturnsStoredTitle()
        {
            _dbSvc.TitleSave("Dressur L");

            Assert.AreEqual("Dressur L", _repo.Load());
        }

        [TestMethod]
        public void Clear_StoresEmptyTitle()
        {
            _dbSvc.TitleSave("Dressur L");

            Assert.AreEqual(string.Empty, _repo.Clear());

            Assert.AreEqual(string.Empty, _dbSvc.TitleLoad());
        }
    }
}
