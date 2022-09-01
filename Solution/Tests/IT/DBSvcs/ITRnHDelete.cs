using DBSvcs.RnHExtensions;
using Models.Common;
using Models.IDBSvc;

namespace Tests.IT.DBSvcs.RnHsDBSvcs
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
                Status = "",
                Mark = 0.0,
                IsRanked = false,
                IsDisqualificated = false
            };
            DBResult<RnH>? insertResult = _dbSvc?.RnHInsert(rnh);
            Assert.IsNotNull(insertResult);
            DBResult<bool>? deleteResult = _dbSvc?.RnHDelete(insertResult.Content);
            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(0, deleteResult.ErrorCode);
            Assert.IsNull(deleteResult.Exception);
            Assert.IsTrue(deleteResult.Content);
        }
    }
}