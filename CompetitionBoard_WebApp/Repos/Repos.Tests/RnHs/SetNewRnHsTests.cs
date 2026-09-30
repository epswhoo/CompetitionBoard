using Base.Models.RnHs;

namespace Repos.Tests.RnHs
{
    [TestClass]
    public class SetNewRnHsTests : RnHsRepoTestBase
    {
        [TestMethod]
        public void SetNewRnHs_CreatesRnHsInGivenOrder()
        {
            List<RnH> result = Repo.SetNewRnHs("3, 5,7").ToList();

            CollectionAssert.AreEqual(new[] { 3, 5, 7 }, result.Select(r => r.HorseNo).ToList());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result.Select(r => r.Order).ToList());
            CollectionAssert.AreEqual(new[] { 3, 5, 7 }, StoredHorseNos());
            Assert.IsEmpty(Errors);
        }

        [TestMethod]
        public void SetNewRnHs_ReplacesExistingList()
        {
            DBSvc.Add(horseNo: 100, order: 1);
            DBSvc.Add(horseNo: 200, order: 2);

            Repo.SetNewRnHs("1,2");

            CollectionAssert.AreEqual(new[] { 1, 2 }, StoredHorseNos());
            CollectionAssert.AreEqual(new[] { 1, 2 }, StoredOrders());
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void SetNewRnHs_Empty_ReturnsNullAndRaisesError(string str)
        {
            DBSvc.Add(horseNo: 100, order: 1);

            Assert.IsNull(Repo.SetNewRnHs(str));

            CollectionAssert.AreEqual(new[] { "Es wurden keine Pferdenummern angegeben." }, Errors);
            CollectionAssert.AreEqual(new[] { 100 }, StoredHorseNos());
        }

        [TestMethod]
        [DataRow("1,x,3", "x")]
        [DataRow("1,,3", "")]
        [DataRow("1;2", "1;2")]
        [DataRow("1.5", "1.5")]
        public void SetNewRnHs_InvalidNumber_ReturnsNullAndKeepsExistingList(string str, string invalidPart)
        {
            DBSvc.Add(horseNo: 100, order: 1);

            Assert.IsNull(Repo.SetNewRnHs(str));

            CollectionAssert.AreEqual(new[] { $"Pferdenummer '{invalidPart}' ist keine gültige Zahl." }, Errors);
            CollectionAssert.AreEqual(new[] { 100 }, StoredHorseNos());
        }

        [TestMethod]
        public void SetNewRnHs_DeleteOfExistingListFails_ReturnsNullAndRaisesError()
        {
            RnH existing = DBSvc.Add(horseNo: 100, order: 1);
            DBSvc.FailDeleteForId = existing.Id;

            Assert.IsNull(Repo.SetNewRnHs("1,2"));

            Assert.Contains("Die bestehende Starterliste konnte nicht gelöscht werden.", Errors);
            CollectionAssert.AreEqual(new[] { 100 }, StoredHorseNos());
        }
    }
}
