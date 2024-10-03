
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Errors;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTSaveMarkAndDis : UTRnHsRepos
    {
        [TestMethod]
        public void TestSaveMarkAndDis()
        {
            RnH rnh = new RnH
            {
                Id = 2,
                Order = 1,
                HorseNo = 1,
                Status = RnHStatus.CompetitionDone,
                Mark = 5.0,
                IsDisqualificated = true,
                IsRanked = false
            };
            Result<RnH>? result = _rnHsRepo?.Save(rnh);
            Assert.IsNotNull(result);
            Assert.AreEqual(int.TryParse(ErrorCodes.IRnHRepoSaveMarkAndDis, out int value) ? value : 0,
                result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Content);
        }
    }
}