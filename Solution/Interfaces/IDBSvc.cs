
using Models.Common;
using Models.IDBSvc;
using Models.Results;

namespace Interfaces
{
    public interface IDBSvc
    {
        Result<bool> SetDBSettings(DBConnectionSettings settings);

        Result<string> TitleSave(string title);

        Result<RnH> RnHInsert(RnH rnh);

        Result<RnH> RnHSave(RnH rnh);

        Result<bool> RnHDelete(RnH rnh);
    }
}