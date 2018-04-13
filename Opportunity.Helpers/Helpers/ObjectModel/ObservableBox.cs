using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple box of value with change notification.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class ObservableBox<T> : BoxBase<T>, IBox<T>, IReadOnlyBox<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// Create new instance of <see cref="ObservableBox{T}"/>.
        /// </summary>
        public ObservableBox() { }

        /// <summary>
        /// Create new instance of <see cref="ObservableBox{T}"/>.
        /// </summary>
        /// <param name="value">Boxed value.</param>
        public ObservableBox(T value) : base(value) { }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal override bool IsReadOnly => false;

        /// <summary>
        /// Value in the <see cref="ObservableBox{T}"/>
        /// </summary>
        public T Value
        {
            get => this.InternalValue;
            set
            {
                this.InternalValue = value;
                OnValueChanged();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IBox.Value
        {
            get => Value;
            set => Value = (T)value;
        }
        T IList<T>.this[int index]
        {
            get => index == 0 ? this.InternalValue : throw new ArgumentOutOfRangeException(nameof(index));
            set
            {
                if (index != 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                this.Value = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static readonly NotifyCollectionChangedEventArgs cargs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static readonly PropertyChangedEventArgs pargs = new PropertyChangedEventArgs(nameof(Value));
        private void OnValueChanged()
        {
            this.PropertyChanged?.Invoke(this, pargs);
            this.CollectionChanged?.Invoke(this, cargs);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
