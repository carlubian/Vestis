using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Vestis.Core;
using Vestis.UWP.Commands;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class ManageUsersPage : Page
    {
        public ManageUsersPage()
        {
            this.InitializeComponent();

            // Bind users to list
            var users = DressingRoom.ListAvailable();
            UserPanel.ItemsSource = users.Select(u => new
            {
                Username = u,
                ProfileColor = DressingRoom.ColorFor(u),
                ProfileIcon = DressingRoom.IconFor(u),
                UserEditCommand = UserEditCommand.Instance,
                UserDeleteCommand = UserDeleteCommand.Instance
            });
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void BtnImport_Click(object sender, RoutedEventArgs e)
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

        private async void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture),
                //CommitButtonText = "Exportar"
            };
            savePicker.FileTypeChoices.Add(".VESTIS", new List<string>() { ".vestis" });

            var file = await savePicker.PickSaveFileAsync();

            var export = DressingRoom.MakeExportPackage();

            await FileIO.WriteBufferAsync(file, await FileIO.ReadBufferAsync(await StorageFile.GetFileFromPathAsync(export)));
            File.Delete(export);
        }

        private void BtnNewUser_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddUserPage));
        }
    }
}
