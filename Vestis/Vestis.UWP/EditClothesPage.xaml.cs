using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Vestis.Core;
using Vestis.Core.Model;
using Vestis.UWP.Dialogs;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class EditClothesPage : Page
    {
        private Wardrobe wardrobe;
        private string garment, oldName;
        private int Step;

        public EditClothesPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            (wardrobe, garment) = ((Wardrobe, string))e?.Parameter;
            var clothing = wardrobe.Garments.First(g => g.ID.Equals(garment, StringComparison.InvariantCulture));
            oldName = clothing.Name;

            Step = 1;
            Step1Grid.Visibility = Visibility.Visible;
            Step2Grid.Visibility = Visibility.Collapsed;

            // Paint elements with user profile color
            DataContext = wardrobe;

            // -------- Fill elements with current values --------
            // Garment name
            NameTextBox.Text = clothing.Name;

            // Garment type
            var types = Enum.GetValues(typeof(ClothingType)).OfType<ClothingType>();
            TypeComboBox.ItemsSource = types.Select(t => new TypeWrapper
            {
                Type = t.ToString()
            });
            TypeComboBox.SelectedIndex = types.ToList().IndexOf(clothing.Type);

            // Purchase date
            var seasons = new string[]
            {
                "Winter", "Spring",
                "Summer", "Autumn",
            };
            var resources = ResourceLoader.GetForCurrentView();
            SeasonComboBox.ItemsSource = seasons.Select(s => new SeasonWrapper
            {
                Season = s
            });
            SeasonComboBox.SelectedIndex = seasons.ToList().IndexOf(clothing.PurchaseDate.Season.ToString());
            YearTextBox.Text = clothing.PurchaseDate.Year.ToString(CultureInfo.InvariantCulture);

            // Garment colors
            GarmentColorGrid.ItemsSource = ClothingColors.All()
                .Select(c => new ColorWrapper
                {
                    Color = c
                });
            var indices = ClothingColors.All()
                .Select(c => new
                {
                    Index = ClothingColors.All().ToList().IndexOf(c),
                    Color = c
                })
                .Where(c => clothing.ColorTags.Contains(c.Color))
                .Select(c => c.Index);
            foreach (var index in indices)
                GarmentColorGrid.SelectRange(new ItemIndexRange(index, 1));

            // Garment styles
            GarmentStyleGrid.ItemsSource = ClothingStyles.All()
                .Select(s => new StyleWrapper
                {
                    Style = s
                });
            indices = ClothingStyles.All()
                .Select(s => new
                {
                    Index = ClothingStyles.All().ToList().IndexOf(s),
                    Style = s
                })
                .Where(s => clothing.StyleTags.Contains(s.Style))
                .Select(s => s.Index);
            foreach (var index in indices)
                GarmentStyleGrid.SelectRange(new ItemIndexRange(index, 1));
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(WardrobePage), wardrobe);

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
                if (wardrobe.Garments.Any(g => g.Name.ToUpperInvariant().Equals(name.ToUpperInvariant(), StringComparison.InvariantCulture)) && name != oldName)
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

                var clothing = wardrobe.Garments.First(g => g.ID.Equals(garment, StringComparison.InvariantCulture));
                clothing.Name = NameTextBox.Text;
                clothing.Type = (ClothingType)Enum.Parse(typeof(ClothingType), (TypeComboBox.SelectedItem as TypeWrapper).Type);
                clothing.PurchaseDate = new PurchaseDate($"{(SeasonComboBox.SelectedItem as SeasonWrapper).Season} {YearTextBox.Text}");
                clothing.ColorTags = GarmentColorGrid.SelectedItems.Select(s => (s as ColorWrapper).Color);
                clothing.StyleTags = GarmentStyleGrid.SelectedItems.Select(s => (s as StyleWrapper).Style);

                wardrobe.UpdateGarment(clothing);
                Frame.Navigate(typeof(WardrobePage), wardrobe);
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
            public string Icon
            {
                get
                {
                    var resources = ResourceLoader.GetForCurrentView();
                    return $"Assets/Icons/ClothingType{Type}.png";
                }
            }
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
