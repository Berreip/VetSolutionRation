using VetSolutionRation.wpf.Services.Navigation;

namespace VetSolutionRation.wpf.Views
{
    internal interface IMainWindowViewModel
    {
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class MainWindowViewModel : IMainWindowViewModel
    {
        public IMenuNavigator MenuNavigator { get; }

        public MainWindowViewModel(IMenuNavigator menuNavigator)
        {
            MenuNavigator = menuNavigator;
        }
    }
}