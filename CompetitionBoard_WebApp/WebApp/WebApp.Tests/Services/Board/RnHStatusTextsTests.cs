using Base.Models.RnHs;
using WebApp.Services.Board;

namespace WebApp.Tests.Services.Board
{
    [TestClass]
    public class RnHStatusTextsTests
    {
        [TestMethod]
        [DataRow(RnHStatus.NotPresent, "Nicht da")]
        [DataRow(RnHStatus.OnWarmUpField, "Abreitplatz")]
        [DataRow(RnHStatus.OnPreparationField, "Vorbereitungsviereck")]
        [DataRow(RnHStatus.OnCompetitionField, "Prüfung")]
        [DataRow(RnHStatus.CompetitionDone, "fertig")]
        [DataRow((RnHStatus)99, "")]
        public void GetText_ReturnsText(RnHStatus status, string expected)
        {
            Assert.AreEqual(expected, RnHStatusTexts.GetText(status));
        }

        [TestMethod]
        [DataRow(RnHStatus.NotPresent, "images/dressageImgs/empty.jpg")]
        [DataRow(RnHStatus.OnWarmUpField, "images/dressageImgs/prepare.jpg")]
        [DataRow(RnHStatus.OnPreparationField, "images/dressageImgs/almost.jpg")]
        [DataRow(RnHStatus.OnCompetitionField, "images/dressageImgs/compesition.jpg")]
        [DataRow(RnHStatus.CompetitionDone, "images/dressageImgs/finished.jpg")]
        public void GetImage_ReturnsImage(RnHStatus status, string expected)
        {
            Assert.AreEqual(expected, RnHStatusTexts.GetImage(status));
        }

        [TestMethod]
        [DataRow(RnHStatus.NotPresent, "images/dressageImgs/empty.jpg")]
        [DataRow(RnHStatus.OnWarmUpField, "images/StatusImgs/prepare.png")]
        [DataRow(RnHStatus.OnPreparationField, "images/StatusImgs/wait.png")]
        [DataRow(RnHStatus.OnCompetitionField, "images/StatusImgs/play.png")]
        [DataRow(RnHStatus.CompetitionDone, "images/StatusImgs/index.png")]
        public void GetIcon_ReturnsIcon(RnHStatus status, string expected)
        {
            Assert.AreEqual(expected, RnHStatusTexts.GetIcon(status));
        }

        [TestMethod]
        public void AllImagesAndIcons_ExistInWwwroot()
        {
            string wwwroot = FindWwwroot();
            foreach (RnHStatus status in Enum.GetValues<RnHStatus>())
            {
                foreach (string path in new[] { RnHStatusTexts.GetImage(status), RnHStatusTexts.GetIcon(status) })
                {
                    Assert.IsTrue(File.Exists(Path.Combine(wwwroot, path)), $"{path} fehlt in wwwroot.");
                }
            }
        }

        private static string FindWwwroot()
        {
            DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "WebApp", "wwwroot");
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }
                dir = dir.Parent;
            }
            Assert.Fail("wwwroot der WebApp nicht gefunden.");
            return null;
        }
    }
}
