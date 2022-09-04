
using Models.Common;
using Models.Errors;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
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