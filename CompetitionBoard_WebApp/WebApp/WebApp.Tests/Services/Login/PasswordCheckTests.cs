using WebApp.Configs;
using WebApp.Services.Login;

namespace WebApp.Tests.Services.Login
{
    [TestClass]
    public class PasswordCheckTests
    {
        private static UIConfig CreateConfig(string password, string adminPassword = "admin", int displayPort = 5081)
        {
            return new UIConfig { Password = password, AdminPassword = adminPassword, DisplayPort = displayPort };
        }

        [TestMethod]
        public void IsDisplayPort_OnDisplayPort_IsTrue()
        {
            Assert.IsTrue(PasswordCheck.IsDisplayPort(CreateConfig("geheim"), 5081));
        }

        [TestMethod]
        public void IsDisplayPort_OnControlPort_IsFalse()
        {
            Assert.IsFalse(PasswordCheck.IsDisplayPort(CreateConfig("geheim"), 5080));
        }

        [TestMethod]
        public void IsDisplayPort_WhenDisplayPortDisabled_IsFalse()
        {
            Assert.IsFalse(PasswordCheck.IsDisplayPort(CreateConfig("geheim", displayPort: 0), 0));
        }

        [TestMethod]
        [DataRow("geheim", "")]
        [DataRow("", "admin")]
        [DataRow("geheim", "admin")]
        public void IsLockEnabled_WithAnyPassword_IsTrue(string password, string adminPassword)
        {
            Assert.IsTrue(PasswordCheck.IsLockEnabled(CreateConfig(password, adminPassword)));
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        public void IsLockEnabled_WithoutPasswords_IsFalse(string password, string adminPassword)
        {
            Assert.IsFalse(PasswordCheck.IsLockEnabled(CreateConfig(password, adminPassword)));
        }

        [TestMethod]
        public void GetAccessLevel_WithPassword_IsOperator()
        {
            Assert.AreEqual(AccessLevel.Operator, PasswordCheck.GetAccessLevel(CreateConfig("geheim"), "geheim"));
        }

        [TestMethod]
        public void GetAccessLevel_WithAdminPassword_IsAdmin()
        {
            Assert.AreEqual(AccessLevel.Admin, PasswordCheck.GetAccessLevel(CreateConfig("geheim"), "admin"));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("Geheim")]
        [DataRow("geheim ")]
        public void GetAccessLevel_WithOtherPassword_IsLocked(string password)
        {
            Assert.AreEqual(AccessLevel.Locked, PasswordCheck.GetAccessLevel(CreateConfig("geheim"), password));
        }

        [TestMethod]
        public void GetAccessLevel_WithEmptyInputAndEmptyPasswords_IsLocked()
        {
            Assert.AreEqual(AccessLevel.Locked, PasswordCheck.GetAccessLevel(CreateConfig("", ""), ""));
        }

        [TestMethod]
        public void ValidateNewPassword_WithValidPassword_ReturnsNull()
        {
            Assert.IsNull(PasswordCheck.ValidateNewPassword(CreateConfig("geheim"), "neu", "neu"));
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("neu", "anders")]
        [DataRow("admin", "admin")]
        public void ValidateNewPassword_WithInvalidPassword_ReturnsMessage(string password, string repeatedPassword)
        {
            Assert.IsNotNull(PasswordCheck.ValidateNewPassword(CreateConfig("geheim"), password, repeatedPassword));
        }
    }
}
