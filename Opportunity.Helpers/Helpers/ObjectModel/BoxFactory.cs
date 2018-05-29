namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Factory methods of <see cref="Box{T}"/> and <see cref="ReadOnlyBox{T}"/>.
    /// </summary>
    public static class Box
    {
        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        public static Box<T> Create<T>() => new Box<T>();
        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public static Box<T> Create<T>(T value) => new Box<T>(value);
        /// <summary>
        /// Create new instance of <see cref="ObservableBox{T}"/>.
        /// </summary>
        public static ObservableBox<T> CreateObservable<T>() => new ObservableBox<T>();
        /// <summary>
        /// Create new instance of <see cref="ObservableBox{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public static ObservableBox<T> CreateObservable<T>(T value) => new ObservableBox<T>(value);
        /// <summary>
        /// Create new instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public static ReadOnlyBox<T> CreateReadonly<T>(T value) => new ReadOnlyBox<T>(value);
        /// <summary>
        /// Get default instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        public static ReadOnlyBox<T> GetDefaultReadonly<T>() => Storage<T>.Default;

        private static class Storage<T>
        {
            public static readonly ReadOnlyBox<T> Default = new ReadOnlyBox<T>(default);
        }
    }
}
