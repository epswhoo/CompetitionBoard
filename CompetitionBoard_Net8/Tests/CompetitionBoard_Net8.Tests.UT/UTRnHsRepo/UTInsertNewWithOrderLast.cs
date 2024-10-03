
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTInsertNewWithOrderLast : UTRnHsRepos
    {
        [TestMethod]
        public void TestInsertNewWithOrderLast()
        {
            Result<RnH>? result = _rnHsRepo?.InsertNewWithOrder(8);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(8, result.Content.Order);
            Result<IEnumerable<RnH>>? readResult = _rnHsRepo?.ReadAll();
            Assert.IsNotNull(readResult?.Content);
            Assert.AreEqual(8, readResult.Content.Count());
            List<RnH> rnhs = readResult.Content.ToList();
            Assert.AreEqual(2, rnhs[0].Order);
            Assert.AreEqual(1, rnhs[1].Order);
            Assert.AreEqual(3, rnhs[2].Order);
            Assert.AreEqual(4, rnhs[3].Order);
            Assert.AreEqual(5, rnhs[4].Order);
            Assert.AreEqual(6, rnhs[5].Order);
            Assert.AreEqual(7, rnhs[6].Order);
            Assert.AreEqual(8, rnhs[7].Order);
        }
    }
}