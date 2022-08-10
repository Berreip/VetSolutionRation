using System;
using System.Collections.Generic;

namespace VetSolutionRationLib.Models.Feed;

/// <summary>
/// A feed that was created from a reference file (inrae for instance)
/// </summary>
public interface IReferenceFeed : IFeed
{
}

/// <inheritdoc cref="VetSolutionRationLib.Models.Feed.IReferenceFeed"/>
public sealed class ReferenceFeed : FeedBase, IReferenceFeed
{
    public ReferenceFeed(IReadOnlyCollection<string> labels,
        IEnumerable<INutritionalFeedDetails> nutritionalDetails,
        IEnumerable<IStringDetailsContent> stringDetails,
        Guid guid)
        : base(labels, nutritionalDetails, stringDetails, guid)
    {
    }

    public ReferenceFeed(string labels,
        IEnumerable<INutritionalFeedDetails> nutritionalDetails,
        IEnumerable<IStringDetailsContent> stringDetails, 
        Guid guid)
        : this(new[] { labels }, nutritionalDetails, stringDetails, guid)
    {
    }
}