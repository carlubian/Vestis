using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vestis.Core;
using Vestis.UWP.Dialogs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vestis.UWP.Commands
{
    public class GarmentEditCommand : ICommand
    {
        private static GarmentEditCommand instance;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            var split = (parameter as string).Split(':');
            var (user, garment) = (split[0], split[1]);
            var wardrobe = DressingRoom.ForUser(user).AsT0;

            (Window.Current.Content as Frame).Navigate(typeof(EditClothesPage), (wardrobe, garment));
            
        }

        public static GarmentEditCommand Instance {
            get
            {
                if (instance is null)
                    instance = new GarmentEditCommand();
                return instance;
            }
        }
    }
}
