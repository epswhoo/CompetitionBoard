using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionBoardWpfApp.Helper.StandByModus
{
    public static class StandBySvc
    {
        private const int POWER_REQUEST_CONTEXT_VERSION = 0;
        private const int POWER_REQUEST_CONTEXT_SIMPLE_STRING = 0x1;

        private static IntPtr currentPowerRequest;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr PowerCreateRequest(ref PowerRequestContext Context);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool PowerSetRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool PowerClearRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        public static void SuppressStandby()
        {
            // Clear current power request if there is any.
            if (currentPowerRequest != IntPtr.Zero)
            {
                PowerClearRequest(currentPowerRequest, PowerRequestType.PowerRequestSystemRequired);
                currentPowerRequest = IntPtr.Zero;
            }

            // Create new power request.
            PowerRequestContext pContext;
            pContext.Flags = POWER_REQUEST_CONTEXT_SIMPLE_STRING;
            pContext.Version = POWER_REQUEST_CONTEXT_VERSION;
            pContext.SimpleReasonString = "Standby suppressed by PowerAvailabilityRequests.exe";

            currentPowerRequest = PowerCreateRequest(ref pContext);

            if (currentPowerRequest == IntPtr.Zero)
            {
                // Failed to create power request.
                var error = Marshal.GetLastWin32Error();

                if (error != 0)
                    throw new Win32Exception(error);
            }

            bool success = PowerSetRequest(currentPowerRequest, PowerRequestType.PowerRequestSystemRequired);

            if (!success)
            {
                // Failed to set power request.
                currentPowerRequest = IntPtr.Zero;
                var error = Marshal.GetLastWin32Error();

                if (error != 0)
                    throw new Win32Exception(error);
            }
        }
    }
}
