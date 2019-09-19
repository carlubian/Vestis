using DotNet.Misc.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Vestis.Core;
using Vestis.Core.Model;
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

            // TODO Temporary layout preview
            var clothes = new List<GarmentWrapper>
            {
                new GarmentWrapper
                {
                    Garment = new Garment
                    {
                        ID = "DebugGarment01",
                        Name = "Camiseta de Pruebas",
                        Type = ClothingType.ShortTShirt,
                        PurchaseDate = new PurchaseDate("Spring 2019"),
                        ColorTags = new List<string>
                        {
                            "blue", "white"
                        },
                        StyleTags = new List<string>
                        {
                            "stripes", "low-cut"
                        }
                    },
                    Wardrobe = wardrobe
                },
                new GarmentWrapper
                {
                    Garment = new Garment
                    {
                        ID = "DebugGarment02",
                        Name = "Pantalón de Pruebas",
                        Type = ClothingType.LongTrouser,
                        PurchaseDate = new PurchaseDate("Winter 2018"),
                        ColorTags = new List<string>
                        {
                            "grey"
                        },
                        StyleTags = new List<string>
                        {
                            "plain", "deep-pockets", "buttons"
                        }
                    },
                    Wardrobe = wardrobe
                }
            };

            ClothesList.ItemsSource = clothes;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void BtnAddClothes_Click(object sender, RoutedEventArgs e)
        {

        }

        class GarmentWrapper
        {
            private ResourceLoader resources = ResourceLoader.GetForCurrentView();

            public Garment Garment { get; set; }
            public Wardrobe Wardrobe { get; set; }

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

            public string ColorTags
            {
                get
                {
                    var Converter = new TagToLocalizedStringConverter();
                    return Garment.ColorTags.Select(Converter.Convert).Stringify(n => n, "; ");
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
                    return $"{Garment.Name}:{Wardrobe.Username}";
                }
            }
        }
    }
}
