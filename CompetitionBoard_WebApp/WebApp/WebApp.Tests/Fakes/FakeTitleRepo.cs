using Interfaces;

namespace WebApp.Tests.Fakes
{
    internal class FakeTitleRepo : ITitleRepo
    {
        public string Title { get; set; } = string.Empty;

        public string Save(string title)
        {
            Title = title;
            return Title;
        }

        public string Load()
        {
            return Title;
        }

        public string Clear()
        {
            Title = string.Empty;
            return Title;
        }
    }
}
