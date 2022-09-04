using Interfaces;
using Models.Common;
using Models.Errors;
using Models.Results;

namespace ViewModels.Repos.RnHsRepoSvcs
{
    // All the code in this file is included in all platforms.
    public class SaveSvc
    {
        private readonly IDBSvc _dbSvc;

        public SaveSvc(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        public Result<RnH> Save(RnH rnh)
        {
            if (rnh.Mark < 0.0 || rnh.Mark > 10.0)
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveMarkOutOfRange,
                                out int value) ? value : 0
                };
            if (!MarkFormatValidator.Validate(rnh.Mark))
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveMarkInvalidFormat,
                                out int value) ? value : 0
                };
            if (rnh.Mark > 0.0 && rnh.Status != RnHStatus.CompetitionDone)
            {
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveStatusNotCompetitionDoneWithMark,
                                out int value) ? value : 0
                };
            }
            if (rnh.Mark > 0.0 && rnh.IsDisqualificated)
            {
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveMarkAndDis,
                                out int value) ? value : 0
                };
            }
            if (rnh.IsDisqualificated && rnh.Status != RnHStatus.CompetitionDone)
            {
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveStatusNotCompetitionDoneWithDis,
                                out int value) ? value : 0
                };
            }
            if (rnh.IsDisqualificated && rnh.IsRanked)
            {
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveDisAndRank,
                                out int value) ? value : 0
                };
            }
            if (rnh.IsRanked && Math.Abs(rnh.Mark) < 0.01)
            {
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveRankAndNoMark,
                                out int value) ? value : 0
                };
            }
            return _dbSvc.Save(rnh);
        }
    }
}