using Interfaces;
using Models.Common;
using Models.Errors;
using Models.Results;
using ViewModels.Repos.RnHsRepoSvcs;

namespace ViewModels.Repos
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

        public RnHsRepo(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
            _deleteSvc = new DeleteSvc(_dbSvc);
            _deleteAllSvc = new DeleteAllSvc(_dbSvc);
            _setNewRnHsSvc = new SetNewRnHsSvc(_dbSvc, _deleteAllSvc);
            _insertNewWithOrderSvc = new InsertNewWithOrderSvc(_dbSvc);
            _saveSvc = new SaveSvc(_dbSvc);
        }

        public Result<IEnumerable<RnH>> ReadAll()
        {
            return _dbSvc.ReadAll();
        }

        public Result<bool> DeleteAll()
        {
            return TryCatchException(_deleteAllSvc.DeleteAll);
        }

        public Result<bool> Delete(RnH rnh)
        {
            return TryCatchException(() => _deleteSvc.Delete(rnh));
        }

        public Result<RnH> Save(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.Save(rnh));
        }

        public Result<RnH> SaveIsRanked(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveIsRanked(rnh));
        }

        public Result<RnH> SaveIsDisqualificated(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveIsDisqualificated(rnh));
        }

        public Result<RnH> SaveMark(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveMark(rnh));
        }

        public Result<RnH> SaveStatus(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveStatus(rnh));
        }

        public Result<RnH> SaveHorseNo(RnH rnh)
        {
            return TryCatchException(() => _saveSvc.SaveHorseNo(rnh));
        }

        public Result<IEnumerable<RnH>> SetNewRnHs(string str)
        {
            return TryCatchException(() => _setNewRnHsSvc.SetNewRnHs(str));
        }

        Result<RnH> IRnHsRepo.InsertNewWithOrder(int order)
        {
            return  TryCatchException(() => _insertNewWithOrderSvc.InsertNewWithOrder(order));
        }

        private Result<T> TryCatchException<T>(Func<Result<T>> todo)
        {
            Result<T> result = new Result<T>();
            try
            {
                return todo();
            }
            catch (Exception ex)
            {
                result.ErrorCode =
                    int.TryParse(ErrorCodes.IRnHRepoException, out int value) ? value : 1;
                result.Exception = ex;
            }
            return result;
        }
    }
}