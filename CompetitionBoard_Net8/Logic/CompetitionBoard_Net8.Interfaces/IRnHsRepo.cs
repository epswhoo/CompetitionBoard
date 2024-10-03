
using CompetitionBoard_Net8.Models.Common;
using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Interfaces
{
    public interface IRnHsRepo
    {
        Result<IEnumerable<RnH>> SetNewRnHs(string str);

        Result<RnH> Save(RnH rnh);

        Result<bool> Delete(RnH rnh);

        Result<IEnumerable<RnH>> ReadAll();

        Result<bool> DeleteAll();

        Result<RnH> InsertNewWithOrder(int order);

        Result<RnH> SaveIsRanked(RnH rnh);

        Result<RnH> SaveIsDisqualificated(RnH rnh);

        Result<RnH> SaveMark(RnH rnh);

        Result<RnH> SaveStatus(RnH rnh);

        Result<RnH> SaveHorseNo(RnH rnh);
    }
}