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
    public class UserDeleteCommand : ICommand
    {
        private static UserDeleteCommand instance;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            var confirm = new ConfirmDeleteUser();

            await confirm.ShowAsync();

            if (confirm.Result is true)
            {
                DressingRoom.DisposeOf(parameter as string);
                (Window.Current.Content as Frame).Navigate(typeof(MainPage));
            }
            
        }

        public static UserDeleteCommand Instance {
            get
            {
                if (instance is null)
                    instance = new UserDeleteCommand();
                return instance;
            }
        }
    }
}
