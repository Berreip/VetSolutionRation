using System;
using PRF.WPFCore;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;

internal interface IFeedForRecipeCreationAdapter
{
    IFeedQuantityForRecipeAdapter FeedQuantity { get; }
    IFeed GetUnderlyingFeed();
}

/// <summary>
/// Represent a single feed for the recipe creation popup
/// </summary>
internal sealed class FeedForRecipeCreationAdapter : ViewModelBase, IFeedForRecipeCreationAdapter
{
    private readonly IVerifyFeed _feed;
    private double _percentage;

    public FeedForRecipeCreationAdapter(IVerifyFeed feed)
    {
        _feed = feed;
        Name = feed.Name;
        FeedQuantity = new FeedQuantityForRecipeAdapter(feed.FeedQuantity.Unit, feed.FeedQuantity.Quantity);
    }

    public string Name { get; }
    public IFeedQuantityForRecipeAdapter FeedQuantity { get; }

    public double Percentage
    {
        get => _percentage;
        set => SetProperty(ref _percentage, value);
    }

    public bool IsValidForRecipe()
    {
        // should have a positive quantity
        return FeedQuantity.Quantity > 0d;
    }

    public IFeed GetUnderlyingFeed()
    {
        return _feed.GetUnderlyingFeed();
    }
}

internal interface IFeedQuantityForRecipeAdapter : IFeedQuantityAdapterBase
{
    event Action? OnQuantityChanged;
}

internal sealed class FeedQuantityForRecipeAdapter : FeedQuantityAdapterBase, IFeedQuantityForRecipeAdapter
{
    public event Action? OnQuantityChanged;
    
    public FeedQuantityForRecipeAdapter(FeedUnit unit, int initialQuantity) : base(unit, initialQuantity)
    {
        
    }

    /// <inheritdoc />
    protected override void QuantityUpdatedSpecific(int quantity)
    {
        RaiseOnQuantityChanged();
    }

    private void RaiseOnQuantityChanged()
    {
        OnQuantityChanged?.Invoke();
    }
}