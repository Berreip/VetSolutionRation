using PRF.WPFCore;
using PRF.WPFCore.Commands;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

internal sealed class FeedVerifySpecificAdapter : ViewModelBase
{
    private readonly IFeedAdapter _feedAdapter;
    private bool _isSelected;
    public IDelegateCommandLight ClickOnLineCommand { get; }
    
    public FeedVerifySpecificAdapter(IFeedAdapter feedAdapter, string quantityUnit, bool initialIsSelected = true)
    {
        FeedName = feedAdapter.FeedName;
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

    public string FeedName { get; }

    public FeedQuantityAdapter FeedQuantity { get; }
}

internal sealed class FeedQuantityAdapter : ViewModelBase
{
    private double _quantity;
    private string _unit;

    public FeedQuantityAdapter(string unit, double initialQuantity = 0d)
    {
        _unit = unit;
    }

    public string Unit
    {
        get => _unit;
        set => SetProperty(ref _unit, value);
    }

    public double Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }
}