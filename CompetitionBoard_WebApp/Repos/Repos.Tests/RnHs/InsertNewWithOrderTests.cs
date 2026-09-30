using Base.Models.RnHs;

namespace Repos.Tests.RnHs
{
    [TestClass]
    public class InsertNewWithOrderTests : RnHsRepoTestBase
    {
        [TestMethod]
        public void InsertNewWithOrder_EmptyList_InsertsAtFirstPosition()
        {
            RnH inserted = Repo.InsertNewWithOrder(1);

            Assert.IsNotNull(inserted);
            Assert.AreEqual(1, inserted.Order);
            Assert.HasCount(1, DBSvc.Stored);
            Assert.IsEmpty(Errors);
        }

        [TestMethod]
        public void InsertNewWithOrder_InTheMiddle_ShiftsFollowingRnHs()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.Add(horseNo: 30, order: 3);

            RnH inserted = Repo.InsertNewWithOrder(2);

            Assert.AreEqual(2, inserted.Order);
            CollectionAssert.AreEqual(new[] { 10, 0, 20, 30 }, StoredHorseNos());
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, StoredOrders());
            Assert.IsEmpty(Errors);
        }

        [TestMethod]
        public void InsertNewWithOrder_AtTheEnd_AppendsRnH()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            DBSvc.Add(horseNo: 20, order: 2);

            RnH inserted = Repo.InsertNewWithOrder(3);

            Assert.AreEqual(3, inserted.Order);
            CollectionAssert.AreEqual(new[] { 10, 20, 0 }, StoredHorseNos());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, StoredOrders());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(4)]
        public void InsertNewWithOrder_InvalidPosition_ReturnsNullAndRaisesError(int order)
        {
            DBSvc.Add(horseNo: 10, order: 1);
            DBSvc.Add(horseNo: 20, order: 2);

            Assert.IsNull(Repo.InsertNewWithOrder(order));

            CollectionAssert.AreEqual(new[] { $"Position {order} ist nicht erlaubt." }, Errors);
            CollectionAssert.AreEqual(new[] { 10, 20 }, StoredHorseNos());
        }

        [TestMethod]
        public void InsertNewWithOrder_ShiftFails_ReturnsNullAndRaisesError()
        {
            DBSvc.Add(horseNo: 10, order: 1);
            RnH second = DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.FailSaveForId = second.Id;

            Assert.IsNull(Repo.InsertNewWithOrder(1));

            Assert.Contains("Reihenfolge konnte vor dem Einfügen an Position 1 nicht angepasst werden.", Errors);
        }

        [TestMethod]
        public void InsertNewWithOrder_ReadAllFails_ReturnsNull()
        {
            DBSvc.FailReadAll = true;

            Assert.IsNull(Repo.InsertNewWithOrder(1));

            Assert.Contains("ReadAll fehlgeschlagen.", Errors);
        }
    }
}
