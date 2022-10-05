using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using PRF.Utils.CoreComponents.Diagnostic;
using VSR.Core.Extensions;
using VSR.Enums;
using VSR.Models.Ingredients;

namespace VetSolutionRation.DataProvider.Models.Helpers;

public static class InraRationImportModelExtensions
{
    private static readonly List<CultureInfo> _allowedCultures = new List<CultureInfo>
    {
        new CultureInfo("Fr-fr"),
        new CultureInfo("En-us"),
    };

    public static IIngredient ToReferenceIngredient(this IInraRationLineImportModel lineImportModel)
    {
        var details = GetAllNutritionalDetails(lineImportModel);
        // TODO PBO NIAK => voir  details.StringDetails value
        var rr = details.StringDetails;
        return new Ingredient(Guid.NewGuid(), lineImportModel.Label, false, details.NutritionalDetails);
    }


    public static IIngredient ToUserDefinedIngredient(this IInraRationLineImportModel lineImportModel)
    {
        var details = GetAllNutritionalDetails(lineImportModel);
        // TODO PBO NIAK => voir  details.StringDetails value
        var rr = details.StringDetails;
        return new Ingredient(Guid.NewGuid(), lineImportModel.Label, false, details.NutritionalDetails);
    }
    
    private static (IReadOnlyList<INutritionalDetails> NutritionalDetails, IReadOnlyList<IStringDetailsContent> StringDetails) GetAllNutritionalDetails(IInraRationLineImportModel lineImportModel)
    {
        var nutritionalDetails = new List<INutritionalDetails>();
        var stringDetails = new List<IStringDetailsContent>();
        foreach (var cell in lineImportModel.GetAllCells())
        {
            if (cell.Key.IsStringContent())
            {
                stringDetails.Add(new StringDetailsContent(cell.Key, cell.Value.Content));
            }
            else if(string.IsNullOrWhiteSpace(cell.Value.Content))
            {
                // empty cell is considered as zero
                nutritionalDetails.Add(new NutritionalDetails(cell.Key, 0d));
            }
            else if(TryParseDouble(cell, out var feedDetail))
            {
                nutritionalDetails.Add(feedDetail);
            }
            else
            {
                DebugCore.Fail($"unable to parse: {cell.Value}");
            }
            
        }
        return (nutritionalDetails, stringDetails);
    }

    private static bool TryParseDouble(KeyValuePair<InraHeader, FeedCellModel> cell,  [MaybeNullWhen(false)] out INutritionalDetails nutritionalDetails)
    {
        foreach (var culture in _allowedCultures)
        {
            if (double.TryParse(cell.Value.Content, NumberStyles.Any, culture, out var doubleValue))
            {
                nutritionalDetails = new NutritionalDetails(cell.Key, doubleValue);
                return true;
            }
        }

        nutritionalDetails = null;
        return false;
    }
}
