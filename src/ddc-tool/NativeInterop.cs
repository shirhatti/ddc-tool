using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Ddc.Tool.Native.Types;

namespace Ddc.Tool
{
    public static class NativeInterop
    {
        private const int MC_CAPS_BRIGHTNESS = 0x2;
        private const int MC_CAPS_CONTRAST = 0x4;
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


        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetMonitorCapabilities(IntPtr hMonitor, out uint pdwMonitorCapabilities, out uint pdwSupportedColorTemperatures);

        [DllImport("Dxva2.dll", EntryPoint = "GetCapabilitiesStringLength")]
        public static extern bool GetCapabilitiesStringLength(IntPtr hMonitor, ref IntPtr pdwCapabilitiesStringLengthInCharacters);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetMonitorBrightness(IntPtr hMonitor, out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool SetMonitorBrightness(IntPtr hMonitor, uint dwNewBrightness);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetMonitorContrast(IntPtr hMonitor, out uint pdwMinimumContrast, out uint pdwCurrentContrast, out uint pdwMaximumContrast);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool SetMonitorContrast(IntPtr hMonitor, uint dwNewContrast);

        [DllImport("dxva2.dll", SetLastError = true)]
        public extern static bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        private static uint GetMonitorCapabilities(PHYSICAL_MONITOR physicalMonitor)
        {
            uint dwMonitorCapabilities, dwSupportedColorTemperatures;
            if (!GetMonitorCapabilities(physicalMonitor.hPhysicalMonitor, out dwMonitorCapabilities, out dwSupportedColorTemperatures))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return dwMonitorCapabilities;
        }

        public static bool GetBrightnessSupport(PHYSICAL_MONITOR physicalMonitor)
        {
            return (GetMonitorCapabilities(physicalMonitor) & MC_CAPS_BRIGHTNESS) != 0;
        }

        public static bool GetContrastSupport(PHYSICAL_MONITOR physicalMonitor)
        {
            return (GetMonitorCapabilities(physicalMonitor) & MC_CAPS_CONTRAST) != 0;
        }

        public static double GetMonitorBrightness(PHYSICAL_MONITOR physicalMonitor)
        {
            uint dwMinimumBrightness, dwCurrentBrightness, dwMaximumBrightness;
            if (!GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out dwMinimumBrightness, out dwCurrentBrightness, out dwMaximumBrightness))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return (double)(dwCurrentBrightness - dwMinimumBrightness) / (double)(dwMaximumBrightness - dwMinimumBrightness);
        }

        public static void SetMonitorBrightness(PHYSICAL_MONITOR physicalMonitor, double brightness)
        {
            uint dwMinimumBrightness, dwCurrentBrightness, dwMaximumBrightness;
            if (!GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out dwMinimumBrightness, out dwCurrentBrightness, out dwMaximumBrightness))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            if (!SetMonitorBrightness(physicalMonitor.hPhysicalMonitor, (uint)(dwMinimumBrightness + (dwMaximumBrightness - dwMinimumBrightness) * brightness)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static double GetMonitorContrast(PHYSICAL_MONITOR physicalMonitor)
        {
            uint dwMinimumContrast, dwCurrentContrast, dwMaximumContrast;
            if (!GetMonitorContrast(physicalMonitor.hPhysicalMonitor, out dwMinimumContrast, out dwCurrentContrast, out dwMaximumContrast))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return (double)(dwCurrentContrast - dwMinimumContrast) / (double)(dwMaximumContrast - dwMinimumContrast);
        }

        public static void SetMonitorContrast(PHYSICAL_MONITOR physicalMonitor, double contrast)
        {
            uint dwMinimumContrast, dwCurrentContrast, dwMaximumContrast;
            if (!GetMonitorContrast(physicalMonitor.hPhysicalMonitor, out dwMinimumContrast, out dwCurrentContrast, out dwMaximumContrast))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            if (!SetMonitorContrast(physicalMonitor.hPhysicalMonitor, (uint)(dwMinimumContrast + (dwMaximumContrast - dwMinimumContrast) * contrast)))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
