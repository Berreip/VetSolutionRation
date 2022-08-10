using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Moq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.JSON;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.DataProvider.UnitTests.Dto;

[TestFixture]
internal sealed class DtoExportTest
{
    [Test]
    public void ExportDto_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new IFeed[]
        {
            new ReferenceFeed(new[] { "foo1" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>(), Guid.NewGuid()),
            new ReferenceFeed(new[] { "foo2" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>(), Guid.NewGuid()),
            new ReferenceFeed(new[] { "foo3" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>(), Guid.NewGuid()),
            new ReferenceFeed(new[] { "foo4" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>(), Guid.NewGuid()),
            new ReferenceFeed(new[] { "foo5" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>(), Guid.NewGuid()),
            new ReferenceFeed(new[] { "foo6" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>(), Guid.NewGuid()),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(sut.ConvertToDto().SerializeToJson());

        //Assert
        Assert.IsNotNull(res.Feeds);
        Assert.AreEqual(6, res.Feeds!.Count);
        Assert.IsTrue(res.Feeds.All(o => o.Guid != Guid.Empty));
    }

    [Test]
    public void ExportDto_then_serialize_then_deserialize_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new IFeed[]
        {
            new ReferenceFeed(new[] { "foo1" },
                new INutritionalFeedDetails[]
                {
                    new NutritionalFeedDetails(InraHeader.Amidon, 1),
                    new NutritionalFeedDetails(InraHeader.C12_0, 2),
                    new NutritionalFeedDetails(InraHeader.C18_0, 3),
                },
                new IStringDetailsContent[]
                {
                    new StringDetailsContent(InraHeader.Amidon, "foo"),
                }, 
                Guid.NewGuid()),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(sut.ConvertToDto().SerializeToJson());

        //Assert
        Assert.IsNotNull(res);
        if (res.Feeds == null)
        {
            throw new ArgumentException("res.Feeds is null");
        }

        var lines = res.Feeds.ToArray();
        Assert.AreEqual(1, lines.Length);

        var cells = lines[0].NutritionDetails;
        if (cells == null)
        {
            throw new ArgumentException("cells is null");
        }

        Assert.AreEqual(3, cells.Count);
        Assert.AreEqual(1, cells[0].CellContent);
        Assert.AreEqual(2, cells[1].CellContent);
        Assert.AreEqual(3, cells[2].CellContent);

        var stringDetails = lines[0].StringDetails;
        if (stringDetails == null)
        {
            throw new ArgumentException("stringDetails is null");
        }

        Assert.AreEqual(1, stringDetails.Count);
        Assert.AreEqual("foo", stringDetails[0].CellContent);
    }

    [Test]
    public void DtoExporter_ConvertFromDto_FeedDto()
    {
        //Arrange
        var dto = new FeedDto
        {
            IsReferenceFeed = true,
            Labels = new List<string> { "label1", "label2" },
            Guid = new Guid("8DB0359F-12CE-464A-9B83-41DDBC9B4F03"),
            NutritionDetails = new List<NutritionDetailDto> { new NutritionDetailDto { CellContent = 56, HeaderKind = InraHeader.Ca.ToDtoKey() } },
            StringDetails = new List<StringDetailDto> { new StringDetailDto { CellContent = "content_foo", HeaderKind = InraHeader.Ca.ToDtoKey() } },
        };

        //Act
        var res = dto.ConvertFromDto();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("label1 | label2", res.Label);
        Assert.AreEqual(1, res.NutritionalDetails.Count);
        Assert.AreEqual(1, res.StringDetailsContent.Count);
        Assert.AreEqual(new Guid("8DB0359F-12CE-464A-9B83-41DDBC9B4F03"), res.Guid);
    }

    [Test]
    public void DtoExporter_ConvertFromDto_RecipeDto()
    {
        //Arrange
        var feed = new FeedDto
        {
            IsReferenceFeed = true,
            Labels = new List<string> { "label1", "label2" },
            NutritionDetails = new List<NutritionDetailDto> { new NutritionDetailDto { CellContent = 56, HeaderKind = InraHeader.Ca.ToDtoKey() } },
            StringDetails = new List<StringDetailDto> { new StringDetailDto { CellContent = "content_foo", HeaderKind = InraHeader.Ca.ToDtoKey() } },
        };
        var dto = new RecipeDto
        {
            Name = "recipe_name_foo",
            UnitLabel = FeedUnit.Kg.ToReferenceLabel(),
            Ingredients = new List<IngredientDto>
            {
                new IngredientDto
                {
                    Percentage = 0.8d,
                    FeedsInRecipe = feed,
                }, 
                new IngredientDto
                {
                    Percentage = 0.2d,
                    FeedsInRecipe = feed,
                },
            },
        };

        //Act
        var res = dto.ConvertFromDto();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("recipe_name_foo", res.RecipeName);
        Assert.AreEqual(2, res.Ingredients.Count);
        Assert.AreEqual(FeedUnit.Kg, res.Unit);
    }

    [Test]
    public void ExportDto_then_import_returns_expected_Recipe()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var sut = new IRecipe[]
        {
            new RecipeModel("foo1", FeedUnit.Kg, new[]
            {
                new IngredientForRecipe(78.7d, new CustomFeed(new[] { "labels" }, new INutritionalFeedDetails[] { new NutritionalFeedDetails(InraHeader.Pabs, 67d) }, guid)),
            }),
            new RecipeModel("foo2", FeedUnit.Kg, Array.Empty<IIngredientForRecipe>()),
            new RecipeModel("foo3", FeedUnit.Kg, Array.Empty<IIngredientForRecipe>()),
            new RecipeModel("foo4", FeedUnit.Kg, Array.Empty<IIngredientForRecipe>()),
            new RecipeModel("foo5", FeedUnit.Kg, Array.Empty<IIngredientForRecipe>()),
            new RecipeModel("foo6", FeedUnit.Kg, Array.Empty<IIngredientForRecipe>()),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(sut.ConvertToDto().SerializeToJson());

        //Assert
        Assert.IsNotNull(res);
        Assert.IsNotNull(res.Recipes);
        Assert.AreEqual(6, res.Recipes!.Count);
    }

    [Test]
    public void Recipe_ConvertToDto()
    {
        //Arrange
        var ingredient1 = new CustomFeed(new[] { "ingredient1" }, new INutritionalFeedDetails[] { new NutritionalFeedDetails(InraHeader.Pabs, It.IsAny<double>()) }, Guid.NewGuid());
        var ingredient2 = new CustomFeed(new[] { "ingredient2" }, new INutritionalFeedDetails[] { new NutritionalFeedDetails(InraHeader.C18_3, It.IsAny<double>()) }, Guid.NewGuid());
        var ingredient3 = new CustomFeed(new[] { "ingredient3" }, new INutritionalFeedDetails[] { new NutritionalFeedDetails(InraHeader.Cl, It.IsAny<double>()) }, Guid.NewGuid());

        var sut = new IRecipe[]
        {
            new RecipeModel("recipe_name", FeedUnit.Kg, new[]
            {
                new IngredientForRecipe(0.787d, ingredient1),
                new IngredientForRecipe(0.13d, ingredient2),
                new IngredientForRecipe(0.2d, ingredient3),
            }),
        };

        //Act
        var res = (sut.ConvertToDto().Recipes ?? throw new InvalidOperationException()).Single();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("recipe_name", res.Name);
        Assert.AreEqual(FeedUnit.Kg.ToReferenceLabel(), res.UnitLabel);
        Assert.IsNotNull(res.Ingredients);
        Assert.AreEqual(3, res.Ingredients!.Count);

        Assert.AreEqual(0.787d, res.Ingredients[0].Percentage);
        Assert.AreEqual("ingredient1", res.Ingredients[0].FeedsInRecipe!.Labels!.Single());

        Assert.AreEqual(0.13d, res.Ingredients[1].Percentage);
        Assert.AreEqual("ingredient2", res.Ingredients[1].FeedsInRecipe!.Labels!.Single());

        Assert.AreEqual(0.2d, res.Ingredients[2].Percentage);
        Assert.AreEqual("ingredient3", res.Ingredients[2].FeedsInRecipe!.Labels!.Single());
    }
}