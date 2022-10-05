using Moq;
using NUnit.Framework;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters.EditionRecipes;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration.Adapters;

[TestFixture]
internal sealed class FeedForRecipeCreationAdapterTests
{
    private IngredientInRecipeCreationAdapter _sut;
    private Mock<IIngredient> _ingredient;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _ingredient = new Mock<IIngredient>();
        _ingredient.Setup(o => o.Label).Returns("foo_name");
        
        // software under test:
        _sut = new IngredientInRecipeCreationAdapter(_ingredient.Object, 60);
    }

    [Test]
    public void Ctor_assign_expected_name()
    {
        //Arrange

        //Act
        var res = _sut.Name;

        //Assert
        Assert.AreEqual(_ingredient.Object.Label, res);
    }

    // /// <summary>
    // /// As FeedQuantity is mutable, it should not be used as a reference. Otherwise, the same reference could be shared and updated by multiples elements
    // /// </summary>
    // [Test]
    // public void Ctor_create_another_reference_of_feed_quantity_instread_of_using_the_same_reference()
    // {
    //     //Arrange
    //
    //     //Act
    //     var res = _sut.Quantity;
    //
    //     //Assert
    //     Assert.AreNotSame(_ingredient.Object.FeedQuantity, res);
    // }
    //
    // [Test]
    // public void Ctor_assign_expected_Feed_Qty()
    // {
    //     //Arrange
    //
    //     //Act
    //     var res = _sut.FeedQuantity;
    // TODO PBO NIAK
    //     //Assert
    //     Assert.AreEqual(_ingredient.Object.FeedQuantity.Quantity, res.Quantity);
    //     Assert.AreEqual(_ingredient.Object.FeedQuantity.Unit, res.Unit);
    // }

}