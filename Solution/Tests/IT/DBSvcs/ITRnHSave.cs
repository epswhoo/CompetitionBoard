using DBSvcs.RnHExtensions;
using Models.Common;
using Models.IDBSvc;

namespace Tests.IT.DBSvcs.RnHsDBSvcs
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
            DBResult<RnH>? insertResult = _dbSvc?.RnHInsert(rnh);
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
            DBResult<RnH>? saveResult = _dbSvc?.RnHSave(insertResult.Content);
            CompareRnH(toSave, saveResult);
        }
    }
}