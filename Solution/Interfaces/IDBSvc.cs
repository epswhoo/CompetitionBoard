using Models.Common;
using Models.IDBSvc;

namespace Interfaces
{
    public interface IDBSvc
    {
        DBResult<string> Initialize(string cnnString);

        DBResult<string> SaveTitle(string title);

        DBResult<string> ClearTitle(string title);

        DBResult<RnH> InsertRnH(RnH RnH);
        
        DBResult<RnH> SaveRnH(RnH RnH);

        DBResult<RnH> DeleteRnH(RnH RnH);

        DBResult<RnH> InsertOnPosRnH(RnH RnH, int Pos);
    }
}