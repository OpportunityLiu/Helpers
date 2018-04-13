using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    [DebuggerDisplay(@"{Value}")]
    public class ReadOnlyBox<T> : BoxBase<T>, IReadOnlyBox<T>
    {
        /// <summary>
        /// Default instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        public static ReadOnlyBox<T> Default { get; } = new ReadOnlyBox<T>(default);

        /// <summary>
        /// Create new instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public ReadOnlyBox(T value) : base(value) { }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal override bool IsReadOnly => true;

        /// <summary>
        /// Value in the <see cref="Box{T}"/>
        /// </summary>
        public T Value => this.InternalValue;
    }

    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IReadOnlyBox<T> : IReadOnlyBox, IReadOnlyList<T>, ICollection<T>, IList<T>
    {
        /// <summary>
        /// Value in the <see cref="IBox{T}"/>
        /// </summary>
        new T Value { get; }
    }

    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    public interface IReadOnlyBox : IList
    {
        /// <summary>
        /// Value in the <see cref="IBox"/>
        /// </summary>
        object Value { get; }
    }
}
