using System;
using PRF.WPFCore;
using PRF.WPFCore.Commands;

namespace VetSolutionRation.wpf.Services.Navigation
{
    public interface INavigationCommand
    {
        string Name { get; }
        IDelegateCommandLight Command { get; }
        bool IsSelected { get; set; }
    }
    
    public sealed class NavigationCommand : ViewModelBase, INavigationCommand
    {
        private bool _isSelected;
        public string Name { get; }
        public IDelegateCommandLight Command { get; }

        public NavigationCommand(string name, Action executeNavigateCallback)
        {
            Name = name;
            Command = new DelegateCommandLight(executeNavigateCallback);
        }


        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}