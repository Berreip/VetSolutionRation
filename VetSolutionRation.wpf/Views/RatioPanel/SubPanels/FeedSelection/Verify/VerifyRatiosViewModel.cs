using System.ComponentModel;
using PRF.WPFCore;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Services;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify.Adapters;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal interface IVerifyRatiosViewModel
{
}

internal sealed class VerifyRatiosViewModel : ViewModelBase, IVerifyRatiosViewModel
{
    private bool _verifyFeedsDropDownOpen;
    private string? _verifyFeedSearchFilter;
    public IFeedProviderHoster FeedProviderHoster { get; }
    private ObservableCollectionRanged<FeedForVerifyAdapter> _selectedFeedForVerify;
    public ICollectionView SelectedFeedForVerify { get; }

    /// <inheritdoc />
    public VerifyRatiosViewModel(IFeedProviderHoster feedProviderHoster)
    {
        FeedProviderHoster = feedProviderHoster;
        SelectedFeedForVerify = ObservableCollectionSource.GetDefaultView(out _selectedFeedForVerify);
    }

    public string? VerifyFeedSearchFilter
    {
        get => _verifyFeedSearchFilter;
        set
        {
            if (SetProperty(ref _verifyFeedSearchFilter, value))
            {
                VerifyFeedsDropDownOpen = true;
                FeedProviderHoster.FilterAvailableFeedForVerify(value);
            }
        }
    }

    public bool VerifyFeedsDropDownOpen
    {
        get => _verifyFeedsDropDownOpen;
        set => SetProperty(ref _verifyFeedsDropDownOpen, value);
    }
}