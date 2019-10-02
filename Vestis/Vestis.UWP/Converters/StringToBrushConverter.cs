using System;
using System.Globalization;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Vestis.UWP.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var input = value as string;
            var parts = input?.Split(':').Select(s => byte.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            return new SolidColorBrush(Color.FromArgb(255, parts[0], parts[1], parts[2]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
