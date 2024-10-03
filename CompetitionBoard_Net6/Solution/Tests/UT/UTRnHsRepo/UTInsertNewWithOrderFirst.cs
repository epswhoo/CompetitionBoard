
using Models.Common;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTInsertNewWithOrderFirst : UTRnHsRepos
    {
        [TestMethod]
        public void TestInsertNewWithOrderFirst()
        {
            Result<RnH>? result = _rnHsRepo?.InsertNewWithOrder(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(1, result.Content.Order);
            Result<IEnumerable<RnH>>? readResult = _rnHsRepo?.ReadAll();
            Assert.IsNotNull(readResult?.Content);
            Assert.AreEqual(8, readResult.Content.Count());
            List<RnH> rnhs = readResult.Content.ToList();
            Assert.AreEqual(3, rnhs[0].Order);
            Assert.AreEqual(2, rnhs[1].Order);
            Assert.AreEqual(4, rnhs[2].Order);
            Assert.AreEqual(5, rnhs[3].Order);
            Assert.AreEqual(6, rnhs[4].Order);
            Assert.AreEqual(7, rnhs[5].Order);
            Assert.AreEqual(8, rnhs[6].Order);
            Assert.AreEqual(1, rnhs[7].Order);
        }
    }
}