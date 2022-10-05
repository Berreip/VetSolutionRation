using System.Collections.Generic;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VSR.Core.Services;

/// <summary>
/// Handle ingredients states change
/// </summary>
public interface IIngredientsChangeMonitor : IChangeMonitorBase<IIngredient>
{
}

/// <summary>
/// Handle recipe states change
/// </summary>
public interface IRecipesChangeMonitor : IChangeMonitorBase<IRecipe>
{
}

public interface IChangeMonitorBase<out T>
{
    IReadOnlyCollection<T> GetAdded();
    IReadOnlyCollection<T> GetRemoved();
}

internal abstract class ChangeMonitorBase<T> : IChangeMonitorBase<T>
{
    private readonly HashSet<T> _added = new HashSet<T>();
    private readonly HashSet<T> _removed = new HashSet<T>();

    public void SignalAdded(T ingredient)
    {
        _added.Add(ingredient);
    }

    public void SignalRemoved(T ingredient)
    {
        _removed.Add(ingredient);
    }

    public bool IsEmpty()
    {
        return _removed.Count == 0 && _added.Count == 0;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<T> GetAdded() => _added;

    /// <inheritdoc />
    public IReadOnlyCollection<T> GetRemoved() => _removed;
}

/// <inheritdoc cref="VSR.Core.Services.IIngredientsChangeMonitor" />
internal sealed class IngredientsChangeMonitor : ChangeMonitorBase<IIngredient>, IIngredientsChangeMonitor
{
}

/// <inheritdoc cref="VSR.Core.Services.IRecipesChangeMonitor" />
internal sealed class RecipesChangeMonitor : ChangeMonitorBase<IRecipe>, IRecipesChangeMonitor
{
}