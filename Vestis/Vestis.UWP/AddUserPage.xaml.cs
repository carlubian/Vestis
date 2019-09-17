using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

            var colors = new string[] { "170:0:0", "233:127:13", "220:163:0",
                                      "101:163:3", "16:112:16", "13:136:170",
                                       "0:60:146", "113:54:146", "88:96:104" };
            UserColorGrid.ItemsSource = colors.Select(c => new
            {
                Color = c
            });

            var icons = new string[]
            {
                "Assets/Icons/man.png",
                "Assets/Icons/woman.png",
                "Assets/Icons/baby-girl.png"
            };
            UserIconGrid.ItemsSource = icons.Select(i => new
            {
                Icon = i
            });
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void BtnSaveUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
