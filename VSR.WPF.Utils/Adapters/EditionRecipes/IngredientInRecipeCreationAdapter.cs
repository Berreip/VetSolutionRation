using System;
using PRF.WPFCore;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Helpers;

namespace VSR.WPF.Utils.Adapters.EditionRecipes;

public interface IIngredientInRecipeCreationAdapter
{
    FeedUnit Unit { get; }
    int Quantity { get; }
    IIngredient GetUnderlyingIngredient();
}

/// <summary>
/// Represent a single feed for a recipe creation
/// </summary>
public sealed class IngredientInRecipeCreationAdapter : ViewModelBase, IIngredientInRecipeCreationAdapter
{
    private readonly IIngredient _ingredient;
    private double _percentage;
    private bool _isValid;
    private string? _quantityString;
    private int _quantity;
    public event Action? OnQuantityChanged;

    public FeedUnit Unit { get; } = FeedUnit.Kg;
    public IngredientInRecipeCreationAdapter(IIngredient ingredient, int quantity)
    {
        _ingredient = ingredient;
        Name = ingredient.Label;
        UnitDisplayName = FeedUnit.Kg.ToDiplayName();
        _quantity = quantity;
        _quantityString = _quantity.ToString();
        _isValid = true;
    }

    public string UnitDisplayName { get; }
    public string Name { get; }

    public double Percentage
    {
        get => _percentage;
        set => SetProperty(ref _percentage, value);
    }

    public int Quantity
    {
        get => _quantity;
        private set
        {
            if (SetProperty(ref _quantity, value))
            {
                OnQuantityChanged?.Invoke();
            }
        }
    }

    /// <inheritdoc />
    public IIngredient GetUnderlyingIngredient() => _ingredient;

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
    
    public bool IsValidForRecipe()
    {
        // should have a positive quantity
        return Quantity > 0d;
    }
}
