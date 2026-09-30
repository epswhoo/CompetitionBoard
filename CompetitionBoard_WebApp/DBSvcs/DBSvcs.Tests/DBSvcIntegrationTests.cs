using Base.Models.DB;
using Base.Models.RnHs;
using DBSvcs.Helpers;
using DBSvcs.Models;
using Microsoft.EntityFrameworkCore;

namespace DBSvcs.Tests
{
    /// <summary>
    /// Integrationstests gegen einen echten SQL Server.
    /// Die Tests legen eine eigene Datenbank <see cref="TestDBName"/> an und löschen sie am Ende wieder.
    /// Sie laufen nur, wenn die Umgebungsvariablen COMPETITIONBOARD_TEST_DB_SERVER,
    /// COMPETITIONBOARD_TEST_DB_USER und COMPETITIONBOARD_TEST_DB_PASSWORD gesetzt sind,
    /// sonst sind sie Inconclusive. Der Benutzer braucht das Recht, Datenbanken anzulegen.
    /// </summary>
    [TestClass]
    [TestCategory("Integration")]
    public class DBSvcIntegrationTests
    {
        private const string TestDBName = "CompetitionBoardDB_Tests";

        private static DBConnectionSettings _settings;

        private DBSvc _dbSvc;
        private List<string> _errors;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _settings = ReadSettingsFromEnvironment();
            if (_settings == null)
            {
                return;
            }
            using CompetitionBoardDBContext dbContext = CreateDBContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_settings == null)
            {
                return;
            }
            using CompetitionBoardDBContext dbContext = CreateDBContext();
            dbContext.Database.EnsureDeleted();
        }

        [TestInitialize]
        public void Init()
        {
            if (_settings == null)
            {
                Assert.Inconclusive("Umgebungsvariablen für die Test-Datenbank sind nicht gesetzt.");
            }

            using (CompetitionBoardDBContext dbContext = CreateDBContext())
            {
                dbContext.RnHsTable.ExecuteDelete();
                dbContext.TitleTable.ExecuteDelete();
            }

            _dbSvc = new DBSvc();
            _errors = new List<string>();
            _dbSvc.ErrorRised += (_, message) => _errors.Add(message);
            Assert.IsTrue(_dbSvc.SetDBSettings(_settings), string.Join(Environment.NewLine, _errors));
        }

        [TestMethod]
        public void TitleLoad_WithoutTitle_ReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, _dbSvc.TitleLoad());
            Assert.IsEmpty(_errors);
        }

        [TestMethod]
        public void TitleSave_ThenTitleLoad_ReturnsSavedTitle()
        {
            Assert.AreEqual("Dressur A", _dbSvc.TitleSave("Dressur A"));
            Assert.AreEqual("Dressur B", _dbSvc.TitleSave("Dressur B"));

            Assert.AreEqual("Dressur B", _dbSvc.TitleLoad());
            Assert.IsEmpty(_errors);
        }

        [TestMethod]
        public void Insert_AssignsIdAndStoresData()
        {
            RnH rnh = new RnH { Id = 999, HorseNo = 12, Order = 1, Status = RnHStatus.OnWarmUpField };

            RnH inserted = _dbSvc.Insert(rnh);

            Assert.IsNotNull(inserted);
            Assert.AreNotEqual(999, inserted.Id);
            RnH read = _dbSvc.ReadById(inserted.Id);
            Assert.AreEqual(12, read.HorseNo);
            Assert.AreEqual(1, read.Order);
            Assert.AreEqual(RnHStatus.OnWarmUpField, read.Status);
            Assert.IsEmpty(_errors);
        }

        [TestMethod]
        public void Save_UpdatesStoredData()
        {
            RnH inserted = _dbSvc.Insert(new RnH { HorseNo = 1, Order = 1 });
            inserted.Status = RnHStatus.CompetitionDone;
            inserted.Mark = 7.5;
            inserted.IsRanked = true;

            _dbSvc.Save(inserted);

            RnH read = _dbSvc.ReadById(inserted.Id);
            Assert.AreEqual(RnHStatus.CompetitionDone, read.Status);
            Assert.AreEqual(7.5, read.Mark);
            Assert.IsTrue(read.IsRanked);
            Assert.IsEmpty(_errors);
        }

        [TestMethod]
        public void Delete_RemovesRnH()
        {
            RnH inserted = _dbSvc.Insert(new RnH { HorseNo = 1, Order = 1 });

            Assert.IsTrue(_dbSvc.Delete(inserted));

            Assert.IsEmpty(_dbSvc.ReadAll());
        }

        [TestMethod]
        public void ReadAll_ReturnsRnHsOrderedByOrder()
        {
            _dbSvc.Insert(new RnH { HorseNo = 30, Order = 3 });
            _dbSvc.Insert(new RnH { HorseNo = 10, Order = 1 });
            _dbSvc.Insert(new RnH { HorseNo = 20, Order = 2 });

            List<int> horseNos = _dbSvc.ReadAll().Select(r => r.HorseNo).ToList();

            CollectionAssert.AreEqual(new[] { 10, 20, 30 }, horseNos);
        }

        [TestMethod]
        public void ReadById_UnknownId_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.ReadById(12345));

            Assert.HasCount(1, _errors);
            Assert.Contains("12345", _errors[0]);
        }

        private static CompetitionBoardDBContext CreateDBContext()
        {
            return new CompetitionBoardDBContext(
                new DbContextOptionsBuilder<CompetitionBoardDBContext>()
                    .UseSqlServer(_settings.GetConnectionString())
                    .Options);
        }

        private static DBConnectionSettings ReadSettingsFromEnvironment()
        {
            string server = Environment.GetEnvironmentVariable("COMPETITIONBOARD_TEST_DB_SERVER");
            string user = Environment.GetEnvironmentVariable("COMPETITIONBOARD_TEST_DB_USER");
            string password = Environment.GetEnvironmentVariable("COMPETITIONBOARD_TEST_DB_PASSWORD");
            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(user) || password == null)
            {
                return null;
            }
            return new DBConnectionSettings
            {
                Server = server,
                DB = TestDBName,
                Username = user,
                Password = password
            };
        }
    }
}
