using Interfaces;
using Models.Common;
using Models.IDBSvc;
using DBSvcs.RnHsDBSvc;
using DBSvcs.SqlConnectionExtensions;
using Microsoft.EntityFrameworkCore;
using DBSvcs.RnHExtensions;
using Models.Results;

namespace DBSvcs
{
    // All the code in this file is included in all platforms.
    public class DBSvc : IDBSvc
    {
        private CompetitionBoardDBContext _dbContext;

        public Result<bool> SetDBSettings(DBConnectionSettings settings)
        {
            return TryCatchException(() =>
                {
                    string cnnString = settings.GetConnectionString();
                    _dbContext = new CompetitionBoardDBContext(
                        SqlServerDbContextOptionsExtensions.UseSqlServer(
                            new DbContextOptionsBuilder<CompetitionBoardDBContext>(),
                            cnnString)
                        .Options);
                    return _dbContext.Database.CanConnect();
                });
        }

        public Result<bool> CheckConnection()
        {
            return TryCatchException(_dbContext.Database.CanConnect);
        }

        public Result<bool> RnHDelete(RnH rnh)
        {
            return TryCatchException(() =>
            {
                RnH toRemove = _dbContext.RnHsTable
                    .FirstOrDefault(r => r.Id == rnh.Id);
                _dbContext.Remove(toRemove);
                _dbContext.SaveChanges();
                bool isDeleted = _dbContext.RnHsTable.Any(r => r.Id == rnh.Id);
                return !isDeleted;
            });
        }

        public Result<RnH> RnHInsert(RnH rnh)
        {
            return TryCatchException(() =>
                {
                    RnH toInsert = new RnH();
                    toInsert.SetData(rnh);
                    _dbContext.AddRange(toInsert);
                    _dbContext.SaveChanges();
                    rnh.SetData(toInsert);
                    return rnh;
                });
        }

        public Result<RnH> RnHSave(RnH rnh)
        {
            return TryCatchException(() =>
            {
                RnH toSave = _dbContext.RnHsTable
                    .FirstOrDefault(r => r.Id == rnh.Id);
                toSave.SetData(rnh);
                _dbContext.SaveChanges();
                rnh.SetData(toSave);
                return rnh;
            });
        }

        public Result<string> TitleSave(string title)
        {
            return TryCatchException(() =>
                { 
                    TitleDBContextModel model = _dbContext.TitleTable.FirstOrDefault();
                    model.Title = title;
                    _dbContext.SaveChanges();
                    return model.Title;
                });
        }

        private Result<T> TryCatchException<T>(Func<T> todo)
        {
            Result<T> result = new Result<T>();
            try
            {
                result.Content = todo();
            }
            catch (Exception ex)
            {
                result.ErrorCode = 1;
                result.Exception = ex;
            }
            return result;
        } 
    }
}