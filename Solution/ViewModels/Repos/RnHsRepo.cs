using Interfaces;
using Models.Common;
using Models.Errors;
using Models.Results;

namespace ViewModels.Repos
{
    // All the code in this file is included in all platforms.
    public class RnHsRepo : IRnHsRepo
    {
        private readonly IDBSvc _dbSvc;

        public RnHsRepo(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        public Result<MultiResult<RnH>> DeleteAll()
        {
            throw new NotImplementedException();
            //return TryCatchException(() =>
            //{
            //    //Result<RnH> insertResult = _dbSvc.RnHInsert(rnh);

            //    //List<RnH> inserts = new List<RnH>();
            //    //MultiResult<RnH> result = new MultiResult<RnH>();
            //    //foreach (RnH rnh in rnhs)
            //    //{
            //    //    rnh.Order = order;
            //    //    Result<RnH> insertResult = _dbSvc.RnHInsert(rnh);
            //    //    if (insertResult.ErrorCode == 0)
            //    //    {
            //    //        inserts.Add(insertResult.Content);
            //    //        order++;
            //    //    }
            //    //    else
            //    //    {
            //    //        // Fehler
            //    //        // Neuer ErrorCode und Results aus DB loggen
            //    //    }
            //    //}
            //    result.Content = inserts;
            //    return result;
            //});
        }

        public Result<bool> RnHDelete(RnH rnh)
        {
            return _dbSvc.RnHDelete(rnh);
        }

        public Result<IEnumerable<RnH>> RnHInsertNewWithOrder(int order)
        {
            throw new NotImplementedException();
        }

        public Result<RnH> RnHSave(RnH rnh)
        {
            return _dbSvc.RnHSave(rnh);
        }

        public Result<MultiResult<RnH>> SetNewRnHs(string str)
        {
            return TryCatchException(() =>
                {
                    IEnumerable<RnH> rnhs = str
                        .Split(',')
                        .Select(s => s.Trim())
                        .Select(s =>
                            int.TryParse(s, out int value) ? value : 0)
                        .Select(h => new RnH
                        {
                            HorseNo = h
                        });
                    int order = 1;
                    List<RnH> inserts = new List<RnH>();
                    List<Result<RnH>> results = new List<Result<RnH>>();
                    foreach (RnH rnh in rnhs)
                    {
                        rnh.Order = order;
                        Result<RnH> insertResult = _dbSvc.RnHInsert(rnh);
                        if (insertResult.ErrorCode == 0)
                        {
                            inserts.Add(insertResult.Content);
                            order++;
                        }
                        else
                        {
                            results.Add(insertResult);
                        }
                    }
                    Result<MultiResult<RnH>> result = new Result<MultiResult<RnH>>();
                    result.Content = new MultiResult<RnH>();
                    if (results.Count > 0)
                    {
                        result.Content.Results = results;
                        result.ErrorCode =
                            int.TryParse(ErrorCodes.IDBSvcException, out int value) ? value : 0;
                    }
                    result.Content.Content = inserts;
                    return result;
                });
        }

        Result<MultiResult<RnH>> IRnHsRepo.RnHInsertNewWithOrder(int order)
        {
            throw new NotImplementedException();
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
                    int.TryParse(ErrorCodes.IDBSvcException, out int value) ? value : 0;
                result.Exception = ex;
            }
            return result;
        }
    }
}