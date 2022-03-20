using System;
using System.Collections.Generic;
using VetSolutionRatioLib.Enums;
using VetSolutionRatioLib.Models;

namespace VetSolutionRatioLib.DataProvider;

public interface IAnimalProvider
{
    IReadOnlyCollection<IAnimal> GetAnimals();
}

internal sealed class AnimalProvider : IAnimalProvider
{
    private readonly IReadOnlyCollection<IAnimal> _animals;

    public AnimalProvider()
    {
        var animals = new List<IAnimal>();
        foreach (AnimalKind kind in Enum.GetValues(typeof(AnimalKind)))
        {
            foreach (AnimalSubKind subKind in Enum.GetValues(typeof(AnimalSubKind)))
            {
                animals.Add(new Animal(kind, subKind));
            }
        }

        _animals = animals;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IAnimal> GetAnimals()
    {
        return _animals;
    }
}