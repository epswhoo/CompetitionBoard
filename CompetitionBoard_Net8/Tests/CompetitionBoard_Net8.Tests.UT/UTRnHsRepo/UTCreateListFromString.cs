
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Tests.UT.UTRnHsRepo
{
    [TestClass]
    public class UTCreateListFromString : UTRnHsRepos
    {
        [TestMethod]
        public void TestCreateListFromString()
        {
            string s = "1,2,3,4,5, 6";
            Result<IEnumerable<RnH>>? result = _rnHsRepo?.SetNewRnHs(s);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Content);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(6, result.Content.Count());
            List<RnH> rnhs = result.Content.ToList();
            Assert.AreEqual(1, rnhs[0].HorseNo);
            Assert.AreEqual(2, rnhs[1].HorseNo);
            Assert.AreEqual(3, rnhs[2].HorseNo);
            Assert.AreEqual(4, rnhs[3].HorseNo);
            Assert.AreEqual(5, rnhs[4].HorseNo);
            Assert.AreEqual(6, rnhs[5].HorseNo);
        }
    }
}