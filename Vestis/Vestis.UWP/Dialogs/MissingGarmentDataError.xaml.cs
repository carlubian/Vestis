﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento del cuadro de diálogo de contenido está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vestis.UWP.Dialogs
{
    public sealed partial class MissingGarmentDataError : ContentDialog
    {
        public MissingGarmentDataError() => InitializeComponent();

        private void Button_Click(object _1, RoutedEventArgs _2) => Hide();
    }
}
