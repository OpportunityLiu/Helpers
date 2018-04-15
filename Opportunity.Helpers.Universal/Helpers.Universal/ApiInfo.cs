using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Opportunity.Helpers.Universal
{
    /// <summary>
    /// Provides api info.
    /// </summary>
    public static class ApiInfo
    {
        /// <summary>
        /// Test if current device family is <paramref name="deviceFamily"/>.
        /// </summary>
        /// <param name="deviceFamily">Expacted device family.</param>
        /// <returns>
        /// If <paramref name="deviceFamily"/> is <see langword="null"/>, whitespace <see cref="string"/>, <c>"Universal"</c> or <c>"Windows.Universal"</c>, <see langword="true"/>;
        /// otherwise, returns <see langword="true"/> if current device family is <paramref name="deviceFamily"/>.
        /// </returns>
        public static bool IsDeviceFamily(string deviceFamily)
        {
            if (string.IsNullOrWhiteSpace(deviceFamily))
                return true;

            deviceFamily = deviceFamily.Trim();
            if (!deviceFamily.StartsWith("Windows.", StringComparison.OrdinalIgnoreCase))
                deviceFamily = "Windows." + deviceFamily;

            if (string.Equals("Windows.Universal", deviceFamily.Trim(), StringComparison.OrdinalIgnoreCase))
                return true;

            var df = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            return string.Equals(df, deviceFamily.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get current device family.
        /// </summary>
        public static string DeviceFamily { get; } = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

        /// <summary>
        /// Is current device family <c>"Windows.Mobile"</c>.
        /// </summary>
        public static bool IsMobile { get; } = IsDeviceFamily("Windows.Mobile");
        /// <summary>
        /// Is current device family <c>"Windows.Desktop"</c>.
        /// </summary>
        public static bool IsDesktop { get; } = IsDeviceFamily("Windows.Desktop");
        /// <summary>
        /// Is current device family <c>"Windows.Xbox"</c>.
        /// </summary>
        public static bool IsXbox { get; } = IsDeviceFamily("Windows.Xbox");
        /// <summary>
        /// Is current device family <c>"Windows.Holographic"</c>.
        /// </summary>
        public static bool IsHolographic { get; } = IsDeviceFamily("Windows.Holographic");
        /// <summary>
        /// Is current device family <c>"Windows.IoT"</c>.
        /// </summary>
        public static bool IsIoT { get; } = IsDeviceFamily("Windows.IoT");
        /// <summary>
        /// Is current device family <c>"Windows.Team"</c>.
        /// </summary>
        public static bool IsTeam { get; } = IsDeviceFamily("Windows.Team");

    }
}
