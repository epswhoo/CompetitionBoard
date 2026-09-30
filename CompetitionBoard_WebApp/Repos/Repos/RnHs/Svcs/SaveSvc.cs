using Base.Models.RnHs;
using Interfaces;

namespace Repos.RnHs.Svcs
{
    // All the code in this file is included in all platforms.
    internal class SaveSvc
    {
        private readonly IDBSvc _dbSvc;

        internal EventHandler<string> ErrorRised;

        internal SaveSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        internal RnH Save(RnH rnh)
        {
            if (!IsValidToSave(rnh))
            {
                return null;
            }
            return _dbSvc.Save(rnh);
        }

        internal RnH SaveIsRanked(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.IsRanked = rnh.IsRanked);
        }

        internal RnH SaveIsDisqualificated(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.IsDisqualificated = rnh.IsDisqualificated);
        }

        internal RnH SaveMark(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.Mark = rnh.Mark);
        }

        internal RnH SaveStatus(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.Status = rnh.Status);
        }

        internal RnH SaveHorseNo(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.HorseNo = rnh.HorseNo);
        }

        internal RnH SaveParameter(RnH rnh, Action<RnH> doUpdate)
        {
            RnH toUpdate = _dbSvc.ReadById(rnh.Id);
            if (toUpdate == null)
            {
                return null;
            }
            doUpdate(toUpdate);
            if (!IsValidToSave(toUpdate))
            {
                return null;
            }
            return _dbSvc.Save(toUpdate);
        }

        private bool IsValidToSave(RnH rnh)
        {
            bool isValid = true;
            if (rnh.Mark < 0.0 || rnh.Mark > 10.0)
            {
                ErrorRised?.Invoke(null, $"Note {rnh.Mark} nicht erlaubt.");
                isValid = false;
            }

            if (rnh.Mark > 0.0 && rnh.Status != RnHStatus.CompetitionDone)
            {
                ErrorRised?.Invoke(null, $"Note kann nicht eingetragen werden, da Reiter mit diesem Pferd die Prüfung noch nicht abgeschlossen hat.");
                isValid = false;
            }

            if (rnh.Mark > 0.0 && rnh.IsDisqualificated)
            {
                ErrorRised?.Invoke(null, $"Note kann nicht eingetragen werden, da Reiter mit diesem Pferd in dieser Prüfung disqualifiziert sind.");
                isValid = false;
            }
            if (rnh.IsDisqualificated && rnh.Status != RnHStatus.CompetitionDone)
            {
                ErrorRised?.Invoke(null, $"Reiter mit Pferd kann nicht disqualifiziert werden, da Reiter mit diesem Pferd die Prüfung noch nicht abgeschlossen hat.");
                isValid = false;
            }
            if (rnh.IsDisqualificated && rnh.IsRanked)
            {
                ErrorRised?.Invoke(null, $"Reiter mit Pferd kann nicht disqualifiziert werden, da Reiter mit diesem Pferd in dieser Prüfung platziert wurden.");
                isValid = false;
            }
            if (rnh.IsRanked && Math.Abs(rnh.Mark) < 0.01)
            {
                ErrorRised?.Invoke(null, $"Reiter mit Pferd kann nicht platziert werden, da Reiter mit diesem Pferd in dieser Prüfung nicht bewertet wurden.");
                isValid = false;
            }
            return isValid;
        }
    }
}
