using System.Linq;
using Vestis.Core;
using Vestis.UWP.Commands;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Vestis.UWP
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();

            // Bind users to list
            var users = DressingRoom.ListAvailable();
            UserPanel.ItemsSource = users.Select(u => new
            {
                Username = u,
                ProfileColor = DressingRoom.ColorFor(u),
                ProfileIcon = DressingRoom.IconFor(u),
                UserClickCommand = UserClickCommand.Instance
            });
        }

        private void BtnManageUsers_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(ManageUsersPage));

        private void BtnAbout_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(AboutPage));

        private void BtnSettings_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(SettingsPage));
    }
}
