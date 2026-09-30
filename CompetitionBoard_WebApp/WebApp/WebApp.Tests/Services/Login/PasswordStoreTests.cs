using System.Text.Json.Nodes;
using WebApp.Services.Login;

namespace WebApp.Tests.Services.Login
{
    [TestClass]
    public class PasswordStoreTests
    {
        private string _settingsFile;

        [TestInitialize]
        public void Initialize()
        {
            _settingsFile = Path.Combine(Path.GetTempPath(), $"appsettings_{Guid.NewGuid():N}.json");
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(_settingsFile);
        }

        [TestMethod]
        public void SavePassword_ReplacesOnlyUIPassword()
        {
            File.WriteAllText(_settingsFile, """
                {
                  "Db": { "Password": "0000" },
                  "UI": { "RefreshTime": 2, "Password": "alt", "AdminPassword": "admin" }
                }
                """);

            new PasswordStore(_settingsFile).SavePassword("neu");

            JsonNode root = JsonNode.Parse(File.ReadAllText(_settingsFile));
            Assert.AreEqual("neu", (string)root["UI"]["Password"]);
            Assert.AreEqual("admin", (string)root["UI"]["AdminPassword"]);
            Assert.AreEqual(2, (int)root["UI"]["RefreshTime"]);
            Assert.AreEqual("0000", (string)root["Db"]["Password"]);
        }

        [TestMethod]
        public void SavePassword_WithoutUISection_CreatesIt()
        {
            File.WriteAllText(_settingsFile, "{ \"Db\": {} }");

            new PasswordStore(_settingsFile).SavePassword("neu");

            JsonNode root = JsonNode.Parse(File.ReadAllText(_settingsFile));
            Assert.AreEqual("neu", (string)root["UI"]["Password"]);
        }

        [TestMethod]
        public void SavePassword_KeepsSpecialCharactersReadable()
        {
            File.WriteAllText(_settingsFile, "{ \"UI\": {} }");

            new PasswordStore(_settingsFile).SavePassword("Größe&<>");

            StringAssert.Contains(File.ReadAllText(_settingsFile), "Größe&<>");
        }
    }
}
