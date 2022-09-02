
using Models.Common;
using Models.Results;

namespace Interfaces
{
    public interface IRnHsRepo
    {
        Result<MultiResult<RnH>> SetNewRnHs(string str);

        Result<RnH> RnHSave(RnH rnh);

        Result<bool> RnHDelete(RnH rnh);

        Result<MultiResult<RnH>> DeleteAll();

        Result<MultiResult<RnH>> RnHInsertNewWithOrder(int order);
    }
}