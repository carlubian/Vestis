﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Vestis.Core;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private void BtnNewUser_Click(object sender, RoutedEventArgs e)
        {
            DressingRoom.SeeWelcomePage();
            Frame.Navigate(typeof(AddUserPage));
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
    }
}
