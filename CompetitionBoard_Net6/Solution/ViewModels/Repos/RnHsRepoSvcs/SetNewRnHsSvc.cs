using Interfaces;
using Models.Common;
using Models.Errors;
using Models.Results;

namespace ViewModels.Repos.RnHsRepoSvcs
{
    // All the code in this file is included in all platforms.
    public class SetNewRnHsSvc
    {
        private readonly IDBSvc _dbSvc;

        private readonly DeleteAllSvc _deleteAllSvc;

        public SetNewRnHsSvc(IDBSvc dBSvc, DeleteAllSvc deleteAllSvc)
        {
            _dbSvc = dBSvc;
            _deleteAllSvc = deleteAllSvc;
        }

        public Result<IEnumerable<RnH>> SetNewRnHs(string str)
        {
            Result<bool> deleteResult = _deleteAllSvc.DeleteAll();
            if (deleteResult.ErrorCode != 0)
            {
                return new Result<IEnumerable<RnH>>
                {
                    ErrorCode = deleteResult.ErrorCode,
                    Exception = deleteResult.Exception
                };
            }
            IEnumerable<string> horseNoStrs = str
                .Split(',')
                .Select(s => s.Trim());
            List<RnH> inserts = new List<RnH>();
            int order = 1;
            foreach (string horseNoStr in horseNoStrs)
            {
                int horseNo = int.Parse(horseNoStr);
                RnH rnh = new RnH
                    {
                        HorseNo = horseNo,
                        Order = order
                    };
                Result<RnH> insertResult = _dbSvc.Insert(rnh);
                if (insertResult.ErrorCode != 0)
                    return new Result<IEnumerable<RnH>>
                        {
                            ErrorCode = insertResult.ErrorCode,
                            Exception = insertResult.Exception
                        };
                else
                    inserts.Add(insertResult.Content);
                order++;
            }
            return new Result<IEnumerable<RnH>>
                {
                    Content = inserts
                };
        }
    }
}