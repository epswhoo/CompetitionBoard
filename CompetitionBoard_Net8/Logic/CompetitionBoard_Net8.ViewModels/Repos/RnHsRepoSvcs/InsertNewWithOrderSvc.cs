using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Errors;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.ViewModels.Repos.RnHsRepoSvcs
{
    // All the code in this file is included in all platforms.
    public class InsertNewWithOrderSvc
    {
        private readonly IDBSvc _dbSvc;

        public InsertNewWithOrderSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        public Result<RnH> InsertNewWithOrder(int order)
        {
            Result<IEnumerable<RnH>> readAllResult = _dbSvc.ReadAll();
            if (readAllResult.ErrorCode != 0)
                return new Result<RnH>
                    {
                        ErrorCode = readAllResult.ErrorCode,
                        Exception = readAllResult.Exception
                    };
            int maxOrder = readAllResult.Content.Max(r => r.Order);
            if (order < 1 || order > maxOrder + 1)
                return new Result<RnH>
                    {
                        ErrorCode = 
                            int.TryParse(ErrorCodes.IRnHRepoInsertNewWithOrderOutOfRange, 
                                out int value) ? value : 0
                    };
            foreach (RnH r in readAllResult.Content.Where(s => s.Order >= order))
            {
                r.Order++;
                Result<RnH> saveResult = _dbSvc.Save(r);
                if (saveResult.ErrorCode != 0)
                    return new Result<RnH>
                        {
                            ErrorCode = readAllResult.ErrorCode,
                            Exception = readAllResult.Exception
                        };
            }
            RnH rnh = new RnH 
                {
                    Order = order,
                };
            Result<RnH> insertResult = _dbSvc.Insert(rnh);
            if (insertResult.ErrorCode != 0)
                return new Result<RnH>
                {
                    ErrorCode = insertResult.ErrorCode,
                    Exception = insertResult.Exception
                };
            return new Result<RnH> { Content = insertResult.Content };
        }
    }
}