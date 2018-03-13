using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace System
{
    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class ReadOnlyBox<T> : IReadOnlyBox<T>
    {
        /// <summary>
        /// Default instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        public static ReadOnlyBox<T> Default { get; } = new ReadOnlyBox<T>(default);

        /// <summary>
        /// Create new instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public ReadOnlyBox(T value) => this.value = value;

        private readonly T value;
        /// <summary>
        /// Value in the <see cref="Box{T}"/>
        /// </summary>
        public T Value => this.value;

        object IReadOnlyBox.Value => Value;

        T IReadOnlyList<T>.this[int index] => ((IList<T>)this)[index];

        object IList.this[int index]
        {
            get => ((IList<T>)this)[index];
            set => throw new NotSupportedException("Read only.");
        }

        int IReadOnlyCollection<T>.Count => 1;
        int ICollection.Count => 1;
        int ICollection<T>.Count => 1;

        bool IList.IsReadOnly => true;
        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsFixedSize => true;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        void ICollection<T>.Add(T item) => throw new NotSupportedException("Read only.");
        int IList.Add(object value) => throw new NotSupportedException("Read only.");
        void ICollection<T>.Clear() => throw new NotSupportedException("Read only.");
        void IList.Clear() => throw new NotSupportedException("Read only.");
        void IList.Insert(int index, object value) => throw new NotSupportedException("Read only.");
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException("Read only.");
        void IList.Remove(object value) => throw new NotSupportedException("Read only.");
        void IList.RemoveAt(int index) => throw new NotSupportedException("Read only.");

        bool ICollection<T>.Contains(T item) => EqualityComparer<T>.Default.Equals(Value, item);
        bool IList.Contains(object value)
        {
            if (value is T item)
                return ((ICollection<T>)this).Contains(item);
            if (value is null)
                return Value == null;
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            array[arrayIndex] = Value;
        }
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array is object[] oa)
            {
                oa[index] = Value;
                return;
            }
            if (!(array is T[] a))
                throw new ArgumentException("Wrong array type", nameof(array));
            a[index] = Value;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return Value;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Value;
        }

        int IList.IndexOf(object value) => EqualityComparer<object>.Default.Equals(Value, value) ? 0 : -1;
    }

    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IReadOnlyBox<T> : IReadOnlyBox, IReadOnlyList<T>, ICollection<T>
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
