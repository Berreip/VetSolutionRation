using System;
using System.Windows;
using System.Windows.Threading;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore.UiWorkerThread;
using VetSolutionRation.wpf.Helpers.Assertions;
using VetSolutionRation.wpf.Services.Injection;
using VetSolutionRation.wpf.Views;

namespace VetSolutionRation.wpf
{
    internal static class Startup
    {
        [STAThread]
        internal static void Main()
        {
            try
            {
                var mainBoot = new VetSolutionRatioBoot();
                var app = new App
                {
                    //close the app when the main window is closed (default value is lastWindow)
                    ShutdownMode = ShutdownMode.OnMainWindowClose
                };
                DebugCore.SetAssertionFailedCallback(OnAssertionFailed);
                
                app.DispatcherUnhandledException += OnUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += AppDomainOnUnhandledException;
                app.Exit += mainBoot.OnExit;
                app.InitializeComponent();

                app.Run(mainBoot.Run<MainWindowView>());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unhandled Exception (will exit after close): {ex} ");
            }
        }

        private static AssertionResponse OnAssertionFailed(AssertionFailedResult assertionfailedResult)
        {
            return UiThreadDispatcher.ExecuteOnUI(() =>
            {
                var assertionVm = new AssertionFailedViewModel(assertionfailedResult);
                var view = new AssertionFailedView(assertionVm);
                assertionVm.OnResponseSet += () => view.Close();
                view.ShowDialog();
                return assertionVm.GetResponse();
            });
        }

        private static void AppDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($@"AppDomainOnUnhandledException error in {sender}: Exception - {e}");
        }

        private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"OnUnhandledException error: {e.Exception}");
        }
    }
}
