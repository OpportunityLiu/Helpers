using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Opportunity.Helpers.ObjectModel
{

    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    [DebuggerDisplay(@"{Value}")]
    public class ReadOnlyBox<T> : IReadOnlyList<T>, IList<T>, IList, IReadOnlyBox<T>
    {
        /// <summary>
        /// Create new instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        protected ReadOnlyBox() { }
        /// <summary>
        /// Create new instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        /// <param name="value">Value in the box.</param>
        public ReadOnlyBox(T value) => this.BoxedValue = value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T value;
        /// <summary>
        /// Actual value in the box.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual T BoxedValue
        {
            get => this.value;
            set => this.value = value;
        }

        /// <summary>
        /// Value in the box.
        /// </summary>
        public T Value => this.BoxedValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IReadOnlyBox.Value => this.BoxedValue;

        /// <summary>
        /// <see langword="true"/> if the box is read only.
        /// </summary>
        public virtual bool IsReadOnly => true;

        private void checkReadOnly()
        {
            if (IsReadOnly)
                throw new NotSupportedException("This collection is read-only.");
        }

        private void checkFixedSize()
        {
            throw new NotSupportedException("This collection is fixed-size.");
        }

        object IList.this[int index]
        {
            get => ((IList<T>)this)[index];
            set => ((IList<T>)this)[index] = (T)value;
        }
        T IList<T>.this[int index]
        {
            get => index == 0 ? this.BoxedValue : throw new ArgumentOutOfRangeException(nameof(index));
            set
            {
                if (index != 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                checkReadOnly();
                this.BoxedValue = value;
            }
        }
        T IReadOnlyList<T>.this[int index] => ((IList<T>)this)[index];

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsFixedSize => true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int ICollection.Count => 1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int ICollection<T>.Count => 1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int IReadOnlyCollection<T>.Count => 1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection.IsSynchronized => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object ICollection.SyncRoot => this;

        bool ICollection<T>.Contains(T item) => EqualityComparer<T>.Default.Equals(this.BoxedValue, item);
        bool IList.Contains(object value)
        {
            if (value is T item)
                return ((ICollection<T>)this).Contains(item);
            if (value is null)
                return this.BoxedValue == null;
            return false;
        }

        int IList<T>.IndexOf(T item) => EqualityComparer<T>.Default.Equals(this.BoxedValue, item) ? 0 : -1;
        int IList.IndexOf(object value)
        {
            if (value is T item)
                return ((IList<T>)this).IndexOf(item);
            if (value is null)
                return this.BoxedValue == null ? 0 : -1;
            return -1;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            array[arrayIndex] = this.BoxedValue;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array is object[] oa)
            {
                oa[index] = this.BoxedValue;
                return;
            }
            if (!(array is T[] a))
                throw new ArgumentException("Wrong array type", nameof(array));
            a[index] = this.BoxedValue;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.BoxedValue;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return this.BoxedValue;
        }

        int IList.Add(object value) { checkFixedSize(); return 0; }
        void ICollection<T>.Add(T item) => checkFixedSize();

        void IList.Insert(int index, object value) => checkFixedSize();
        void IList<T>.Insert(int index, T item) => checkFixedSize();

        void IList.Remove(object value) => checkFixedSize();
        bool ICollection<T>.Remove(T item) { checkFixedSize(); return false; }

        void IList.RemoveAt(int index) => checkFixedSize();
        void IList<T>.RemoveAt(int index) => checkFixedSize();

        void IList.Clear() => checkFixedSize();
        void ICollection<T>.Clear() => checkFixedSize();
    }


    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IReadOnlyBox<out T> : IReadOnlyBox
    {
        /// <summary>
        /// Value in the <see cref="IReadOnlyBox{T}"/>.
        /// </summary>
        new T Value { get; }
    }

    /// <summary>
    /// Simple read-only box of value.
    /// </summary>
    public interface IReadOnlyBox
    {
        /// <summary>
        /// Value in the <see cref="IReadOnlyBox"/>.
        /// </summary>
        object Value { get; }
    }
}
