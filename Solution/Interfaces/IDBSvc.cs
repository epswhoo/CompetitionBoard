
using Models.Common;
using Models.IDBSvc;
using Models.Results;

namespace Interfaces
{
    public interface IDBSvc
    {
        Result<bool> SetDBSettings(DBConnectionSettings settings);

        Result<string> TitleSave(string title);

        Result<RnH> Insert(RnH rnh);

        Result<RnH> Save(RnH rnh);

        Result<bool> Delete(RnH rnh);

        Result<IEnumerable<RnH>> ReadAll();
    }
}