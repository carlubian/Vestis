using System;
using System.IO;
using Vestis.Core;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage() => InitializeComponent();

        private void BtnNewUser_Click(object _1, RoutedEventArgs _2)
        {
            DressingRoom.SeeWelcomePage();
            Frame.Navigate(typeof(AddUserPage));
        }

        private async void BtnImport_Click(object _1, RoutedEventArgs _2)
        {
            var openPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                //CommitButtonText = "Importar"
            };
            openPicker.FileTypeFilter.Add(".vestis");

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");

                var folder = await StorageFolder.GetFolderFromPathAsync(path);

                var stream = await file.CopyAsync(folder);
                DressingRoom.ImportPackage(stream.Path);
            }

            await CoreApplication.RequestRestartAsync("");
        }
    }
}
