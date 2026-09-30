using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WebApp.Services.StandByModus
{
    /// <summary>
    /// Verhindert, dass der Rechner in den Standby geht, solange der Dienst läuft.
    /// </summary>
    public class StandBySvc : IHostedService
    {
        private const int POWER_REQUEST_CONTEXT_VERSION = 0;
        private const int POWER_REQUEST_CONTEXT_SIMPLE_STRING = 0x1;

        private readonly ILogger<StandBySvc> _logger;

        private IntPtr _currentPowerRequest;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr PowerCreateRequest(ref PowerRequestContext Context);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool PowerSetRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool PowerClearRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        public StandBySvc(ILogger<StandBySvc> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!OperatingSystem.IsWindows())
            {
                return Task.CompletedTask;
            }
            try
            {
                SuppressStandby();
            }
            catch (Win32Exception ex)
            {
                _logger.LogWarning(ex, "Standby konnte nicht unterdrückt werden.");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ClearRequest();
            return Task.CompletedTask;
        }

        private void SuppressStandby()
        {
            ClearRequest();

            PowerRequestContext pContext;
            pContext.Flags = POWER_REQUEST_CONTEXT_SIMPLE_STRING;
            pContext.Version = POWER_REQUEST_CONTEXT_VERSION;
            pContext.SimpleReasonString = "Standby suppressed by CompetitionBoard";

            _currentPowerRequest = PowerCreateRequest(ref pContext);
            if (_currentPowerRequest == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (!PowerSetRequest(_currentPowerRequest, PowerRequestType.PowerRequestSystemRequired))
            {
                int error = Marshal.GetLastWin32Error();
                CloseHandle(_currentPowerRequest);
                _currentPowerRequest = IntPtr.Zero;
                throw new Win32Exception(error);
            }
        }

        private void ClearRequest()
        {
            if (_currentPowerRequest == IntPtr.Zero)
            {
                return;
            }
            PowerClearRequest(_currentPowerRequest, PowerRequestType.PowerRequestSystemRequired);
            CloseHandle(_currentPowerRequest);
            _currentPowerRequest = IntPtr.Zero;
        }
    }
}
