using System;

namespace VSR.WPF.Utils.Navigation;

/// <summary>
/// interface behind which should be register every navigation command to display
/// </summary>
public interface INavigationCommandRegistration
{
    /// <summary>
    /// the display order (the lower is in first position in AvailableMenuCommands)
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// The display name of the element
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Get or create the view to navigate toward
    /// </summary>
    INavigeablePanel GetView();
}

/// <inheritdoc />
public sealed class NavigationCommandRegistration : INavigationCommandRegistration
{
    private readonly Func<INavigeablePanel> _getViewCallback;

    /// <inheritdoc />
    public int Priority { get; }

    /// <inheritdoc />
    public string DisplayName { get; }

    public NavigationCommandRegistration(int priority, string displayName, Func<INavigeablePanel> getViewCallback)
    {
        _getViewCallback = getViewCallback;
        Priority = priority;
        DisplayName = displayName;
    }

    /// <inheritdoc />
    public INavigeablePanel GetView()
    {
        return _getViewCallback.Invoke();
    }
    
}