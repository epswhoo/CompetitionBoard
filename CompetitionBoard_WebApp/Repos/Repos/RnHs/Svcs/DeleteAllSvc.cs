using Base.Models.RnHs;
using Interfaces;

namespace Repos.RnHs.Svcs
{
    // All the code in this file is included in all platforms.
    internal class DeleteAllSvc
    {
        private readonly IDBSvc _dbSvc;

        internal DeleteAllSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        internal bool DeleteAll()
        {
            IEnumerable<RnH> rnHs = _dbSvc.ReadAll();
            if (rnHs == null)
            {
                return false;
            }

            bool allDeleted = true;
            foreach (RnH rnh in rnHs)
            {
                allDeleted &= _dbSvc.Delete(rnh);
            }
            return allDeleted;
        }
    }
}
