

using Base.Models.RnHs;

namespace Interfaces
{
    public interface IRnHsRepo
    {
        event EventHandler<string> ErrorRised;

        IEnumerable<RnH> SetNewRnHs(string str);

        RnH Save(RnH rnh);

        bool Delete(RnH rnh);

        IEnumerable<RnH> ReadAll();

        bool DeleteAll();

        RnH InsertNewWithOrder(int order);

        RnH SaveIsRanked(RnH rnh);

        RnH SaveIsDisqualificated(RnH rnh);

        RnH SaveMark(RnH rnh);

        RnH SaveStatus(RnH rnh);

        RnH SaveHorseNo(RnH rnh);
    }
}