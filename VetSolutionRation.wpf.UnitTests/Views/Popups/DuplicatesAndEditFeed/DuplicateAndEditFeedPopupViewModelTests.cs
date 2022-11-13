using System.Windows.Data;
using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views.PopupDuplicatesAndEditFeed;
using VSR.Core.Services;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters;
using VSR.WPF.Utils.Adapters.EditionIngredients;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.DuplicatesAndEditFeed;

[TestFixture]
internal class DuplicateAndEditFeedPopupViewModelTests
{
    private DuplicateAndEditFeedPopupViewModel _sut;
    private Mock<IIngredientsManager> _ingredientsManager;
    private Mock<IPopupManagerLight> _popupManager;
    private Mock<IIngredient> _ingredient;
    private int _nbProvidedHeaders;
    private INutritionalDetails _nutritionalDetails;
    private const string FEED_NAME = "Feed name foo";

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _ingredientsManager = new Mock<IIngredientsManager>();
        _popupManager = new Mock<IPopupManagerLight>();
        _ingredient = new Mock<IIngredient>();

        // setup
        _ingredient.Setup(o => o.Label).Returns(FEED_NAME);
        _ingredient.Setup(o => o.IsUserAdded).Returns(true);

        _nutritionalDetails = Mock.Of<INutritionalDetails>();
        _ingredient.Setup(o => o.TryGetNutritionDetail(It.IsAny<InraHeader>(), out _nutritionalDetails)).Returns(true);

        var ingredient = _ingredient.Object;
        _ingredientsManager.Setup(o => o.TryGetIngredientByName(FEED_NAME, out ingredient)).Returns(true);

        _nbProvidedHeaders = FeedInEditionHelpers.GetDefaultHeaderForEdition(_ingredient.Object, () => { }).Length;
    }


    [Test]
    public void Ctor_throw_if_editing_a_reference_feed()
    {
        //Arrange
        var referenceFeed = new Mock<IIngredient>();
        referenceFeed.Setup(o => o.IsUserAdded).Returns(false);

        //Act
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<InvalidOperationException>(() => new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, referenceFeed.Object, FeedEditionMode.Edition));

        //Assert
    }

    [Test]
    public void ctor_assign_correct_value_for_duplication()
    {
        //Arrange

        //Act
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Duplication);

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
        
        var details = _sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>();
        Assert.AreEqual(_nbProvidedHeaders, details.Count);

        foreach (var feed in details)
        {
            Assert.IsTrue(feed.IsValid);
        }
    }

    [Test]
    public void ctor_assign_correct_value_for_edition()
    {
        //Arrange

        //Act
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Edition);

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

        var details = _sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>();
        Assert.AreEqual(_nbProvidedHeaders, details.Count);

        foreach (var feed in details)
        {
            Assert.IsTrue(feed.IsValid);
        }
    }

    [Test]
    public void Can_validate_if_name_is_not_a_duplicate()
    {
        //Arrange
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Duplication);
        IIngredient foo = null;
        _ingredientsManager.Setup(o => o.TryGetIngredientByName("foo", out foo)).Returns(false);

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
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Edition);

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
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Edition);

        //Act
        foreach (var feed in _sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>())
        {
            _sut.DeleteFeedCommand.Execute(feed);
        }

        //Assert
        Assert.AreEqual(0, ((ListCollectionView)_sut.IngredientDetailsInEdition).Count);
        Assert.AreEqual(false, _sut.ValidateDuplicateAndEditCommand.CanExecute());
    }

    [Test]
    public void SelectedHeader_Enable_Add_category_command()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Edition);

        //Act
        _sut.SelectedHeader = _sut.AvailableHeaders.ToArray<InraHeaderAdapter>().Last();

        //Assert
        Assert.AreEqual(true, _sut.AddCategoryCommand.CanExecute());
    }

    [Test]
    public void Adding_Header_remove_it_from_available_and_add_it_to_detail_list()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Edition);
        var selectedHeader = _sut.AvailableHeaders.ToArray<InraHeaderAdapter>().Last();
        var nutritionDetail = Mock.Of<INutritionalDetails>();
        _ingredient.Setup(o => o.TryGetNutritionDetail(selectedHeader.HeaderKind, out nutritionDetail)).Returns(true);
        var countBefore = _sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>().Count;

        //Act
        _sut.SelectedHeader = selectedHeader;
        _sut.AddCategoryCommand.Execute();

        //Assert
        Assert.AreEqual(false, _sut.AddCategoryCommand.CanExecute());
        Assert.IsNull(_sut.SelectedHeader);
        var nutritionalDetailsAdapters = _sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>();
        Assert.IsTrue(nutritionalDetailsAdapters.Any(o => o.Header == selectedHeader.HeaderKind));
        Assert.AreEqual(countBefore + 1, nutritionalDetailsAdapters.Count);
    }

    [Test]
    public void Removing_feed_add_it_to_the_header_available()
    {
        //Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        _sut = new DuplicateAndEditFeedPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _ingredient.Object, FeedEditionMode.Edition);
        var feedToDelete = _sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>()[0];

        //
        //Act
        _sut.DeleteFeedCommand.Execute(feedToDelete);

        //Assert
        Assert.IsFalse(_sut.IngredientDetailsInEdition.ToArray<NutritionalDetailsAdapter>().Contains(feedToDelete));
        Assert.IsNotNull(_sut.AvailableHeaders.ToArray<InraHeaderAdapter>().SingleOrDefault(o => o.HeaderKind == feedToDelete.Header));
    }
}