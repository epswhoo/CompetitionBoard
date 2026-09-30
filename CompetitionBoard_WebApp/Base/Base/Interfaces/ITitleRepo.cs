
namespace Interfaces
{
    public interface ITitleRepo
    {
        string Save(string title);

        string Load();

        string Clear();
    }
}