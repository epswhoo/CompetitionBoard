using Base.Models.RnHs;
using Interfaces;
using Repos.RnHs.Svcs;

namespace Repos.RnHs
{
    // All the code in this file is included in all platforms.
    public class RnHsRepo : IRnHsRepo
    {
        private readonly IDBSvc _dbSvc;
        private readonly DeleteSvc _deleteSvc;
        private readonly SaveSvc _saveSvc;
        private readonly DeleteAllSvc _deleteAllSvc;
        private readonly SetNewRnHsSvc _setNewRnHsSvc;
        private readonly InsertNewWithOrderSvc _insertNewWithOrderSvc;

        public event EventHandler<string> ErrorRised;

        public RnHsRepo(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
            _deleteSvc = new DeleteSvc(_dbSvc);
            _deleteAllSvc = new DeleteAllSvc(_dbSvc);
            _setNewRnHsSvc = new SetNewRnHsSvc(_dbSvc, _deleteAllSvc);
            _insertNewWithOrderSvc = new InsertNewWithOrderSvc(_dbSvc);
            _saveSvc = new SaveSvc(_dbSvc);

            _dbSvc.ErrorRised += (sender, message) => ErrorRised?.Invoke(sender, message);
            _saveSvc.ErrorRised += (sender, message) => ErrorRised?.Invoke(sender, message);
            _insertNewWithOrderSvc.ErrorRised += (sender, message) => ErrorRised?.Invoke(sender, message);
            _deleteSvc.ErrorRised += (sender, message) => ErrorRised?.Invoke(sender, message);
            _setNewRnHsSvc.ErrorRised += (sender, message) => ErrorRised?.Invoke(sender, message);
        }

        public IEnumerable<RnH> ReadAll()
        {
            return _dbSvc.ReadAll();
        }

        public bool DeleteAll()
        {
            return TryCatchException(_deleteAllSvc.DeleteAll);
        }

        public bool Delete(RnH rnh)
        {
            return TryCatchException(() => _deleteSvc.Delete(rnh));
        }

        public RnH Save(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.Save(rnh));
        }

        public RnH SaveIsRanked(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveIsRanked(rnh));
        }

        public RnH SaveIsDisqualificated(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveIsDisqualificated(rnh));
        }

        public RnH SaveMark(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveMark(rnh));
        }

        public RnH SaveStatus(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveStatus(rnh));
        }

        public RnH SaveHorseNo(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveHorseNo(rnh));
        }

        public IEnumerable<RnH> SetNewRnHs(string str)
        {
            return TryCatchException(() => _setNewRnHsSvc.SetNewRnHs(str));
        }

        public RnH InsertNewWithOrder(int order)
        {
            return TryCatchException(() => _insertNewWithOrderSvc.InsertNewWithOrder(order));
        }

        private T TryCatchException<T>(Func<T> todo)
        {
            try
            {
                return todo();
            }
            catch (Exception ex)
            {
                ErrorRised?.Invoke(null, ex.Message);
                return default;
            }
        }
    }
}
