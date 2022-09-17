using Interfaces;
using Models.Results;

namespace ViewModels.Repos
{
    // All the code in this file is included in all platforms.
    public class TitleRepo : ITitleRepo
    {
        private readonly IDBSvc _dbSvc;

        public TitleRepo(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        public Result<string> Save(string title)
        {
            return _dbSvc.TitleSave(title);
        }

        public Result<string> Load()
        {
            return _dbSvc.TitleLoad();
        }

        public Result<string> Clear()
        {
            return _dbSvc.TitleSave(string.Empty);
        }
    }
}