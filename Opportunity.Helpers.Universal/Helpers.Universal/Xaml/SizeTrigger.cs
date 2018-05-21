using Windows.UI.Xaml;

namespace Opportunity.Helpers.Universal.Xaml
{
    /// <summary>
    /// State trigger for control size.
    /// </summary>
    public sealed class SizeTrigger : StateTriggerBase
    {
        /// <summary>
        /// Target of size test.
        /// </summary>
        public FrameworkElement Target
        {
            get => (FrameworkElement)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="Target"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(FrameworkElement), typeof(SizeTrigger), new PropertyMetadata(null, TargetPropertyChanged));

        private static void TargetPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (FrameworkElement)e.OldValue;
            var newValue = (FrameworkElement)e.NewValue;
            if (oldValue == newValue)
                return;
            var sender = (SizeTrigger)dp;
            if (oldValue != null)
                oldValue.SizeChanged -= sender.Target_SizeChanged;
            if (newValue != null)
                newValue.SizeChanged += sender.Target_SizeChanged;
            sender.set();
        }

        private void Target_SizeChanged(object sender, SizeChangedEventArgs e) => set();

        /// <summary>
        /// Minimun target width to active the trigger.
        /// </summary>
        public double MinWidth
        {
            get => (double)GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="MinWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register(nameof(MinWidth), typeof(double), typeof(SizeTrigger), new PropertyMetadata(0d, MinWidthPropertyChanged));

        private static void MinWidthPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (double)e.OldValue;
            var newValue = (double)e.NewValue;
            if (oldValue == newValue)
                return;
            var sender = (SizeTrigger)dp;
            sender.set();
        }

        /// <summary>
        /// Minimun target height to active the trigger.
        /// </summary>
        public double MinHeight
        {
            get => (double)GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="MinHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinHeightProperty =
            DependencyProperty.Register(nameof(MinHeight), typeof(double), typeof(SizeTrigger), new PropertyMetadata(0d, MinHeightPropertyChanged));

        private static void MinHeightPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (double)e.OldValue;
            var newValue = (double)e.NewValue;
            if (oldValue == newValue)
                return;
            var sender = (SizeTrigger)dp;
            sender.set();
        }

        private void set()
        {
            if (Target is null)
            {
                SetActive(false);
                return;
            }
            SetActive(
                Target.ActualHeight >= MinHeight &&
                Target.ActualWidth >= MinWidth);
        }
    }
}
