
using Models.Common;
using Models.Errors;
using Models.Results;

namespace Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTSaveStatusNotCompetitionDoneWithDis : UTRnHsRepos
    {
        [TestMethod]
        public void TestSaveStatusNotCompetitionDoneWithDis()
        {
            RnH rnh = new RnH
            {
                Id = 2,
                Order = 1,
                HorseNo = 1,
                Status = RnHStatus.OnCompetitionField,
                Mark = 0.0,
                IsDisqualificated = true,
                IsRanked = false
            };
            Result<RnH>? result = _rnHsRepo?.Save(rnh);
            Assert.IsNotNull(result);
            Assert.AreEqual(int.TryParse(ErrorCodes.IRnHRepoSaveStatusNotCompetitionDoneWithDis, out int value) ? value : 0,
                result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Content);
        }
    }
}