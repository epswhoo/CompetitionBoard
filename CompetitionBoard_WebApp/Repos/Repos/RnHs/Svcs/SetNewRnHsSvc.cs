using Base.Models.RnHs;
using Interfaces;

namespace Repos.RnHs.Svcs
{
    // All the code in this file is included in all platforms.
    internal class SetNewRnHsSvc
    {
        private readonly IDBSvc _dbSvc;

        private readonly DeleteAllSvc _deleteAllSvc;

        internal EventHandler<string> ErrorRised;

        internal SetNewRnHsSvc(IDBSvc dBSvc, DeleteAllSvc deleteAllSvc)
        {
            _dbSvc = dBSvc;
            _deleteAllSvc = deleteAllSvc;
        }

        internal IEnumerable<RnH> SetNewRnHs(string str)
        {
            List<int> horseNos = ParseHorseNos(str);
            if (horseNos == null)
            {
                return null;
            }

            bool deleteResult = _deleteAllSvc.DeleteAll();
            if (!deleteResult)
            {
                ErrorRised?.Invoke(null, "Die bestehende Starterliste konnte nicht gelöscht werden.");
                return null;
            }

            List<RnH> inserts = new List<RnH>();
            int order = 1;
            foreach (int horseNo in horseNos)
            {
                RnH rnh = new RnH
                    {
                        HorseNo = horseNo,
                        Order = order
                    };
                RnH insertResult = _dbSvc.Insert(rnh);
                if (insertResult == null)
                    return null;
                else
                    inserts.Add(insertResult);
                order++;
            }
            return inserts;
        }

        private List<int> ParseHorseNos(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                ErrorRised?.Invoke(null, "Es wurden keine Pferdenummern angegeben.");
                return null;
            }

            List<int> horseNos = new List<int>();
            foreach (string horseNoStr in str.Split(',').Select(s => s.Trim()))
            {
                if (!int.TryParse(horseNoStr, out int horseNo))
                {
                    ErrorRised?.Invoke(null, $"Pferdenummer '{horseNoStr}' ist keine gültige Zahl.");
                    return null;
                }
                horseNos.Add(horseNo);
            }
            return horseNos;
        }
    }
}
