using PRF.WPFCore;
using PRF.WPFCore.Commands;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify.Adapters;

internal interface IFeedForVerifyAdapter
{
}

internal sealed class FeedForVerifyAdapter : ViewModelBase, IFeedForVerifyAdapter
{
    private bool _isSelected;
    public IDelegateCommandLight ClickOnLineCommand { get; }

    public FeedForVerifyAdapter(string feedName, string quantityUnit, bool initialIsSelected = true)
    {
        FeedName = feedName;
        FeedQuantity = new FeedQuantityAdapter(quantityUnit);
        _isSelected = initialIsSelected;
        ClickOnLineCommand = new DelegateCommandLight(ExecuteClickOnLineCommand);
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