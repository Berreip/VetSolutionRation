using Newtonsoft.Json;
using PRF.Utils.CoreComponents.Diagnostic;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.DataProvider.Dto;

public static class DtoExporter
{
    /// <summary>
    /// Export a model into a dto
    /// </summary>
    public static ReferenceDataDto ConvertToDto(this IEnumerable<IFeedOrRecipe> data)
    {
        var feeds = new List<IFeed>();
        var recipes = new List<IRecipe>();
        foreach (var item in data)
        {
            switch (item)
            {
                case IFeed feed:
                    feeds.Add(feed);
                    break;
                case IRecipe recipe:
                    recipes.Add(recipe);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item));
            }
        }
        
        return new ReferenceDataDto
        {
            Feeds = CreateFeedsDto(feeds),
            Recipes = CreateRecipeDto(recipes),
        };
    }

    /// <summary>
    /// Export a model into a dto
    /// </summary>
    public static ReferenceDataDto ConvertToDto(this IEnumerable<IRecipe> recipes)
    {
        return new ReferenceDataDto
        {
            Recipes = CreateRecipeDto(recipes),
        };
    }

    public static string SerializeReferenceToJson(this ReferenceDataDto model)
    {
        return JsonConvert.SerializeObject(model);
    }

    public static ReferenceDataDto DeserializeFromJson(string json)
    {
        return JsonConvert.DeserializeObject<ReferenceDataDto>(json) ?? throw new ArgumentException("DeSerializeFromJson: null result");
    }

    private static List<RecipeDto> CreateRecipeDto(IEnumerable<IRecipe> recipes)
    {
        return recipes.Select(o => new RecipeDto
        {
            Name = o.RecipeName,
            UnitLabel = o.Unit.ToReferenceLabel(),
            Ingredients = o.Ingredients.Select(CreateIngredientsDto).ToList(),
        }).ToList();
    }

    private static IngredientDto CreateIngredientsDto(IIngredientForRecipe ingredientForRecipe)
    {
        return new IngredientDto
        {
            Percentage = ingredientForRecipe.Percentage,
            FeedsInRecipe = ingredientForRecipe.Ingredient.ConvertToDto(),
        };
    }

    private static List<FeedDto> CreateFeedsDto(IEnumerable<IFeed> feeds)
    {
        return feeds.Select(ConvertToDto).ToList();
    }

    private static FeedDto ConvertToDto(this IFeed o)
    {
        return new FeedDto
        {
            Guid = o.Guid,
            IsReferenceFeed = o is IReferenceFeed,
            Labels = o.GetLabels(),
            NutritionDetails = ConvertToDto(o.NutritionalDetails),
            StringDetails = ConvertToDto(o.StringDetailsContent),
        };
    }

    private static List<StringDetailDto> ConvertToDto(IReadOnlyList<IStringDetailsContent> stringDetailsContent)
    {
        return stringDetailsContent
            .Select(o => new StringDetailDto { CellContent = o.Details, HeaderKind = o.Header.ToDtoKey() })
            .ToList();
    }

    private static List<NutritionDetailDto> ConvertToDto(IReadOnlyList<INutritionalFeedDetails> nutritionalDetails)
    {
        return nutritionalDetails
            .Select(o => new NutritionDetailDto { CellContent = o.Value, HeaderKind = o.Header.ToDtoKey() })
            .ToList();
    }

    /// <summary>
    /// Import a dto Feed into a model
    /// </summary>
    public static IFeed ConvertFromDto(this FeedDto dto)
    {
        if (dto.IsReferenceFeed)
        {
            return new ReferenceFeed(
                dto.Labels ?? throw new InvalidOperationException(),
                ConvertFromDto(dto.NutritionDetails),
                ConvertFromDto(dto.StringDetails), 
                dto.Guid);
        }

        return new CustomFeed(
            dto.Labels ?? throw new InvalidOperationException(),
            ConvertFromDto(dto.NutritionDetails), dto.Guid);
    }

    /// <summary>
    /// Import a dto into a model
    /// </summary>
    public static IRecipe ConvertFromDto(this RecipeDto dto)
    {
        return new RecipeModel(
            dto.Name ?? throw new InvalidOperationException(),
            FeedUnitExtensions.ParseFromReferenceLabel(dto.UnitLabel),
            dto.Ingredients.ConvertFromDto());
    }

    private static IReadOnlyList<IIngredientForRecipe> ConvertFromDto(this IReadOnlyCollection<IngredientDto>? ingredientDtos)
    {
        if (ingredientDtos == null)
        {
            return Array.Empty<IIngredientForRecipe>();
        }

        var percentageAll = 0d;
        var converted = new List<IIngredientForRecipe>(ingredientDtos.Count);
        foreach (var detail in ingredientDtos)
        {
            if (detail.FeedsInRecipe == null || detail.Percentage == null)
            {
                DebugCore.Fail($"invalid data loaded for recipe: {detail}");
                return Array.Empty<IIngredientForRecipe>();
            }

            percentageAll += detail.Percentage.Value;
            
            converted.Add(new IngredientForRecipe(detail.Percentage.Value, detail.FeedsInRecipe.ConvertFromDto()));
        }

        if (Math.Abs(percentageAll - 1) > 0.001)
        {
            DebugCore.Fail($"the total percentage for ingredientDtos: {string.Join(";", ingredientDtos)} is not 100%");
            return Array.Empty<IIngredientForRecipe>();
        }
        return converted;
    }

    
    private static IEnumerable<IStringDetailsContent> ConvertFromDto(IEnumerable<StringDetailDto>? dtoStringDetails)
    {
        if (dtoStringDetails == null)
        {
            return Array.Empty<IStringDetailsContent>();
        }

        var converted = new List<IStringDetailsContent>();
        foreach (var detail in dtoStringDetails)
        {
            if (detail.HeaderKind != null &&
                detail.CellContent != null &&
                InraHeaderExtensions.TryParseDtoInraHeader(detail.HeaderKind, out var inraHeader))
            {
                converted.Add(new StringDetailsContent(inraHeader, detail.CellContent));
            }
        }

        return converted;
    }

    private static IEnumerable<INutritionalFeedDetails> ConvertFromDto(IEnumerable<NutritionDetailDto>? nutritionDetail)
    {
        if (nutritionDetail == null)
        {           
            return Array.Empty<INutritionalFeedDetails>();
        }

        var converted = new List<INutritionalFeedDetails>();
        foreach (var detail in nutritionDetail)
        {
            if (detail.HeaderKind != null &&
                detail.CellContent != null &&
                InraHeaderExtensions.TryParseDtoInraHeader(detail.HeaderKind, out var inraHeader))
            {
                converted.Add(new NutritionalFeedDetails(inraHeader, detail.CellContent.Value));
            }
        }

        return converted;
    }
}