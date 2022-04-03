using System.ComponentModel;
using System.Linq;
using System.Windows.Xps.Packaging;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.LiveCollection;
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
    
    public ICollectionView SelectedFeedsForVerifyPanel { get; }
    private readonly ObservableCollectionRanged<FeedForVerifyAdapter> _selectedFeedForVerifyPanel;
    
    private FeedVerificationAdapter? _selectedFeedFromAvailable;
    
    public IDelegateCommandLight AddFeedToSelectionCommand { get; }
    public IDelegateCommandLight<FeedForVerifyAdapter> RemoveFromSelectedFeedsCommand { get; }
    public IDelegateCommandLight OnEnterFeedSearchPressCommand { get; }

    /// <inheritdoc />
    public VerifyRatiosViewModel(IFeedProviderHoster feedProviderHoster)
    {
        FeedProviderHoster = feedProviderHoster;
        SelectedFeedsForVerifyPanel = ObservableCollectionSource.GetDefaultView(out _selectedFeedForVerifyPanel);

        AddFeedToSelectionCommand = new DelegateCommandLight(ExecuteAddFeedToSelectionCommand, CanExecuteAddFeedToSelectionCommand);
        RemoveFromSelectedFeedsCommand = new DelegateCommandLight<FeedForVerifyAdapter>(ExecuteRemoveFromSelectedFeedsCommand);
        OnEnterFeedSearchPressCommand = new DelegateCommandLight(ExecuteOnEnterFeedSearchPressCommand);
    }

    private void ExecuteOnEnterFeedSearchPressCommand()
    {
        AddSelectedFeedToSelectedFeedItems();
    }

    private void ExecuteRemoveFromSelectedFeedsCommand(FeedForVerifyAdapter feed)
    {
        _selectedFeedForVerifyPanel.Remove(feed);
    }

    private bool CanExecuteAddFeedToSelectionCommand()
    {
        return _selectedFeedFromAvailable != null && IsCurrentNotAddedYet(_selectedFeedFromAvailable);
    }

    private bool IsCurrentNotAddedYet(FeedVerificationAdapter selectedFeedFromAvailable)
    {
        return _selectedFeedForVerifyPanel.All(o => o.FeedName != selectedFeedFromAvailable.Name);
    }

    private void ExecuteAddFeedToSelectionCommand()
    {
        //TODO TCX ajout duplicat ?
        AddSelectedFeedToSelectedFeedItems();
    }

    private void AddSelectedFeedToSelectedFeedItems()
    {
        if (_selectedFeedFromAvailable != null && IsCurrentNotAddedYet(_selectedFeedFromAvailable))
        {
            _selectedFeedForVerifyPanel.Add(new FeedForVerifyAdapter(_selectedFeedFromAvailable.Name, "kg"));
            SelectedFeedFromAvailable = null;
            AddFeedToSelectionCommand.RaiseCanExecuteChanged();
        }
    }

    public FeedVerificationAdapter? SelectedFeedFromAvailable
    {
        get => _selectedFeedFromAvailable;
        set
        {
            if (SetProperty(ref _selectedFeedFromAvailable, value))
            {
                AddFeedToSelectionCommand.RaiseCanExecuteChanged();
            }
        }
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