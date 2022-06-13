using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.Recipe;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

internal interface IVerifyFeed : IFeedThatCouldBeAddedIntoReciepe
{
    IFeedAdapter GetUnderlyingFeedAdapter();
}

internal interface IFeedVerifySpecificAdapter : IVerifyFeed
{
}

internal sealed class FeedVerifySpecificAdapter : ViewModelBase, IFeedVerifySpecificAdapter
{
    private readonly IFeedAdapter _feedAdapter;
    private bool _isSelected;
    public IDelegateCommandLight ClickOnLineCommand { get; }
    
    public FeedVerifySpecificAdapter(IFeedAdapter feedAdapter, FeedUnit quantityUnit, bool initialIsSelected = true)
    {
        Name = feedAdapter.FeedName;
        FeedQuantity = new FeedQuantityAdapter(quantityUnit);
        _feedAdapter = feedAdapter;
        _isSelected = initialIsSelected;
        ClickOnLineCommand = new DelegateCommandLight(ExecuteClickOnLineCommand);
    }

    public IFeedAdapter GetUnderlyingFeedAdapter()
    {
        return _feedAdapter;
    }

    private void ExecuteClickOnLineCommand()
    {
        IsSelected = !IsSelected;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Name { get; }

    public IFeedQuantityAdapter FeedQuantity { get; }
}

internal interface IFeedQuantityAdapter
{
    string UnitDisplayName { get; set; }
    double Quantity { get; set; }
    FeedUnit Unit { get; }
}

internal sealed class FeedQuantityAdapter : ViewModelBase, IFeedQuantityAdapter
{
    public FeedUnit Unit { get; }
    private double _quantity;
    private string _unitDisplayName;

    public FeedQuantityAdapter(FeedUnit unit, double initialQuantity = 0d)
    {
        Unit = unit;
        _unitDisplayName = unit.ToDiplayName();
        _quantity = initialQuantity;
    }

    public string UnitDisplayName
    {
        get => _unitDisplayName;
        set => SetProperty(ref _unitDisplayName, value);
    }

    public double Quantity
    {
        get => _quantity;
        set
        {
            if (SetProperty(ref _quantity, value))
            {
                // set 0 if user select less than that:
                if (value < 0)
                {
#pragma warning disable CA2011
                    Quantity = 0;
#pragma warning restore CA2011
                }
            }
        }
    }
}