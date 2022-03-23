using System;
using PRF.WPFCore;

namespace VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;

internal interface IFeedSelectionModeView
{
}

internal interface IFeedSelectionModeAdapter
{
    event Action<IFeedSelectionModeAdapter> OnFeedSelectionModeSelected;
    IFeedSelectionModeView View { get; }
}

internal sealed class FeedSelectionModeAdapter : ViewModelBase, IFeedSelectionModeAdapter
{
    private bool _isSelected;
    public string TabHeader { get; }
    public event Action<IFeedSelectionModeAdapter>? OnFeedSelectionModeSelected;

    /// <inheritdoc />
    public IFeedSelectionModeView View { get; }

    public FeedSelectionModeAdapter(string header, IFeedSelectionModeView feedSelectionModeView)
    {
        TabHeader = header;
        View = feedSelectionModeView;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if(SetProperty(ref _isSelected, value))
            {
                if (value)
                {
                    RaiseOnFeedSelectionModeSelected(this);
                }
            }
        }
    }

    private void RaiseOnFeedSelectionModeSelected(IFeedSelectionModeAdapter obj)
    {
        OnFeedSelectionModeSelected?.Invoke(obj);
    }
}
