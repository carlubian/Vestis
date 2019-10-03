using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vestis.Core;
using Vestis.UWP.Dialogs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class EditUserPage : Page
    {
        private string OldUsername;

        public EditUserPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Username
            OldUsername = e?.Parameter as string;
            UserNameTextBox.Text = OldUsername;

            // Profile colors
            var colors = new List<string> { "218:47:47", "233:127:13", "220:163:0",
                                      "101:163:3", "32:128:32", "13:152:186",
                                       "48:108:194", "145:86:178", "120:128:136" };
            var userColor = DressingRoom.ColorFor(OldUsername);
            if (!colors.Contains(userColor))
                colors.Add(userColor);
            UserColorGrid.ItemsSource = colors.Select(c => new ColorWrapper
            {
                Color = c
            });
            var color = (UserColorGrid.ItemsSource as IEnumerable<ColorWrapper>)
                .First(c => c.Color.Equals(userColor, StringComparison.InvariantCulture));
            UserColorGrid.SelectedIndex = colors.IndexOf(color.Color);

            // Profile icons
            var icons = new List<string>
            {
                "Assets/Icons/man.png",
                "Assets/Icons/woman.png",
                "Assets/Icons/baby-girl.png"
            };
            var userIcon = DressingRoom.IconFor(OldUsername);
            if (!icons.Contains(userIcon))
                icons.Add(userIcon);
            UserIconGrid.ItemsSource = icons.Select(i => new IconWrapper
            {
                Icon = i
            });
            var icon = (UserIconGrid.ItemsSource as IEnumerable<IconWrapper>)
                .First(i => i.Icon.Equals(userIcon, StringComparison.InvariantCulture));
            UserIconGrid.SelectedIndex = icons.IndexOf(icon.Icon);
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(ManageUsersPage));

        private async void BtnSave_Click(object _1, RoutedEventArgs _2)
        {
            var username = UserNameTextBox.Text;

            // Validation: User name must have between 1 and 24 characters
            if (username.Length < 1 || username.Length > 24)
            {
                await new UserNameFormatError().ShowAsync();
                return;
            }
            // Validation: User name can only contain letters and dashes
            var regex = new Regex("^[A-Z\\-]+$");
            if (!regex.IsMatch(username.ToUpperInvariant()))
            {
                await new UserNameFormatError().ShowAsync();
                return;
            }
            // Validation: User name must not exist already (except for maintaining it)
            if (DressingRoom.ListAvailable().Contains(username) && !username.Equals(OldUsername, StringComparison.InvariantCulture))
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

            // Edit user data
            var color = (UserColorGrid.SelectedItem as ColorWrapper).Color;
            var icon = (UserIconGrid.SelectedItem as IconWrapper).Icon;

            DressingRoom.Update(OldUsername, username, color, icon);

            Frame.Navigate(typeof(MainPage));
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
}
