using System.Windows.Data;
using Moq;
using NUnit.Framework;
using PRF.WPFCore.PopupManager;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Popups.Adapters;
using VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.DuplicatesAndEditFeed;

[TestFixture]
internal class DuplicateAndEditFeedPopupViewModelTests
{
    private DuplicateAndEditFeedPopupViewModel _sut;
    private Mock<IFeedProvider> _feedProvider;
    private Mock<IPopupManagerLight> _popupManager;
    private Mock<IFeedAdapter> _feed;
    private int _nbProvidedHeaders;
    private const string FEED_NAME = "Feed name foo";
    private const int CELL_VALUE = 1;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _feedProvider = new Mock<IFeedProvider>();
        _popupManager = new Mock<IPopupManagerLight>();
        _feed = new Mock<IFeedAdapter>();
        
        // setup
        _feed.Setup(o => o.FeedName).Returns(FEED_NAME);
        _feed.Setup(o => o.GetInraValue(It.IsAny<InraHeader>())).Returns(CELL_VALUE);
        _feedProvider.Setup(o => o.ContainsFeedName(FEED_NAME)).Returns(true);
        _nbProvidedHeaders = FeedInEditionHelpers.GetDefaultHeaderForEdition(_feed.Object, () => {}).Length;
    }


    [Test]
    public void Ctor_throw_if_editing_a_reference_feed()
    {
        //Arrange
        var referenceFeed = new Mock<IReferenceFeedAdapter>();

        //Act
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<InvalidOperationException>(() => new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, referenceFeed.Object, FeedEditionMode.Edition));

        //Assert
    }

    [Test]
    public void ctor_assign_correct_value_for_duplication()
    {
        //Arrange

        //Act
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Duplication);

        //Assert
        Assert.AreEqual(FEED_NAME, _sut.FeedEditedName);
        Assert.AreEqual(true, _sut.IsDuplicatedLabel);
        Assert.AreEqual(false, _sut.ValidateDuplicateAndEditCommand.CanExecute());
        Assert.AreEqual(false, _sut.AddCategoryCommand.CanExecute());
        Assert.AreEqual(null, _sut.SearchFilter);
        Assert.AreEqual(true, _sut.CouldEditName);
        Assert.AreEqual(null, _sut.SelectedHeader);
        var expected = Enum.GetValues(typeof(InraHeader)).Length - _nbProvidedHeaders;
        Assert.AreEqual(expected, _sut.AvailableHeaders.Count());
        Assert.AreEqual(_nbProvidedHeaders, _sut.FeedDetailsInEdition.Count());
        foreach (FeedDetailInEditionAdapter feed in _sut.FeedDetailsInEdition)
        {
            Assert.IsTrue(feed.IsValid);
            Assert.AreEqual(CELL_VALUE.ToString(), feed.CellValue);
        }
    }

    [Test]
    public void ctor_assign_correct_value_for_edition()
    {
        //Arrange

        //Act
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Edition);

        //Assert
        Assert.AreEqual(FEED_NAME, _sut.FeedEditedName);
        Assert.AreEqual(false, _sut.IsDuplicatedLabel);
        Assert.AreEqual(true, _sut.ValidateDuplicateAndEditCommand.CanExecute());
        Assert.AreEqual(false, _sut.AddCategoryCommand.CanExecute());
        Assert.AreEqual(null, _sut.SearchFilter);
        Assert.AreEqual(false, _sut.CouldEditName);
        Assert.AreEqual(null, _sut.SelectedHeader);
        var expected = Enum.GetValues(typeof(InraHeader)).Length - _nbProvidedHeaders;
        Assert.AreEqual(expected, _sut.AvailableHeaders.Count());
        Assert.AreEqual(_nbProvidedHeaders, _sut.FeedDetailsInEdition.Count());
        foreach (FeedDetailInEditionAdapter feed in _sut.FeedDetailsInEdition)
        {
            Assert.IsTrue(feed.IsValid);
            Assert.AreEqual(CELL_VALUE.ToString(), feed.CellValue);
        }
    }

    [Test]
    public void Can_validate_if_name_is_not_a_duplicate()
    {
        //Arrange
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Duplication);
        _feedProvider.Setup(o => o.ContainsFeedName("foo")).Returns(false);
        
        //Act
        _sut.FeedEditedName = "foo";

        //Assert
        Assert.AreEqual("foo", _sut.FeedEditedName);
        Assert.AreEqual(false, _sut.IsDuplicatedLabel);
        Assert.AreEqual(true, _sut.ValidateDuplicateAndEditCommand.CanExecute());
    }
    
    [Test]
    public void Cannot_change_name_for_edition_mode()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Edition);
        
        //Act
        _sut.FeedEditedName = "foo";

        //Assert
        Assert.AreEqual(FEED_NAME, _sut.FeedEditedName);
    }

    [Test]
    public void Deleting_All_feed_disable_validation()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Edition);
      
        
        //Act
        foreach (var feed in _sut.FeedDetailsInEdition.ToArray<FeedDetailInEditionAdapter>())
        {
            _sut.DeleteFeedCommand.Execute(feed);
        }

        //Assert
        Assert.AreEqual(0, ((ListCollectionView)_sut.FeedDetailsInEdition).Count);
        Assert.AreEqual(false, _sut.ValidateDuplicateAndEditCommand.CanExecute());
    }
    
    [Test]
    public void SelectedHeader_Enable_Add_category_command()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Edition);
      
        
        //Act
        _sut.SelectedHeader = _sut.AvailableHeaders.ToArray<HeaderAdapter>().Last();

        //Assert
        Assert.AreEqual(true, _sut.AddCategoryCommand.CanExecute());
    }
    
    [Test]
    public void Adding_Header_remove_it_from_available_and_add_it_to_detail_list()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Edition);
        var selectedHeader = _sut.AvailableHeaders.ToArray<HeaderAdapter>().Last();
        _feed.Setup(o => o.GetInraValue(selectedHeader.HeaderKind)).Returns(78d);
        
        //Act
        _sut.SelectedHeader = selectedHeader;
        _sut.AddCategoryCommand.Execute();

        //Assert
        Assert.AreEqual(false, _sut.AddCategoryCommand.CanExecute());
        Assert.IsNull(_sut.SelectedHeader);
        var feedDetail = _sut.FeedDetailsInEdition.ToArray<FeedDetailInEditionAdapter>().Single(o => o.Header == selectedHeader.HeaderKind);
        Assert.AreEqual(78d, feedDetail.DoubleCellValue);
    }

    [Test]
    public void Removing_feed_add_it_to_the_header_available()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _feedProvider.Object, _feed.Object, FeedEditionMode.Edition);
        var feedToDelete = _sut.FeedDetailsInEdition.ToArray<FeedDetailInEditionAdapter>().First();
        
        //
        //Act
        _sut.DeleteFeedCommand.Execute(feedToDelete);

        //Assert
        Assert.IsFalse(_sut.FeedDetailsInEdition.ToArray<FeedDetailInEditionAdapter>().Contains(feedToDelete));
        Assert.IsNotNull(_sut.AvailableHeaders.ToArray<HeaderAdapter>().SingleOrDefault(o => o.HeaderKind == feedToDelete.Header));
    }

}