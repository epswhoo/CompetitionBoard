
using Models.Common;
using Models.IDBSvc;

namespace Interfaces
{
    public interface IDBSvc
    {
        DBResult<bool> SetDBSettings(DBConnectionSettings settings);

        DBResult<string> TitleSave(string title);

        DBResult<RnH> RnHInsert(RnH rnh);

        DBResult<RnH> RnHSave(RnH rnh);

        DBResult<bool> RnHDelete(RnH rnh);
    }
}