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
    public class GarmentDeleteCommand : ICommand
    {
        private static GarmentDeleteCommand instance;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            var split = (parameter as string).Split(':');
            var (user, garment) = (split[0], split[1]);
            var confirm = new ConfirmDeleteGarment();

            await confirm.ShowAsync();

            if (confirm.Result is true)
            {
                DressingRoom.ForUser(user).Match(wardrobe => {
                        wardrobe.RemoveGarment(garment);
                        return 0;
                    },
                    error =>
                    {
                        return -1;
                    }
                );
                (Window.Current.Content as Frame).Navigate(typeof(WardrobePage), DressingRoom.ForUser(user).AsT0);
            }
            
        }

        public static GarmentDeleteCommand Instance {
            get
            {
                if (instance is null)
                    instance = new GarmentDeleteCommand();
                return instance;
            }
        }
    }
}
