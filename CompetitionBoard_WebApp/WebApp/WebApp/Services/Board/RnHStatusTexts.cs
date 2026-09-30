using Base.Models.RnHs;

namespace WebApp.Services.Board
{
    public static class RnHStatusTexts
    {
        public static string GetText(RnHStatus status)
        {
            switch (status)
            {
                case RnHStatus.NotPresent:
                    return "Nicht da";
                case RnHStatus.OnWarmUpField:
                    return "Abreitplatz";
                case RnHStatus.OnPreparationField:
                    return "Vorbereitungsviereck";
                case RnHStatus.OnCompetitionField:
                    return "Prüfung";
                case RnHStatus.CompetitionDone:
                    return "fertig";
                default:
                    return string.Empty;
            }
        }

        public static string GetImage(RnHStatus status)
        {
            switch (status)
            {
                case RnHStatus.OnWarmUpField:
                    return "images/dressageImgs/prepare.jpg";
                case RnHStatus.OnPreparationField:
                    return "images/dressageImgs/almost.jpg";
                case RnHStatus.OnCompetitionField:
                    return "images/dressageImgs/compesition.jpg";
                case RnHStatus.CompetitionDone:
                    return "images/dressageImgs/finished.jpg";
                default:
                    return "images/dressageImgs/empty.jpg";
            }
        }

        public static string GetIcon(RnHStatus status)
        {
            switch (status)
            {
                case RnHStatus.OnWarmUpField:
                    return "images/StatusImgs/prepare.png";
                case RnHStatus.OnPreparationField:
                    return "images/StatusImgs/wait.png";
                case RnHStatus.OnCompetitionField:
                    return "images/StatusImgs/play.png";
                case RnHStatus.CompetitionDone:
                    return "images/StatusImgs/index.png";
                default:
                    return "images/dressageImgs/empty.jpg";
            }
        }
    }
}
