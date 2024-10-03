
using Models.Common;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTDelete : UTRnHsRepos
    {
        [TestMethod]
        public void TestDelete()
        {
            RnH rnh = new RnH
            {
                Id = 2,
                Order = 1,
                HorseNo = 255,
                Status = RnHStatus.CompetitionDone,
                Mark = 0.0,
                IsDisqualificated = true,
                IsRanked = false
            };
            Result<bool>? result = _rnHsRepo?.Delete(rnh);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content);
            Result<IEnumerable<RnH>>? readResult = _rnHsRepo?.ReadAll();
            Assert.IsNotNull(readResult?.Content);
            Assert.AreEqual(6, readResult.Content.Count());
            List<RnH> rnhs = readResult.Content.ToList();
            Assert.AreEqual(1, rnhs[0].Order);
            Assert.AreEqual(2, rnhs[1].Order);
            Assert.AreEqual(3, rnhs[2].Order);
            Assert.AreEqual(4, rnhs[3].Order);
            Assert.AreEqual(5, rnhs[4].Order);
            Assert.AreEqual(6, rnhs[5].Order);
        }
    }
}