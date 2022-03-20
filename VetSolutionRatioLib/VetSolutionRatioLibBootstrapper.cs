using PRF.Utils.Injection.BootStrappers;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using VetSolutionRatioLib.DataProvider;

namespace VetSolutionRatioLib;

public sealed class VetSolutionRatioLibBootstrapper : BootStrapperCore
{
    /// <inheritdoc />
    public override void Register(IInjectionContainerRegister container)
    {
        container.Register<IAnimalProvider, AnimalProvider>(LifeTime.Singleton);
    }
}