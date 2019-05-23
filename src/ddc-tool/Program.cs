using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ddc.Tool.Native.Types;

namespace Ddc.Tool
{
    public partial class Program
    {

        public static void Main(string[] args)
        {
            var hMonitorList = new List<IntPtr>();
            var monitorList = new List<PHYSICAL_MONITOR>();
            var gcHandle = GCHandle.Alloc(hMonitorList);
            NativeInterop.MonitorEnumProc lpfnEnum = delegate (IntPtr hMonitor, IntPtr hDCMonitor, ref RECT lprcMonitor, IntPtr dwData)
            {
                var list = (List<IntPtr>)GCHandle.FromIntPtr(dwData).Target;
                list.Add(hMonitor);
                return true;
            };
            if (NativeInterop.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, lpfnEnum, GCHandle.ToIntPtr(gcHandle)))
            {
                foreach (var hMonitor in hMonitorList)
                {
                    var numberOfMonitors = uint.MinValue;
                    if (NativeInterop.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref numberOfMonitors))
                    {
                        var monitors = new PHYSICAL_MONITOR[numberOfMonitors];
                        if (NativeInterop.GetPhysicalMonitorsFromHMONITOR(hMonitor, numberOfMonitors, monitors))
                        {
                            foreach (var monitor in monitors)
                            {
                                monitorList.Add(monitor);
                            }
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
            }
            else
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            foreach(var monitor in monitorList)
            {
                var brightness = NativeInterop.GetMonitorBrightness(monitor);
                //NativeInterop.SetMonitorBrightness(monitor, (brightness - 0.5));
                Console.WriteLine(monitor.MonitorDescription);
            }

            NativeInterop.DestroyPhysicalMonitors((uint)monitorList.Count, monitorList.ToArray());

        }
    }
}
