using System;
using System.Runtime.InteropServices;

namespace Ddc.Tool.Native.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 128)]
        private char[] szPhysicalMonitorDescription;
        public string MonitorDescription
        {
            get
            {
                return new string(szPhysicalMonitorDescription).TrimEnd('\0');
            }
        }
    }
}
