﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Vestis.Core;
using Vestis.UWP.Dialogs;
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
    public sealed partial class AddUserPage : Page
    {
        public AddUserPage()
        {
            this.InitializeComponent();

            var colors = new string[] { "194:24:24", "233:127:13", "220:163:0",
                                      "101:163:3", "16:112:16", "13:136:170",
                                       "24:84:170", "129:70:162", "104:112:120" };
            UserColorGrid.ItemsSource = colors.Select(c => new ColorWrapper
            {
                Color = c
            });

            var icons = new string[]
            {
                "Assets/Icons/man.png",
                "Assets/Icons/woman.png",
                "Assets/Icons/baby-girl.png"
            };
            UserIconGrid.ItemsSource = icons.Select(i => new IconWrapper
            {
                Icon = i
            });
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void BtnSaveUser_Click(object sender, RoutedEventArgs e)
        {
            var username = UserNameTextBox.Text;

            // Validation: User name must have between 1 and 24 characters
            if (username.Length < 1 || username.Length > 24)
            {
                await new UserNameFormatError().ShowAsync();
                return;
            }
            // Validation: User name can only contain letters and dashes
            var regex = new Regex("^[a-z\\-]+$");
            if (!regex.IsMatch(username.ToLowerInvariant()))
            {
                await new UserNameFormatError().ShowAsync();
                return;
            }
            // Validation: User name must not exist already
            if (DressingRoom.ListAvailable().Contains(username))
            {
                await new UserNameExistingError().ShowAsync();
                return;
            }
            // Validation: A profile color must be selected
            if (UserColorGrid.SelectedItem is null)
            {
                await new MissingProfileDataError().ShowAsync();
                return;
            }
            // Validation: A profile icon must be selected
            if (UserIconGrid.SelectedItem is null)
            {
                await new MissingProfileDataError().ShowAsync();
                return;
            }

            // Register new user
            var color = (UserColorGrid.SelectedItem as ColorWrapper).Color;
            var icon = (UserIconGrid.SelectedItem as IconWrapper).Icon;

            DressingRoom.CreateNew(username, color, icon);

            Frame.Navigate(typeof(MainPage));
        }
    }

    class ColorWrapper
    {
        public string Color { get; set; }
    }

    class IconWrapper
    {
        public string Icon { get; set; }
    }
}
