using System;
using System.Collections.Generic;
using VSR.Core.Services;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VSR.Core.Helpers;

internal static class DictionaryExtensions
{
    public static void UpdateAndNotify(this Dictionary<string, IIngredient> ingredientByLabel, Dictionary<Guid, IIngredient> ingredientByGuid, IIngredient previous, IIngredient newItem, IngredientsChangeMonitor monitor)
    {
        ingredientByLabel[newItem.Label] = newItem;
        monitor.SignalRemoved(previous);
        monitor.SignalAdded(newItem);
        ingredientByGuid.Remove(previous.Guid);
        ingredientByGuid.Add(newItem.Guid, newItem);
    } 
    
    public static void AddAndNotify(this Dictionary<string, IIngredient> ingredientByLabel, Dictionary<Guid, IIngredient> ingredientByGuid, IIngredient ingredient, IngredientsChangeMonitor monitor)
    {
        ingredientByLabel.Add(ingredient.Label, ingredient);
        ingredientByGuid.Add(ingredient.Guid, ingredient);
        monitor.SignalAdded(ingredient);
    } 
    
    public static void UpdateAndNotify(this Dictionary<string, IRecipe> dictionary, IRecipe previous, IRecipe newItem, RecipesChangeMonitor monitor)
    {
        dictionary[newItem.RecipeName] = newItem;
        monitor.SignalRemoved(previous);
        monitor.SignalAdded(newItem);
    }
}