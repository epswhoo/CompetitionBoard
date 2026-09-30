using System.Globalization;
using Base.Models.DB;
using Base.Models.RnHs;
using Interfaces;
using Microsoft.Extensions.Options;
using WebApp.Configs;

namespace WebApp.Services.Board
{
    /// <summary>
    /// Zustand der Anzeigetafel für eine Browser-Verbindung (Blazor-Circuit).
    /// Entspricht dem CompetitionBoardViewModel, RnHsViewModel, TitleViewModel
    /// und dem TimerSvc der WPF-Anwendung.
    /// </summary>
    public class BoardSvc : IDisposable
    {
        public const int ColumnCount = 5;

        private static readonly CultureInfo MarkCulture = CultureInfo.GetCultureInfo("de-DE");

        private readonly IDBSvc _dbSvc;
        private readonly IRnHsRepo _rnHsRepo;
        private readonly ITitleRepo _titleRepo;
        private readonly DBConnectionSettings _dbSettings;
        private readonly UIConfig _uiConfig;

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly List<string> _errors = new List<string>();
        private readonly object _errorsLock = new object();

        private Timer _timer;
        private bool _isStarted;
        private bool _isDisposed;

        public event Action Changed;

        public string Title { get; private set; } = string.Empty;

        public IReadOnlyList<IReadOnlyList<RnH>> Columns { get; private set; } = CreateEmptyColumns();

        public int RowCount { get; private set; }

        public bool IsEditModus { get; private set; }

        public bool IsEditListModus { get; private set; }

        /// <summary>
        /// Über den Anzeige-Port verbunden: der Bearbeitungsmodus lässt sich nicht einschalten.
        /// </summary>
        public bool IsDisplayOnly { get; private set; }

        public string ErrorMessage
        {
            get
            {
                lock (_errorsLock)
                {
                    return string.Join(Environment.NewLine, _errors);
                }
            }
        }

        public BoardSvc(IDBSvc dbSvc, IRnHsRepo rnHsRepo, ITitleRepo titleRepo,
            IOptions<DBConnectionSettings> dbSettings, IOptions<UIConfig> uiConfig)
        {
            _dbSvc = dbSvc;
            _rnHsRepo = rnHsRepo;
            _titleRepo = titleRepo;
            _dbSettings = dbSettings.Value;
            _uiConfig = uiConfig.Value;

            // RnHsRepo leitet auch die Meldungen des IDBSvc weiter.
            _rnHsRepo.ErrorRised += OnErrorRised;
        }

        public static string FormatMark(double mark)
        {
            return mark.ToString("0.0", MarkCulture);
        }

        public static string FormatMarkForEdit(double mark)
        {
            return mark.ToString(MarkCulture);
        }

        public async Task StartAsync()
        {
            if (_isStarted)
            {
                return;
            }
            _isStarted = true;

            await RunLockedAsync(() =>
            {
                bool isConnected = _dbSvc.SetDBSettings(_dbSettings);
                if (!isConnected && string.IsNullOrEmpty(ErrorMessage))
                {
                    AddError($"Keine Verbindung zur Datenbank {_dbSettings.DB} auf {_dbSettings.Server}.");
                }
            });

            TimeSpan interval = GetTimerInterval();
            _timer = new Timer(_ => _ = OnTimerElapsedAsync(), null, interval, interval);
        }

        public void SetDisplayOnly()
        {
            IsDisplayOnly = true;
            IsEditModus = false;
            IsEditListModus = false;
        }

        public async Task SetEditModusAsync(bool isEditModus)
        {
            IsEditModus = isEditModus && !IsDisplayOnly;
            if (!IsEditModus)
            {
                IsEditListModus = false;
            }
            TimeSpan interval = GetTimerInterval();
            _timer?.Change(interval, interval);
            await RunLockedAsync(() => { });
        }

        public void SetEditListModus(bool isEditListModus)
        {
            IsEditListModus = IsEditModus && isEditListModus;
            RaiseChanged();
        }

        public Task SaveTitleAsync(string title)
        {
            return RunUserActionAsync(() => _titleRepo.Save(title));
        }

        public Task AddNewListAsync(string newListStr)
        {
            if (string.IsNullOrWhiteSpace(newListStr))
            {
                return ReportErrorAsync("Die Liste der Startnummern ist leer.");
            }
            return RunUserActionAsync(() => _rnHsRepo.SetNewRnHs(newListStr));
        }

        public Task SaveMarkAsync(RnH rnh, string markStr)
        {
            string normalized = (markStr ?? string.Empty).Trim().Replace(',', '.');
            if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double mark))
            {
                return ReportErrorAsync("Die Note ist kein gültiger Zahlenwert");
            }
            RnH toSave = Copy(rnh);
            toSave.Mark = mark;
            return RunUserActionAsync(() => _rnHsRepo.SaveMark(toSave));
        }

        public Task SaveHorseNoAsync(RnH rnh, string horseNoStr)
        {
            if (!int.TryParse((horseNoStr ?? string.Empty).Trim(), out int horseNo))
            {
                return ReportErrorAsync("Die Startnummer ist kein gültiger Zahlenwert");
            }
            RnH toSave = Copy(rnh);
            toSave.HorseNo = horseNo;
            return RunUserActionAsync(() => _rnHsRepo.SaveHorseNo(toSave));
        }

        public Task SaveStatusAsync(RnH rnh, RnHStatus status)
        {
            RnH toSave = Copy(rnh);
            toSave.Status = status;
            return RunUserActionAsync(() => _rnHsRepo.SaveStatus(toSave));
        }

        public Task SaveIsRankedAsync(RnH rnh, bool isRanked)
        {
            RnH toSave = Copy(rnh);
            toSave.IsRanked = isRanked;
            return RunUserActionAsync(() => _rnHsRepo.SaveIsRanked(toSave));
        }

        public Task SaveIsDisqualificatedAsync(RnH rnh, bool isDisqualificated)
        {
            RnH toSave = Copy(rnh);
            toSave.IsDisqualificated = isDisqualificated;
            return RunUserActionAsync(() => _rnHsRepo.SaveIsDisqualificated(toSave));
        }

        public Task AddPreviousAsync(RnH rnh)
        {
            return RunUserActionAsync(() => _rnHsRepo.InsertNewWithOrder(rnh.Order));
        }

        public Task AddFollowingAsync(RnH rnh)
        {
            return RunUserActionAsync(() => _rnHsRepo.InsertNewWithOrder(rnh.Order + 1));
        }

        public Task DeleteAsync(RnH rnh)
        {
            return RunUserActionAsync(() => _rnHsRepo.Delete(Copy(rnh)));
        }

        private async Task OnTimerElapsedAsync()
        {
            if (_isDisposed)
            {
                return;
            }
            ClearErrors();
            await RunLockedAsync(() => { });
        }

        private Task RunUserActionAsync(Action action)
        {
            ClearErrors();
            return RunLockedAsync(action);
        }

        private Task RunUserActionAsync<T>(Func<T> action)
        {
            return RunUserActionAsync(() => { _ = action(); });
        }

        private async Task RunLockedAsync(Action action)
        {
            if (_isDisposed)
            {
                return;
            }
            await _lock.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    TryRun(action);
                    TryRun(Load);
                });
            }
            finally
            {
                _lock.Release();
            }
            RaiseChanged();
        }

        private Task ReportErrorAsync(string message)
        {
            ClearErrors();
            AddError(message);
            RaiseChanged();
            return Task.CompletedTask;
        }

        private void TryRun(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }

        private void Load()
        {
            string title = _titleRepo.Load();
            if (title != null)
            {
                Title = title;
            }

            IEnumerable<RnH> rnhs = _rnHsRepo.ReadAll();
            if (rnhs != null)
            {
                SetColumns(rnhs);
            }
        }

        private void SetColumns(IEnumerable<RnH> total)
        {
            List<RnH> totalOrdered = total.OrderBy(r => r.Order).ToList();
            int totalCount = totalOrdered.Count;
            int usedColumnCount = totalCount > 20 ? 5 : 4;
            int rowCount = GausNo.Get(totalCount, usedColumnCount);

            List<IReadOnlyList<RnH>> columns = new List<IReadOnlyList<RnH>>();
            for (int currentColumn = 0; currentColumn < ColumnCount; currentColumn++)
            {
                columns.Add(totalOrdered
                    .Skip(currentColumn * rowCount)
                    .Take(rowCount)
                    .ToList());
            }
            RowCount = rowCount;
            Columns = columns;
        }

        private TimeSpan GetTimerInterval()
        {
            int seconds = IsEditModus ? _uiConfig.EditTime : _uiConfig.RefreshTime;
            return TimeSpan.FromSeconds(Math.Max(1, seconds));
        }

        private void OnErrorRised(object sender, string message)
        {
            AddError(message);
        }

        private void AddError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }
            lock (_errorsLock)
            {
                if (!_errors.Contains(message))
                {
                    _errors.Add(message);
                }
            }
        }

        private void ClearErrors()
        {
            lock (_errorsLock)
            {
                _errors.Clear();
            }
        }

        private void RaiseChanged()
        {
            if (!_isDisposed)
            {
                Changed?.Invoke();
            }
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

        private static IReadOnlyList<IReadOnlyList<RnH>> CreateEmptyColumns()
        {
            return Enumerable.Range(0, ColumnCount)
                .Select(_ => (IReadOnlyList<RnH>)new List<RnH>())
                .ToList();
        }

        public void Dispose()
        {
            _isDisposed = true;
            _timer?.Dispose();
            _rnHsRepo.ErrorRised -= OnErrorRised;
        }
    }
}
