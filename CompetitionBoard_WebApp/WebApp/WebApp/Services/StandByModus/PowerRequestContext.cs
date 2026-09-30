using System.Runtime.InteropServices;

namespace WebApp.Services.StandByModus
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PowerRequestContext
    {
        public UInt32 Version;

        public UInt32 Flags;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string SimpleReasonString;
    }
}
