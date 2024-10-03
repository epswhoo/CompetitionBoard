
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Errors;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
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