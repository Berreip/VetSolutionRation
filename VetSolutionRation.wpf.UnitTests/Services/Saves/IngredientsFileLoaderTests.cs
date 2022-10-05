using Moq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;
using VSR.Core.Services;
using VSR.Dto;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Helpers;
using VSR.WPF.Utils.Services;
using VSR.WPF.Utils.Services.Configuration;

namespace VetSolutionRation.wpf.UnitTests.Services.Saves;

[TestFixture]
internal sealed class IngredientsFileLoaderTests
{
    private IngredientsFileLoader _sut;
    private Mock<IConfigurationManager> _configurationManager;
    private Mock<IIngredientsManager> _ingredientManager;
    private DirectoryInfo _cacheFolder;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _cacheFolder = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, $"FeedProviderTests_{Guid.NewGuid()}"));
        _configurationManager = new Mock<IConfigurationManager>();
        _ingredientManager = new Mock<IIngredientsManager>();

        // defaulty setup => empty list
        _ingredientManager.Setup(o => o.GetAllIngredients()).Returns(Array.Empty<IIngredient>());
        _ingredientManager.Setup(o => o.GetAllRecipes()).Returns(Array.Empty<IRecipe>());

        _configurationManager.Setup(o => o.GetCacheDataFolder()).Returns(_cacheFolder);

        // software under test:
        _sut = new IngredientsFileLoader(_configurationManager.Object, _ingredientManager.Object);
    }

    [TearDown]
    public void TestCleanup()
    {
        _cacheFolder.DeleteIfExistAndWaitDeletion(TimeSpan.FromSeconds(5));
    }

    private static Mock<IIngredientForRecipe> CreateIngredient(double percentage, string label)
    {
        var ingredient = new Mock<IIngredientForRecipe>();
        ingredient.Setup(o => o.Percentage).Returns(percentage);
        var feed = UtilsUnitTests.CreateIngredient(label, true);
        ingredient.Setup(o => o.Ingredient).Returns(feed.Object);
        return ingredient;
    }

    [Test]
    public void Verify_no_event_subscribtion_is_made_before_loading()
    {
        //Arrange

        //Act
        _ingredientManager.VerifyAdd(m => m.OnIngredientsChanged += It.IsAny<Action<IIngredientsChangeMonitor>>(), Times.Never);

        //Assert
    }

    [Test]
    public void LoadInitialSavedFeeds_do_not_load_anything_when_directory_does_not_exists_BUT_subscribe_to_event()
    {
        //Arrange

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        _ingredientManager.VerifyAdd(m => m.OnIngredientsChanged += It.IsAny<Action<IIngredientsChangeMonitor>>(), Times.Once);
        _ingredientManager.VerifyAdd(m => m.OnRecipesChanged += It.IsAny<Action<IRecipesChangeMonitor>>(), Times.Once);
    }


    [Test]
    public void AddFeedsAndSave_save_reference_feed_when_provided_with_correct_content()
    {
        //Arrange
        var feed = UtilsUnitTests.CreateIngredient("foo_label");
        _sut.LoadInitialSavedFeeds();
        _ingredientManager.Setup(o => o.GetAllIngredients()).Returns(new[] { feed.Object });

        //Act
        _ingredientManager.Raise(m => m.OnIngredientsChanged += null, new IngredientsChangeMonitor());

        //Assert
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out var referenceFile));
        var fileContent = DtoExporter.DeserializeFromJson(referenceFile.ReadAllText());
        var dto = fileContent.Ingredients!.Single();
        Assert.AreEqual(feed.Object.Guid, dto.Guid);
        Assert.AreEqual(feed.Object.Label, dto.Label);
    }

    [Test]
    public void AddFeedsAndSave_save_user_feed_when_provided()
    {
        //Arrange
        var feed = UtilsUnitTests.CreateIngredient("foo_label", true);
        _sut.LoadInitialSavedFeeds();
        _ingredientManager.Setup(o => o.GetAllIngredients()).Returns(new[] { feed.Object });

        //Act
        _ingredientManager.Raise(m => m.OnIngredientsChanged += null, new IngredientsChangeMonitor());

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out _));
    }

    [Test]
    public void AddRecipes_save_recipe_feed_when_provided()
    {
        //Arrange
        var recipe = UtilsUnitTests.CreateRecipe("foo_label_recipe", CreateIngredient(1d, "foo_1").Object);
        _sut.LoadInitialSavedFeeds();
        _ingredientManager.Setup(o => o.GetAllRecipes()).Returns(new[] { recipe.Object });

        //Act
        _ingredientManager.Raise(m => m.OnRecipesChanged += null, new RecipesChangeMonitor());

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out _));
    }

    [Test]
    public void AddRecipes_save_recipe_feed_when_provided_and_check_content()
    {
        //Arrange
        var recipe = UtilsUnitTests.CreateRecipe("foo_label_recipe", CreateIngredient(1d, "foo_1").Object);
        _sut.LoadInitialSavedFeeds();
        _ingredientManager.Setup(o => o.GetAllRecipes()).Returns(new[] { recipe.Object });

        //Act
        _ingredientManager.Raise(m => m.OnRecipesChanged += null, new RecipesChangeMonitor());

        //Assert
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out var recipeFile));
        var content = DtoExporter.DeserializeFromJson(recipeFile.ReadAllText()).Recipes!.Single();
        var singleIngredient = content.Ingredients!.Single();
        Assert.AreEqual(1d, singleIngredient.Percentage);
        Assert.AreEqual(recipe.Object.RecipeName, content.Name);
    }
}