using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using PRF.Utils.CoreComponents.Diagnostic;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.DataProvider.Models.Helpers;

public static class InraRationImportModelExtensions
{
    private static readonly List<CultureInfo> _allowedCultures = new List<CultureInfo>
    {
        new CultureInfo("Fr-fr"),
        new CultureInfo("En-us"),
    };

    public static IReferenceFeed ToReferenceFeed(this IInraRationLineImportModel lineImportModel)
    {
        var details = GetAllNutritionalDetails(lineImportModel);
        return new ReferenceFeed(lineImportModel.GetLabels(), details.NutritionalDetails, details.StringDetails);
    }


    public static ICustomFeed ToCustomFeed(this IInraRationLineImportModel lineImportModel)
    {
        var details = GetAllNutritionalDetails(lineImportModel);
        return new CustomFeed(lineImportModel.GetLabels(), details.NutritionalDetails, details.StringDetails);
    }
    
    private static (IReadOnlyList<INutritionalFeedDetails> NutritionalDetails, IReadOnlyList<IStringDetailsContent> StringDetails) GetAllNutritionalDetails(IInraRationLineImportModel lineImportModel)
    {
        var nutritionalDetails = new List<INutritionalFeedDetails>();
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
                nutritionalDetails.Add(new NutritionalFeedDetails(cell.Key, 0d));
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

    private static bool TryParseDouble(KeyValuePair<InraHeader, FeedCellModel> cell,  [MaybeNullWhen(false)] out INutritionalFeedDetails nutritionalDetails)
    {
        foreach (var culture in _allowedCultures)
        {
            if (double.TryParse(cell.Value.Content, NumberStyles.Any, culture, out var doubleValue))
            {
                nutritionalDetails = new NutritionalFeedDetails(cell.Key, doubleValue);
                return true;
            }
        }

        nutritionalDetails = null;
        return false;
    }
}
