﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Vestis.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private readonly ResourceLoader resources;

        public UserPage()
        {
            InitializeComponent();
            resources = ResourceLoader.GetForCurrentView();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var target = e?.Parameter;
            var result = DressingRoom.ForUser(target as string);

            user = result.AsT0;

            // Paint elements with user profile color
            DataContext = user;

            if (user.Garments.Any())
            {
                FillStatistics();
                FillNotifications();
            }

            // Preload weather information to avoid lag spike
            // when accessing combination page. Weather information
            // is cached for the next 30 minutes.
            new Thread(() =>
            {
                Console.WriteLine(DressingRoom.WeatherData);
            }).Start();
        }

        private void FillStatistics() => StatsList.ItemsSource = new List<Stat>
            {
                new Stat
                {
                    Prefix = resources.GetString("UserPageStatTotalClothesPrefix"),
                    Value = user.Garments.Count().ToString(CultureInfo.InvariantCulture),
                    Suffix = resources.GetString("UserPageStatTotalClothesSuffix"),
                    ProfileColor = user.ProfileColor
                },
                new Stat
                {
                    Prefix = resources.GetString("UserPageStatMostCommonTypePrefix"),
                    Value = resources.GetString($@"ClothingType{user.Garments.GroupBy(g => g.Type).Select(n => new {
                        ClothingType = n.Key,
                        Amount = n.Count()
                    }).OrderByDescending(n => n.Amount).First().ClothingType.ToString() }"),
                    Suffix = resources.GetString("UserPageStatMostCommonTypeSuffix"),
                    ProfileColor = user.ProfileColor
                },
                new Stat
                {
                    Prefix = resources.GetString("UserPageStatLeastCommonTypePrefix"),
                    Value = resources.GetString($@"ClothingType{user.Garments.GroupBy(g => g.Type).Select(n => new {
                        ClothingType = n.Key,
                        Amount = n.Count()
                    }).OrderBy(n => n.Amount).First().ClothingType.ToString() }"),
                    Suffix = resources.GetString("UserPageStatLeastCommonTypeSuffix"),
                    ProfileColor = user.ProfileColor
                },
                new Stat
                {
                    Prefix = resources.GetString("UserPageStatOldestGarmentPrefix"),
                    Value = user.Garments.OrderBy(g => g.PurchaseDate.AgeInSeasons).First().Name,
                    Suffix = resources.GetString("UserPageStatOldestGarmentSuffix"),
                    ProfileColor = user.ProfileColor
                },
                new Stat
                {
                    Prefix = resources.GetString("UserPageStatNewestGarmentPrefix"),
                    Value = user.Garments.OrderByDescending(g => g.PurchaseDate.AgeInSeasons).First().Name,
                    Suffix = resources.GetString("UserPageStatNewestGarmentSuffix"),
                    ProfileColor = user.ProfileColor
                },
            };

        private void FillNotifications()
        {
            IEnumerable<Noti> Notis()
            {
                foreach (var prenda in user.Garments.Where(g => g.PurchaseDate.AgeInYears >= 3))
                    yield return new Noti
                    {
                        Title = $"{prenda.Name} {resources.GetString("UserPageNotiOldGarmentTitle")}",
                        Content = resources.GetString("UserPageNotiOldGarmentText"),
                        ProfileColor = user.ProfileColor
                    };
            }
            NotificationList.ItemsSource = Notis();
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(MainPage));

        private void BtnManageClothes_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(WardrobePage), user);

        private void BtnCombine_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(CombineStartPage), user);

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
