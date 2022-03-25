using System.Linq;
using PRF.Utils.Injection.Containers;
using PRF.WPFCore;
using VetSolutionRation.wpf.Views.Import;
using VetSolutionRation.wpf.Views.Parameters;
using VetSolutionRation.wpf.Views.RatioPanel;

namespace VetSolutionRation.wpf.Services.Navigation
{
    internal interface IMenuNavigator
    {
        INavigationCommand[] AvailableMenuCommands { get; }
        INavigeablePanel? MainPanel { get; }
        bool ShouldDisplayMenu { get; set; }
        void NavigateToFirstView();
    }

    internal interface INavigeablePanel
    {
        /// <summary>
        /// Calls when arriving to the view
        /// </summary>
        void OnNavigateTo();
        
        /// <summary>
        /// Call when exiting from the view
        /// </summary>
        void OnNavigateExit();
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class MenuNavigator : ViewModelBase, IMenuNavigator
    {
        private INavigeablePanel? _mainPanel;
        private bool _shouldDisplayMenu;

        public MenuNavigator(IInjectionContainer container)
        {
            AvailableMenuCommands = new INavigationCommand[]
            {
                new NavigationCommand("Ration Calculation", () => MainPanel = container.Resolve<RatioPanelView>()),
                new NavigationCommand("Import", () => MainPanel = container.Resolve<ImportView>()),
                new NavigationCommand("Parameters", () => MainPanel = container.Resolve<ParametersView>()),
            };
        }

        public INavigationCommand[] AvailableMenuCommands { get; }

        public bool ShouldDisplayMenu
        {
            get => _shouldDisplayMenu;
            set => SetProperty(ref _shouldDisplayMenu, value);
        }
        
        public INavigeablePanel? MainPanel
        {
            get => _mainPanel;
            private set
            {
                if(_mainPanel == value) return;
                // call the exiting method
                _mainPanel?.OnNavigateExit();
                // call the entering method on the new one before displaying
                value?.OnNavigateTo();
                // hide selection menu
                ShouldDisplayMenu = false;
                // and assign
                _mainPanel = value;
                RaisePropertyChanged();
            }
        }

        public void NavigateToFirstView()
        {
            var firstNavigablePanel = AvailableMenuCommands.FirstOrDefault();
            if (firstNavigablePanel == null)
            {
                return;
            }

            firstNavigablePanel.IsSelected = true;
            firstNavigablePanel.Command.Execute();

        }
    }
}