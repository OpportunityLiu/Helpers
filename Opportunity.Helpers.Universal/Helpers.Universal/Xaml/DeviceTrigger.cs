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
        /// Device family to deactive the trigger.
        /// </summary>
        public string DeactiveDeviceFamily
        {
            get => (string)GetValue(DeactiveDeviceFamilyProperty);
            set => SetValue(DeactiveDeviceFamilyProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="DeactiveDeviceFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeactiveDeviceFamilyProperty =
            DependencyProperty.Register(nameof(DeactiveDeviceFamily), typeof(string), typeof(DeviceTrigger), new PropertyMetadata("", DeactiveDeviceFamilyPropertyChanged));

        private static void DeactiveDeviceFamilyPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (string)e.OldValue;
            var newValue = (string)e.NewValue;
            var sender = (DeviceTrigger)dp;
            if (string.Equals(newValue, oldValue, StringComparison.OrdinalIgnoreCase))
                return;
            sender.set();
        }

        /// <summary>
        /// Device family to active the trigger.
        /// </summary>
        public string ActiveDeviceFamily
        {
            get => (string)GetValue(ActiveDeviceFamilyProperty);
            set => SetValue(ActiveDeviceFamilyProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="ActiveDeviceFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActiveDeviceFamilyProperty =
            DependencyProperty.Register(nameof(ActiveDeviceFamily), typeof(string), typeof(DeviceTrigger), new PropertyMetadata("", ActiveDeviceFamilyPropertyChanged));

        private static void ActiveDeviceFamilyPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (string)e.OldValue;
            var newValue = (string)e.NewValue;
            var sender = (DeviceTrigger)dp;
            if (string.Equals(newValue, oldValue, StringComparison.OrdinalIgnoreCase))
                return;
            sender.set();
        }

        private void set()
        {
            var a = ActiveDeviceFamily;
            var d = DeactiveDeviceFamily;
            var an = string.IsNullOrWhiteSpace(a);
            var dn = string.IsNullOrWhiteSpace(d);
            if (an && dn)
                SetActive(true);
            else if (dn)
                SetActive(ApiInfo.IsDeviceFamily(a));
            else if (an)
                SetActive(ApiInfo.IsDeviceFamily(d));
            else
                throw new InvalidOperationException(nameof(ActiveDeviceFamily) + " and " + nameof(DeactiveDeviceFamily) + " set at same time.");
        }
    }
}
