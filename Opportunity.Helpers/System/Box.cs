using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace System
{
    /// <summary>
    /// Factory methods of <see cref="Box{T}"/> and <see cref="ReadOnlyBox{T}"/>.
    /// </summary>
    public static class Box
    {
        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public static Box<T> Create<T>(T value) => new Box<T>(value);
        /// <summary>
        /// Create new instance of <see cref="ReadOnlyBox{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public static ReadOnlyBox<T> CreateReadonly<T>(T value) => new ReadOnlyBox<T>(value);
    }

    /// <summary>
    /// Simple box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class Box<T> : IBox<T>, IReadOnlyBox<T>
    {
        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        public Box() { }

        /// <summary>
        /// Create new instance of <see cref="Box{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public Box(T value) => this.value = value;

        private T value;
        /// <summary>
        /// Value in the <see cref="Box{T}"/>
        /// </summary>
        public T Value
        {
            get => this.value;
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }

        private void OnValueChanged()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        object IBox.Value
        {
            get => Value;
            set => Value = (T)value;
        }
        object IReadOnlyBox.Value => Value;

        T IList<T>.this[int index]
        {
            get => index == 0 ? Value : throw new IndexOutOfRangeException();
            set
            {
                if (index != 0)
                    throw new IndexOutOfRangeException();
                Value = value;
            }
        }

        T IReadOnlyList<T>.this[int index] => ((IList<T>)this)[index];

        object IList.this[int index]
        {
            get => ((IList<T>)this)[index];
            set => ((IList<T>)this)[index] = (T)value;
        }

        int ICollection<T>.Count => 1;
        int IReadOnlyCollection<T>.Count => 1;
        int ICollection.Count => 1;

        bool ICollection<T>.IsReadOnly => false;
        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => true;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        void ICollection<T>.Add(T item) => throw new NotSupportedException("Fixed size.");
        int IList.Add(object value) => throw new NotSupportedException("Fixed size.");
        void ICollection<T>.Clear() => throw new NotSupportedException("Fixed size.");
        void IList.Clear() => throw new NotSupportedException("Fixed size.");
        void IList<T>.Insert(int index, T item) => throw new NotSupportedException("Fixed size.");
        void IList.Insert(int index, object value) => throw new NotSupportedException("Fixed size.");
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException("Fixed size.");
        void IList.Remove(object value) => throw new NotSupportedException("Fixed size.");
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException("Fixed size.");
        void IList.RemoveAt(int index) => throw new NotSupportedException("Fixed size.");

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

        int IList<T>.IndexOf(T item) => EqualityComparer<T>.Default.Equals(Value, item) ? 0 : -1;
        int IList.IndexOf(object value) => EqualityComparer<object>.Default.Equals(Value, value) ? 0 : -1;
    }

    /// <summary>
    /// Simple box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IBox<T> : IBox, IList<T>, IReadOnlyList<T>
    {
        /// <summary>
        /// Value in the <see cref="IBox{T}"/>
        /// </summary>
        new T Value { get; set; }
    }

    /// <summary>
    /// Simple box of value.
    /// </summary>
    public interface IBox : IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// Value in the <see cref="IBox"/>
        /// </summary>
        object Value { get; set; }
    }
}
