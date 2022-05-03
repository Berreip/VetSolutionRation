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
    private readonly IFeed _feed;
    private readonly bool _isReferenceFeed;
    public string LoadedDataLabel => _feed.Label;
    public PackIconKind DataIconKind { get; }

    public RegisteredDataAdapter(IFeed feed)
    {
        _feed = feed;
        _isReferenceFeed = feed is IReferenceFeed;
        DataIconKind = _isReferenceFeed ? PackIconKind.FileChart : PackIconKind.AccountEdit;
    }

    public RegisteredNutrionalDetailsAdapter[] GetDetails()
    {
        var detailsAdapters = new List<RegisteredNutrionalDetailsAdapter>();
        foreach (var stringDetails in _feed.StringDetailsContent)
        {
            detailsAdapters.Add(new RegisteredNutrionalDetailsAdapter(stringDetails.Header.GetInraHeaderLabel(), stringDetails.Details));
        }

        foreach (var nutritionalDetail in _feed.NutritionalDetails)
        {
            detailsAdapters.Add(new RegisteredNutrionalDetailsAdapter(nutritionalDetail.Header.GetInraHeaderLabel(), nutritionalDetail.Value.ToString(CultureInfo.InvariantCulture)));
        }

        return detailsAdapters.ToArray();
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