using System.Collections.Generic;
using System.Globalization;
using MaterialDesignThemes.Wpf;
using PRF.WPFCore;
using VSR.Core.Extensions;
using VSR.Models.Ingredients;

namespace VSR.WPF.Utils.Adapters.ImportPanel;

/// <summary>
/// Represent a loaded feed data (could be custom or reference)
/// </summary>
public sealed class RegisteredDataAdapter : ViewModelBase
{
    private readonly IIngredient _backingdata;
    public string LoadedDataLabel => _backingdata.Label;
    public PackIconKind DataIconKind { get; }

    public RegisteredDataAdapter(IIngredient backingdata)
    {
        _backingdata = backingdata;
        DataIconKind = backingdata.IsUserAdded ? PackIconKind.AccountEdit : PackIconKind.FileChart;
    }

    public IReadOnlyCollection<RegisteredNutrionalDetailsAdapter> GetDetails()
    {
        var detailsAdapters = new List<RegisteredNutrionalDetailsAdapter>
        {
            new RegisteredNutrionalDetailsAdapter(_backingdata.Label, string.Empty),
        };

        foreach (var nutritionalDetail in _backingdata.GetNutritionDetails())
        {
            detailsAdapters.Add(new RegisteredNutrionalDetailsAdapter(nutritionalDetail.Header.GetInraHeaderLabel(), nutritionalDetail.Value.ToString(CultureInfo.InvariantCulture)));
        }
        return detailsAdapters.ToArray();
    }
}

public sealed class RegisteredNutrionalDetailsAdapter : ViewModelBase
{
    public string Value { get; }
    public string HeaderKind { get; }

    public RegisteredNutrionalDetailsAdapter(string header, string value)
    {
        Value = value;
        HeaderKind = header;
    }
}