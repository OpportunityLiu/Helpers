using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Simple box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class Box<T> : IBox<T>, IList<T>, IReadOnlyList<T>, IList
    {
        /// <summary>
        /// Value in the <see cref="Box{T}"/>
        /// </summary>
        public T Value { get; set; }
        object IBox.Value
        {
            get => Value;
            set => Value = (T)value;
        }

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

        T IReadOnlyList<T>.this[int index] => index == 0 ? Value : throw new IndexOutOfRangeException();

        object IList.this[int index]
        {
            get => index == 0 ? Value : throw new IndexOutOfRangeException();
            set
            {
                if (index != 0)
                    throw new IndexOutOfRangeException();
                Value = (T)value;
            }
        }

        int ICollection<T>.Count => 1;
        int IReadOnlyCollection<T>.Count => 1;
        int ICollection.Count => 1;

        bool ICollection<T>.IsReadOnly => false;
        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => true;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        void ICollection<T>.Add(T item) => throw new InvalidOperationException();
        int IList.Add(object value) => throw new InvalidOperationException();
        void ICollection<T>.Clear() => throw new InvalidOperationException();
        void IList.Clear() => throw new InvalidOperationException();
        void IList<T>.Insert(int index, T item) => throw new InvalidOperationException();
        void IList.Insert(int index, object value) => throw new InvalidOperationException();
        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException();
        void IList.Remove(object value) => throw new InvalidOperationException();
        void IList<T>.RemoveAt(int index) => throw new InvalidOperationException();
        void IList.RemoveAt(int index) => throw new InvalidOperationException();

        bool ICollection<T>.Contains(T item) => EqualityComparer<T>.Default.Equals(Value, item);
        bool IList.Contains(object value) => EqualityComparer<object>.Default.Equals(Value, value);

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
    public interface IBox
    {
        /// <summary>
        /// Value in the <see cref="IBox"/>
        /// </summary>
        object Value { get; set; }
    }
}
