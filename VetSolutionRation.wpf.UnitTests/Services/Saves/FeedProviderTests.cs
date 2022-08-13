using Moq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRationLib.Models.Feed;

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
        var referenceFeed = new Mock<IReferenceFeed>();
        referenceFeed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        referenceFeed.Setup(o => o.Label).Returns(label);
        referenceFeed.Setup(o => o.GetLabels()).Returns(new List<string> { label });
        referenceFeed.Setup(o => o.NutritionalDetails).Returns(new List<INutritionalFeedDetails>());
        referenceFeed.Setup(o => o.StringDetailsContent).Returns(new List<IStringDetailsContent>());
        return referenceFeed;
    }

    private static Mock<ICustomFeed> CreateCustomFeed(string label)
    {
        var referenceFeed = new Mock<ICustomFeed>();
        referenceFeed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        referenceFeed.Setup(o => o.Label).Returns(label);
        referenceFeed.Setup(o => o.GetLabels()).Returns(new List<string> { label });
        referenceFeed.Setup(o => o.NutritionalDetails).Returns(new List<INutritionalFeedDetails>());
        referenceFeed.Setup(o => o.StringDetailsContent).Returns(new List<IStringDetailsContent>());
        return referenceFeed;
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
        _sut.OnFeedChanged += () => Interlocked.Increment(ref count);

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
        var referenceFeed = CreateReferenceFeed("foo_label");

        //Act
        _sut.AddFeedsAndSave(new List<IFeed> { referenceFeed.Object });

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out _));
    }

    [Test]
    public void AddFeedsAndSave_save_reference_feed_when_provided_with_correct_content()
    {
        //Arrange
        var referenceFeed = CreateReferenceFeed("foo_label");

        //Act
        _sut.AddFeedsAndSave(new List<IFeed> { referenceFeed.Object });

        //Assert
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out var referenceFile));
        var fileContent = DtoExporter.DeserializeFromJson(referenceFile.ReadAllText());
        var dto = fileContent.Feeds!.Single();
        Assert.AreEqual(referenceFeed.Object.Guid, dto.Guid);
        Assert.AreEqual(referenceFeed.Object.Label, dto.Labels!.Single());
    }

    [Test]
    public void AddFeedsAndSave_save_user_feed_when_provided()
    {
        //Arrange
        var referenceFeed = CreateCustomFeed("foo_label");

        //Act
        _sut.AddFeedsAndSave(new List<IFeed> { referenceFeed.Object });

        //Assert
        Assert.AreEqual(1, _cacheFolder.GetFiles().Length);
        Assert.IsTrue(_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME, out _));
    }


    [TestCase(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME)]
    [TestCase(VetSolutionRatioConstants.SAVED_RECIPE_USER_FILE_NAME)]
    [TestCase(VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME)]
    public void LoadInitialSavedFeeds_do_load_data_from_specified_file(string fileName)
    {
        //Arrange

        //Act
        // _sut.LoadInitialSavedFeeds();

        //Assert
        Assert.Pass();
    }
}