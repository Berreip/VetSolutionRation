using System;
using PRF.WPFCore;

namespace VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;

internal interface IResultView
{
}

internal interface IFeedSelectionModeView
{
}

internal interface IFeedSelectionModeAdapter
{
    event Action<IFeedSelectionModeAdapter> OnFeedSelectionModeSelected;
    IFeedSelectionModeView SelectionView { get; }
    IResultView ResultView { get; }
    bool IsSelected { get; set; }
}

internal sealed class FeedSelectionModeAdapter : ViewModelBase, IFeedSelectionModeAdapter
{
    private bool _isSelected;
    public string TabHeader { get; }
    public event Action<IFeedSelectionModeAdapter>? OnFeedSelectionModeSelected;

    /// <inheritdoc />
    public IFeedSelectionModeView SelectionView { get; }

    public IResultView ResultView { get; }

    public FeedSelectionModeAdapter(string header, IFeedSelectionModeView feedSelectionModeSelectionView, IResultView resultView)
    {
        TabHeader = header;
        SelectionView = feedSelectionModeSelectionView;
        ResultView = resultView;
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