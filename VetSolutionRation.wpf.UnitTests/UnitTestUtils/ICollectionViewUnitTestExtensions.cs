using System.ComponentModel;

namespace VetSolutionRation.wpf.UnitTests.UnitTestUtils;

internal static class CollectionViewUnitTestExtensions
{
    public static IReadOnlyList<T> ToArray<T>(this ICollectionView collectionView)
    {
        var elements = new List<T>();

        foreach (var element in collectionView)
        {
            if (element is T casted)
            {
                elements.Add(casted);
            }
            else
            {
                throw new ArgumentException($"CollectionView is projected as an array of <{typeof(T).Name}> but one element is of type <{element.GetType().Name}> at least");
            }
        }
        return elements;
    }
    public static int Count(this ICollectionView collectionView)
    {
        return collectionView.OfType<object>().Count();
    }
}