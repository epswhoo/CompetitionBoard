
using Models.Common;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTDeleteAll : UTRnHsRepos
    {
        [TestMethod]
        public void TestDeleteAll()
        {
            Result<bool>? result = _rnHsRepo?.DeleteAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Result<IEnumerable<RnH>>? readResult = _rnHsRepo?.ReadAll();
            Assert.IsNotNull(readResult?.Content);
            Assert.AreEqual(0, readResult?.Content.Count());
        }
    }
}