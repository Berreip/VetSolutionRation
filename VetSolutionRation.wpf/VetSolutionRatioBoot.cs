using System.Collections.Generic;
using System.Windows;
using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VetSolutionRation.wpf.ImportView;
using VetSolutionRation.wpf.Views;
using VetSolutionRation.wpf.Views.AnimalSelection;
using VetSolutionRation.wpf.Views.Calculation;
using VetSolutionRation.wpf.Views.IngredientsAndRecipesList;
using VSR.Core;
using VSR.WPF.Utils;
using VSR.WPF.Utils.Navigation;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf
{
    internal sealed class VetSolutionRatioBoot
    {
        private readonly IInjectionContainer _internalContainer;

        private readonly List<IBootstrapperCore> _bootstrappers = new List<IBootstrapperCore>
        {
            new VsrCoreBootstrapper(),
            new VsrWpfUtilsBootstrapper(),
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

            // views:
            _internalContainer.RegisterType<MainPanelView>(LifeTime.Singleton);
            _internalContainer.Register<IMainPanelViewModel, MainPanelViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<AnimalSelectionView>(LifeTime.Singleton);
            _internalContainer.Register<IAnimalSelectionViewModel, AnimalSelectionViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<IngredientsAndRecipesListView>(LifeTime.Singleton);
            _internalContainer.Register<IIngredientsAndRecipesListViewModel, IngredientsAndRecipesListViewModel>(LifeTime.Singleton);

            _internalContainer.RegisterType<CalculationView>(LifeTime.Singleton);
            _internalContainer.Register<ICalculationViewModel, CalculationViewModel>(LifeTime.Singleton);
            
            _internalContainer.RegisterType<ImportView.ImportView>(LifeTime.Singleton);
            _internalContainer.Register<IImportViewModel, ImportViewModel>(LifeTime.Singleton);

            // Navigation:
            _internalContainer.RegisterOrAppendCollectionInstances<INavigationCommandRegistration>(
                new NavigationCommandRegistration(1, "Ration Calculation", () => _internalContainer.Resolve<MainPanelView>()),
                new NavigationCommandRegistration(2, "Import", () => _internalContainer.Resolve<ImportView.ImportView>())
            );

            // // VM related services
         
            _internalContainer.Register<IRecipeCalculator, RecipeCalculator>(LifeTime.Singleton);
            _internalContainer.Register<IPopupManagerLight , PopupManagerLight>(LifeTime.Singleton);
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