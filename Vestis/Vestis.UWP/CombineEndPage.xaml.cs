using DotNet.Misc.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
    public sealed partial class CombineEndPage : Page
    {
        private Wardrobe wardrobe;
        private IEnumerable<string> types;

        public CombineEndPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            (wardrobe, types) = ((Wardrobe, IEnumerable<string>))e.Parameter;
            ClothesList.ItemsSource = types.Select(t => wardrobe.Garments.Where(g => g.Type.ToString().Equals(t)).Random())
                .Select(g => new GarmentWrapper
                {
                    Garment = g,
                    Wardrobe = wardrobe,
                    GarmentRefreshCommand = new GarmentRefreshCommand(this)
                });
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CombineStartPage), wardrobe);
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage), wardrobe.Username);
        }

        internal void RefreshGarment(string type)
        {
            var current = ClothesList.Items.Cast<GarmentWrapper>().ToList();
            var indexToReplace = current.IndexOf(current.First(g => g.InternalType.Equals(type)));

            // The replacement should appear at the same position in the list
            // TODO Make sure the replacement is different from the original?
            var newGarment = wardrobe.Garments
                .Where(g => g.Type.ToString().Equals(type))
                .Random();

            current[indexToReplace] = new GarmentWrapper
            {
                Garment = newGarment,
                Wardrobe = wardrobe,
                GarmentRefreshCommand = new GarmentRefreshCommand(this)
            };

            ClothesList.ItemsSource = current;
        }

        class GarmentWrapper
        {
            private ResourceLoader resources = ResourceLoader.GetForCurrentView();

            public Garment Garment { get; set; }
            public Wardrobe Wardrobe { get; set; }
            public ICommand GarmentRefreshCommand { get; set; }

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

            public string InternalType
            {
                get
                {
                    return Garment.Type.ToString();
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

        class GarmentRefreshCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            private CombineEndPage source;

            public GarmentRefreshCommand(CombineEndPage source)
            {
                this.source = source;
            }

            public bool CanExecute(object parameter) => true;
            public void Execute(object parameter)
            {
                var type = parameter as string;
                source.RefreshGarment(type);
            }
        }
    }
}
