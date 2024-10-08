
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.IDBSvc;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.IT.DBSvcs
{
    [TestClass]
    public class ITRnH : ITDBSvc
    {
        protected void CompareRnH(RnH expected, Result<RnH>? result, bool checkIds = true)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            if (checkIds)
                Assert.AreEqual(expected.Id, result.Content.Id);
            else
                Assert.IsTrue(result.Content.Id > 0);
            Assert.AreEqual(expected.Order, result.Content.Order);
            Assert.AreEqual(expected.HorseNo, result.Content.HorseNo);
            Assert.IsTrue(Math.Abs(expected.Mark - result.Content.Mark) < _tolerance);
            Assert.AreEqual(expected.Status, result.Content.Status);
            Assert.AreEqual(expected.IsRanked, result.Content.IsRanked);
        }
    }
}