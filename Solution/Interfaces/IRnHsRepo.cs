
using Models.Common;
using Models.Results;

namespace Interfaces
{
    public interface IRnHsRepo
    {
        Result<IEnumerable<RnH>> SetNewRnHs(string str);

        Result<RnH> Save(RnH rnh);

        Result<bool> Delete(RnH rnh);

        Result<IEnumerable<RnH>> ReadAll();

        Result<bool> DeleteAll();

        Result<RnH> InsertNewWithOrder(int order);
    }
}