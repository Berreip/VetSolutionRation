using System;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VSR.Models.Ingredients;

namespace VSR.WPF.Utils.Adapters.CalculationAdapters;

public sealed class IngredientInCalculationAdapter : ViewModelBase, IAdapterInCalculation
{
    private readonly IIngredient _ingredient;
    public string Name => _ingredient.Label;
    private readonly Action<bool>? _onSelectedChangedCallback;
    public IngredientOrRecipeQuantityAdapter IngredientQuantity { get; }
    public IDelegateCommandLight ClickOnLineCommand { get; }
    private bool _isSelected;
    
    public IngredientInCalculationAdapter(IIngredient ingredient, Action<bool>? onSelectedChangedCallback, bool initialIsSelected = true)
    {
        _ingredient = ingredient;
        IngredientQuantity = new IngredientOrRecipeQuantityAdapter();
        _onSelectedChangedCallback = onSelectedChangedCallback;
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
        set
        {
            if (SetProperty(ref _isSelected, value))
            {
                _onSelectedChangedCallback?.Invoke(value);
            };
        }
    }

    public Guid Guid => _ingredient.Guid;

    public IIngredient GetUnderlyingIngredient() => _ingredient;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Name} (isSelected:{IsSelected}) Qty:{IngredientQuantity} ";
    }
}

