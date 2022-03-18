using System.Windows;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VetSolutionRatio.wpf.Main;
using VetSolutionRatio.wpf.Services.Navigation;
using VetSolutionRatio.wpf.Views.Parameters;
using VetSolutionRatio.wpf.Views.RatioPanel;
using VetSolutionRatio.wpf.Views.RatioPanel.Components;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels;

namespace VetSolutionRatio.wpf.Services.Injection
{
    internal sealed class VetSolutionRatioBoot
    {
        private readonly IInjectionContainer _internalContainer;

        public VetSolutionRatioBoot()
        {
            _internalContainer = new InjectionContainerSimpleInjector();
        }

        public TMainWindow Run<TMainWindow>() where TMainWindow : class
        {
            Register();
            Initialize();
            return _internalContainer.Resolve<TMainWindow>();
        }

        private void Register()
        {
            _internalContainer.RegisterType<MainWindowView>(LifeTime.Singleton);
            _internalContainer.Register<IMainWindowViewModel, MainWindowViewModel>(LifeTime.Singleton);

            _internalContainer.Register<IMenuNavigator, MenuNavigator>(LifeTime.Singleton);

            // // views:
            _internalContainer.RegisterType<RatioPanelView>(LifeTime.Singleton);
            _internalContainer.Register<IRatioPanelViewModel, RatioPanelViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<ParametersView>(LifeTime.Singleton);
            _internalContainer.Register<IParametersViewModel, ParametersViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<AnimalSelectionView>(LifeTime.Singleton);
            _internalContainer.Register<IAnimalSelectionViewModel, AnimalSelectionViewModel>(LifeTime.Singleton);
            
            // // services:
            // _internalContainer.Register<IImageCacheService, ImageCacheService>(LifeTime.Singleton);
            //
            // // VM related services
            _internalContainer.Register<IAnimalKindHoster, AnimalKindHoster>(LifeTime.Singleton);
            // _internalContainer.Register<IImageDisplayer, ImageDisplayer>(LifeTime.Singleton);
            // _internalContainer.Register<IResultDisplayer, ResultDisplayer>(LifeTime.Singleton);
        }

        private void Initialize()
        {
            _internalContainer.Resolve<IMenuNavigator>().NavigateToFirstView();
        }

        public void OnExit(object sender, ExitEventArgs e)
        {
            _internalContainer.Dispose();
        }
    }
}