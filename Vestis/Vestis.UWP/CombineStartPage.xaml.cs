using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Vestis.Core;
using Vestis.Core.Model;
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
    public sealed partial class CombineStartPage : Page
    {
        private Wardrobe wardrobe;

        public CombineStartPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            wardrobe = e.Parameter as Wardrobe;

            // Populate available type list
            var types = wardrobe.Garments.Select(g => g.Type.ToString())
                .Distinct()
                .Select(t => new TypeWrapper
                {
                    TypeName = t,
                    TypeSelectCommand = new TypeSelectCommand(this),
                    TypeUnselectCommand = new TypeUnselectCommand(this)
                });
            UnselectedList.ItemsSource = types;
        }

        internal void SelectType(string type)
        {
            // Add type to selected types list
            var selected = SelectedList.Items.Cast<TypeWrapper>().ToList();
            selected.Add(new TypeWrapper
            {
                TypeName = type,
                TypeSelectCommand = new TypeSelectCommand(this),
                TypeUnselectCommand = new TypeUnselectCommand(this)
            });
            SelectedList.ItemsSource = selected;

            // Unselected types list must only
            // show types compatible with all
            // the selected ones.
            var available = wardrobe.Garments.Select(g => g.Type.ToString());
            var compatibilities = selected.Select(w => w.TypeName)
                .Select(t => new
                {
                    TypeName = t,
                    Compatibles = ClothingTypeUtil.CompatibleWith(t)
                });
            foreach (var compatible in compatibilities)
                available = available.Intersect(compatible.Compatibles);
            UnselectedList.ItemsSource = available.Select(t => new TypeWrapper
            {
                TypeName = t,
                TypeSelectCommand = new TypeSelectCommand(this),
                TypeUnselectCommand = new TypeUnselectCommand(this)
            });
        }

        internal void UnselectType(string type)
        {
            // Remove types from selected types list
            var selected = SelectedList.Items.Cast<TypeWrapper>().ToList();
            selected.Remove(selected.First(t => t.TypeName.Equals(type)));
            SelectedList.ItemsSource = selected;

            // Unselected types list must only
            // show types compatible with all
            // the selected ones.
            var available = wardrobe.Garments.Select(g => g.Type.ToString());
            var compatibilities = selected.Select(w => w.TypeName)
                .Select(t => new
                {
                    TypeName = t,
                    Compatibles = ClothingTypeUtil.CompatibleWith(t)
                });
            foreach (var compatible in compatibilities)
                available = available.Intersect(compatible.Compatibles);
            UnselectedList.ItemsSource = available.Select(t => new TypeWrapper
            {
                TypeName = t,
                TypeSelectCommand = new TypeSelectCommand(this),
                TypeUnselectCommand = new TypeUnselectCommand(this)
            });
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserPage), wardrobe.Username);
        }

        private void BtnCombine_Click(object sender, RoutedEventArgs e)
        {

        }

        class TypeWrapper
        {
            public string TypeName { get; set; }
            public ICommand TypeSelectCommand { get; set; }
            public ICommand TypeUnselectCommand { get; set; }
            public string TypeIcon
            {
                get
                {
                    return $"Assets/Icons/ClothingType{TypeName}.png";
                }
            }
            public string LocalizedName
            {
                get
                {
                    ResourceLoader resources = ResourceLoader.GetForCurrentView();
                    return resources.GetString($"ClothingType{TypeName}");
                }
            }
        }

        class TypeSelectCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private readonly CombineStartPage source;

            public TypeSelectCommand(CombineStartPage source)
            {
                this.source = source;
            }

            public bool CanExecute(object parameter) => true;
            public void Execute(object parameter)
            {
                var type = parameter as string;
                source.SelectType(type);
            }
        }

        class TypeUnselectCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private readonly CombineStartPage source;

            public TypeUnselectCommand(CombineStartPage source)
            {
                this.source = source;
            }

            public bool CanExecute(object parameter) => true;
            public void Execute(object parameter)
            {
                var type = parameter as string;
                source.UnselectType(type);
            }
        }
    }
}
