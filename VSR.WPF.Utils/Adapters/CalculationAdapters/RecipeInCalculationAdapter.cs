using System;
using System.Collections.Generic;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VSR.WPF.Utils.Adapters.CalculationAdapters;

public interface IRecipeIngredientInCalculationAdapter
{
    string Name { get; }
    public int QuantityPercentage { get; }
    public int Quantity { get; }
    
    IIngredient GetUnderlyingIngredient();
}

public sealed class RecipeInCalculationAdapter : ViewModelBase, IAdapterInCalculation
{
    public string Name { get; }
    private readonly IRecipe _recipe;
    private bool _isSelected = true;
    public IDelegateCommandLight ClickOnRecipeLineCommand { get; }

    public RecipeInCalculationAdapter(IRecipe recipe)
    {
        _recipe = recipe;
        Name = recipe.RecipeName;
        RecipeQuantity = new IngredientOrRecipeQuantityAdapter();
        Ingredients = recipe.IngredientsForRecipe.Select(o => new RecipeIngredientInCalculationAdapter(o.Ingredient)).ToArray();
        ClickOnRecipeLineCommand = new DelegateCommandLight(ExecuteClickOnRecipeLineCommand);
    }

    public IngredientOrRecipeQuantityAdapter RecipeQuantity { get; }

    private void ExecuteClickOnRecipeLineCommand()
    {
        IsSelected = !IsSelected;
    }

    public IReadOnlyList<IRecipeIngredientInCalculationAdapter> Ingredients { get; }

    /// <inheritdoc />
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public Guid Guid => _recipe.Guid;

    /// <summary>
    /// Adapter of an ingredient
    /// </summary>
    private sealed class RecipeIngredientInCalculationAdapter : ViewModelBase, IRecipeIngredientInCalculationAdapter
    {
        private readonly IIngredient _ingredient;
        private int _quantityPercentage;
        private int _quantity;

        public RecipeIngredientInCalculationAdapter(IIngredient ingredient)
        {
            _ingredient = ingredient;
            Name = ingredient.Label;
        }

        public string Name { get; }

        /// <inheritdoc />
        public int QuantityPercentage
        {
            get => _quantityPercentage;
            private set => SetProperty(ref _quantityPercentage, value);
        }

        /// <inheritdoc />
        public int Quantity
        {
            get => _quantity;
            private set => SetProperty(ref _quantity, value);
        }

        /// <inheritdoc />
        public IIngredient GetUnderlyingIngredient() => _ingredient;
    }
}