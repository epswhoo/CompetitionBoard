using Base.Models.RnHs;

namespace DBSvcs.Tests
{
    /// <summary>
    /// Ohne gesetzte Datenbankeinstellungen darf der DBSvc keine Exception werfen,
    /// sondern muss den Fehler über ErrorRised melden und default zurückgeben.
    /// </summary>
    [TestClass]
    public class DBSvcWithoutSettingsTests
    {
        private const string NoSettingsMessage = "Es sind keine Datenbankeinstellungen gesetzt.";

        private DBSvc _dbSvc;
        private List<string> _errors;

        [TestInitialize]
        public void Init()
        {
            _dbSvc = new DBSvc();
            _errors = new List<string>();
            _dbSvc.ErrorRised += (_, message) => _errors.Add(message);
        }

        [TestMethod]
        public void CheckConnection_ReturnsFalseAndRaisesError()
        {
            Assert.IsFalse(_dbSvc.CheckConnection());
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void TitleSave_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.TitleSave("Titel"));
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void TitleLoad_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.TitleLoad());
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void Insert_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.Insert(new RnH()));
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void Save_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.Save(new RnH { Id = 1 }));
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void Delete_ReturnsFalseAndRaisesError()
        {
            Assert.IsFalse(_dbSvc.Delete(new RnH { Id = 1 }));
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void ReadById_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.ReadById(1));
            AssertSingleNoSettingsError();
        }

        [TestMethod]
        public void ReadAll_ReturnsNullAndRaisesError()
        {
            Assert.IsNull(_dbSvc.ReadAll());
            AssertSingleNoSettingsError();
        }

        private void AssertSingleNoSettingsError()
        {
            Assert.HasCount(1, _errors);
            Assert.AreEqual(NoSettingsMessage, _errors[0]);
        }
    }
}
