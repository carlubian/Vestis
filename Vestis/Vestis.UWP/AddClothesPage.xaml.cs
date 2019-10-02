using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vestis.Core;
using Vestis.Core.Model;
using Vestis.UWP.Dialogs;
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
    public sealed partial class AddClothesPage : Page
    {
        private Wardrobe user;
        private int Step;

        public AddClothesPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            user = e?.Parameter as Wardrobe;
            Step = 1;
            Step1Grid.Visibility = Visibility.Visible;
            Step2Grid.Visibility = Visibility.Collapsed;

            // Paint elements with user profile color
            DataContext = user;

            // Fill type combobox
            var types = Enum.GetValues(typeof(ClothingType)).OfType<ClothingType>()
                .Select(t => new TypeWrapper
                {
                    Type = t.ToString()
                });
            TypeComboBox.ItemsSource = types;
            TypeComboBox.SelectedIndex = 0;

            // Fill purchase date fields
            var seasons = new string[]
            {
                "Spring", "Summer",
                "Autumn", "Winter"
            };
            var resources = ResourceLoader.GetForCurrentView();
            SeasonComboBox.ItemsSource = seasons.Select(s => new SeasonWrapper
            {
                Season = s
            });

            // Fill color grid
            GarmentColorGrid.ItemsSource = ClothingColors.All()
                .Select(c => new ColorWrapper
                {
                    Color = c
                });

            // Fill style grid
            GarmentStyleGrid.ItemsSource = ClothingStyles.All()
                .Select(s => new StyleWrapper
                {
                    Style = s
                });
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(WardrobePage), user);

        private async void BtnContinue_Click(object _1, RoutedEventArgs _2)
        {
            if (Step is 1)
            {
                var name = NameTextBox.Text;

                // Validation: Name must be between 1 and 64 characters long
                if (name.Length < 1 || name.Length > 64)
                {
                    await new GarmentNameFormatError().ShowAsync();
                    return;
                }
                // Validation: Name must only contain letters, numbers, spaces and dashes
                var regex = new Regex("^[A-Z0-9ÁÉÍÓÚÀÈÌÒÙÄËÏÖÜÂÊÎÔÛÇÑ \\-]+$");
                if (!regex.IsMatch(name.ToUpperInvariant()))
                {
                    await new GarmentNameFormatError().ShowAsync();
                    return;
                }
                // Validation: Name must not exist already
                if (user.Garments.Any(g => g.Name.ToUpperInvariant().Equals(name.ToUpperInvariant(), StringComparison.InvariantCulture)))
                {
                    await new GarmentNameExistingError().ShowAsync();
                    return;
                }
                // Validation: A type must be selected
                if (TypeComboBox.SelectedItem is null)
                {
                    await new MissingGarmentDataError().ShowAsync();
                    return;
                }
                // Validation: A season must be selected
                if (SeasonComboBox.SelectedItem is null)
                {
                    await new MissingGarmentDataError().ShowAsync();
                    return;
                }

                var year = YearTextBox.Text;
                // Validation: Year must be a number between 1900 and current year
                if (!int.TryParse(year, out var yearNum))
                {
                    await new GarmentYearFormatError().ShowAsync();
                    return;
                }
                if (yearNum < 1900 || yearNum > DateTime.Today.Year)
                {
                    await new GarmentYearFormatError().ShowAsync();
                    return;
                }

                Step = 2;
                Step1Grid.Visibility = Visibility.Collapsed;
                Step2Grid.Visibility = Visibility.Visible;
            }
            else if (Step is 2)
            {
                // Validation: At least one color must be selected
                if (GarmentColorGrid.SelectedItems.Count is 0)
                {
                    await new MissingGarmentTagsError().ShowAsync();
                    return;
                }
                // Validation: At least one style must be selected
                if (GarmentStyleGrid.SelectedItems.Count is 0)
                {
                    await new MissingGarmentTagsError().ShowAsync();
                    return;
                }

                var garment = new Garment
                {
                    Name = NameTextBox.Text,
                    Type = (ClothingType)Enum.Parse(typeof(ClothingType), (TypeComboBox.SelectedItem as TypeWrapper).Type),
                    PurchaseDate = new PurchaseDate($"{(SeasonComboBox.SelectedItem as SeasonWrapper).Season} {YearTextBox.Text}"),
                    ColorTags = GarmentColorGrid.SelectedItems.Select(s => (s as ColorWrapper).Color),
                    StyleTags = GarmentStyleGrid.SelectedItems.Select(s => (s as StyleWrapper).Style)
                };
                user.AddGarment(garment);
                Analytics.TrackEvent("Added new garment", new Dictionary<string, string>()
                {
                    { "ClothingType", garment.Type.ToString() }
                });
                Frame.Navigate(typeof(WardrobePage), user);
            }
        }

        class TypeWrapper
        {
            public string Type { get; set; }
            public string LocalizedName
            {
                get
                {
                    var resources = ResourceLoader.GetForCurrentView();
                    return resources.GetString($"ClothingType{Type}");
                }
            }
            public string Icon => $"Assets/Icons/ClothingType{Type}.png";
        }

        class ColorWrapper
        {
            public string Color { get; set; }
        }

        class StyleWrapper
        {
            public string Style { get; set; }
            public string LocalizedName
            {
                get
                {
                    var resources = ResourceLoader.GetForCurrentView();
                    return resources.GetString($"Tag{Style}");
                }
            }
        }

        class SeasonWrapper
        {
            public string Season { get; set; }
            public string LocalizedName
            {
                get
                {
                    var resources = ResourceLoader.GetForCurrentView();
                    return resources.GetString($"Season{Season}");
                }
            }
        }
    }
}
