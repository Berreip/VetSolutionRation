using System;
using System.Collections.Generic;
using System.Globalization;
using MaterialDesignThemes.Wpf;
using PRF.WPFCore;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Import.Adapters;

/// <summary>
/// Represent a loaded feed data (could be custom or reference)
/// </summary>
internal sealed class RegisteredDataAdapter : ViewModelBase
{
    private readonly IFeedOrRecipe _backingdata;
    private readonly bool _isReferenceFeed;
    public string LoadedDataLabel => _backingdata.UniqueReferenceKey;
    public PackIconKind DataIconKind { get; }

    public RegisteredDataAdapter(IFeedOrRecipe backingdata)
    {
        _backingdata = backingdata;
        _isReferenceFeed = backingdata is IReferenceFeed;
        DataIconKind = _isReferenceFeed ? PackIconKind.FileChart : PackIconKind.AccountEdit;
    }

    public IReadOnlyCollection<RegisteredNutrionalDetailsAdapter> GetDetails()
    {
        if (_backingdata is IFeed feed)
        {
            var detailsAdapters = new List<RegisteredNutrionalDetailsAdapter>();
            foreach (var stringDetails in feed.StringDetailsContent)
            {
                detailsAdapters.Add(new RegisteredNutrionalDetailsAdapter(stringDetails.Header.GetInraHeaderLabel(), stringDetails.Details));
            }

            foreach (var nutritionalDetail in feed.NutritionalDetails)
            {
                detailsAdapters.Add(new RegisteredNutrionalDetailsAdapter(nutritionalDetail.Header.GetInraHeaderLabel(), nutritionalDetail.Value.ToString(CultureInfo.InvariantCulture)));
            }

            return detailsAdapters.ToArray();
        }

        return Array.Empty<RegisteredNutrionalDetailsAdapter>();
    }
}

internal sealed class RegisteredNutrionalDetailsAdapter : ViewModelBase
{
    public string Value { get; }
    public string HeaderKind { get;  }
    
    public RegisteredNutrionalDetailsAdapter(string header, string value)
    {
        Value = value;
        HeaderKind = header;
    }
}