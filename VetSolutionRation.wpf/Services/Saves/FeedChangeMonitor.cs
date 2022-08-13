namespace VetSolutionRation.wpf.Services.Saves;

internal interface IFeedChangeMonitor
{
    void SignalReferenceChanged();
    void SignalCustomDataChanged();
    void SignalRecipeChanged();
    bool HasAnySave();
    bool ShouldSaveReference();
    bool ShouldSaveCustomDataChanged();
    bool ShouldSaveRecipeChanged();
}

/// <summary>
/// A monitor implementation that will never request saving (usefull for initial loading)
/// </summary>
internal sealed class NoSaveFeedMonitor : IFeedChangeMonitor
{
    /// <inheritdoc />
    public void SignalReferenceChanged()
    {
        // ignore it
    }

    /// <inheritdoc />
    public void SignalCustomDataChanged()
    {
        // ignore it
    }

    /// <inheritdoc />
    public void SignalRecipeChanged()
    {
        // ignore it
    }

    /// <inheritdoc />
    public bool HasAnySave() => false;

    /// <inheritdoc />
    public bool ShouldSaveReference() => false;

    /// <inheritdoc />
    public bool ShouldSaveCustomDataChanged() => false;

    /// <inheritdoc />
    public bool ShouldSaveRecipeChanged() => false;
}

/// <summary>
/// Handle all feed states change
/// </summary>
internal sealed class FeedChangeMonitor : IFeedChangeMonitor
{
    private bool _referenceChanged;
    private bool _customDataChanged;
    private bool _recipeChanged;

    public void SignalReferenceChanged()
    {
        _referenceChanged = true;
    }
    
    public void SignalCustomDataChanged()
    {
        _customDataChanged = true;
    }
    
    public void SignalRecipeChanged()
    {
        _recipeChanged = true;
    }
    
    public bool HasAnySave()
    {
        return _referenceChanged || _customDataChanged || _recipeChanged;
    }

    public bool ShouldSaveReference()
    {
        return _referenceChanged;
    }

    public bool ShouldSaveCustomDataChanged()
    {
        return _customDataChanged;
    }

    public bool ShouldSaveRecipeChanged()
    {
        return _recipeChanged;
    }
}