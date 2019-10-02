using DotNet.Misc.Extensions.Linq;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Vestis.Core;
using Vestis.Core.Model;
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
    public sealed partial class CombineStartPage : Page
    {
        private Wardrobe wardrobe;

        public CombineStartPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            wardrobe = e?.Parameter as Wardrobe;

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

            // Populate weather info, if available
            DataContext = new WeatherWrapper();

            // Populate weather advice
            var advices = WeatherUtil.GenerateAdvice()
                .Select(a => new CodeToLocalizedWeatherAdviceConverter().Convert(a))
                .ToList();
            WeatherAdvice.ItemsSource = advices.Select(a => (WeatherAdviceWrapper)a);
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
            selected.Remove(selected.First(t => t.TypeName.Equals(type, StringComparison.InvariantCulture)));
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

        private void BtnGoBack_Click(object _1, RoutedEventArgs _2) => Frame.Navigate(typeof(UserPage), wardrobe.Username);

        private void BtnCombine_Click(object _1, RoutedEventArgs _2)
        {
            var types = SelectedList.Items.Cast<TypeWrapper>().Select(t => t.TypeName);
            Analytics.TrackEvent("Creating combination", new Dictionary<string, string>()
            {
                { "Types", types.Stringify(n => n, " ") }
            });
            Frame.Navigate(typeof(CombineEndPage), (wardrobe, types));
        }

        class TypeWrapper
        {
            public string TypeName { get; set; }
            public ICommand TypeSelectCommand { get; set; }
            public ICommand TypeUnselectCommand { get; set; }
            public string TypeIcon => $"Assets/Icons/ClothingType{TypeName}.png";
            public string LocalizedName
            {
                get
                {
                    var resources = ResourceLoader.GetForCurrentView();
                    return resources.GetString($"ClothingType{TypeName}");
                }
            }
        }

        class TypeSelectCommand : ICommand
        {
#pragma warning disable CS0067
            public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067
            private readonly CombineStartPage source;

            public TypeSelectCommand(CombineStartPage source) => this.source = source;

            public bool CanExecute(object parameter) => true;
            public void Execute(object parameter)
            {
                var type = parameter as string;
                source.SelectType(type);
            }
        }

        class TypeUnselectCommand : ICommand
        {
#pragma warning disable CS0067
            public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067
            private readonly CombineStartPage source;

            public TypeUnselectCommand(CombineStartPage source) => this.source = source;

            public bool CanExecute(object parameter) => true;
            public void Execute(object parameter)
            {
                var type = parameter as string;
                source.UnselectType(type);
            }
        }

        class WeatherWrapper
        {
            private readonly dynamic WeatherData = DressingRoom.WeatherData;

            public string WeatherIcon
            {
                get
                {
                    var code = WeatherData is null ? "" : DressingRoom.WeatherData?.current?.weather_code;
                    var type = WeatherUtil.ParseWeatherCode(code.Value);
                    return WeatherData is null ? "" : $"Assets/Icons/Weather{type}.png";
                }
            }
            public string WeatherType
            {
                get
                {
                    var code = WeatherData is null ? "" : DressingRoom.WeatherData?.current?.weather_code;
                    var type = WeatherUtil.ParseWeatherCode(code.Value);
                    return new CodeToLocalizedWeatherConverter().Convert(type);
                }
            }
            public string WeatherTemp => WeatherData is null ? "" : $"{DressingRoom.WeatherData?.current?.temperature} \u00B0C";
            public string WeatherLocation => WeatherData is null ? "" : $"{DressingRoom.WeatherData?.location?.name}, {DressingRoom.WeatherData?.location?.country}";
        }

        class WeatherAdviceWrapper
        {
            public string Advice { get; set; }

            public static implicit operator WeatherAdviceWrapper(string source) => new WeatherAdviceWrapper
            {
                Advice = source
            };
        }
    }
}
