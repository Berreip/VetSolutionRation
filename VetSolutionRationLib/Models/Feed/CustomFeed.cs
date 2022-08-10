using System;
using System.Collections.Generic;

namespace VetSolutionRationLib.Models.Feed;

/// <summary>
/// A feed that was created by a user or imported from another user
/// </summary>
public interface ICustomFeed: IFeed
{
}

/// <inheritdoc cref="VetSolutionRationLib.Models.Feed.ICustomFeed" />
public sealed class CustomFeed : FeedBase, ICustomFeed
{
    public CustomFeed(IReadOnlyCollection<string> labels,
        IEnumerable<INutritionalFeedDetails> nutritionalDetails, 
        Guid guid) 
        : base(labels, nutritionalDetails, Array.Empty<IStringDetailsContent>(), guid)
    {
    }
}