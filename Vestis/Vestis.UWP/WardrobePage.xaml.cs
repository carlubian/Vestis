using DotNet.Misc.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Vestis.Core;
using Vestis.Core.Model;
using Vestis.UWP.Commands;
using Vestis.UWP.Converters;
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
    public sealed partial class WardrobePage : Page
    {
        private Wardrobe wardrobe;

        public WardrobePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            wardrobe = e.Parameter as Wardrobe;

            ClothesList.ItemsSource = wardrobe.Garments.Select(g => new GarmentWrapper
            {
                Garment = g,
                Wardrobe = wardrobe,
                GarmentDeleteCommand = GarmentDeleteCommand.Instance
            });
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage), wardrobe.Username);
        }

        private void BtnAddClothes_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddClothesPage), wardrobe);
        }

        class GarmentWrapper
        {
            private ResourceLoader resources = ResourceLoader.GetForCurrentView();

            public Garment Garment { get; set; }
            public Wardrobe Wardrobe { get; set; }
            public ICommand GarmentDeleteCommand { get; set;  }

            public string ClothIcon
            {
                get
                {
                    return $"Assets/Icons/ClothingType{Garment.Type.ToString()}.png";
                }
            }

            public string ClothType
            {
                get
                {
                    return resources.GetString($"ClothingType{Garment.Type.ToString()}");
                }
            }

            public string ProfileColor
            {
                get
                {
                    return Wardrobe.ProfileColor;
                }
            }

            public IEnumerable<ColorWrapper> ColorTags
            {
                get
                {
                    return Garment.ColorTags.Select(c => new ColorWrapper
                    {
                        Color = c
                    });
                }
            }

            public string StyleTags
            {
                get
                {
                    var Converter = new TagToLocalizedStringConverter();
                    return Garment.StyleTags.Select(Converter.Convert).Stringify(n => n, "; ");
                }
            }

            public string QualifiedName
            {
                get
                {
                    return $"{Wardrobe.Username}:{Garment.ID}";
                }
            }
        }

        class ColorWrapper
        {
            public string Color { get; set; }
        }
    }
}
