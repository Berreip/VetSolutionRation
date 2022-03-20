using VetSolutionRatioLib.Enums;

namespace VetSolutionRatioLib.Models;

public interface IAnimal
{
    AnimalKind Kind { get; }
    AnimalSubKind SubKind { get; }
}

internal sealed class Animal : IAnimal
{
    public AnimalKind Kind { get; }
    public AnimalSubKind SubKind { get; }

    public Animal(AnimalKind kind, AnimalSubKind subKind = AnimalSubKind.Undefined)
    {
        Kind = kind;
        SubKind = subKind;
    }
}