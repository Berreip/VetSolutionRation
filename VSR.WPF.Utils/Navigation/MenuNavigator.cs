using System.Collections.Generic;
using System.Linq;
using PRF.WPFCore;

namespace VSR.WPF.Utils.Navigation
{
    public interface IMenuNavigator
    {
        IReadOnlyList<INavigationCommand> AvailableMenuCommands { get; }
        INavigeablePanel? MainPanel { get; }
        bool ShouldDisplayMenu { get; set; }
        void NavigateToFirstView();
    }

    public interface INavigeablePanel
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
    public sealed class MenuNavigator : ViewModelBase, IMenuNavigator
    {
        private INavigeablePanel? _mainPanel;
        private bool _shouldDisplayMenu;

        public MenuNavigator(IEnumerable<INavigationCommandRegistration> navigationCommandRegistrations)
        {
            AvailableMenuCommands = navigationCommandRegistrations
                .OrderBy(o => o.Priority)
                .Select(o => new NavigationCommand(o.DisplayName, () => MainPanel = o.GetView()))
                .ToArray();
        }

        public IReadOnlyList<INavigationCommand> AvailableMenuCommands { get; }

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