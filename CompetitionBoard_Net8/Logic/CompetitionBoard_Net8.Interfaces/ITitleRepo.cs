using CompetitionBoard_Net8.Models.Results;

namespace CompetitionBoard_Net8.Interfaces
{
    public interface ITitleRepo
    {
        Result<string> Save(string title);

        Result<string> Load();

        Result<string> Clear();
    }
}