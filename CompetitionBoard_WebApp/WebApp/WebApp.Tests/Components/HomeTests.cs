using Base.Models.DB;
using Bunit;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApp.Components.Pages;
using WebApp.Configs;
using WebApp.Services.Board;
using WebApp.Services.Login;
using WebApp.Tests.Fakes;

namespace WebApp.Tests.Components
{
    [TestClass]
    public class HomeTests
    {
        private BunitContext _context;
        private FakeDBSvc _dbSvc;
        private UIConfig _uiConfig;
        private string _settingsFile;

        [TestInitialize]
        public void Init()
        {
            _settingsFile = Path.Combine(Path.GetTempPath(), $"appsettings_{Guid.NewGuid():N}.json");
            File.WriteAllText(_settingsFile, "{ \"UI\": {} }");

            // Große Zeiten, damit der Timer während der Tests nicht auslöst.
            _uiConfig = new UIConfig { EditTime = 3600, RefreshTime = 3600, Password = "geheim", AdminPassword = "admin" };
            _dbSvc = new FakeDBSvc();
            FakeRnHsRepo rnHsRepo = new FakeRnHsRepo(_dbSvc);

            _context = new BunitContext();
            _context.JSInterop.Mode = JSRuntimeMode.Loose;
            _context.Services.AddSingleton<IDBSvc>(_dbSvc);
            _context.Services.AddSingleton<IRnHsRepo>(rnHsRepo);
            _context.Services.AddSingleton<ITitleRepo>(new FakeTitleRepo());
            _context.Services.AddSingleton(Options.Create(new DBConnectionSettings { Server = "Server", DB = "DB" }));
            _context.Services.AddSingleton(Options.Create(_uiConfig));
            _context.Services.AddSingleton<IOptionsMonitor<UIConfig>>(new FakeOptionsMonitor<UIConfig>(_uiConfig));
            _context.Services.AddSingleton(new PasswordStore(_settingsFile));
            _context.Services.AddScoped<BoardSvc>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
            File.Delete(_settingsFile);
        }

        private IRenderedComponent<Home> Render(bool isDisplayOnly = false)
        {
            return _context.Render<Home>(parameters => parameters.AddCascadingValue("IsDisplayOnly", isDisplayOnly));
        }

        private static void Login(IRenderedComponent<Home> cut, string password)
        {
            cut.Find("#login-password").Input(password);
            cut.Find(".login-form").Submit();
        }

        private static List<string> HeaderButtonTitles(IRenderedComponent<Home> cut)
        {
            return cut.FindAll(".board-header-buttons [title]").Select(e => e.GetAttribute("title")).ToList();
        }

        [TestMethod]
        public void Render_WithPassword_ShowsLoginAndDoesNotStartBoard()
        {
            IRenderedComponent<Home> cut = Render();

            Assert.AreEqual(1, cut.FindAll(".login-view").Count);
            Assert.AreEqual(0, cut.FindAll(".board").Count);
            Assert.AreEqual(0, _dbSvc.SetDBSettingsCount);
        }

        [TestMethod]
        public void Login_WithWrongPassword_ShowsErrorAndStaysLocked()
        {
            IRenderedComponent<Home> cut = Render();

            Login(cut, "falsch");

            cut.WaitForAssertion(() => Assert.AreEqual("Das Passwort ist falsch.", cut.Find(".login-error").TextContent),
                TimeSpan.FromSeconds(5));
            Assert.AreEqual(0, cut.FindAll(".board").Count);
            Assert.AreEqual(0, _dbSvc.SetDBSettingsCount);
        }

        [TestMethod]
        public void Login_WithPassword_ShowsBoardWithoutChangePassword()
        {
            IRenderedComponent<Home> cut = Render();

            Login(cut, "geheim");

            cut.WaitForAssertion(() => Assert.AreEqual(1, cut.FindAll(".board").Count));
            List<string> titles = HeaderButtonTitles(cut);
            CollectionAssert.Contains(titles, "Bearbeiten");
            CollectionAssert.DoesNotContain(titles, "Passwort ändern");
            Assert.AreEqual(1, _dbSvc.SetDBSettingsCount);
        }

        [TestMethod]
        public void Login_WithAdminPassword_ShowsChangePassword()
        {
            IRenderedComponent<Home> cut = Render();

            Login(cut, "admin");

            cut.WaitForAssertion(() => Assert.AreEqual(1, cut.FindAll(".board").Count));
            List<string> titles = HeaderButtonTitles(cut);
            CollectionAssert.Contains(titles, "Bearbeiten");
            CollectionAssert.Contains(titles, "Passwort ändern");
        }

        [TestMethod]
        public void Render_OnDisplayPort_ShowsBoardWithoutLoginAndButtons()
        {
            IRenderedComponent<Home> cut = Render(isDisplayOnly: true);

            Assert.AreEqual(0, cut.FindAll(".login-view").Count);
            Assert.AreEqual(1, cut.FindAll(".board").Count);
            Assert.AreEqual(0, HeaderButtonTitles(cut).Count);
            Assert.IsTrue(_context.Services.GetRequiredService<BoardSvc>().IsDisplayOnly);
        }

        [TestMethod]
        public void Render_WithoutPasswords_ShowsBoardWithoutLogin()
        {
            _uiConfig.Password = "";
            _uiConfig.AdminPassword = "";

            IRenderedComponent<Home> cut = Render();

            Assert.AreEqual(0, cut.FindAll(".login-view").Count);
            Assert.AreEqual(1, cut.FindAll(".board").Count);
            CollectionAssert.DoesNotContain(HeaderButtonTitles(cut), "Passwort ändern");
        }

        [TestMethod]
        public void Render_AgainAfterLogin_AsksForPasswordAgain()
        {
            IRenderedComponent<Home> first = Render();
            Login(first, "geheim");
            first.WaitForAssertion(() => Assert.AreEqual(1, first.FindAll(".board").Count));

            // Ein neuer Seitenaufruf erzeugt eine neue Komponente, die wieder gesperrt beginnt.
            IRenderedComponent<Home> second = Render();

            Assert.AreEqual(1, second.FindAll(".login-view").Count);
        }
    }
}
