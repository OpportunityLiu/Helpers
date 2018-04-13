using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

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
    }

    /// <summary>
    /// Base class of <see cref="Box{T}"/> and <see cref="ReadOnlyBox{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay(@"{Value}")]
    public abstract class BoxBase<T> : IList<T>, IList, IReadOnlyBox<T>
    {
        internal BoxBase() { }
        internal BoxBase(T value) => this.InternalValue = value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal T InternalValue;

        T IReadOnlyBox<T>.Value => this.InternalValue;

        object IReadOnlyBox.Value => this.InternalValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal abstract bool IsReadOnly { get; }

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
            get => index == 0 ? this.InternalValue : throw new ArgumentOutOfRangeException(nameof(index));
            set
            {
                if (index != 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                checkReadOnly();
                this.InternalValue = value;
            }
        }
        T IReadOnlyList<T>.this[int index] => ((IList<T>)this)[index];

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsFixedSize => true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsReadOnly => IsReadOnly;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<T>.IsReadOnly => IsReadOnly;

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

        bool ICollection<T>.Contains(T item) => EqualityComparer<T>.Default.Equals(this.InternalValue, item);
        bool IList.Contains(object value)
        {
            if (value is T item)
                return ((ICollection<T>)this).Contains(item);
            if (value is null)
                return this.InternalValue == null;
            return false;
        }

        int IList<T>.IndexOf(T item) => EqualityComparer<T>.Default.Equals(this.InternalValue, item) ? 0 : -1;
        int IList.IndexOf(object value)
        {
            if (value is T item)
                return ((IList<T>)this).IndexOf(item);
            if (value is null)
                return this.InternalValue == null ? 0 : -1;
            return -1;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            array[arrayIndex] = this.InternalValue;
        }
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array is object[] oa)
            {
                oa[index] = this.InternalValue;
                return;
            }
            if (!(array is T[] a))
                throw new ArgumentException("Wrong array type", nameof(array));
            a[index] = this.InternalValue;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.InternalValue;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return this.InternalValue;
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
}
