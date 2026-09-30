using Interfaces;

namespace Repos.Title
{
    // All the code in this file is included in all platforms.
    public class TitleRepo : ITitleRepo
    {
        private readonly IDBSvc _dbSvc;

        public TitleRepo(IDBSvc dBSvc)
        {
            _dbSvc = dBSvc;
        }

        public string Save(string title)
        {
            return _dbSvc.TitleSave(title);
        }

        public string Load()
        {
            return _dbSvc.TitleLoad();
        }

        public string Clear()
        {
            return _dbSvc.TitleSave(string.Empty);
        }
    }
}