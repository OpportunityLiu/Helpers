using Windows.UI.Xaml;

namespace Windows.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Extension methods for <see cref="ElementTheme"/> and <see cref="ApplicationTheme"/>.
    /// </summary>
    public static class ThemeExtension
    {
        /// <summary>
        /// Convert <see cref="ShareUITheme"/> to <see cref="ElementTheme"/>.
        /// </summary>
        /// <param name="value">The <see cref="ShareUITheme"/> to convert.</param>
        /// <returns>An <see cref="ElementTheme"/> has the same value with <paramref name="value"/>.</returns>
        public static ElementTheme ToElementTheme(this ShareUITheme value)
        {
            switch (value)
            {
            case ShareUITheme.Light:
                return ElementTheme.Light;
            case ShareUITheme.Dark:
                return ElementTheme.Dark;
            default:
                return ElementTheme.Default;
            }
        }

        /// <summary>
        /// Convert <see cref="ShareUITheme"/> to <see cref="ApplicationTheme"/>.
        /// </summary>
        /// <param name="value">The <see cref="ShareUITheme"/> to convert.</param>
        /// <returns>An <see cref="ApplicationTheme"/> has the same value with <paramref name="value"/>.</returns>
        public static ApplicationTheme ToApplicationTheme(this ShareUITheme value)
        {
            switch (value)
            {
            case ShareUITheme.Light:
                return ApplicationTheme.Light;
            case ShareUITheme.Dark:
                return ApplicationTheme.Dark;
            default:
                return Application.Current.RequestedTheme;
            }
        }

        /// <summary>
        /// Convert <see cref="ElementTheme"/> to <see cref="ShareUITheme"/>.
        /// </summary>
        /// <param name="value">The <see cref="ElementTheme"/> to convert.</param>
        /// <returns>An <see cref="ShareUITheme"/> has the same value with <paramref name="value"/>.</returns>
        public static ShareUITheme ToShareUITheme(this ElementTheme value)
        {
            switch (value)
            {
            case ElementTheme.Light:
                return ShareUITheme.Light;
            case ElementTheme.Dark:
                return ShareUITheme.Dark;
            default:
                return ShareUITheme.Default;
            }
        }

        /// <summary>
        /// Convert <see cref="ApplicationTheme"/> to <see cref="ShareUITheme"/>.
        /// </summary>
        /// <param name="value">The <see cref="ApplicationTheme"/> to convert.</param>
        /// <returns>An <see cref="ShareUITheme"/> has the same value with <paramref name="value"/>.</returns>
        public static ShareUITheme ToShareUITheme(this ApplicationTheme value)
        {
            switch (value)
            {
            case ApplicationTheme.Light:
                return ShareUITheme.Light;
            case ApplicationTheme.Dark:
                return ShareUITheme.Dark;
            default:
                return ShareUITheme.Default;
            }
        }
    }
}
