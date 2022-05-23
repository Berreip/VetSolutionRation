using System.ComponentModel;

namespace VetSolutionRation.wpf.UnitTests.UnitTestUtils;

internal static class CollectionViewUnitTestExtensions
{
    public static IReadOnlyList<T> ToArray<T>(this ICollectionView collectionView)
    {
        return collectionView.OfType<T>().ToArray();
    }
    public static int Count(this ICollectionView collectionView)
    {
        return collectionView.OfType<object>().Count();
    }
}