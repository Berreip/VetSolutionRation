using PRF.WPFCore;
using VSR.Enums;
using VSR.WPF.Utils.Helpers;

namespace VSR.WPF.Utils.Adapters.CalculationAdapters;

public sealed class IngredientOrRecipeQuantityAdapter : ViewModelBase
{
    private bool _isValid;
    private string? _quantityString;
    private int _quantity;

    public IngredientOrRecipeQuantityAdapter(int initialQuantity = 1)
    {
        UnitDisplayName = FeedUnit.Kg.ToDiplayName();
        _quantity = initialQuantity;
        _quantityString = _quantity.ToString();
        _isValid = true;
    }
    
    public string UnitDisplayName { get; }

    public int Quantity
    {
        get => _quantity;
        private set => SetProperty(ref _quantity, value);
    } 
    
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

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Quantity} {UnitDisplayName}";
    }
}
