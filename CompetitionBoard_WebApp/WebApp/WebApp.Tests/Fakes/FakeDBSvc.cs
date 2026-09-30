using Base.Models.DB;
using Base.Models.RnHs;
using Interfaces;

namespace WebApp.Tests.Fakes
{
    /// <summary>
    /// BoardSvc nutzt vom IDBSvc nur SetDBSettings. Meldungen kommen über FakeRnHsRepo an.
    /// </summary>
    internal class FakeDBSvc : IDBSvc
    {
        public event EventHandler<string> ErrorRised;

        public bool CanConnect { get; set; } = true;

        public string ConnectErrorMessage { get; set; }

        public int SetDBSettingsCount { get; private set; }

        public DBConnectionSettings LastSettings { get; private set; }

        public void RaiseError(string message)
        {
            ErrorRised?.Invoke(this, message);
        }

        public bool SetDBSettings(DBConnectionSettings settings)
        {
            SetDBSettingsCount++;
            LastSettings = settings;
            if (ConnectErrorMessage != null)
            {
                RaiseError(ConnectErrorMessage);
            }
            return CanConnect;
        }

        public string TitleSave(string title) => throw new NotSupportedException();

        public string TitleLoad() => throw new NotSupportedException();

        public RnH Insert(RnH rnh) => throw new NotSupportedException();

        public RnH Save(RnH rnh) => throw new NotSupportedException();

        public bool Delete(RnH rnh) => throw new NotSupportedException();

        public RnH ReadById(int id) => throw new NotSupportedException();

        public IEnumerable<RnH> ReadAll() => throw new NotSupportedException();
    }
}
