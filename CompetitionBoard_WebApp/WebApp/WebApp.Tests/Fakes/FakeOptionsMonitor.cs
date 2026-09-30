using Microsoft.Extensions.Options;

namespace WebApp.Tests.Fakes
{
    /// <summary>
    /// Liefert immer den gesetzten Wert, ohne Datei und ohne Nachladen.
    /// </summary>
    internal class FakeOptionsMonitor<T> : IOptionsMonitor<T>
    {
        public FakeOptionsMonitor(T currentValue)
        {
            CurrentValue = currentValue;
        }

        public T CurrentValue { get; set; }

        public T Get(string name) => CurrentValue;

        public IDisposable OnChange(Action<T, string> listener) => null;
    }
}
