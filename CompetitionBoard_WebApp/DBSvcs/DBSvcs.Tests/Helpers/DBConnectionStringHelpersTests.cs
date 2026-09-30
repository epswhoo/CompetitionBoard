using Base.Models.DB;
using DBSvcs.Helpers;
using Microsoft.Data.SqlClient;

namespace DBSvcs.Tests.Helpers
{
    [TestClass]
    public class DBConnectionStringHelpersTests
    {
        [TestMethod]
        public void GetConnectionString_SetsAllSettings()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = @"MYSERVER\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "user",
                Password = "secret"
            };

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(settings.GetConnectionString());

            Assert.AreEqual(@"MYSERVER\SQLEXPRESS", builder.DataSource);
            Assert.AreEqual("CompetitionBoardDB", builder.InitialCatalog);
            Assert.AreEqual("user", builder.UserID);
            Assert.AreEqual("secret", builder.Password);
            Assert.IsTrue(builder.TrustServerCertificate);
            Assert.AreEqual(5, builder.ConnectTimeout);
        }
    }
}
