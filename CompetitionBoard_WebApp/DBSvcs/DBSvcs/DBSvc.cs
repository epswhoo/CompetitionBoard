using Base.Models.DB;
using Base.Models.RnHs;
using DBSvcs.Helpers;
using DBSvcs.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DBSvcs
{
    // All the code in this file is included in all platforms.
    public class DBSvc : IDBSvc
    {
        private string _connectionString;

        public event EventHandler<string> ErrorRised;

        public bool SetDBSettings(DBConnectionSettings settings)
        {
            return TryCatchException(() =>
                {
                    _connectionString = settings.GetConnectionString();
                    using CompetitionBoardDBContext dbContext = CreateDBContext();
                    return dbContext.Database.CanConnect();
                });
        }

        public bool CheckConnection()
        {
            return TryCatchException(() =>
                {
                    using CompetitionBoardDBContext dbContext = CreateDBContext();
                    return dbContext.Database.CanConnect();
                });
        }

        public bool Delete(RnH rnh)
        {
            return TryCatchException(() =>
            {
                using CompetitionBoardDBContext dbContext = CreateDBContext();
                RnH toRemove = FindRnH(dbContext, rnh.Id);
                dbContext.Remove(toRemove);
                dbContext.SaveChanges();
                bool isDeleted = dbContext.RnHsTable.Any(r => r.Id == rnh.Id);
                return !isDeleted;
            });
        }

        public RnH Insert(RnH rnh)
        {
            return TryCatchException(() =>
                {
                    using CompetitionBoardDBContext dbContext = CreateDBContext();
                    RnH toInsert = new RnH();
                    toInsert.SetData(rnh);
                    toInsert.Id = 0;
                    dbContext.Add(toInsert);
                    dbContext.SaveChanges();
                    rnh.SetData(toInsert);
                    return rnh;
                });
        }

        public RnH Save(RnH rnh)
        {
            return TryCatchException(() =>
            {
                using CompetitionBoardDBContext dbContext = CreateDBContext();
                RnH toSave = FindRnH(dbContext, rnh.Id);
                toSave.SetData(rnh);
                dbContext.SaveChanges();
                rnh.SetData(toSave);
                return rnh;
            });
        }

        public string TitleSave(string title)
        {
            return TryCatchException(() =>
                {
                    using CompetitionBoardDBContext dbContext = CreateDBContext();
                    TitleDBContextModel model = dbContext.TitleTable.FirstOrDefault();
                    if (model == null)
                    {
                        model = new TitleDBContextModel();
                        dbContext.TitleTable.Add(model);
                    }
                    model.Title = title;
                    dbContext.SaveChanges();
                    return model.Title;
                });
        }

        public string TitleLoad()
        {
            return TryCatchException(() =>
            {
                using CompetitionBoardDBContext dbContext = CreateDBContext();
                TitleDBContextModel model = dbContext.TitleTable
                    .AsNoTracking()
                    .FirstOrDefault();
                return model?.Title ?? string.Empty;
            });
        }

        public RnH ReadById(int id)
        {
            return TryCatchException(() =>
            {
                using CompetitionBoardDBContext dbContext = CreateDBContext();
                RnH copy = new RnH();
                copy.SetData(FindRnH(dbContext, id));
                return copy;
            });
        }

        public IEnumerable<RnH> ReadAll()
        {
            return TryCatchException(() =>
                {
                    using CompetitionBoardDBContext dbContext = CreateDBContext();
                    return dbContext.RnHsTable
                        .AsNoTracking()
                        .OrderBy(r => r.Order)
                        .ToList();
                });
        }

        private CompetitionBoardDBContext CreateDBContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Es sind keine Datenbankeinstellungen gesetzt.");
            }
            return new CompetitionBoardDBContext(
                new DbContextOptionsBuilder<CompetitionBoardDBContext>()
                    .UseSqlServer(_connectionString)
                    .Options);
        }

        private static RnH FindRnH(CompetitionBoardDBContext dbContext, int id)
        {
            RnH rnh = dbContext.RnHsTable.FirstOrDefault(r => r.Id == id);
            if (rnh == null)
            {
                throw new InvalidOperationException($"Reiter und Pferd mit Id {id} nicht gefunden.");
            }
            return rnh;
        }

        private T TryCatchException<T>(Func<T> todo)
        {
            try
            {
                return todo();
            }
            catch (Exception ex)
            {
                string errorMessage = $"{ex.Message}";
                ErrorRised?.Invoke(this, errorMessage);
                return default;
            }
        }
    }
}
