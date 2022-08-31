
using Models.IDBSvc;

namespace Interfaces
{
    public interface IDBSvc
    {
        DBResult<string> SetSettingsToConnection(DBConnectionSettings dBConnectionSettings);

        //DBResult<RnH> InsertRnH(RnH RnH);
        
        //DBResult<RnH> SaveRnH(RnH RnH);

        //DBResult<RnH> DeleteRnH(RnH RnH);

        //DBResult<RnH> InsertOnPosRnH(RnH RnH, int Pos);
    }
}