using System.Collections.Generic;
using System.Linq;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRationLib.Models.Feed;

/// <summary>
/// Represent a feed. Could be either a reference feed or a custom one
/// </summary>
public interface IFeed
{
    /// <summary>
    /// The main label of the feed
    /// </summary>
    string Label { get; }
    
    /// <summary>
    /// The list of string details (like INRA code, inra NUMBER) if they exists
    /// </summary>
    IReadOnlyList<IStringDetailsContent> StringDetailsContent { get; }
    
    /// <summary>
    /// The list of nutrional details
    /// </summary>
    IReadOnlyList<INutritionalFeedDetails> NutritionalDetails { get; }

    List<string> GetLabels();
    bool TryGetInraValue(InraHeader inraHeader, out double currentValue);
}


/// <inheritdoc />
public abstract class FeedBase : IFeed
{
    private readonly IReadOnlyCollection<string> _labels;
    private readonly Dictionary<InraHeader, INutritionalFeedDetails> _nutritionByInraHeader;

    protected FeedBase(IReadOnlyCollection<string> labels,
        IEnumerable<INutritionalFeedDetails> nutritionalDetails,
        IEnumerable<IStringDetailsContent> stringDetails)
    {
        _labels = labels;
        Label = labels.JoinAsLabel();
        NutritionalDetails = nutritionalDetails.ToArray();
        _nutritionByInraHeader = NutritionalDetails.ToDictionary(o => o.Header);
        StringDetailsContent = stringDetails.ToArray();
    }

    /// <inheritdoc />
    public string Label { get; }

    /// <inheritdoc />
    public IReadOnlyList<IStringDetailsContent> StringDetailsContent { get; }

    /// <inheritdoc />
    public IReadOnlyList<INutritionalFeedDetails> NutritionalDetails { get; }

    public bool TryGetInraValue(InraHeader inraHeader, out double currentValue)
    {
        if (_nutritionByInraHeader.TryGetValue(inraHeader, out var nutritionalFeedDetails))
        {
            currentValue = nutritionalFeedDetails.Value;
            return true;
        }

        currentValue = 0;
        return false;
    }
    
    /// <inheritdoc />
    public List<string> GetLabels()
    {
        return _labels.ToList();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Label;
    }
}

public interface IFeedDetails
{
    /// <summary>
    /// The details category
    /// </summary>
    InraHeader Header { get; }
}