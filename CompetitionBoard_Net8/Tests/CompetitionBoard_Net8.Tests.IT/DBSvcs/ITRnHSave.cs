using CompetitionBoard_Net8.DBSvc.RnHExtensions;
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.IT.DBSvcs
{
    [TestClass]
    public class ITRnHSave : ITRnH
    {
        [TestMethod]
        public void TestRnHSave()
        {
            RnH rnh = new RnH
            {
                Order = 2,
                HorseNo = 1,
                Status = RnHStatus.NotPresent,
                Mark = 0.0,
                IsRanked = false,
                IsDisqualificated = false
            };
            Result<RnH>? insertResult = _dbSvc?.Insert(rnh);
            Assert.IsNotNull(insertResult);
            RnH toSave = new RnH
            {
                Id = insertResult.Content.Id,
                Order = 102,
                HorseNo = 101,
                Status = RnHStatus.CompetitionDone,
                Mark = 1.2,
                IsRanked = true,
                IsDisqualificated = true
            };
            insertResult.Content.SetData(toSave);
            Result<RnH>? saveResult = _dbSvc?.Save(insertResult.Content);
            CompareRnH(toSave, saveResult);
        }
    }
}