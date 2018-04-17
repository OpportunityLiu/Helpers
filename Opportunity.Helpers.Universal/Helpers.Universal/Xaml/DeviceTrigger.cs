using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Opportunity.Helpers.Universal.Xaml
{
    /// <summary>
    /// State trigger for device family.
    /// </summary>
    public sealed class DeviceTrigger : StateTriggerBase
    {
        /// <summary>
        /// Active status when current device family matches <see cref="DeviceFamily"/>.
        /// </summary>
        public bool ActiveStatusOfDeviceFamily
        {
            get => (bool)GetValue(ActiveStatusOfDeviceFamilyProperty);
            set => SetValue(ActiveStatusOfDeviceFamilyProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="ActiveStatusOfDeviceFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActiveStatusOfDeviceFamilyProperty =
            DependencyProperty.Register(nameof(ActiveStatusOfDeviceFamily), typeof(bool), typeof(DeviceTrigger), new PropertyMetadata(true, ActiveStatusOfDeviceFamilyPropertyChanged));

        private static void ActiveStatusOfDeviceFamilyPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;
            if (oldValue == newValue)
                return;
            var sender = (DeviceTrigger)dp;
            sender.set();
        }

        /// <summary>
        /// Device family of the trigger.
        /// </summary>
        public string DeviceFamily
        {
            get => (string)GetValue(DeviceFamilyProperty);
            set => SetValue(DeviceFamilyProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="DeviceFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeviceFamilyProperty =
            DependencyProperty.Register(nameof(DeviceFamily), typeof(string), typeof(DeviceTrigger), new PropertyMetadata("", DeviceFamilyPropertyChanged));

        private static void DeviceFamilyPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (string)e.OldValue;
            var newValue = (string)e.NewValue;
            if (string.Equals(newValue, oldValue, StringComparison.OrdinalIgnoreCase))
                return;
            var sender = (DeviceTrigger)dp;
            sender.set();
        }

        private void set()
        {
            if (ActiveStatusOfDeviceFamily)
                SetActive(ApiInfo.IsDeviceFamily(DeviceFamily));
            else
                SetActive(!ApiInfo.IsDeviceFamily(DeviceFamily));
        }
    }
}
