using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Ddc.Tool.Native.Types;

namespace Ddc.Tool
{
    public static class NativeInterop
    {
        public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hDCMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll", EntryPoint = "EnumDisplayMonitors")]
        public static extern bool EnumDisplayMonitors(
            IntPtr hdc,
            IntPtr lprcClip,
            MonitorEnumProc lpfnEnum,
            IntPtr dwData);

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(
            IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);


        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(
            IntPtr hMonitor,
            uint dwPhysicalMonitorArraySize,
            [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);


        [DllImport("Dxva2.dll", EntryPoint = "GetCapabilitiesStringLength")]
        public static extern bool GetCapabilitiesStringLength(IntPtr hMonitor, ref IntPtr pdwCapabilitiesStringLengthInCharacters);
    }
}
