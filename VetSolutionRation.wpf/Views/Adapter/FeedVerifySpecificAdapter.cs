using System;
using System.ComponentModel;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Adapter;

internal interface IVerifyFeed : IFeedThatCouldBeAddedIntoRecipe
{
    IFeed GetUnderlyingFeed();
    Guid Guid { get; }
}

internal interface IFeedVerifySpecificAdapter : IVerifyFeed
{
}

internal sealed class FeedVerifySpecificAdapter : ViewModelBase, IFeedVerifySpecificAdapter
{
    private readonly IFeed _feed;
    private readonly Action<bool>? _onSelectedChangedCallback;
    private bool _isSelected;
    public IDelegateCommandLight ClickOnLineCommand { get; }
    
    public FeedVerifySpecificAdapter(IFeed feed, FeedUnit quantityUnit, Action<bool>? onSelectedChangedCallback, bool initialIsSelected = true)
    {
        Name = feed.Label;
        FeedQuantity = new FeedQuantityAdapter(quantityUnit);
        _feed = feed;
        _onSelectedChangedCallback = onSelectedChangedCallback;
        _isSelected = initialIsSelected;
        ClickOnLineCommand = new DelegateCommandLight(ExecuteClickOnLineCommand);
    }

    public IFeed GetUnderlyingFeed() => _feed;

    /// <inheritdoc />
    public Guid Guid => _feed.Guid;

    private void ExecuteClickOnLineCommand()
    {
        IsSelected = !IsSelected;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetProperty(ref _isSelected, value))
            {
                _onSelectedChangedCallback?.Invoke(value);
            };
        }
    }

    public string Name { get; }

    public IFeedQuantityAdapter FeedQuantity { get; }
}

internal interface IFeedQuantityAdapter : IFeedQuantityAdapterBase
{
}

internal interface IFeedQuantityAdapterBase
{
    FeedUnit Unit { get; }
    string UnitDisplayName { get; set; }
    int Quantity { get; }
    bool IsValid { get; }
    string? QuantityString { get; set; }
}

internal abstract class FeedQuantityAdapterBase : ViewModelBase, IFeedQuantityAdapterBase
{
    public FeedUnit Unit { get; }
    private string _unitDisplayName;
    private bool _isValid;
    private string? _quantityString;
    private int _quantity;

    public FeedQuantityAdapterBase(FeedUnit unit, int initialQuantity = 1)
    {
        Unit = unit;
        _unitDisplayName = unit.ToDiplayName();
        _quantity = initialQuantity;
        _quantityString = _quantity.ToString();
        _isValid = true;
    }

    public string UnitDisplayName
    {
        get => _unitDisplayName;
        set => SetProperty(ref _unitDisplayName, value);
    }

    public int Quantity
    {
        get => _quantity;
        private set
        {
            if(SetProperty(ref _quantity, value))
            {
                QuantityUpdatedSpecific(value);
            }
        }
    }

    protected abstract void QuantityUpdatedSpecific(int quantity);

    public bool IsValid
    {
        get => _isValid;
        private set => SetProperty(ref _isValid, value);
    }

    public string? QuantityString
    {
        get => _quantityString;
        set
        {
            if (SetProperty(ref _quantityString, value))
            {
                if (value != null && int.TryParse(value, out var parsedQuantity) && parsedQuantity > 0)
                {
                    Quantity = parsedQuantity;
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
            }
        }
    }
}

internal sealed class FeedQuantityAdapter : FeedQuantityAdapterBase, IFeedQuantityAdapter
{
    public FeedQuantityAdapter(FeedUnit unit, int initialQuantity = 1) : base(unit, initialQuantity)
    {
    }

    /// <inheritdoc />
    protected override void QuantityUpdatedSpecific(int quantity)
    {
        // do nothing
    }
}