using System;
using System.Collections.Generic;
using System.Windows;
using VetSolutionRation.Common.Async;

namespace VetSolutionRation.wpf.Services.PopupManager;

internal interface IPopupManagerLight
{
    /// <summary>
    /// Display a popup with a ShowDialog and register it for self-closing
    /// </summary>
    void ShowDialog<TViewModel>(Func<TViewModel> createViewModel, Func<TViewModel, Window> createView)
        where TViewModel :IPopupViewModel;

    bool RequestClosing(IPopupViewModel vmToClose);
}


internal sealed class PopupManagerLight : IPopupManagerLight
{
    private readonly object _key = new object();
    private readonly Dictionary<IPopupViewModel, Window> _registeredVm = new Dictionary<IPopupViewModel, Window>();

    /// <inheritdoc />
    public void ShowDialog<TViewModel>(Func<TViewModel> createViewModel, Func<TViewModel, Window> createView) where TViewModel : IPopupViewModel
    {
        var vm = createViewModel();
        var view = createView(vm);
        view.Closed += ViewOnClosed;
        RegisterForClosing(vm, view);
        view.ShowDialog();
    }

    /// <inheritdoc />
    public bool RequestClosing(IPopupViewModel vmToClose)
    {
        Window? relatedWindow;
        lock (_key)
        {
            if (_registeredVm.TryGetValue(vmToClose, out relatedWindow))
            {
                // unregister to avoid multiple call before closing
                UnRegister(vmToClose);
            }
        }

        if (relatedWindow == null)
        {
            return false;
        }

        relatedWindow.Close();
        return true;
    }

    private void ViewOnClosed(object? sender, EventArgs e)
    {
        AsyncWrapper.Wrap(() =>
        {
            lock (_key)
            {
                if (sender is Window window && window.DataContext is IPopupViewModel vm)
                {
                    UnRegister(vm);
                    // unregister itself
                    window.Closed -= ViewOnClosed;
                    // warning memory leak => manualy detach datacontext
                    window.DataContext = null;
                }
            }
        });
    }

    private void UnRegister(IPopupViewModel vm)
    {
        lock (_key)
        {
            _registeredVm.Remove(vm);
        }
    }

    private void RegisterForClosing(IPopupViewModel vm, Window window)
    {
        lock (_key)
        {
            _registeredVm.Add(vm, window);
        }
    }
}

/// <summary>
/// Signal a popup viewModel
/// </summary>
internal interface IPopupViewModel
{
}