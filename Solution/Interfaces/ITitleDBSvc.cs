
using Models.IDBSvc;

namespace Interfaces
{
    public interface ITitleDBSvc : IDBSvc
    {
        DBResult<string> SaveTitle(string title);

        DBResult<string> ClearTitle(string title);
    }
}