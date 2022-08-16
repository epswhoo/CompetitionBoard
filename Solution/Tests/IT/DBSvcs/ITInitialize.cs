

using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITInitialize : ITDBSvc
    {
        [TestMethod]
        public void TestMethod1()
        {
            DBResult<string> result = _dbSvc.Initialize();
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
        }
    }
}