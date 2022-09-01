using Microsoft.Data.SqlClient;
using Models.IDBSvc;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DBConnectionSettings settings = new DBConnectionSettings
            {
                Server = "DESKTOP-34KSSOT\\SQLEXPRESS",
                DB = "CompetitionBoardDB",
                Username = "CompetitionBoard",
                Password = "Tafeltafel0#"
            };
            string cnnString = $"Data Source={settings.Server};" +
                $"Initial Catalog={settings.DB};" +
                "TrustServerCertificate=True;" +
                $"User ID={settings.Username};" +
                $"Password={settings.Password}";
            //_dc = new DataContext(connectionString);
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
            cnnString);

            // Display the connection string, which should now
            // contain the "Data Source" key, as opposed to the
            // supplied "Network Address".
            Console.WriteLine(builder.ConnectionString);

            // Retrieve the DataSource property.
            Console.WriteLine("DataSource = " + builder.DataSource);

            DataContext dc = new System.Data.Linq.DataContext(builder.ConnectionString);
        }
    }
}