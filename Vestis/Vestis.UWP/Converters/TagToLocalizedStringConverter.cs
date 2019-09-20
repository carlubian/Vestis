using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Vestis.UWP.Converters
{
    internal class TagToLocalizedStringConverter
    {
        private readonly ResourceLoader resources = ResourceLoader.GetForCurrentView();

        internal string Convert(string tag)
        {
            return resources.GetString($"Tag{tag}");
        }
    }
}
