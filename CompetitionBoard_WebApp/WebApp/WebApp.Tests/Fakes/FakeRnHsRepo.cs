using Base.Models.RnHs;
using Interfaces;

namespace WebApp.Tests.Fakes
{
    /// <summary>
    /// Merkt sich alle Aufrufe und gibt bei ReadAll die Liste <see cref="RnHs"/> zurück.
    /// Leitet wie RnHsRepo die Meldungen des IDBSvc weiter.
    /// </summary>
    internal class FakeRnHsRepo : IRnHsRepo
    {
        public event EventHandler<string> ErrorRised;

        public FakeRnHsRepo(IDBSvc dbSvc)
        {
            dbSvc.ErrorRised += (sender, message) => ErrorRised?.Invoke(sender, message);
        }

        public List<RnH> RnHs { get; set; } = new List<RnH>();

        public List<(string Method, object Arg)> Calls { get; } = new List<(string Method, object Arg)>();

        /// <summary>Wird bei jedem schreibenden Aufruf über ErrorRised gemeldet.</summary>
        public string ErrorOnCall { get; set; }

        public Exception ThrowOnReadAll { get; set; }

        public void RaiseError(string message)
        {
            ErrorRised?.Invoke(this, message);
        }

        public IEnumerable<RnH> ReadAll()
        {
            if (ThrowOnReadAll != null)
            {
                throw ThrowOnReadAll;
            }
            return RnHs;
        }

        public IEnumerable<RnH> SetNewRnHs(string str) => Record<IEnumerable<RnH>>(nameof(SetNewRnHs), str, null);

        public RnH Save(RnH rnh) => Record(nameof(Save), rnh, rnh);

        public bool Delete(RnH rnh) => Record(nameof(Delete), rnh, true);

        public bool DeleteAll() => Record(nameof(DeleteAll), null, true);

        public RnH InsertNewWithOrder(int order) => Record(nameof(InsertNewWithOrder), order, new RnH { Order = order });

        public RnH SaveIsRanked(RnH rnh) => Record(nameof(SaveIsRanked), rnh, rnh);

        public RnH SaveIsDisqualificated(RnH rnh) => Record(nameof(SaveIsDisqualificated), rnh, rnh);

        public RnH SaveMark(RnH rnh) => Record(nameof(SaveMark), rnh, rnh);

        public RnH SaveStatus(RnH rnh) => Record(nameof(SaveStatus), rnh, rnh);

        public RnH SaveHorseNo(RnH rnh) => Record(nameof(SaveHorseNo), rnh, rnh);

        private T Record<T>(string method, object arg, T result)
        {
            Calls.Add((method, arg));
            if (ErrorOnCall != null)
            {
                RaiseError(ErrorOnCall);
                return default;
            }
            return result;
        }
    }
}
