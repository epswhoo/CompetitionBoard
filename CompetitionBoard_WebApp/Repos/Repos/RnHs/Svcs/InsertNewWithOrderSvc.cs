using Base.Models.RnHs;
using Interfaces;

namespace Repos.RnHs.Svcs
{
    // All the code in this file is included in all platforms.
    internal class InsertNewWithOrderSvc
    {
        private readonly IDBSvc _dbSvc;

        internal EventHandler<string> ErrorRised;

        internal InsertNewWithOrderSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        internal RnH InsertNewWithOrder(int order)
        {
            IEnumerable<RnH> rnHs = _dbSvc.ReadAll();
            if (rnHs == null)
            {
                return null;
            }
            int maxOrder = rnHs
                .Select(r => r.Order)
                .DefaultIfEmpty(0)
                .Max();
            if (order < 1 || order > maxOrder + 1)
            {
                ErrorRised?.Invoke(null, $"Position {order} ist nicht erlaubt.");
                return null;
            }

            foreach (RnH r in rnHs.Where(s => s.Order >= order))
            {
                r.Order++;
                RnH saveResult = _dbSvc.Save(r);
                if (saveResult == null)
                {
                    ErrorRised?.Invoke(null, $"Reihenfolge konnte vor dem Einfügen an Position {order} nicht angepasst werden.");
                    return null;
                }
            }
            RnH rnh = new RnH
                {
                    Order = order,
                };
            RnH insertResult = _dbSvc.Insert(rnh);
            return insertResult;
        }
    }
}
