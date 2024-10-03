using Azure.Identity;
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
            Result<RnH> checkResult = CheckToSave(rnh);
            if (checkResult != null)
            {
                return checkResult;
            }
            else
            {
                return _dbSvc.Save(rnh);
            }
        }

        public Result<RnH> SaveIsRanked(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.IsRanked = rnh.IsRanked);
        }

        public Result<RnH> SaveIsDisqualificated(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.IsDisqualificated = rnh.IsDisqualificated);
        }

        public Result<RnH> SaveMark(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.Mark = rnh.Mark);
        }

        public Result<RnH> SaveStatus(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.Status = rnh.Status);
        }

        public Result<RnH> SaveHorseNo(RnH rnh)
        {
            return SaveParameter(rnh, toUpdate => toUpdate.HorseNo = rnh.HorseNo);
        }

        public Result<RnH> SaveParameter(RnH rnh, Action<RnH> doUpdate)
        {
            Result<IEnumerable<RnH>> reloadResult = _dbSvc.ReadAll();
            if (reloadResult.ErrorCode != 0)
            {
                return new Result<RnH>
                {
                    ErrorCode = reloadResult.ErrorCode,
                    Exception = reloadResult.Exception
                };
            }
            RnH toUpdate = reloadResult.Content?.FirstOrDefault(r => r.Id == rnh.Id);
            if (toUpdate == null)
            {
                return new Result<RnH>
                {
                    ErrorCode = int.TryParse(ErrorCodes.IRnHRepoSaveRnHNotFound, out int value) ? value : 0,
                };
            }
            doUpdate(toUpdate);
            Result<RnH> checkResult = CheckToSave(toUpdate);
            if (checkResult != null)
            {
                return checkResult;
            }
            else
            {
                return _dbSvc.Save(rnh);
            }
        }

        private Result<RnH> CheckToSave(RnH rnh)
        {
            if (rnh.Mark < 0.0 || rnh.Mark > 10.0)
                return new Result<RnH>
                {
                    ErrorCode =
                            int.TryParse(ErrorCodes.IRnHRepoSaveMarkOutOfRange,
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
            return null;
        }
    }
}