using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PRF.Utils.CoreComponents.Diagnostic;
using VSR.Core.Helpers;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VSR.Core.Services;

public interface IIngredientsManager
{
    /// <summary>
    /// Returns all registered Ingredients
    /// </summary>
    IReadOnlyCollection<IIngredient> GetAllIngredients();

    /// <summary>
    /// Returns all registered recipes
    /// </summary>
    IReadOnlyCollection<IRecipe> GetAllRecipes();

    /// <summary>
    /// Event raised when ingredient changes
    /// </summary>
    event Action<IIngredientsChangeMonitor>? OnIngredientsChanged;

    /// <summary>
    /// Event raised when ingredient changes
    /// </summary>
    event Action<IRecipesChangeMonitor>? OnRecipesChanged;

    bool TryGetIngredientByName(string name, [MaybeNullWhen(false)] out IIngredient ingredient);
    bool TryGetRecipeByName(string name, [MaybeNullWhen(false)] out IRecipe recipe);

    void AddIngredients(IEnumerable<IIngredient> ingredient);
    IReadOnlyList<IRecipe> AddRecipes(IEnumerable<IRecipeCandidate> recipeCandidates);
    
    void DeleteIngredient(IIngredient ingredient);
    void DeleteRecipe(IRecipe recipe);
}

public sealed class IngredientsManager : IIngredientsManager
{
    private readonly object _key = new object();
    private readonly Dictionary<string, IIngredient> _ingredientByLabel = new Dictionary<string, IIngredient>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Guid, IIngredient> _ingredientByGuid = new Dictionary<Guid, IIngredient>();
    private readonly Dictionary<string, IRecipe> _recipeByLabel = new Dictionary<string, IRecipe>(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public event Action<IIngredientsChangeMonitor>? OnIngredientsChanged;

    /// <inheritdoc />
    public event Action<IRecipesChangeMonitor>? OnRecipesChanged;

    /// <inheritdoc />
    public IReadOnlyCollection<IIngredient> GetAllIngredients()
    {
        lock (_key)
        {
            return _ingredientByGuid.Values.ToArray();
        }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IRecipe> GetAllRecipes()
    {
        lock (_key)
        {
            return _recipeByLabel.Values.ToArray();
        }
    }

    /// <inheritdoc />
    public bool TryGetIngredientByName(string name, [MaybeNullWhen(false)] out IIngredient ingredient)
    {
        lock (_key)
        {
            return _ingredientByLabel.TryGetValue(name, out ingredient);
        }
    }

    /// <inheritdoc />
    public bool TryGetRecipeByName(string name, [MaybeNullWhen(false)] out IRecipe recipe)
    {
        lock (_key)
        {
            return _recipeByLabel.TryGetValue(name, out recipe);
        }
    }

    /// <inheritdoc />
    public void AddIngredients(IEnumerable<IIngredient> ingredients)
    {
        var monitor = new IngredientsChangeMonitor();
        lock (_key)
        {
            foreach (var ingredient in ingredients)
            {
                AddIngredient(ingredient, monitor);
            }
        }

        RaiseOnIngredientsChanged(monitor);
    }

    /// <inheritdoc />
    public void DeleteIngredient(IIngredient ingredient)
    {
        var monitor = new IngredientsChangeMonitor();
        lock (_key)
        {
            if (_ingredientByLabel.TryGetValue(ingredient.Label, out var ingredientMatching))
            {
                _ingredientByLabel.Remove(ingredient.Label);
                _ingredientByGuid.Remove(ingredientMatching.Guid);
                monitor.SignalRemoved(ingredientMatching);
            }
        }
        RaiseOnIngredientsChanged(monitor);
    }

    private void AddIngredient(IIngredient ingredient, IngredientsChangeMonitor monitor)
    {
        // 2 cases:
        // 1) there was already an existing ingredient:
        if (_ingredientByLabel.TryGetValue(ingredient.Label, out var previous))
        {
            // if the new one is user added: 2 cases:
            if (ingredient.IsUserAdded)
            {
                // 1.1) the previous is a reference one: ignore the new one and trace error:
                if (!previous.IsUserAdded)
                {
                    DebugCore.Fail($"the ingredient {ingredient.Label} is a user added data that try to override reference data");
                    return;
                }
                // 2) if the previous was also a user added ingredient, just replace it
                DebugCore.Assert(previous.Guid != ingredient.Guid, "previous.Guid == ingredient.Guid => trying to add the same element");
                _ingredientByLabel.UpdateAndNotify(_ingredientByGuid, previous, ingredient, monitor);
            }
            // else if the new one is a reference, replace 
            else
            {
                _ingredientByLabel.UpdateAndNotify(_ingredientByGuid, previous, ingredient, monitor);
            }
        }
        else
        {
            // if no previous, just add
            _ingredientByLabel.AddAndNotify(_ingredientByGuid,  ingredient, monitor);
        }
    }


    /// <inheritdoc />
    public void DeleteRecipe(IRecipe recipe)
    {
        var monitor = new RecipesChangeMonitor();
        lock (_key)
        {
            if (_recipeByLabel.Remove(recipe.RecipeName))
            {
                monitor.SignalRemoved(recipe);
            }
            else
            {
                DebugCore.Fail($"Trying to remove recipe {recipe.RecipeName} but it was not found in reference list");
            }
        }
        RaiseOnRecipesChanged(monitor);
    }
    
    /// <inheritdoc />
    public IReadOnlyList<IRecipe> AddRecipes(IEnumerable<IRecipeCandidate> recipeCandidates)
    {
        var monitor = new RecipesChangeMonitor();
        var addedRecipes = new List<IRecipe>();
        lock (_key)
        {
            foreach (var recipeCandidate in recipeCandidates)
            {
                if (!TryGetMatchingIngredientForRecipe(recipeCandidate.Ingredients, out var ingredientsForRecipe))
                {
                    continue;
                }

                var newRecipe = new Recipe(recipeCandidate.Guid, recipeCandidate.Name, ingredientsForRecipe);
                addedRecipes.Add(newRecipe);
                
                if (_recipeByLabel.TryGetValue(recipeCandidate.Name, out var recipe))
                {
                    // remove the previous and add the new one
                    _recipeByLabel[recipeCandidate.Name] = newRecipe;
                    monitor.SignalRemoved(recipe);
                }
                else
                {
                    _recipeByLabel.Add(recipeCandidate.Name, newRecipe);
                }
                // signal added in both case
                monitor.SignalAdded(newRecipe);
            }
        }
        RaiseOnRecipesChanged(monitor);
        return addedRecipes;
    }

    /// <summary>
    /// Try to match each Guid candidate with an exisiting ingredient
    /// </summary>
    private bool TryGetMatchingIngredientForRecipe(
        IReadOnlyCollection<IIngredientForRecipeCandidate> recipeCandidateIngredients,
        [MaybeNullWhen(false)] out IReadOnlyList<IIngredientForRecipe> matchingIngredientForRecipe)
    { 
        var ingredientsForRecipe = new List<IIngredientForRecipe>(recipeCandidateIngredients.Count);
                
        // first, check that all ingredients are available:
        var totalPercentage = 0d;
        foreach (var candidateIngredient in recipeCandidateIngredients)
        {
            if (_ingredientByGuid.TryGetValue(candidateIngredient.IngredientGuid, out var ingredient))
            {
                totalPercentage += candidateIngredient.Percentage;
                ingredientsForRecipe.Add(new IngredientForRecipe(candidateIngredient.Percentage, ingredient));
            }
            else
            {
                DebugCore.Fail($"a recipe ingredient guid GUID: {candidateIngredient.IngredientGuid} was not available among known ingredient list");
                matchingIngredientForRecipe = null;
                return false;
            }
        }

        if (Math.Abs(totalPercentage - 1) > 0.001)
        {
            DebugCore.Fail($"the total recipe ingredients percentage is not 100% ({totalPercentage}%)");
            matchingIngredientForRecipe = null;
            return false;
        }

        matchingIngredientForRecipe = ingredientsForRecipe;
        return true;
    }

    private void RaiseOnIngredientsChanged(IngredientsChangeMonitor monitor)
    {
        if (!monitor.IsEmpty())
        {
            OnIngredientsChanged?.Invoke(monitor);
        }
    }

    private void RaiseOnRecipesChanged(RecipesChangeMonitor monitor)
    {
        if (!monitor.IsEmpty())
        {
            OnRecipesChanged?.Invoke(monitor);
        }
    }
}