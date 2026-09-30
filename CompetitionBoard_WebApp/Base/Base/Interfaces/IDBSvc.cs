using Base.Models.RnHs;
using Base.Models.DB;

namespace Interfaces
{
    public interface IDBSvc
    {
        event EventHandler<string> ErrorRised;

        bool SetDBSettings(DBConnectionSettings settings);

        string TitleSave(string title);

        string TitleLoad();

        RnH Insert(RnH rnh);

        RnH Save(RnH rnh);

        bool Delete(RnH rnh);

        RnH ReadById(int id);

        IEnumerable<RnH> ReadAll();
    }
}