using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VSR.Enums;

namespace VSR.Models.Ingredients;

/// <summary>
/// Represent an ingredient.
/// </summary>
public interface IIngredient
{
    /// <summary>
    /// The main label
    /// </summary>
    string Label { get; }

    Guid Guid { get; }
    
    /// <summary>
    /// Whether or not the ingredient has been added by a user
    /// </summary>
    bool IsUserAdded { get; }

    bool TryGetNutritionDetail(InraHeader inraHeader, [MaybeNullWhen(false)] out INutritionalDetails nutritionDetail);
    
    IReadOnlyCollection<INutritionalDetails> GetNutritionDetails();
}

/// <inheritdoc />
public sealed class Ingredient : IIngredient
{
    /// <inheritdoc />
    public string Label { get; }

    /// <inheritdoc />
    public Guid Guid { get; }

    /// <inheritdoc />
    public bool IsUserAdded { get; }

    private readonly Dictionary<InraHeader, INutritionalDetails> _nutritionByInraHeader;

    public Ingredient(Guid guid,
        string label,
        bool isUserAdded,
        IEnumerable<INutritionalDetails> nutritionalDetails)
    {
        Label = label;
        IsUserAdded = isUserAdded;
        Guid = guid;
        _nutritionByInraHeader = nutritionalDetails.ToDictionary(o => o.Header);
    }

    public bool TryGetNutritionDetail(InraHeader inraHeader, [MaybeNullWhen(false)] out INutritionalDetails nutritionDetail)
    {
        if (_nutritionByInraHeader.TryGetValue(inraHeader, out var nutritionalFeedDetails))
        {
            nutritionDetail = nutritionalFeedDetails;
            return true;
        }

        nutritionDetail = null;
        return false;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<INutritionalDetails> GetNutritionDetails()
    {
        return _nutritionByInraHeader.Values;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Label;
    }
}