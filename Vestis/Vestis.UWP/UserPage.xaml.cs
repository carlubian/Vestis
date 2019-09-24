using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Vestis.Core;
using Windows.ApplicationModel.Resources;
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
    public sealed partial class UserPage : Page
    {
        private Wardrobe user;
        private ResourceLoader resources;

        public UserPage()
        {
            this.InitializeComponent();
            resources = ResourceLoader.GetForCurrentView();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var target = e.Parameter;
            var result = DressingRoom.ForUser(target as string);

            if (result.IsT1)
                ; // TODO Redirect to error page?

            user = result.AsT0;

            // Paint elements with user profile color
            DataContext = user;

            FillStatistics();
            FillNotifications();

            Analytics.TrackEvent("User profile accesed");
        }

        private void FillStatistics()
        {
            StatsList.ItemsSource = new List<Stat>
            {
                new Stat
                {
                    Prefix = resources.GetString("UserPageStatTotalClothesPrefix"),
                    Value = user.Garments.Count().ToString(),
                    Suffix = resources.GetString("UserPageStatTotalClothesSuffix"),
                    ProfileColor = user.ProfileColor
                }
            };
        }

        private void FillNotifications()
        {
            NotificationList.ItemsSource = new List<Noti>
            {
                new Noti
                {
                    Title = "Notificación de prueba",
                    Content = "Recuerda beber agua a menudo; La hidratación es importante.",
                    ProfileColor = user.ProfileColor
                }
            };
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void BtnManageClothes_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WardrobePage), user);
        }

        private void BtnCombine_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CombineStartPage), user);
        }

        class Stat
        {
            public string Prefix { get; set; }
            public string Value { get; set; }
            public string Suffix { get; set; }
            public string ProfileColor { get; set; }
        }

        class Noti
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public string ProfileColor { get; set; }
        }
    }
}
