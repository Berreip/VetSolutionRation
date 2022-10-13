using System.Threading.Tasks;

namespace VSR.WPF.Utils.Loading;

/// <summary>
/// Signal a class that should be resolve and initialized after the application starts
/// </summary>
public interface IPostStartupLoading
{
    Task InitializeAsync();

}