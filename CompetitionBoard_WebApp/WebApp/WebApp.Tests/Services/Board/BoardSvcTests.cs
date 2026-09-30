using Base.Models.DB;
using Base.Models.RnHs;
using Microsoft.Extensions.Options;
using WebApp.Configs;
using WebApp.Services.Board;
using WebApp.Tests.Fakes;

namespace WebApp.Tests.Services.Board
{
    [TestClass]
    public class BoardSvcTests
    {
        private FakeDBSvc _dbSvc;
        private FakeRnHsRepo _rnHsRepo;
        private FakeTitleRepo _titleRepo;
        private DBConnectionSettings _dbSettings;
        private BoardSvc _boardSvc;
        private int _changedCount;

        [TestInitialize]
        public void Init()
        {
            _dbSvc = new FakeDBSvc();
            _rnHsRepo = new FakeRnHsRepo(_dbSvc);
            _titleRepo = new FakeTitleRepo();
            _dbSettings = new DBConnectionSettings { Server = "Server", DB = "DB" };
            // Große Zeiten, damit der Timer während der Tests nicht auslöst.
            UIConfig uiConfig = new UIConfig { EditTime = 3600, RefreshTime = 3600 };
            _boardSvc = new BoardSvc(_dbSvc, _rnHsRepo, _titleRepo, Options.Create(_dbSettings), Options.Create(uiConfig));
            _boardSvc.Changed += () => _changedCount++;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _boardSvc.Dispose();
        }

        [TestMethod]
        [DataRow(7.5, "7,5")]
        [DataRow(8.0, "8,0")]
        [DataRow(7.26, "7,3")]
        public void FormatMark_UsesGermanFormatWithOneDecimal(double mark, string expected)
        {
            Assert.AreEqual(expected, BoardSvc.FormatMark(mark));
        }

        [TestMethod]
        [DataRow(7.25, "7,25")]
        [DataRow(8.0, "8")]
        public void FormatMarkForEdit_UsesGermanFormat(double mark, string expected)
        {
            Assert.AreEqual(expected, BoardSvc.FormatMarkForEdit(mark));
        }

        [TestMethod]
        public void StartAsync_SetsDBSettingsAndLoadsData()
        {
            _titleRepo.Title = "Dressur A";
            _rnHsRepo.RnHs = CreateRnHs(3);

            Start();

            Assert.AreSame(_dbSettings, _dbSvc.LastSettings);
            Assert.AreEqual("Dressur A", _boardSvc.Title);
            Assert.AreEqual(1, _boardSvc.RowCount);
            Assert.AreEqual(string.Empty, _boardSvc.ErrorMessage);
            Assert.AreEqual(1, _changedCount);
        }

        [TestMethod]
        public void StartAsync_CalledTwice_ConnectsOnlyOnce()
        {
            Start();
            Start();

            Assert.AreEqual(1, _dbSvc.SetDBSettingsCount);
        }

        [TestMethod]
        public void StartAsync_NoConnection_ShowsConnectionError()
        {
            _dbSvc.CanConnect = false;

            Start();

            Assert.AreEqual("Keine Verbindung zur Datenbank DB auf Server.", _boardSvc.ErrorMessage);
        }

        [TestMethod]
        public void StartAsync_NoConnectionWithErrorOfDBSvc_ShowsOnlyErrorOfDBSvc()
        {
            _dbSvc.CanConnect = false;
            _dbSvc.ConnectErrorMessage = "Login fehlgeschlagen";

            Start();

            Assert.AreEqual("Login fehlgeschlagen", _boardSvc.ErrorMessage);
        }

        [TestMethod]
        public void StartAsync_ReadAllThrows_ShowsExceptionMessage()
        {
            _rnHsRepo.ThrowOnReadAll = new InvalidOperationException("kaputt");

            Start();

            Assert.AreEqual("kaputt", _boardSvc.ErrorMessage);
        }

        [TestMethod]
        [DataRow(0, 0, new[] { 0, 0, 0, 0, 0 })]
        [DataRow(3, 1, new[] { 1, 1, 1, 0, 0 })]
        [DataRow(8, 2, new[] { 2, 2, 2, 2, 0 })]
        [DataRow(17, 5, new[] { 5, 5, 5, 2, 0 })]
        [DataRow(20, 5, new[] { 5, 5, 5, 5, 0 })]
        [DataRow(21, 5, new[] { 5, 5, 5, 5, 1 })]
        [DataRow(30, 6, new[] { 6, 6, 6, 6, 6 })]
        public void Columns_AreFilledColumnByColumn(int count, int expectedRowCount, int[] expectedColumnCounts)
        {
            _rnHsRepo.RnHs = CreateRnHs(count);

            Start();

            Assert.AreEqual(expectedRowCount, _boardSvc.RowCount);
            CollectionAssert.AreEqual(expectedColumnCounts, _boardSvc.Columns.Select(c => c.Count).ToList());
        }

        [TestMethod]
        public void Columns_AreSortedByOrder()
        {
            _rnHsRepo.RnHs = CreateRnHs(5);
            _rnHsRepo.RnHs.Reverse();

            Start();

            List<int> orders = _boardSvc.Columns.SelectMany(c => c).Select(r => r.Order).ToList();
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, orders);
        }

        [TestMethod]
        public void SaveTitleAsync_SavesAndReloadsTitle()
        {
            Start();

            _boardSvc.SaveTitleAsync("Neuer Titel").GetAwaiter().GetResult();

            Assert.AreEqual("Neuer Titel", _titleRepo.Title);
            Assert.AreEqual("Neuer Titel", _boardSvc.Title);
        }

        [TestMethod]
        public void AddNewListAsync_PassesListToRepo()
        {
            _boardSvc.AddNewListAsync("1, 2, 3").GetAwaiter().GetResult();

            AssertSingleCall(nameof(FakeRnHsRepo.SetNewRnHs), "1, 2, 3");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void AddNewListAsync_EmptyList_ShowsErrorWithoutCallingRepo(string list)
        {
            _boardSvc.AddNewListAsync(list).GetAwaiter().GetResult();

            Assert.AreEqual("Die Liste der Startnummern ist leer.", _boardSvc.ErrorMessage);
            Assert.IsEmpty(_rnHsRepo.Calls);
            Assert.AreEqual(1, _changedCount);
        }

        [TestMethod]
        [DataRow("7,5", 7.5)]
        [DataRow("7.5", 7.5)]
        [DataRow(" 8 ", 8.0)]
        public void SaveMarkAsync_ParsesMarkAndSavesCopy(string markStr, double expectedMark)
        {
            RnH rnh = new RnH { Id = 4, HorseNo = 12, Order = 2, Status = RnHStatus.CompetitionDone };

            _boardSvc.SaveMarkAsync(rnh, markStr).GetAwaiter().GetResult();

            RnH saved = AssertSingleCall<RnH>(nameof(FakeRnHsRepo.SaveMark));
            Assert.AreNotSame(rnh, saved);
            Assert.AreEqual(4, saved.Id);
            Assert.AreEqual(12, saved.HorseNo);
            Assert.AreEqual(expectedMark, saved.Mark);
            Assert.AreEqual(0.0, rnh.Mark);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("")]
        [DataRow(null)]
        public void SaveMarkAsync_InvalidMark_ShowsErrorWithoutCallingRepo(string markStr)
        {
            _boardSvc.SaveMarkAsync(new RnH(), markStr).GetAwaiter().GetResult();

            Assert.AreEqual("Die Note ist kein gültiger Zahlenwert", _boardSvc.ErrorMessage);
            Assert.IsEmpty(_rnHsRepo.Calls);
        }

        [TestMethod]
        public void SaveHorseNoAsync_ParsesHorseNoAndSavesCopy()
        {
            RnH rnh = new RnH { Id = 4, HorseNo = 12 };

            _boardSvc.SaveHorseNoAsync(rnh, " 33 ").GetAwaiter().GetResult();

            RnH saved = AssertSingleCall<RnH>(nameof(FakeRnHsRepo.SaveHorseNo));
            Assert.AreEqual(4, saved.Id);
            Assert.AreEqual(33, saved.HorseNo);
            Assert.AreEqual(12, rnh.HorseNo);
        }

        [TestMethod]
        [DataRow("x")]
        [DataRow("1,5")]
        [DataRow(null)]
        public void SaveHorseNoAsync_InvalidHorseNo_ShowsErrorWithoutCallingRepo(string horseNoStr)
        {
            _boardSvc.SaveHorseNoAsync(new RnH(), horseNoStr).GetAwaiter().GetResult();

            Assert.AreEqual("Die Startnummer ist kein gültiger Zahlenwert", _boardSvc.ErrorMessage);
            Assert.IsEmpty(_rnHsRepo.Calls);
        }

        [TestMethod]
        public void SaveStatusAsync_SavesCopyWithNewStatus()
        {
            RnH rnh = new RnH { Id = 4 };

            _boardSvc.SaveStatusAsync(rnh, RnHStatus.OnPreparationField).GetAwaiter().GetResult();

            RnH saved = AssertSingleCall<RnH>(nameof(FakeRnHsRepo.SaveStatus));
            Assert.AreEqual(RnHStatus.OnPreparationField, saved.Status);
            Assert.AreEqual(RnHStatus.NotPresent, rnh.Status);
        }

        [TestMethod]
        public void SaveIsRankedAsync_SavesCopyWithNewValue()
        {
            RnH rnh = new RnH { Id = 4 };

            _boardSvc.SaveIsRankedAsync(rnh, true).GetAwaiter().GetResult();

            Assert.IsTrue(AssertSingleCall<RnH>(nameof(FakeRnHsRepo.SaveIsRanked)).IsRanked);
            Assert.IsFalse(rnh.IsRanked);
        }

        [TestMethod]
        public void SaveIsDisqualificatedAsync_SavesCopyWithNewValue()
        {
            RnH rnh = new RnH { Id = 4 };

            _boardSvc.SaveIsDisqualificatedAsync(rnh, true).GetAwaiter().GetResult();

            Assert.IsTrue(AssertSingleCall<RnH>(nameof(FakeRnHsRepo.SaveIsDisqualificated)).IsDisqualificated);
            Assert.IsFalse(rnh.IsDisqualificated);
        }

        [TestMethod]
        public void AddPreviousAsync_InsertsAtOrderOfRnH()
        {
            _boardSvc.AddPreviousAsync(new RnH { Order = 3 }).GetAwaiter().GetResult();

            AssertSingleCall(nameof(FakeRnHsRepo.InsertNewWithOrder), 3);
        }

        [TestMethod]
        public void AddFollowingAsync_InsertsAfterRnH()
        {
            _boardSvc.AddFollowingAsync(new RnH { Order = 3 }).GetAwaiter().GetResult();

            AssertSingleCall(nameof(FakeRnHsRepo.InsertNewWithOrder), 4);
        }

        [TestMethod]
        public void DeleteAsync_DeletesCopy()
        {
            RnH rnh = new RnH { Id = 4 };

            _boardSvc.DeleteAsync(rnh).GetAwaiter().GetResult();

            RnH deleted = AssertSingleCall<RnH>(nameof(FakeRnHsRepo.Delete));
            Assert.AreNotSame(rnh, deleted);
            Assert.AreEqual(4, deleted.Id);
        }

        [TestMethod]
        public void SetDisplayOnly_EndsEditModus()
        {
            _boardSvc.SetEditModusAsync(true).GetAwaiter().GetResult();
            _boardSvc.SetEditListModus(true);

            _boardSvc.SetDisplayOnly();

            Assert.IsTrue(_boardSvc.IsDisplayOnly);
            Assert.IsFalse(_boardSvc.IsEditModus);
            Assert.IsFalse(_boardSvc.IsEditListModus);
        }

        [TestMethod]
        public void SetEditModusAsync_WhenDisplayOnly_StaysOff()
        {
            _boardSvc.SetDisplayOnly();

            _boardSvc.SetEditModusAsync(true).GetAwaiter().GetResult();
            _boardSvc.SetEditListModus(true);

            Assert.IsFalse(_boardSvc.IsEditModus);
            Assert.IsFalse(_boardSvc.IsEditListModus);
        }

        [TestMethod]
        public void UserAction_ReloadsDataAndRaisesChanged()
        {
            _boardSvc.SaveStatusAsync(new RnH(), RnHStatus.OnWarmUpField).GetAwaiter().GetResult();
            Assert.AreEqual(0, _boardSvc.RowCount);

            _rnHsRepo.RnHs = CreateRnHs(2);
            _boardSvc.SaveStatusAsync(new RnH(), RnHStatus.OnWarmUpField).GetAwaiter().GetResult();

            Assert.AreEqual(1, _boardSvc.RowCount);
            Assert.AreEqual(2, _changedCount);
        }

        [TestMethod]
        public void ErrorsOfRepoAndDBSvc_AreShownWithoutDuplicates()
        {
            _rnHsRepo.ErrorOnCall = "Note nicht erlaubt.";

            _boardSvc.SaveMarkAsync(new RnH(), "11").GetAwaiter().GetResult();
            _dbSvc.RaiseError("Note nicht erlaubt.");
            _dbSvc.RaiseError("Verbindung verloren");

            Assert.AreEqual($"Note nicht erlaubt.{Environment.NewLine}Verbindung verloren", _boardSvc.ErrorMessage);
        }

        [TestMethod]
        public void UserAction_ClearsPreviousErrors()
        {
            _boardSvc.SaveMarkAsync(new RnH(), "abc").GetAwaiter().GetResult();
            Assert.AreNotEqual(string.Empty, _boardSvc.ErrorMessage);

            _boardSvc.SaveMarkAsync(new RnH(), "7").GetAwaiter().GetResult();

            Assert.AreEqual(string.Empty, _boardSvc.ErrorMessage);
        }

        [TestMethod]
        public void SetEditListModus_WithoutEditModus_StaysFalse()
        {
            _boardSvc.SetEditListModus(true);

            Assert.IsFalse(_boardSvc.IsEditListModus);
        }

        [TestMethod]
        public void SetEditListModus_InEditModus_IsSet()
        {
            _boardSvc.SetEditModusAsync(true).GetAwaiter().GetResult();

            _boardSvc.SetEditListModus(true);

            Assert.IsTrue(_boardSvc.IsEditModus);
            Assert.IsTrue(_boardSvc.IsEditListModus);
        }

        [TestMethod]
        public void SetEditModusAsync_False_ResetsEditListModus()
        {
            _boardSvc.SetEditModusAsync(true).GetAwaiter().GetResult();
            _boardSvc.SetEditListModus(true);

            _boardSvc.SetEditModusAsync(false).GetAwaiter().GetResult();

            Assert.IsFalse(_boardSvc.IsEditModus);
            Assert.IsFalse(_boardSvc.IsEditListModus);
        }

        [TestMethod]
        public void Dispose_StopsErrorsAndChanged()
        {
            _boardSvc.Dispose();
            _changedCount = 0;

            _rnHsRepo.RaiseError("nach Dispose");
            _dbSvc.RaiseError("nach Dispose");
            _boardSvc.SaveStatusAsync(new RnH(), RnHStatus.OnWarmUpField).GetAwaiter().GetResult();

            Assert.AreEqual(string.Empty, _boardSvc.ErrorMessage);
            Assert.AreEqual(0, _changedCount);
            Assert.IsEmpty(_rnHsRepo.Calls);
        }

        private void Start()
        {
            _boardSvc.StartAsync().GetAwaiter().GetResult();
        }

        private void AssertSingleCall(string method, object expectedArg)
        {
            Assert.HasCount(1, _rnHsRepo.Calls);
            Assert.AreEqual(method, _rnHsRepo.Calls[0].Method);
            Assert.AreEqual(expectedArg, _rnHsRepo.Calls[0].Arg);
        }

        private T AssertSingleCall<T>(string method)
        {
            Assert.HasCount(1, _rnHsRepo.Calls);
            Assert.AreEqual(method, _rnHsRepo.Calls[0].Method);
            return (T)_rnHsRepo.Calls[0].Arg;
        }

        private static List<RnH> CreateRnHs(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new RnH { Id = i, HorseNo = 100 + i, Order = i })
                .ToList();
        }
    }
}
