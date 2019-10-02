using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento del cuadro de diálogo de contenido está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP.Dialogs
{
    public sealed partial class ConfirmDeleteGarment : ContentDialog
    {
#pragma warning disable CA1051
        public bool? Result = null;
#pragma warning restore CS1051

        public ConfirmDeleteGarment() => InitializeComponent();

        private void RestoreDlgBtnYes_Click(object _1, RoutedEventArgs _2)
        {
            Result = true;
            Hide();
        }

        private void RestoreDlgBtnNo_Click(object _1, RoutedEventArgs _2)
        {
            Result = false;
            Hide();
        }
    }
}
