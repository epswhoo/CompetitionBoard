using Base.Models.DB;
using Base.Models.RnHs;
using Interfaces;

namespace Repos.Tests.Fakes
{
    /// <summary>
    /// IDBSvc im Speicher. Verhält sich wie DBSvc: gibt nur Kopien heraus,
    /// meldet Fehler über ErrorRised und gibt dann default zurück.
    /// </summary>
    internal class InMemoryDBSvc : IDBSvc
    {
        private readonly List<RnH> _rnhs = new List<RnH>();
        private int _nextId = 1;
        private string _title = string.Empty;

        public event EventHandler<string> ErrorRised;

        /// <summary>Id, bei der Save fehlschlägt.</summary>
        public int? FailSaveForId { get; set; }

        /// <summary>Id, bei der Delete fehlschlägt.</summary>
        public int? FailDeleteForId { get; set; }

        public bool FailReadAll { get; set; }

        /// <summary>Wird statt ReadById geworfen, um Exceptions im Repo zu testen.</summary>
        public Exception ThrowOnReadById { get; set; }

        public IReadOnlyList<RnH> Stored => _rnhs.OrderBy(r => r.Order).Select(Copy).ToList();

        public RnH Add(int horseNo, int order, RnHStatus status = RnHStatus.NotPresent, double mark = 0.0,
            bool isDisqualificated = false, bool isRanked = false)
        {
            RnH rnh = new RnH
            {
                Id = _nextId++,
                HorseNo = horseNo,
                Order = order,
                Status = status,
                Mark = mark,
                IsDisqualificated = isDisqualificated,
                IsRanked = isRanked
            };
            _rnhs.Add(rnh);
            return Copy(rnh);
        }

        public void RaiseError(string message)
        {
            ErrorRised?.Invoke(this, message);
        }

        public bool SetDBSettings(DBConnectionSettings settings)
        {
            return true;
        }

        public string TitleSave(string title)
        {
            _title = title;
            return _title;
        }

        public string TitleLoad()
        {
            return _title;
        }

        public RnH Insert(RnH rnh)
        {
            RnH toInsert = Copy(rnh);
            toInsert.Id = _nextId++;
            _rnhs.Add(toInsert);
            return Copy(toInsert);
        }

        public RnH Save(RnH rnh)
        {
            RnH stored = _rnhs.FirstOrDefault(r => r.Id == rnh.Id);
            if (stored == null || rnh.Id == FailSaveForId)
            {
                return Fail<RnH>($"Save für Id {rnh.Id} fehlgeschlagen.");
            }
            _rnhs.Remove(stored);
            _rnhs.Add(Copy(rnh));
            return Copy(rnh);
        }

        public bool Delete(RnH rnh)
        {
            RnH stored = _rnhs.FirstOrDefault(r => r.Id == rnh.Id);
            if (stored == null || rnh.Id == FailDeleteForId)
            {
                return Fail<bool>($"Delete für Id {rnh.Id} fehlgeschlagen.");
            }
            _rnhs.Remove(stored);
            return true;
        }

        public RnH ReadById(int id)
        {
            if (ThrowOnReadById != null)
            {
                throw ThrowOnReadById;
            }
            RnH stored = _rnhs.FirstOrDefault(r => r.Id == id);
            if (stored == null)
            {
                return Fail<RnH>($"Reiter und Pferd mit Id {id} nicht gefunden.");
            }
            return Copy(stored);
        }

        public IEnumerable<RnH> ReadAll()
        {
            if (FailReadAll)
            {
                return Fail<IEnumerable<RnH>>("ReadAll fehlgeschlagen.");
            }
            return Stored;
        }

        private T Fail<T>(string message)
        {
            ErrorRised?.Invoke(this, message);
            return default;
        }

        private static RnH Copy(RnH rnh)
        {
            return new RnH
            {
                Id = rnh.Id,
                HorseNo = rnh.HorseNo,
                Order = rnh.Order,
                Status = rnh.Status,
                Mark = rnh.Mark,
                IsDisqualificated = rnh.IsDisqualificated,
                IsRanked = rnh.IsRanked
            };
        }
    }
}
