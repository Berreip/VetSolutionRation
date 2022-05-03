using System.Collections.Generic;
using System.Windows;
using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VetSolutionRation.wpf.Main;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Services.Navigation;
using VetSolutionRation.wpf.Views.Import;
using VetSolutionRation.wpf.Views.Parameters;
using VetSolutionRation.wpf.Views.RatioPanel;
using VetSolutionRation.wpf.Views.RatioPanel.Components;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.AnimalSelection;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Calculate;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Verify;
using VetSolutionRationLib;

namespace VetSolutionRation.wpf.Services.Injection
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
            _internalContainer.RegisterWithInitializer<IFeedProvider, FeedProvider>(LifeTime.Singleton, o => o.LoadInitialSavedFeeds());
            _internalContainer.Register<IConfigurationManager, ConfigurationManager>(LifeTime.Singleton);

            // // views:
            _internalContainer.RegisterType<RatioPanelView>(LifeTime.Singleton);
            _internalContainer.Register<IRatioPanelViewModel, RatioPanelViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<ParametersView>(LifeTime.Singleton);
            _internalContainer.Register<IParametersViewModel, ParametersViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<AnimalSelectionView>(LifeTime.Singleton);
            _internalContainer.Register<IAnimalSelectionViewModel, AnimalSelectionViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<FeedSelectionView>(LifeTime.Singleton);
            _internalContainer.Register<IFeedSelectionViewModel, FeedSelectionViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<VerifyRatiosView>(LifeTime.Singleton);
            _internalContainer.Register<IVerifyRatiosViewModel, VerifyRatiosViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<CalculateRatiosView>(LifeTime.Singleton);
            _internalContainer.Register<ICalculateRatiosViewModel, CalculateRatiosViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<VerifyResultView>(LifeTime.Singleton);
            _internalContainer.Register<IVerifyResultViewModel, VerifyResultViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<CalculateResultView>(LifeTime.Singleton);
            _internalContainer.Register<ICalculateResultViewModel, CalculateResultViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<ImportView>(LifeTime.Singleton);
            _internalContainer.Register<IImportViewModel, ImportViewModel>(LifeTime.Singleton);

            // // VM related services
            _internalContainer.Register<IAnimalAdaptersHoster, AnimalAdaptersHoster>(LifeTime.Singleton);
            _internalContainer.Register<IFeedProviderHoster, FeedProviderHoster>(LifeTime.Singleton);
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