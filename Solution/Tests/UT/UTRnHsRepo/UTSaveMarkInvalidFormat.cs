
using Models.Common;
using Models.Errors;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTSaveMarkInvalidFormat : UTRnHsRepos
    {
        [TestMethod]
        public void TestSaveMarkInvalidFormat()
        {
            RnH rnh = new RnH
            {
                Id = 2,
                Order = 1,
                HorseNo = 1,
                Status = RnHStatus.CompetitionDone,
                Mark = 1.11,
                IsDisqualificated = false,
                IsRanked = false
            };
            Result<RnH>? result = _rnHsRepo?.Save(rnh);
            Assert.IsNotNull(result);
            Assert.AreEqual(int.TryParse(ErrorCodes.IRnHRepoSaveMarkInvalidFormat, out int value) ? value : 0,
                result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Content);
        }
    }
}