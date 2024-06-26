//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{    
    internal static class Power
    {        
        internal static PowerManagementNativeMethods.SystemPowerCapabilities
            GetSystemPowerCapabilities()
        {
            PowerManagementNativeMethods.SystemPowerCapabilities powerCap;

            uint retval = PowerManagementNativeMethods.CallNtPowerInformation(
              PowerManagementNativeMethods.PowerInformationLevel.SystemPowerCapabilities,
              IntPtr.Zero, 0, out powerCap,
              (uint)Marshal.SizeOf(typeof(PowerManagementNativeMethods.SystemPowerCapabilities))
              );

            if (retval == CoreNativeMethods.StatusAccessDenied)
            {
                throw new UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessCapabilities);
            }

            return powerCap;
        }
        
        internal static PowerManagementNativeMethods.SystemBatteryState GetSystemBatteryState()
        {
            PowerManagementNativeMethods.SystemBatteryState batteryState;

            uint retval = PowerManagementNativeMethods.CallNtPowerInformation(
              PowerManagementNativeMethods.PowerInformationLevel.SystemBatteryState,
              IntPtr.Zero, 0, out batteryState,
              (uint)Marshal.SizeOf(typeof(PowerManagementNativeMethods.SystemBatteryState))
              );

            if (retval == CoreNativeMethods.StatusAccessDenied)
            {
                throw new UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessBatteryState);
            }

            return batteryState;
        }

        /// <summary>
        /// Registers the application to receive power setting notifications 
        /// for the specific power setting event.
        /// </summary>
        /// <param name="handle">Handle indicating where the power setting 
        /// notifications are to be sent.</param>
        /// <param name="powerSetting">The GUID of the power setting for 
        /// which notifications are to be sent.</param>
        /// <returns>Returns a notification handle for unregistering 
        /// power notifications.</returns>
        internal static int RegisterPowerSettingNotification(
            IntPtr handle, Guid powerSetting)
        {
            int outHandle = PowerManagementNativeMethods.RegisterPowerSettingNotification(
                handle,
                ref powerSetting,
                0);

            return outHandle;
        }        
    }
}