
using Models.Common;
using Models.Errors;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTCreateListFromStringFailed : UTRnHsRepos
    {
        [TestMethod]
        public void TestCreateListFromStringFailed()
        {
            string s = "1,2,3,4,5, 6s";
            Result<IEnumerable<RnH>>? result = _rnHsRepo?.SetNewRnHs(s);
            Assert.IsNotNull(result);
            Assert.AreEqual(int.TryParse(ErrorCodes.IRnHRepoException, out int value) ? value : 0,
                result.ErrorCode);
            Assert.IsNotNull(result.Exception);
        }
    }
}