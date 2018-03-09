using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Opportunity.Helpers.Universal.Xaml
{
    public sealed class StringResource : MarkupExtension
    {
        public StringResource(string path) { this.Path = path; }
        public StringResource() { }

        public string Path { get; set; }

        protected override object ProvideValue()
        {
            var r = LocalizedStrings.GetValue(Path);
            if (string.IsNullOrEmpty(r))
                return Path;
            return r;
        }
    }
}
