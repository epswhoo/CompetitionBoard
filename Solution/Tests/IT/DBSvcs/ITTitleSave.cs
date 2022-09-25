

using Models.Results;

namespace Tests.IT.DBSvcs
{
    [TestClass]
    public class ITTitleSave : ITDBSvc
    {
        [TestMethod]
        public void TestSave()
        { 
            string title = "Pr�fung ABCD";
            Result<string>? result = _dbSvc?.TitleSave(title);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(title, result.Content);
        }
    }
}