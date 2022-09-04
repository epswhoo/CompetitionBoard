
using Models.Common;
using Models.Errors;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTSaveRankedAndNoMark : UTRnHsRepos
    {
        [TestMethod]
        public void TestSaveRankedAndNoMark()
        {
            RnH rnh = new RnH
            {
                Id = 2,
                Order = 1,
                HorseNo = 1,
                Status = RnHStatus.CompetitionDone,
                Mark = 0.0,
                IsDisqualificated = false,
                IsRanked = true
            };
            Result<RnH>? result = _rnHsRepo?.Save(rnh);
            Assert.IsNotNull(result);
            Assert.AreEqual(int.TryParse(ErrorCodes.IRnHRepoSaveRankAndNoMark, out int value) ? value : 0,
                result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Content);
        }
    }
}