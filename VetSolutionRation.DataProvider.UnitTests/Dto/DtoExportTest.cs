using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.JSON;
using VSR.Dto;
using VSR.Dto.Ingredients;
using VSR.Dto.Recipe;
using VSR.Dto.Utils;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VetSolutionRation.DataProvider.UnitTests.Dto;

[TestFixture]
internal sealed class DtoExportTest
{
    [Test]
    public void ExportDto_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new IIngredient[]
        {
            new Ingredient(Guid.NewGuid(), "foo1", true, Array.Empty<INutritionalDetails>()),
            new Ingredient(Guid.NewGuid(), "foo2", true, Array.Empty<INutritionalDetails>()),
            new Ingredient(Guid.NewGuid(), "foo3", true, Array.Empty<INutritionalDetails>()),
            new Ingredient(Guid.NewGuid(), "foo4", true, Array.Empty<INutritionalDetails>()),
            new Ingredient(Guid.NewGuid(), "foo5", true, Array.Empty<INutritionalDetails>()),
            new Ingredient(Guid.NewGuid(), "foo6", true, Array.Empty<INutritionalDetails>()),
        };

        //Act
        var data = DtoExporter.ConvertFromModelsToDto(sut, new List<IRecipe>());
        var res = DtoExporter.DeserializeFromJson(data.SerializeToJson());

        //Assert
        Assert.IsNotNull(res.Ingredients);
        Assert.AreEqual(6, res.Ingredients!.Count);
        Assert.IsTrue(res.Ingredients.All(o => o.Guid != Guid.Empty));
    }

    [Test]
    public void ExportDto_then_serialize_then_deserialize_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new IIngredient[]
        {
            new Ingredient(Guid.NewGuid(), "foo1",
                true,
                new INutritionalDetails[]
                {
                    new NutritionalDetails(InraHeader.Amidon, 1),
                    new NutritionalDetails(InraHeader.C12_0, 2),
                    new NutritionalDetails(InraHeader.C18_0, 3),
                }),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(DtoExporter.ConvertFromModelsToDto(sut, Array.Empty<IRecipe>()).SerializeToJson());

        //Assert
        Assert.IsNotNull(res);
        Assert.IsNotNull(res.Ingredients);
        Assert.AreEqual(1, res.Ingredients!.Count);

        var nutritionDetails = res.Ingredients[0].NutritionDetails;
        Assert.IsNotNull(nutritionDetails);
        Assert.AreEqual(3, nutritionDetails!.Count);
        Assert.AreEqual(1, nutritionDetails[0].Value);
        Assert.AreEqual(2, nutritionDetails[1].Value);
        Assert.AreEqual(3, nutritionDetails[2].Value);
    }

    [Test]
    public void DtoExporter_ConvertFromDto_FeedDto()
    {
        //Arrange
        var dto = new IngredientDto
        {
            IsUserAdded = false,
            Label = "label1|label2",
            Guid = new Guid("8DB0359F-12CE-464A-9B83-41DDBC9B4F03"),
            NutritionDetails = new List<NutritionalDetailDto> { new NutritionalDetailDto { Value = 56, HeaderKind = InraHeader.Ca.ToDtoKey() } },
        };

        //Act
        var res = dto.ConvertFromDtoToModel();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("label1|label2", res.Label);
        Assert.AreEqual(1, res.GetNutritionDetails().Count);
        Assert.AreEqual(new Guid("8DB0359F-12CE-464A-9B83-41DDBC9B4F03"), res.Guid);
    }

    [Test]
    public void DtoExporter_ConvertFromDto_RecipeDto()
    {
        //Arrange
        var dto = new RecipeDto
        {
            Name = "recipe_name_foo",
            Guid = Guid.NewGuid(),
            Ingredients = new List<IngredientForRecipeDto>
            {
                new IngredientForRecipeDto
                {
                    Percentage = 0.8d,
                    IngredientGuid = Guid.NewGuid(),
                },
                new IngredientForRecipeDto
                {
                    Percentage = 0.2d,
                    IngredientGuid = Guid.NewGuid(),
                },
            },
        };

        //Act
        var res = dto.ConvertFromDtoToModel();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("recipe_name_foo", res.Name);
        Assert.AreEqual(2, res.Ingredients.Count);
    }

    [Test]
    public void ExportDto_then_import_returns_expected_Recipe()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var sut = new IRecipe[]
        {
            new Recipe(Guid.NewGuid(), "foo1", new[]
            {
                new IngredientForRecipe(78.7d, new Ingredient(guid, "labels", true, new INutritionalDetails[] { new NutritionalDetails(InraHeader.Pabs, 67d) })),
            }),
            new Recipe(Guid.NewGuid(), "foo2", Array.Empty<IIngredientForRecipe>()),
            new Recipe(Guid.NewGuid(), "foo3", Array.Empty<IIngredientForRecipe>()),
            new Recipe(Guid.NewGuid(), "foo4", Array.Empty<IIngredientForRecipe>()),
            new Recipe(Guid.NewGuid(), "foo5", Array.Empty<IIngredientForRecipe>()),
            new Recipe(Guid.NewGuid(), "foo6", Array.Empty<IIngredientForRecipe>()),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(DtoExporter.ConvertFromModelsToDto(Array.Empty<IIngredient>(), sut).SerializeToJson());

        //Assert
        Assert.IsNotNull(res);
        Assert.IsNotNull(res.Recipes);
        Assert.AreEqual(6, res.Recipes!.Count);
    }

    [Test]
    public void Recipe_ConvertToDto()
    {
        //Arrange
        var ingredient1 = new Ingredient(Guid.NewGuid(), "ingredient1", true, new INutritionalDetails[] { new NutritionalDetails(InraHeader.Pabs, It.IsAny<double>()) });
        var ingredient2 = new Ingredient(Guid.NewGuid(), "ingredient2", true, new INutritionalDetails[] { new NutritionalDetails(InraHeader.C18_3, It.IsAny<double>()) });
        var ingredient3 = new Ingredient(Guid.NewGuid(), "ingredient3", true, new INutritionalDetails[] { new NutritionalDetails(InraHeader.Cl, It.IsAny<double>()) });

        var sut = new IRecipe[]
        {
            new Recipe(Guid.NewGuid(), "recipe_name", new[]
            {
                new IngredientForRecipe(0.787d, ingredient1),
                new IngredientForRecipe(0.13d, ingredient2),
                new IngredientForRecipe(0.2d, ingredient3),
            }),
        };

        //Act
        var res = (DtoExporter.ConvertFromModelsToDto(Array.Empty<IIngredient>(), sut).Recipes ?? throw new InvalidOperationException()).Single();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("recipe_name", res.Name);
        Assert.IsNotNull(res.Ingredients);
        Assert.AreEqual(3, res.Ingredients!.Count);

        Assert.AreEqual(0.787d, res.Ingredients[0].Percentage);
        Assert.AreEqual(ingredient1.Guid, res.Ingredients[0].IngredientGuid);

        Assert.AreEqual(0.13d, res.Ingredients[1].Percentage);
        Assert.AreEqual(ingredient2.Guid, res.Ingredients[1].IngredientGuid);

        Assert.AreEqual(0.2d, res.Ingredients[2].Percentage);
        Assert.AreEqual(ingredient3.Guid, res.Ingredients[2].IngredientGuid);
    }
}