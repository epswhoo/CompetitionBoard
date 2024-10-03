
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTReadAll : UTRnHsRepos
    {
        [TestMethod]
        public void TestReadAll()
        {
            Result<IEnumerable<RnH>>? result = _rnHsRepo?.ReadAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(7, result.Content.Count());
        }
    }
}