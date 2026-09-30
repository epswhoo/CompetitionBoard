using System.Text.Json.Nodes;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApp.Components.Views;
using WebApp.Configs;
using WebApp.Services.Login;
using WebApp.Tests.Fakes;

namespace WebApp.Tests.Components
{
    [TestClass]
    public class ChangePasswordViewTests
    {
        private BunitContext _context;
        private string _settingsFile;

        [TestInitialize]
        public void Init()
        {
            _settingsFile = Path.Combine(Path.GetTempPath(), $"appsettings_{Guid.NewGuid():N}.json");
            File.WriteAllText(_settingsFile, "{ \"UI\": { \"Password\": \"alt\", \"AdminPassword\": \"admin\" } }");

            _context = new BunitContext();
            _context.Services.AddSingleton<IOptionsMonitor<UIConfig>>(
                new FakeOptionsMonitor<UIConfig>(new UIConfig { Password = "alt", AdminPassword = "admin" }));
            _context.Services.AddSingleton(new PasswordStore(_settingsFile));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
            File.Delete(_settingsFile);
        }

        private IRenderedComponent<ChangePasswordView> RenderOpened()
        {
            IRenderedComponent<ChangePasswordView> cut = _context.Render<ChangePasswordView>();
            cut.Find(".image-button").Click();
            return cut;
        }

        private static void Save(IRenderedComponent<ChangePasswordView> cut, string password, string repeatedPassword)
        {
            cut.Find("#new-password").Input(password);
            cut.Find("#repeated-password").Input(repeatedPassword);
            cut.Find(".change-password-popup").Submit();
        }

        private string SavedPassword()
        {
            return (string)JsonNode.Parse(File.ReadAllText(_settingsFile))["UI"]["Password"];
        }

        [TestMethod]
        public void Render_IsClosed()
        {
            IRenderedComponent<ChangePasswordView> cut = _context.Render<ChangePasswordView>();

            Assert.AreEqual(0, cut.FindAll(".change-password-popup").Count);
        }

        [TestMethod]
        public void Click_OpensAndClosesPopup()
        {
            IRenderedComponent<ChangePasswordView> cut = RenderOpened();
            Assert.AreEqual(1, cut.FindAll(".change-password-popup").Count);

            cut.Find(".image-button").Click();

            Assert.AreEqual(0, cut.FindAll(".change-password-popup").Count);
        }

        [TestMethod]
        public void Save_WithValidPassword_StoresItAndShowsSuccess()
        {
            IRenderedComponent<ChangePasswordView> cut = RenderOpened();

            Save(cut, "neu", "neu");

            Assert.AreEqual("Das Passwort wurde geändert.", cut.Find(".change-password-message").TextContent);
            Assert.AreEqual("neu", SavedPassword());
        }

        [TestMethod]
        [DataRow("neu", "anders", "Die Passwörter stimmen nicht überein.")]
        [DataRow("", "", "Das Passwort darf nicht leer sein.")]
        [DataRow("admin", "admin", "Das Passwort darf nicht dem Admin-Passwort entsprechen.")]
        public void Save_WithInvalidPassword_ShowsErrorAndKeepsOldPassword(string password, string repeatedPassword, string expected)
        {
            IRenderedComponent<ChangePasswordView> cut = RenderOpened();

            Save(cut, password, repeatedPassword);

            AngleSharp.Dom.IElement message = cut.Find(".change-password-message");
            Assert.AreEqual(expected, message.TextContent);
            Assert.IsTrue(message.ClassList.Contains("error"));
            Assert.AreEqual("alt", SavedPassword());
        }

        [TestMethod]
        public void Close_ClearsMessage()
        {
            IRenderedComponent<ChangePasswordView> cut = RenderOpened();
            Save(cut, "neu", "anders");

            cut.Find(".change-password-buttons button[type=button]").Click();
            cut.Find(".image-button").Click();

            Assert.AreEqual(0, cut.FindAll(".change-password-message").Count);
        }
    }
}
