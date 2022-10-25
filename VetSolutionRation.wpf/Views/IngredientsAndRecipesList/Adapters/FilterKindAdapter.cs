using System;
using PRF.WPFCore;
using VSR.WPF.Utils.Services;

namespace VetSolutionRation.wpf.Views.IngredientsAndRecipesList.Adapters;

internal sealed class FilterKindAdapter : ViewModelBase
{
    public string DisplayText { get; }
    public FilterKind Kind { get; }

    public FilterKindAdapter(FilterKind kind)
    {
        Kind = kind;
        DisplayText = GetDisplayTextFromKind(kind);
    }

    private static string GetDisplayTextFromKind(FilterKind kind)
    {
        switch (kind)
        {
            case FilterKind.Ingredient:
                return "Ingredient";
            case FilterKind.Recipe:
                return "Recette";
            case FilterKind.All:
                return "Tous";
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }

    /// <inheritdoc />
    public override string ToString() => DisplayText;
}