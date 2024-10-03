using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Errors;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.ViewModels.Repos.RnHsRepoSvcs
{
    // All the code in this file is included in all platforms.
    public class DeleteSvc
    {
        private readonly IDBSvc _dbSvc;

        public DeleteSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        public Result<bool> Delete(RnH rnh)
        {
            Result<IEnumerable<RnH>> readAllResult = _dbSvc.ReadAll();
            if (readAllResult.ErrorCode != 0)
                return new Result<bool>
                    {
                        ErrorCode = readAllResult.ErrorCode,
                        Exception = readAllResult.Exception
                    };
            foreach (RnH r in readAllResult.Content.Where(s => s.Order > rnh.Order))
            {
                r.Order--;
                Result<RnH> saveResult = _dbSvc.Save(r);
                if (saveResult.ErrorCode != 0)
                    return new Result<bool>
                        {
                            ErrorCode = readAllResult.ErrorCode,
                            Exception = readAllResult.Exception
                        };
            }
            Result<bool> deleteResult = _dbSvc.Delete(rnh);
            if (deleteResult.ErrorCode != 0)
                return new Result<bool>
                {
                    ErrorCode = deleteResult.ErrorCode,
                    Exception = deleteResult.Exception
                };
            return new Result<bool> { Content = true };
        }
    }
}