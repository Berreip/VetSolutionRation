using Moq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.UnitTests.Services.Saves;

[TestFixture]
internal sealed class FeedProviderTests
{
    private FeedProvider _sut;
    private Mock<IConfigurationManager> _configurationManager;
    private DirectoryInfo _cacheFolder;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _cacheFolder = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, $"FeedProviderTests_{Guid.NewGuid()}"));
        _configurationManager = new Mock<IConfigurationManager>();
        _configurationManager.Setup(o => o.GetCacheDataFolder()).Returns(_cacheFolder);

        // software under test:
        _sut = new FeedProvider(_configurationManager.Object);
    }

    [TearDown]
    public void TestCleanup()
    {
        _cacheFolder.DeleteIfExistAndWaitDeletion(TimeSpan.FromSeconds(5));
    }

    private static Mock<IReferenceFeed> CreateReferenceFeed(string label)
    {
        var feed = new Mock<IReferenceFeed>();
        feed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        feed.Setup(o => o.Label).Returns(label);
        feed.Setup(o => o.UniqueReferenceKey).Returns(label);
        feed.Setup(o => o.GetLabels()).Returns(new List<string> { label });
        feed.Setup(o => o.NutritionalDetails).Returns(new List<INutritionalFeedDetails>());
        feed.Setup(o => o.StringDetailsContent).Returns(new List<IStringDetailsContent>());
        return feed;
    }

    private static Mock<IIngredientForRecipe> CreateIngredient(double percentage, string label)
    {
        var ingredient = new Mock<IIngredientForRecipe>();
        ingredient.Setup(o => o.Percentage).Returns(percentage);
        var feed = CreateCustomFeed(label);
        ingredient.Setup(o => o.Ingredient).Returns(feed.Object);
        return ingredient;
    }

    private static Mock<ICustomFeed> CreateCustomFeed(string label)
    {
        var feed = new Mock<ICustomFeed>();
        feed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        feed.Setup(o => o.Label).Returns(label);
        feed.Setup(o => o.UniqueReferenceKey).Returns(label);
        feed.Setup(o => o.GetLabels()).Returns(new List<string> { label });
        feed.Setup(o => o.NutritionalDetails).Returns(new List<INutritionalFeedDetails>());
        feed.Setup(o => o.StringDetailsContent).Returns(new List<IStringDetailsContent>());
        return feed;
    }

    private static Mock<IRecipe> CreateRecipeFeed(string recipeName, params IIngredientForRecipe[] ingredients)
    {
        var recipe = new Mock<IRecipe>();
        recipe.Setup(o => o.RecipeName).Returns(recipeName);
        recipe.Setup(o => o.UniqueReferenceKey).Returns(recipeName);
        recipe.Setup(o => o.Unit).Returns(FeedUnit.Kg);
        recipe.Setup(o => o.Ingredients).Returns(ingredients.ToList());
        return recipe;
    }


    [Test]
    public void LoadInitialSavedFeeds_do_not_load_anything_when_directory_does_not_exists()
    {
        //Arrange

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        Assert.Pass();
    }

    [Test]
    public void AddFeedsAndSave_do_not_raise_OnFeedChanged_event_when_empty()
    {
        //Arrange
        var count = 0;
        _sut.OnFeedOrRecipeChanged += () => Interlocked.Increment(ref count);

        //Act
        _sut.AddFeedsAndSave(Array.Empty<IFeed>());

        //Assert
        Assert.AreEqual(0, count);
    }

    [Test]
    public void AddFeedsAndSave_do_not_create_file_when_empty()
    {
        //Arrange

        //Act
        _sut.AddFeedsAndSave(Array.Empty<IFeed>());

        //Assert
        Assert.IsFalse(_cacheFolder.ExistsExplicit());
    }

    [Test]
    public void AddFeedsAndSave_save_reference_feed_when_provided()
    {
        //Arrange
        var feed = CreateReferenceFeed("foo_label");

        //Act
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out _));
    }

    [Test]
    public void AddFeedsAndSave_save_reference_feed_when_provided_with_correct_content()
    {
        //Arrange
        var feed = CreateReferenceFeed("foo_label");

        //Act
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });

        //Assert
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out var referenceFile));
        var fileContent = DtoExporter.DeserializeFromJson(referenceFile.ReadAllText());
        var dto = fileContent.Feeds!.Single();
        Assert.AreEqual(feed.Object.Guid, dto.Guid);
        Assert.AreEqual(feed.Object.Label, dto.Labels!.Single());
    }

    [Test]
    public void AddFeedsAndSave_save_user_feed_when_provided()
    {
        //Arrange
        var feed = CreateCustomFeed("foo_label");

        //Act
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME, out _));
    }

    [Test]
    public void AddRecipeAndSave_save_recipe_feed_when_provided()
    {
        //Arrange
        var ingredient = CreateIngredient(1d, "foo_1");
        var feed = CreateRecipeFeed("foo_label_recipe", ingredient.Object);

        //Act
        _sut.AddRecipeAndSave(feed.Object);

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_RECIPE_USER_FILE_NAME, out _));
    }

    [Test]
    public void AddRecipeAndSave_raise_event_when_data_provided()
    {
        //Arrange
        var count = 0;
        _sut.OnFeedOrRecipeChanged += () => Interlocked.Increment(ref count);
        var ingredient = CreateIngredient(1d, "foo_1");
        var feed = CreateRecipeFeed("foo_label_recipe", ingredient.Object);

        //Act
        _sut.AddRecipeAndSave(feed.Object);

        //Assert
        Assert.AreEqual(1, count);
    }

    [Test]
    public void AddRecipeAndSave_save_recipe_feed_when_provided_and_check_content()
    {
        //Arrange
        var ingredient = CreateIngredient(1d, "foo_1");
        var recipe = CreateRecipeFeed("foo_label_recipe", ingredient.Object);

        //Act
        _sut.AddRecipeAndSave(recipe.Object);

        //Assert
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_RECIPE_USER_FILE_NAME, out var recipeFile));
        var content = DtoExporter.DeserializeFromJson(recipeFile.ReadAllText()).Recipes!.Single();
        var singleIngredient = content.Ingredients!.Single();
        Assert.AreEqual(1d, singleIngredient.Percentage);
        Assert.AreEqual("foo_1", singleIngredient.FeedsInRecipe!.Labels!.Single());
        Assert.AreEqual(recipe.Object.RecipeName, content.Name);
        Assert.AreEqual(recipe.Object.Unit.ToReferenceLabel(), content.UnitLabel);
    }

    [Test]
    public void LoadInitialSavedFeeds_raise_OnFeedChanged_event_for_custom_feed()
    {
        //Arrange
        var count = 0;
        var feed = CreateCustomFeed("foo_label");
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });
        _sut.OnFeedOrRecipeChanged += () => Interlocked.Increment(ref count);

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        Assert.AreEqual(1, count);
    }

    [Test]
    public void LoadInitialSavedFeeds_register_custom_feed()
    {
        //Arrange
        var feed = CreateCustomFeed("foo_label");
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        var res = _sut.GetFeedsOrRecipes().Cast<ICustomFeed>().ToArray();
        Assert.AreEqual(1, res.Length);
        Assert.AreEqual(feed.Object.Label, res[0].Label);
    }

    [Test]
    public void LoadInitialSavedFeeds_raise_OnFeedChanged_event_for_reference_feed()
    {
        //Arrange
        var count = 0;
        var feed = CreateReferenceFeed("foo_label");
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });
        _sut.OnFeedOrRecipeChanged += () => Interlocked.Increment(ref count);

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        Assert.AreEqual(1, count);
    }

    [Test]
    public void LoadInitialSavedFeeds_register_reference_feed()
    {
        //Arrange
        var feed = CreateReferenceFeed("foo_label");
        _sut.AddFeedsAndSave(new List<IFeed> { feed.Object });

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        var res = _sut.GetFeedsOrRecipes().Cast<IReferenceFeed>().ToArray();
        Assert.AreEqual(1, res.Length);
        Assert.AreEqual(feed.Object.Label, res[0].Label);
    }


    [Test]
    public void LoadInitialSavedFeeds_register_recipe()
    {
        //Arrange
        var ingredient = CreateIngredient(1d, "foo_1");
        var recipe = CreateRecipeFeed("foo_label_recipe", ingredient.Object);
        _sut.AddRecipeAndSave(recipe.Object);

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        var res = _sut.GetFeedsOrRecipes().Cast<IRecipe>().ToArray();
        Assert.AreEqual(1, res.Length);
        Assert.AreEqual("foo_label_recipe", res[0].RecipeName);
    }

    [Test]
    public void LoadInitialSavedFeeds_raise_OnRecipeChanged_event()
    {
        //Arrange
        var count = 0;
        var ingredient = CreateIngredient(1d, "foo_1");
        var recipe = CreateRecipeFeed("foo_label_recipe", ingredient.Object);
        _sut.AddRecipeAndSave(recipe.Object);

        _sut.OnFeedOrRecipeChanged += () => Interlocked.Increment(ref count);

        //Act
        _sut.LoadInitialSavedFeeds();

        //Assert
        Assert.AreEqual(1, count);
    }
}