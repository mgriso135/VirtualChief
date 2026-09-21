using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KIS.App_Sources
{
    public static class TimeZoneHelper
    {
        private static readonly Dictionary<string, string> WindowsToIanaMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "W. Europe Standard Time", "Europe/Berlin" },
            { "Central Europe Standard Time", "Europe/Budapest" },
            { "Eastern Standard Time", "America/New_York" },
            { "Central Standard Time", "America/Chicago" },
            { "Mountain Standard Time", "America/Denver" },
            { "Pacific Standard Time", "America/Los_Angeles" },
            { "GMT Standard Time", "Europe/London" },
            { "Romance Standard Time", "Europe/Paris" },
            { "Central European Standard Time", "Europe/Warsaw" },
            { "FLE Standard Time", "Europe/Helsinki" },
            { "GTB Standard Time", "Europe/Athens" },
            { "Russia Time Zone 3", "Europe/Moscow" },
            { "Arab Standard Time", "Asia/Riyadh" },
            { "E. Africa Standard Time", "Africa/Nairobi" },
            { "India Standard Time", "Asia/Kolkata" },
            { "China Standard Time", "Asia/Shanghai" },
            { "Singapore Standard Time", "Asia/Singapore" },
            { "Tokyo Standard Time", "Asia/Tokyo" },
            { "AUS Eastern Standard Time", "Australia/Sydney" },
            { "UTC", "UTC" }
        };

        public static TimeZoneInfo FindTimeZone(string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return GetDefaultTimeZone();
            }

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                if (WindowsToIanaMap.TryGetValue(timeZoneId, out var ianaId))
                {
                    try
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById(ianaId);
                    }
                    catch (TimeZoneNotFoundException)
                    {
                    }
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    try
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                    }
                    catch
                    {
                    }
                }
                else
                {
                    try
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
                    }
                    catch
                    {
                    }
                }

                return TimeZoneInfo.Utc;
            }
            catch (ArgumentNullException)
            {
                return GetDefaultTimeZone();
            }
        }

        private static TimeZoneInfo GetDefaultTimeZone()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); } catch { }
            }
            else
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin"); } catch { }
            }
            return TimeZoneInfo.Utc;
        }
    }
}