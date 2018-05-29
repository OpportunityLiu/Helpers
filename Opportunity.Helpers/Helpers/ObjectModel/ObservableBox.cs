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
    public class ObservableBox<T> : Box<T>, INotifyPropertyChanged, INotifyCollectionChanged
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

        /// <summary>
        /// Actual value in the box.
        /// </summary>
        protected override T BoxedValue
        {
            get => base.BoxedValue;
            set
            {
                this.BoxedValue = value;
                OnValueChanged();
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
