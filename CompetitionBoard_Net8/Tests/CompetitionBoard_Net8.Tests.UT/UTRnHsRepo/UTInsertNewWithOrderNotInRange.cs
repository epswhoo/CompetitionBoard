
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Errors;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTInsertNewWithOrderNotInRange : UTRnHsRepos
    {
        [TestMethod]
        public void TestInsertNewWithOrderNotInRange()
        {
            Result<RnH>? result = _rnHsRepo?.InsertNewWithOrder(10);
            Assert.IsNotNull(result);
            Assert.AreEqual(int.TryParse(ErrorCodes.IRnHRepoInsertNewWithOrderOutOfRange, out int value) ? value : 0,
                result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Content);
        }
    }
}