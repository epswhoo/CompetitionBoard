using Base.Models.RnHs;

namespace Repos.Tests.RnHs
{
    [TestClass]
    public class DeleteTests : RnHsRepoTestBase
    {
        [TestMethod]
        public void Delete_RemovesRnHAndShiftsFollowingRnHs()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            RnH second = DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.Add(horseNo: 30, order: 3);
            DBSvc.Add(horseNo: 40, order: 4);

            Assert.IsTrue(Repo.Delete(second));

            CollectionAssert.AreEqual(new[] { 10, 30, 40 }, StoredHorseNos());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, StoredOrders());
            Assert.IsEmpty(Errors);
        }

        [TestMethod]
        public void Delete_UsesStoredOrderInsteadOfPassedOrder()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            RnH second = DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.Add(horseNo: 30, order: 3);
            second.Order = 99;

            Assert.IsTrue(Repo.Delete(second));

            CollectionAssert.AreEqual(new[] { 1, 2 }, StoredOrders());
        }

        [TestMethod]
        public void Delete_UnknownRnH_ReturnsFalseAndRaisesError()
        {
            DBSvc.Add(horseNo: 10, order: 1);

            Assert.IsFalse(Repo.Delete(new RnH { Id = 999 }));

            Assert.Contains("Reiter und Pferd mit Id 999 nicht gefunden.", Errors);
            CollectionAssert.AreEqual(new[] { 10 }, StoredHorseNos());
        }

        [TestMethod]
        public void Delete_ShiftFails_ReturnsFalseAndRaisesError()
        {
            RnH first = DBSvc.Add(horseNo: 10, order: 1);
            RnH second = DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.FailSaveForId = second.Id;

            Assert.IsFalse(Repo.Delete(first));

            Assert.Contains("Reihenfolge konnte nach dem Löschen nicht angepasst werden.", Errors);
        }

        [TestMethod]
        public void DeleteAll_RemovesAllRnHs()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            DBSvc.Add(horseNo: 20, order: 2);

            Assert.IsTrue(Repo.DeleteAll());

            Assert.IsEmpty(DBSvc.Stored);
        }

        [TestMethod]
        public void DeleteAll_OneDeleteFails_ReturnsFalseButDeletesOthers()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            RnH second = DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.Add(horseNo: 30, order: 3);
            DBSvc.FailDeleteForId = second.Id;

            Assert.IsFalse(Repo.DeleteAll());

            CollectionAssert.AreEqual(new[] { 20 }, StoredHorseNos());
        }

        [TestMethod]
        public void DeleteAll_ReadAllFails_ReturnsFalse()
        {
            DBSvc.FailReadAll = true;

            Assert.IsFalse(Repo.DeleteAll());
        }
    }
}
