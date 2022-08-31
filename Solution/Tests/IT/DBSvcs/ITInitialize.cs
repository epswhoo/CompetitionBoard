

using Models.IDBSvc;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITInitialize : ITDBSvc
    {
        [TestMethod]
        public void TestDBConnect()
        {
            string cnnString = "Data Source=DESKTOP-34KSSOT\\SQLEXPRESS;Initial Catalog=CompetitionBoardDB;TrustServerCertificate=True;User ID=CompetitionBoard;Password=Tafeltafel0#";
            DBResult<string> result = _dbSvc.Initialize(cnnString);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
        }
    }
}