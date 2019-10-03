using System;
using Vestis.Core;
using Vestis.UWP.Dialogs;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            ApiTextBox.Text = DressingRoom.WeatherApiKey ?? "";
            LocationTextBox.Text = DressingRoom.WeatherLocation ?? "";
        }

        private async void BtnRestore_Click(object _1, RoutedEventArgs _2)
        {
            var confirm = new ConfirmRestore();
            await confirm.ShowAsync();

            if (confirm.Result is true)
            {
                DressingRoom.RestoreSettings();
                await CoreApplication.RequestRestartAsync("");
            }
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(MainPage));

        private void BtnSave_Click(object _1, RoutedEventArgs _2)
        {
            DressingRoom.WeatherApiKey = ApiTextBox.Text;
            DressingRoom.WeatherLocation = LocationTextBox.Text;
            Frame.Navigate(typeof(MainPage));
        }

        private async void BtnApiInfo_Click(object _1, RoutedEventArgs _2) => await new WeatherApiInfo().ShowAsync();

        private async void BtnLocationAuto_Click(object _1, RoutedEventArgs _2) => await new WeatherLocationInfo().ShowAsync();
    }
}
