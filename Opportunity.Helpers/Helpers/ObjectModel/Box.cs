using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class Box<T> : ReadOnlyBox<T>, IBox<T>
    {
        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        public Box() { }

        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public Box(T value) : base(value) { }

        /// <summary>
        /// <see langword="true"/> if the box is read only.
        /// </summary>
        public override bool IsReadOnly => false;

        /// <summary>
        /// Value in the <see cref="Box{T}"/>.
        /// </summary>
        public new T Value { get => this.BoxedValue; set => this.BoxedValue = value; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IBox.Value
        {
            get => Value;
            set => Value = (T)value;
        }
    }

    /// <summary>
    /// Simple box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IBox<T> : IBox
    {
        /// <summary>
        /// Value in the <see cref="IBox{T}"/>
        /// </summary>
        new T Value { get; set; }
    }

    /// <summary>
    /// Simple box of value.
    /// </summary>
    public interface IBox : IList
    {
        /// <summary>
        /// Value in the <see cref="IBox"/>
        /// </summary>
        object Value { get; set; }
    }
}
