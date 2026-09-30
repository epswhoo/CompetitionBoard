using Base.Models.RnHs;

namespace Repos.Tests.RnHs
{
    [TestClass]
    public class SaveTests : RnHsRepoTestBase
    {
        private const string MarkNotDoneError = "Note kann nicht eingetragen werden, da Reiter mit diesem Pferd die Prüfung noch nicht abgeschlossen hat.";
        private const string MarkDisqualifiedError = "Note kann nicht eingetragen werden, da Reiter mit diesem Pferd in dieser Prüfung disqualifiziert sind.";
        private const string DisqualifiedNotDoneError = "Reiter mit Pferd kann nicht disqualifiziert werden, da Reiter mit diesem Pferd die Prüfung noch nicht abgeschlossen hat.";
        private const string DisqualifiedRankedError = "Reiter mit Pferd kann nicht disqualifiziert werden, da Reiter mit diesem Pferd in dieser Prüfung platziert wurden.";
        private const string RankedWithoutMarkError = "Reiter mit Pferd kann nicht platziert werden, da Reiter mit diesem Pferd in dieser Prüfung nicht bewertet wurden.";

        [TestMethod]
        [DataRow(RnHStatus.NotPresent, 0.0, false, false)]
        [DataRow(RnHStatus.OnCompetitionField, 0.0, false, false)]
        [DataRow(RnHStatus.CompetitionDone, 0.0, false, false)]
        [DataRow(RnHStatus.CompetitionDone, 10.0, false, false)]
        [DataRow(RnHStatus.CompetitionDone, 7.5, false, true)]
        [DataRow(RnHStatus.CompetitionDone, 0.0, true, false)]
        public void Save_ValidRnH_IsSaved(RnHStatus status, double mark, bool isDisqualificated, bool isRanked)
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1);
            stored.Status = status;
            stored.Mark = mark;
            stored.IsDisqualificated = isDisqualificated;
            stored.IsRanked = isRanked;

            RnH result = Repo.Save(stored);

            Assert.IsNotNull(result);
            Assert.IsEmpty(Errors);
            RnH saved = DBSvc.Stored.Single();
            Assert.AreEqual(status, saved.Status);
            Assert.AreEqual(mark, saved.Mark);
            Assert.AreEqual(isDisqualificated, saved.IsDisqualificated);
            Assert.AreEqual(isRanked, saved.IsRanked);
        }

        [TestMethod]
        [DataRow(RnHStatus.OnCompetitionField, 7.0, false, false, MarkNotDoneError)]
        [DataRow(RnHStatus.CompetitionDone, 7.0, true, false, MarkDisqualifiedError)]
        [DataRow(RnHStatus.OnCompetitionField, 0.0, true, false, DisqualifiedNotDoneError)]
        [DataRow(RnHStatus.CompetitionDone, 7.0, true, true, DisqualifiedRankedError)]
        [DataRow(RnHStatus.CompetitionDone, 0.0, false, true, RankedWithoutMarkError)]
        public void Save_InvalidRnH_ReturnsNullAndRaisesError(RnHStatus status, double mark, bool isDisqualificated,
            bool isRanked, string expectedError)
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1);
            RnH toSave = new RnH
            {
                Id = stored.Id,
                HorseNo = 10,
                Order = 1,
                Status = status,
                Mark = mark,
                IsDisqualificated = isDisqualificated,
                IsRanked = isRanked
            };

            Assert.IsNull(Repo.Save(toSave));

            Assert.Contains(expectedError, Errors);
            Assert.AreEqual(RnHStatus.NotPresent, DBSvc.Stored.Single().Status);
        }

        [TestMethod]
        [DataRow(-0.1)]
        [DataRow(10.1)]
        public void Save_MarkOutOfRange_ReturnsNullAndRaisesError(double mark)
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone);
            stored.Mark = mark;

            Assert.IsNull(Repo.Save(stored));

            Assert.Contains($"Note {mark} nicht erlaubt.", Errors);
            Assert.AreEqual(0.0, DBSvc.Stored.Single().Mark);
        }

        [TestMethod]
        public void SaveMark_UpdatesOnlyMark()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone);
            RnH toSave = new RnH { Id = stored.Id, HorseNo = 99, Order = 5, Mark = 8.2 };

            RnH result = Repo.SaveMark(toSave);

            Assert.AreEqual(8.2, result.Mark);
            RnH saved = DBSvc.Stored.Single();
            Assert.AreEqual(8.2, saved.Mark);
            Assert.AreEqual(10, saved.HorseNo);
            Assert.AreEqual(1, saved.Order);
            Assert.AreEqual(RnHStatus.CompetitionDone, saved.Status);
        }

        [TestMethod]
        public void SaveMark_CompetitionNotDone_ReturnsNullAndKeepsMark()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.OnCompetitionField);

            Assert.IsNull(Repo.SaveMark(new RnH { Id = stored.Id, Mark = 7.0, Status = RnHStatus.CompetitionDone }));

            CollectionAssert.AreEqual(new[] { MarkNotDoneError }, Errors);
            Assert.AreEqual(0.0, DBSvc.Stored.Single().Mark);
        }

        [TestMethod]
        public void SaveHorseNo_UpdatesOnlyHorseNo()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone, mark: 7.0);

            Repo.SaveHorseNo(new RnH { Id = stored.Id, HorseNo = 55 });

            RnH saved = DBSvc.Stored.Single();
            Assert.AreEqual(55, saved.HorseNo);
            Assert.AreEqual(7.0, saved.Mark);
            Assert.AreEqual(RnHStatus.CompetitionDone, saved.Status);
        }

        [TestMethod]
        public void SaveStatus_UpdatesOnlyStatus()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1);

            Repo.SaveStatus(new RnH { Id = stored.Id, HorseNo = 55, Status = RnHStatus.OnWarmUpField });

            RnH saved = DBSvc.Stored.Single();
            Assert.AreEqual(RnHStatus.OnWarmUpField, saved.Status);
            Assert.AreEqual(10, saved.HorseNo);
        }

        [TestMethod]
        public void SaveStatus_BackFromDoneWithMark_ReturnsNull()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone, mark: 7.0);

            Assert.IsNull(Repo.SaveStatus(new RnH { Id = stored.Id, Status = RnHStatus.OnCompetitionField }));

            CollectionAssert.AreEqual(new[] { MarkNotDoneError }, Errors);
            Assert.AreEqual(RnHStatus.CompetitionDone, DBSvc.Stored.Single().Status);
        }

        [TestMethod]
        public void SaveIsRanked_WithMark_IsSaved()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone, mark: 7.0);

            Repo.SaveIsRanked(new RnH { Id = stored.Id, IsRanked = true });

            Assert.IsTrue(DBSvc.Stored.Single().IsRanked);
            Assert.IsEmpty(Errors);
        }

        [TestMethod]
        public void SaveIsRanked_WithoutMark_ReturnsNull()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone);

            Assert.IsNull(Repo.SaveIsRanked(new RnH { Id = stored.Id, IsRanked = true, Mark = 7.0 }));

            CollectionAssert.AreEqual(new[] { RankedWithoutMarkError }, Errors);
            Assert.IsFalse(DBSvc.Stored.Single().IsRanked);
        }

        [TestMethod]
        public void SaveIsDisqualificated_CompetitionDoneWithoutMark_IsSaved()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone);

            Repo.SaveIsDisqualificated(new RnH { Id = stored.Id, IsDisqualificated = true });

            Assert.IsTrue(DBSvc.Stored.Single().IsDisqualificated);
            Assert.IsEmpty(Errors);
        }

        [TestMethod]
        public void SaveIsDisqualificated_Ranked_ReturnsNull()
        {
            RnH stored = DBSvc.Add(horseNo: 10, order: 1, status: RnHStatus.CompetitionDone, mark: 7.0, isRanked: true);

            Assert.IsNull(Repo.SaveIsDisqualificated(new RnH { Id = stored.Id, IsDisqualificated = true }));

            Assert.Contains(DisqualifiedRankedError, Errors);
            Assert.IsFalse(DBSvc.Stored.Single().IsDisqualificated);
        }

        [TestMethod]
        public void SaveMark_UnknownRnH_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(Repo.SaveMark(new RnH { Id = 999, Mark = 7.0 }));

            Assert.Contains("Reiter und Pferd mit Id 999 nicht gefunden.", Errors);
        }

        [TestMethod]
        public void SaveMark_DBSvcThrows_ReturnsNullAndRaisesError()
        {
            DBSvc.ThrowOnReadById = new InvalidOperationException("DB weg");

            Assert.IsNull(Repo.SaveMark(new RnH { Id = 1, Mark = 7.0 }));

            CollectionAssert.AreEqual(new[] { "DB weg" }, Errors);
        }

        [TestMethod]
        public void ErrorOfDBSvc_IsForwarded()
        {
            DBSvc.RaiseError("Verbindung verloren");

            CollectionAssert.AreEqual(new[] { "Verbindung verloren" }, Errors);
        }

        [TestMethod]
        public void ReadAll_ReturnsRnHsOfDBSvc()
        {
            DBSvc.Add(horseNo: 20, order: 2);
            DBSvc.Add(horseNo: 10, order: 1);

            CollectionAssert.AreEqual(new[] { 10, 20 }, Repo.ReadAll().Select(r => r.HorseNo).ToList());
        }
    }
}
