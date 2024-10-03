using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.IT.DBSvcs
{
    [TestClass]
    public class ITRnHInsert : ITRnH
    {
        [TestMethod]
        public void TestRnHInsert()
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
            Result<RnH>? result = _dbSvc?.Insert(rnh);
            CompareRnH(rnh, result, false);
        }
    }
}