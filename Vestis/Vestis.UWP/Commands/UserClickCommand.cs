using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vestis.UWP.Commands
{
    public class UserClickCommand : ICommand
    {
        private static UserClickCommand instance;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            //TODO temporary implementation
            Console.WriteLine($"User {parameter} clicked");
        }

        public static UserClickCommand Instance {
            get
            {
                if (instance is null)
                    instance = new UserClickCommand();
                return instance;
            }
        }
    }
}
