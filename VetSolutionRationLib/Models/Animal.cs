using VetSolutionRationLib.Enums;

namespace VetSolutionRationLib.Models;

public interface IAnimal
{
    AnimalKind Kind { get; }
    string Description { get; }
}

internal sealed class Animal : IAnimal
{
    public string Description { get; }
    public AnimalKind Kind { get; }

    public Animal(AnimalKind kind, string description)
    {
        Description = description;
        Kind = kind;
    }
}