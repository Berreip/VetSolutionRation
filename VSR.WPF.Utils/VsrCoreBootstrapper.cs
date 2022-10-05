using System;
using System.Threading.Tasks;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
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
    }

    /// <inheritdoc />
    public override async Task InitializeAsync(IInjectionContainer container)
    {
        try
        {
            // load the initial configuration
            container.Resolve<IngredientsFileLoader>().LoadInitialSavedFeeds();
        }
        catch (Exception e)
        {
            DebugCore.Fail($"Error while loading initial data: {e}");
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }
}