using DBSvcs.RnHExtensions;
using Models.Common;
using Models.IDBSvc;
using Models.Results;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITRnHDelete : ITRnH
    {
        [TestMethod]
        public void TestRnHDelete()
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
            Result<bool>? deleteResult = _dbSvc?.Delete(insertResult.Content);
            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(0, deleteResult.ErrorCode);
            Assert.IsNull(deleteResult.Exception);
            Assert.IsTrue(deleteResult.Content);
        }
    }
}