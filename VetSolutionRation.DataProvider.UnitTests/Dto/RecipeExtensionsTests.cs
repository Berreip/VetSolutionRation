using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using VSR.Dto.Recipe;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VetSolutionRation.DataProvider.UnitTests.Dto;

[TestFixture]
internal sealed class RecipeExtensionsTests
{
    private Mock<IRecipe> _recipe = null!;
    private Mock<IIngredientForRecipe> _ingredient1 = null!;
    private Mock<IIngredientForRecipe> _ingredient2 = null!;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _ingredient1 = new Mock<IIngredientForRecipe>();
        _ingredient2 = new Mock<IIngredientForRecipe>();

        _ingredient1.Setup(o => o.Ingredient.Guid).Returns(Guid.NewGuid());
        _ingredient1.Setup(o => o.Percentage).Returns(0.8);
        _ingredient2.Setup(o => o.Ingredient.Guid).Returns(Guid.NewGuid());
        _ingredient2.Setup(o => o.Percentage).Returns(0.8);
        
        _recipe = new Mock<IRecipe>();
        _recipe.Setup(o => o.RecipeName).Returns("name");
        _recipe.Setup(o => o.IngredientsForRecipe).Returns(new List<IIngredientForRecipe> { _ingredient1.Object, _ingredient2.Object });
        _recipe.Setup(o => o.Unit).Returns(FeedUnit.Kg);
        _recipe.Setup(o => o.Guid).Returns(Guid.NewGuid());
    }

    [Test]
    public void ConvertFromModelToDto_extract_name()
    {
        //Arrange
        
        //Act
        var res = _recipe.Object.ConvertFromModelToDto();

        //Assert
        Assert.AreEqual("name", res.Name);
    }

    [Test]
    public void ConvertFromModelToDto_extract_guid()
    {
        //Arrange
        
        //Act
        var res = _recipe.Object.ConvertFromModelToDto();

        //Assert
        Assert.AreEqual(_recipe.Object.Guid, res.Guid);
    }

    [Test]
    public void ConvertFromModelToDto_extract_ingredient()
    {
        //Arrange
        
        //Act
        var res = _recipe.Object.ConvertFromModelToDto();

        //Assert
        Assert.IsNotNull(res.Ingredients);
        Assert.AreEqual(2, res.Ingredients!.Count);
        
        Assert.AreEqual(_ingredient1.Object.Percentage, res.Ingredients[0].Percentage);
        Assert.AreEqual(_ingredient1.Object.Ingredient.Guid, res.Ingredients[0].IngredientGuid);
        
        Assert.AreEqual(_ingredient2.Object.Percentage, res.Ingredients[1].Percentage);
        Assert.AreEqual(_ingredient2.Object.Ingredient.Guid, res.Ingredients[1].IngredientGuid);
    }
}