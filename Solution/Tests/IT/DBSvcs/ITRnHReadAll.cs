using DBSvcs.RnHExtensions;
using Models.Common;
using Models.IDBSvc;
using Models.Results;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITRnHReadAll : ITRnH
    {
        [TestMethod]
        public void TestRnHReadAll()
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
            Result<RnH>? insertResult = _dbSvc?.RnHInsert(rnh);
            Assert.IsNotNull(insertResult);
            Result<IEnumerable<RnH>>? readAllResult = _dbSvc?.ReadAll();
            Assert.IsNotNull(readAllResult);
            Assert.AreEqual(0, readAllResult.ErrorCode);
            Assert.IsNull(readAllResult.Exception);
            Assert.IsNotNull(readAllResult.Content);
            Assert.IsTrue(readAllResult.Content.Count() > 0);
        }
    }
}