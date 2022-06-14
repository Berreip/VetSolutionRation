using System;
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

internal interface IFeedQuantityAdapter
{
    string UnitDisplayName { get; set; }
    int Quantity { get; }
    string? QuantityString { get; set; }
    FeedUnit Unit { get; }
    bool IsValid { get; }
}

internal sealed class FeedQuantityAdapter : ViewModelBase, IFeedQuantityAdapter
{
    public FeedUnit Unit { get; }
    private string _unitDisplayName;
    private bool _isValid;
    private string? _quantityString;

    public FeedQuantityAdapter(FeedUnit unit, int initialQuantity = 1)
    {
        Unit = unit;
        _unitDisplayName = unit.ToDiplayName();
        Quantity = initialQuantity;
        _quantityString = Quantity.ToString();
        _isValid = true;
    }

    public string UnitDisplayName
    {
        get => _unitDisplayName;
        set => SetProperty(ref _unitDisplayName, value);
    }

    public int Quantity { get; private set; }

    public bool IsValid
    {
        get => _isValid;
        private set => SetProperty(ref _isValid, value);
    }

    /// <inheritdoc />
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