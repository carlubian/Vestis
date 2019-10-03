using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();

            var versionElements = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Vestis UWP App", "1.0.0"),
                new KeyValuePair<string, string>("Vestis Core Backend", "1.0.0"),
                new KeyValuePair<string, string>("ConfigAdapter", "2.2.2"),
                new KeyValuePair<string, string>("ConfigAdapter.Xml", "2.3.3"),
                new KeyValuePair<string, string>("DotNet.Misc.Extensions", "1.2.0")
            };
            VersionList.ItemsSource = versionElements;

            var thirdPartyElements = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(".NET Standard", "Copyright (c) .NET Foundation and Contributors. Licensed under MIT license."),
                new KeyValuePair<string, string>("OneOf library", "Copyright (c) 2016 Harry McIntyre. Licensed under MIT license."),
                new KeyValuePair<string, string>("FluentAssertions library", "Copyright (c) Dennis Doomen, Jonas Nyrup. Licensed under Apache 2.0 license."),
                new KeyValuePair<string, string>("DotNetZip library", "Copyright (c) Dino Chiesa and Microsoft Corporation. Licensed under Microsoft Public License."),
                new KeyValuePair<string, string>("Weatherstack API", "Interactions are governed by their own privacy policy, available on: https://weatherstack.com/privacy"),
                new KeyValuePair<string, string>("Cloth hanger icon", "Nikita Golubev from www.flaticon.com"),
                new KeyValuePair<string, string>("Artificial Intelligence icon", "photo3idea_studio from www.flaticon.com"),
                new KeyValuePair<string, string>("Left arrow icon", "Lucy G from www.flaticon.com"),
                new KeyValuePair<string, string>("Right arrow icon", "Lucy G from www.flaticon.com"),
                new KeyValuePair<string, string>("Information icon", "Smashicons from www.flaticon.com"),
                new KeyValuePair<string, string>("Settings icon", "Smashicons from www.flaticon.com"),
                new KeyValuePair<string, string>("User icon", "Smashicons from www.flaticon.com"),
                new KeyValuePair<string, string>("Users icon", "Smashicons from www.flaticon.com"),
                new KeyValuePair<string, string>("Merge icon", "Smashicons from www.flaticon.com"),
                new KeyValuePair<string, string>("Cloud icon", "Smashicons from www.flaticon.com"),
                new KeyValuePair<string, string>("Plus sign icon", "Lyolya from www.flaticon.com"),
                new KeyValuePair<string, string>("Import icon", "Anatoly from www.flaticon.com"),
                new KeyValuePair<string, string>("Export icon", "Anatoly from www.flaticon.com"),
                new KeyValuePair<string, string>("Man icon", "Iconnice from www.flaticon.com"),
                new KeyValuePair<string, string>("Woman icon", "Iconnice from www.flaticon.com"),
                new KeyValuePair<string, string>("Baby girl icon", "Iconnice from www.flaticon.com"),
                new KeyValuePair<string, string>("Remove icon", "itim2101 from www.flaticon.com"),
                new KeyValuePair<string, string>("Long blouse icon", "itim2101 from www.flaticon.com"),
                new KeyValuePair<string, string>("Long polo icon", "itim201 from www.flaticon.com"),
                new KeyValuePair<string, string>("Wardrobe icon", "xnimrodx from www.flaticon.com"),
                new KeyValuePair<string, string>("Short t-shirt icon", "Good-Ware from www.flaticon.com"),
                new KeyValuePair<string, string>("Short blouse icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Short polo icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Sport sweater icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Cozy sweater icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Coat icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Boot icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Jacket icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Long dress icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Verified icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Sun icon", "Freepik from www.flaticon.com"),
                new KeyValuePair<string, string>("Reload icon", "Gregor Cresnar from www.flaticon.com"),
                new KeyValuePair<string, string>("Sun and cloud icon", "Good Ware from www.flaticon.com"),
                new KeyValuePair<string, string>("Short skirt icon", "iconixar from www.flaticon.com"),
                new KeyValuePair<string, string>("Long skirt icon", "iconixar from www.flaticon.com")
            };
            ThirdPartyList.ItemsSource = thirdPartyElements;
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(MainPage));
    }
}
