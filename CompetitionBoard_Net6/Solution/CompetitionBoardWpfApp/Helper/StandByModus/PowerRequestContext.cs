
using System.Runtime.InteropServices;
using System;

namespace CompetitionBoardWpfApp.Helper.StandByModus
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
