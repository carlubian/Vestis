using DotNet.Misc.Extensions.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Vestis.Core;
using Vestis.Core.Model;
using Vestis.UWP.Commands;
using Vestis.UWP.Converters;
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
    public sealed partial class WardrobePage : Page
    {
        private Wardrobe wardrobe;

        public WardrobePage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            wardrobe = e?.Parameter as Wardrobe;

            ClothesList.ItemsSource = wardrobe.Garments.Select(g => new GarmentWrapper
            {
                Garment = g,
                Wardrobe = wardrobe,
                GarmentDeleteCommand = GarmentDeleteCommand.Instance,
                GarmentEditCommand = GarmentEditCommand.Instance
            });
        }

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(UserPage), wardrobe.Username);

        private void BtnAddClothes_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(AddClothesPage), wardrobe);

        class GarmentWrapper
        {
            private readonly ResourceLoader resources = ResourceLoader.GetForCurrentView();

            public Garment Garment { get; set; }
            public Wardrobe Wardrobe { get; set; }
            public ICommand GarmentDeleteCommand { get; set; }
            public ICommand GarmentEditCommand { get; set; }

            public string ClothIcon => $"Assets/Icons/ClothingType{Garment.Type.ToString()}.png";

            public string ClothType => resources.GetString($"ClothingType{Garment.Type.ToString()}");

            public string ProfileColor => Wardrobe.ProfileColor;

            public IEnumerable<ColorWrapper> ColorTags => Garment.ColorTags.Select(c => new ColorWrapper
            {
                Color = c
            });

            public string StyleTags
            {
                get
                {
                    var Converter = new TagToLocalizedStringConverter();
                    return Garment.StyleTags.Select(Converter.Convert).Stringify(n => n, "; ");
                }
            }

            public string QualifiedName => $"{Wardrobe.Username}:{Garment.ID}";
        }

        class ColorWrapper
        {
            public string Color { get; set; }
        }
    }
}
