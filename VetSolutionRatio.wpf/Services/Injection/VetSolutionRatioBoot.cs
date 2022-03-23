using System.Collections.Generic;
using System.Windows;
using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VetSolutionRatio.wpf.Main;
using VetSolutionRatio.wpf.Services.Navigation;
using VetSolutionRatio.wpf.Views.Parameters;
using VetSolutionRatio.wpf.Views.RatioPanel;
using VetSolutionRatio.wpf.Views.RatioPanel.Components;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.AnimalSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.Results;
using VetSolutionRatioLib;

namespace VetSolutionRatio.wpf.Services.Injection
{
    internal sealed class VetSolutionRatioBoot
    {
        private readonly IInjectionContainer _internalContainer;
        private readonly List<IBootstrapperCore> _bootstrappers = new List<IBootstrapperCore>
        {
            new VetSolutionRatioLibBootstrapper()
        };

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
            foreach (var bootstrapper in _bootstrappers)
            {
                bootstrapper.Register(_internalContainer);
            }
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

            _internalContainer.RegisterType<FeedSelectionView>(LifeTime.Singleton);
            _internalContainer.Register<IFeedSelectionViewModel, FeedSelectionViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<ResultView>(LifeTime.Singleton);
            _internalContainer.Register<IResultViewModel, ResultViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<VerifyRatiosView>(LifeTime.Singleton);
            _internalContainer.Register<IVerifyRatiosViewModel, VerifyRatiosViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<CalculateRatiosView>(LifeTime.Singleton);
            _internalContainer.Register<ICalculateRatiosViewModel, CalculateRatiosViewModel>(LifeTime.Singleton);

            // // VM related services
            _internalContainer.Register<IAnimalAdaptersHoster, AnimalAdaptersHoster>(LifeTime.Singleton);
        }

        private void Initialize()
        {
            foreach (var bootstrapper in _bootstrappers)
            {
                bootstrapper.InitializeAsync(_internalContainer).Wait();
            }
            _internalContainer.Resolve<IMenuNavigator>().NavigateToFirstView();
        }

        public void OnExit(object sender, ExitEventArgs e)
        {
            _internalContainer.Dispose();
        }
    }
}