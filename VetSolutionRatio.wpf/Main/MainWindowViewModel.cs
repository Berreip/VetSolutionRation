
using VetSolutionRatio.wpf.Services.Navigation;

namespace VetSolutionRatio.wpf.Main
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