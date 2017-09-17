namespace Windows.UI.Xaml
{
    /// <summary>
    /// Extension methods for <see cref="ElementTheme"/> and <see cref="ApplicationTheme"/>.
    /// </summary>
    public static class ThemeExtension
    {
        /// <summary>
        /// Convert <see cref="ApplicationTheme"/> to <see cref="ElementTheme"/>.
        /// </summary>
        /// <param name="value">The <see cref="ApplicationTheme"/> to convert.</param>
        /// <returns>An <see cref="ElementTheme"/> has the same value with <paramref name="value"/>.</returns>
        public static ElementTheme ToElementTheme(this ApplicationTheme value)
        {
            switch (value)
            {
            case ApplicationTheme.Light:
                return ElementTheme.Light;
            case ApplicationTheme.Dark:
                return ElementTheme.Dark;
            default:
                return ElementTheme.Default;
            }
        }

        /// <summary>
        /// Convert <see cref="ElementTheme"/> to <see cref="ApplicationTheme"/>.
        /// </summary>
        /// <param name="value">The <see cref="ElementTheme"/> to convert.</param>
        /// <returns>An <see cref="ApplicationTheme"/> has the same value with <paramref name="value"/>.</returns>
        public static ApplicationTheme ToApplicationTheme(this ElementTheme value)
        {
            switch (value)
            {
            case ElementTheme.Light:
                return ApplicationTheme.Light;
            case ElementTheme.Dark:
                return ApplicationTheme.Dark;
            default:
                return Application.Current.RequestedTheme;
            }
        }
    }
}
