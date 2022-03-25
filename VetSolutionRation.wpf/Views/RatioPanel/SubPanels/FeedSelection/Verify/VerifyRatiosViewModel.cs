using PRF.WPFCore;
using VetSolutionRation.wpf.Services;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal interface IVerifyRatiosViewModel
{
}

internal sealed class VerifyRatiosViewModel : ViewModelBase, IVerifyRatiosViewModel
{
    private bool _verifyFeedsDropDownOpen;
    private string? _verifyFeedSearchFilter;
    public IFeedProviderHoster FeedProviderHoster { get; }

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

    /// <inheritdoc />
    public VerifyRatiosViewModel(IFeedProviderHoster feedProviderHoster)
    {
        FeedProviderHoster = feedProviderHoster;
    }

}