using Base.Models.RnHs;
using Interfaces;

namespace Repos.RnHs.Svcs
{
    // All the code in this file is included in all platforms.
    internal class DeleteSvc
    {
        private readonly IDBSvc _dbSvc;

        internal EventHandler<string> ErrorRised;

        internal DeleteSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        internal bool Delete(RnH rnh)
        {
            RnH stored = _dbSvc.ReadById(rnh.Id);
            if (stored == null)
            {
                return false;
            }

            bool deleteResult = _dbSvc.Delete(stored);
            if (!deleteResult)
            {
                return false;
            }

            IEnumerable<RnH> rnHs = _dbSvc.ReadAll();
            if (rnHs == null)
            {
                return false;
            }

            foreach (RnH r in rnHs.Where(s => s.Order > stored.Order))
            {
                r.Order--;
                RnH saveResult = _dbSvc.Save(r);
                if (saveResult == null)
                {
                    ErrorRised?.Invoke(null, $"Reihenfolge konnte nach dem Löschen nicht angepasst werden.");
                    return false;
                }
            }
            return true;
        }
    }
}
