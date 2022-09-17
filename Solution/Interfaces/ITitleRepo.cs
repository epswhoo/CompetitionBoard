
using Models.Common;
using Models.Results;

namespace Interfaces
{
    public interface ITitleRepo
    {
        Result<string> Save(string title);

        Result<string> Load();

        Result<string> Clear();
    }
}