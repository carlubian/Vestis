using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using System;
using Vestis.Core;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Vestis.UWP
{
    /// <summary>
    /// Proporciona un comportamiento específico de la aplicación para complementar la clase Application predeterminada.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Inicializa el objeto de aplicación Singleton. Esta es la primera línea de código creado
        /// ejecutado y, como tal, es el equivalente lógico de main() o WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            this.Suspending += OnSuspending;
            AppCenter.Start("7e231a4a-ecc8-4690-aba4-a96dba7471f4", typeof(Analytics));
        }

        /// <summary>
        /// Se invoca cuando la aplicación la inicia normalmente el usuario final. Se usarán otros puntos
        /// de entrada cuando la aplicación se inicie para abrir un archivo específico, por ejemplo.
        /// </summary>
        /// <param name="e">Información detallada acerca de la solicitud y el proceso de inicio.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var directory = Windows.Storage.ApplicationData.Current.RoamingFolder.Path;
            DressingRoom.SetAppDirectory(directory);

            // No repetir la inicialización de la aplicación si la ventana tiene contenido todavía,
            // solo asegurarse de que la ventana está activa.
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Crear un marco para que actúe como contexto de navegación y navegar a la primera página.
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Cargar el estado de la aplicación suspendida previamente
                }

                // Poner el marco en la ventana actual.
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // TODO Comprobar si se debe mostrar la página de bienvenida
                    if (DressingRoom.WelcomePageSeen())
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    else
                        rootFrame.Navigate(typeof(WelcomePage), e.Arguments);
                }
                // Asegurarse de que la ventana actual está activa.
                Window.Current.Activate();

                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }

        /// <summary>
        /// Se invoca cuando la aplicación la inicia normalmente el usuario final. Se usarán otros puntos
        /// </summary>
        /// <param name="sender">Marco que produjo el error de navegación</param>
        /// <param name="e">Detalles sobre el error de navegación</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        /// <summary>
        /// Se invoca al suspender la ejecución de la aplicación. El estado de la aplicación se guarda
        /// sin saber si la aplicación se terminará o se reanudará con el contenido
        /// de la memoria aún intacto.
        /// </summary>
        /// <param name="sender">Origen de la solicitud de suspensión.</param>
        /// <param name="e">Detalles sobre la solicitud de suspensión.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Guardar el estado de la aplicación y detener toda actividad en segundo plano
            deferral.Complete();
        }
    }
}
