﻿using CompetitionBoard_Net8.Interfaces;
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.IDBSvc;
using CompetitionBoard_Net8.DBSvc.RnHsDBSvc;
using CompetitionBoard_Net8.DBSvc.SqlConnectionExtensions;
using Microsoft.EntityFrameworkCore;
using CompetitionBoard_Net8.DBSvc.RnHExtensions;
using CompetitionBoard_Net8.Models.Results;
using CompetitionBoard_Net8.Models.Errors;

namespace CompetitionBoard_Net8.DBSvc
{
    // All the code in this file is included in all platforms.
    public class DBSvc : IDBSvc
    {
        private CompetitionBoardDBContext _dbContext;
        private string _connectionString;

        public Result<bool> SetDBSettings(DBConnectionSettings settings)
        {
            return TryCatchException(() =>
                {
                    _connectionString = settings.GetConnectionString();
                    Reload();
                    return _dbContext.Database.CanConnect();
                });
        }

        private void Reload()
        {
            TryCatchException(() =>
            {
    //            new DbContextOptionsBuilder<ApplicationDbContext>()
    //.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test;ConnectRetryCount=0")
    //.Options;

                _dbContext = new CompetitionBoardDBContext(
                        new DbContextOptionsBuilder<CompetitionBoardDBContext>()
                        .UseSqlServer(
                            _connectionString,
                            options => options.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(5),
                                errorNumbersToAdd: null))
                        .Options);

                
                return true;
            });
        }

        public Result<bool> CheckConnection()
        {
            return TryCatchException(_dbContext.Database.CanConnect);
        }

        public Result<bool> Delete(RnH rnh)
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

        public Result<RnH> Insert(RnH rnh)
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

        public Result<RnH> Save(RnH rnh)
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

        public Result<string> TitleLoad()
        {
            return TryCatchException(() =>
            {
                Reload();
                TitleDBContextModel model = _dbContext.TitleTable.FirstOrDefault();
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
                result.ErrorCode = 
                    int.TryParse(ErrorCodes.IDBSvcException, out int value) ? value : 1;
                result.Exception = ex;
            }
            return result;
        }

        private Result<T> TryCatchException<T>(Func<Result<T>> todo)
        {
            try
            {
                return todo();
            }
            catch (Exception ex)
            {
                Result<T> result = new Result<T>();
                result.ErrorCode =
                    int.TryParse(ErrorCodes.IDBSvcException, out int value) ? value : 1;
                result.Exception = ex;
                return result;
            }
        }

        public Result<IEnumerable<RnH>> ReadAll()
        {
            return TryCatchException<IEnumerable<RnH>>(() =>
                {
                    Reload();
                    List<RnH> rnhs = new List<RnH>();
                    foreach (RnH rnh in _dbContext.RnHsTable)
                    {
                        RnH copy = new RnH();
                        copy.SetData(rnh);
                        rnhs.Add(copy);
                    }
                    return rnhs;
                });
        }

        public Result<RnH> SaveIsRanked(RnH rnh)
        {
            return TryCatchException<RnH>(() =>
            {
                Reload();
                RnH toSave = _dbContext.RnHsTable
                        .FirstOrDefault(r => r.Id == rnh.Id);
                toSave.IsRanked = rnh.IsRanked;
                return Save(toSave);
            });
        }
    }
}