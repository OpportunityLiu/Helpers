using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.System
{
    /// <summary>
    /// Simple box of value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class Box<T> : IBox
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
