using Base.Models.RnHs;
using DBSvcs.Helpers;

namespace DBSvcs.Tests.Helpers
{
    [TestClass]
    public class RnHHelpersTests
    {
        [TestMethod]
        public void SetData_CopiesAllProperties()
        {
            RnH source = new RnH
            {
                Id = 7,
                Order = 3,
                HorseNo = 42,
                Mark = 7.5,
                Status = RnHStatus.CompetitionDone,
                IsDisqualificated = true,
                IsRanked = true
            };
            RnH target = new RnH();

            target.SetData(source);

            Assert.AreEqual(7, target.Id);
            Assert.AreEqual(3, target.Order);
            Assert.AreEqual(42, target.HorseNo);
            Assert.AreEqual(7.5, target.Mark);
            Assert.AreEqual(RnHStatus.CompetitionDone, target.Status);
            Assert.IsTrue(target.IsDisqualificated);
            Assert.IsTrue(target.IsRanked);
        }

        [TestMethod]
        public void SetData_DoesNotChangeSource()
        {
            RnH source = new RnH { Id = 1, HorseNo = 5 };
            RnH target = new RnH();

            target.SetData(source);
            target.HorseNo = 99;

            Assert.AreEqual(5, source.HorseNo);
        }
    }
}
