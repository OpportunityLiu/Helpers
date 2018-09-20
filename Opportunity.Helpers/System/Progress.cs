namespace System
{
    /// <summary>
    /// Factory class for <see cref="IProgress{T}"/>.
    /// </summary>
    public static class Progress
    {
        /// <summary>
        /// Create a new <see cref="IProgress{T}"/> from <paramref name="report"/>.
        /// </summary>
        /// <typeparam name="T">Type of progress value.</typeparam>
        /// <param name="report">Report pregress update.</param>
        /// <returns>A <see cref="IProgress{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="report"/> is <see langword="null"/>.</exception>
        public static IProgress<T> Create<T>(Action<T> report)
        {
            if (report is null)
                throw new ArgumentNullException(nameof(report));
            return new ProgressProvider<T>(report);
        }

        private sealed class ProgressProvider<T> : IProgress<T>
        {
            private readonly Action<T> report;

            public ProgressProvider(Action<T> report) => this.report = report;

            public void Report(T value)
            {

            }
        }
    }
}
