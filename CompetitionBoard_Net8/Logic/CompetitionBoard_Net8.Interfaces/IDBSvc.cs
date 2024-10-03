
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.IDBSvc;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Interfaces
{
    public interface IDBSvc
    {
        Result<bool> SetDBSettings(DBConnectionSettings settings);

        Result<string> TitleSave(string title);

        Result<string> TitleLoad();

        Result<RnH> Insert(RnH rnh);

        Result<RnH> Save(RnH rnh);

        Result<bool> Delete(RnH rnh);

        Result<IEnumerable<RnH>> ReadAll();
    }
}