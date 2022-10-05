using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VSR.Core.DataProvider;
using VSR.Core.Services;

namespace VSR.Core;

public sealed class VsrCoreBootstrapper : BootStrapperCore
{
    /// <inheritdoc />
    public override void Register(IInjectionContainerRegister container)
    {
        container.Register<IIngredientsManager, IngredientsManager>(LifeTime.Singleton);
        container.Register<IAnimalProvider, AnimalProvider>(LifeTime.Singleton);
    }
}