using System.Threading.Tasks;
using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VSR.WPF.Utils.Loading;
using VSR.WPF.Utils.Services;
using VSR.WPF.Utils.Services.Configuration;

namespace VSR.WPF.Utils;

public sealed class VsrWpfUtilsBootstrapper : BootStrapperCore
{
    /// <inheritdoc />
    public override void Register(IInjectionContainerRegister container)
    {
        container.RegisterType<IngredientsFileLoader>(LifeTime.Singleton);
        container.Register<IConfigurationManager, ConfigurationManager>(LifeTime.Singleton);
        
        container.Register<IAnimalAdaptersHoster, AnimalAdaptersHoster>(LifeTime.Singleton);
        container.Register<IIngredientAdaptersHoster, IngredientAdaptersHoster>(LifeTime.Singleton);
        container.RegisterOrAppendCollection<IPostStartupLoading>(LifeTime.Transient, new []{typeof(IngredientFileLoaderOnStartup)});
    }
}

internal sealed class IngredientFileLoaderOnStartup : IPostStartupLoading
{
    private readonly IngredientsFileLoader _ingredientsFileLoader;

    public IngredientFileLoaderOnStartup(IngredientsFileLoader ingredientsFileLoader)
    {
        _ingredientsFileLoader = ingredientsFileLoader;
    }
    
    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        // load the initial configuration
        _ingredientsFileLoader.LoadInitialSavedFeeds();
        await Task.CompletedTask.ConfigureAwait(false);
    }
}