using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Vestis.Core;
using Vestis.UWP.Dialogs;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            this.InitializeComponent();

            ApiTextBox.Text = DressingRoom.WeatherApiKey ?? "";
            LocationTextBox.Text = DressingRoom.WeatherLocation ?? "";
        }

        private async void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            var confirm = new ConfirmRestore();
            await confirm.ShowAsync();

            if (confirm.Result is true)
            {
                DressingRoom.RestoreSettings();
                await CoreApplication.RequestRestartAsync("");
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DressingRoom.WeatherApiKey = ApiTextBox.Text;
            DressingRoom.WeatherLocation = LocationTextBox.Text;
            Frame.Navigate(typeof(MainPage));
        }

        private async void BtnApiInfo_Click(object sender, RoutedEventArgs e)
        {
            await new WeatherApiInfo().ShowAsync();
        }

        private void BtnLocationAuto_Click(object sender, RoutedEventArgs e)
        {
            LocationTextBox.Text = "";
        }
    }
}
