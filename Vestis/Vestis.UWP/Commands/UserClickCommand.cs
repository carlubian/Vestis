using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vestis.UWP.Commands
{
    public class UserClickCommand : ICommand
    {
        private static UserClickCommand instance;

#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => (Window.Current.Content as Frame).Navigate(typeof(UserPage), parameter);

        public static UserClickCommand Instance
        {
            get
            {
                if (instance is null)
                    instance = new UserClickCommand();
                return instance;
            }
        }
    }
}
