
using Models.Common;
using Models.Results;

namespace Interfaces
{
    public interface IRnHsRepo
    {
        Result<IEnumerable<RnH>> SetNewRnHs(string str);

        Result<RnH> RnHSave(RnH rnh);

        Result<bool> RnHDelete(RnH rnh);

        Result<IEnumerable<RnH>> RnHInsertNewWithOrder(int order);
    }
}