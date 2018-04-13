using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Opportunity.Helpers.Universal.Xaml
{
    /// <summary>
    /// Markup extension for localized strings.
    /// </summary>
    public sealed class StringResource : MarkupExtension
    {
        /// <summary>
        /// Create new instance of <see cref="StringResource"/>.
        /// </summary>
        /// <param name="path">Path of resource string.</param>
        public StringResource(string path) { this.Path = path; }
        /// <summary>
        /// Create new instance of <see cref="StringResource"/>.
        /// </summary>
        public StringResource() { }

        /// <summary>
        /// Path of resource string.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Get localized string of <see cref="Path"/>.
        /// </summary>
        /// <returns></returns>
        protected override object ProvideValue()
        {
            var r = LocalizedStrings.GetValue(Path);
            if (string.IsNullOrEmpty(r))
                return Path;
            return r;
        }
    }
}
