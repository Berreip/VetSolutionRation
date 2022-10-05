using System.Collections.Generic;
using VSR.Enums;
using VSR.Models;

namespace VSR.Core.DataProvider;

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
       

        _animals = new []
        {
            new Animal(AnimalKind.Bovine, "Boeuf de plus de 2 ans race à viande"),
            new Animal(AnimalKind.Bovine, "Taurillon 1-2 ans"),
            new Animal(AnimalKind.Bovine, "Taureaux"),
            
            new Animal(AnimalKind.Bovine, "Génisses de souche de plus de 2 ans - laitière"),
            new Animal(AnimalKind.Bovine, "Génisses de souche de 1 à 2 ans - laitière"),
            new Animal(AnimalKind.Bovine, "Génisses de souche de moins d'1 an - laitière"),
            new Animal(AnimalKind.Bovine, "Génisses race à viande engraissement de moins d'un an"),
            new Animal(AnimalKind.Bovine, "Génisses de souche de plus de 2 ans - allaitante"),
            new Animal(AnimalKind.Bovine, "Génisses de souche de 1 à 2 ans - allaitante"),
            new Animal(AnimalKind.Bovine, "Génisses de souche de moins de 1 an - allaitante"),
            
            new Animal(AnimalKind.Bovine, "Vaches laitières inf à 5000 L/an"),
            new Animal(AnimalKind.Bovine, "Vaches laitières de 6000 à 7000 L/an"),
            new Animal(AnimalKind.Bovine, "Vaches laitières de 5000 à 6000 L/an"),
            new Animal(AnimalKind.Bovine, "Vaches laitières de 7000 à 8000 L/an"),
            new Animal(AnimalKind.Bovine, "Vaches laitières > 8000 L/an"),
            new Animal(AnimalKind.Bovine, "Vaches nourrices (allaitantes)"),
            
            new Animal(AnimalKind.Sheep, "Brebis viandes"),
            new Animal(AnimalKind.Sheep, "Agnelles"),
            new Animal(AnimalKind.Sheep, "Agnelets ou agneaux de lait"),
            new Animal(AnimalKind.Sheep, "Agneaux engraissement"),
            
            new Animal(AnimalKind.Goat, "Chèvres laitières lait non transformé"),
            new Animal(AnimalKind.Goat, "Chèvres laitières lait transformé"),
            new Animal(AnimalKind.Goat, "Chevrettes"),
            new Animal(AnimalKind.Goat, "Chevreaux"),
            
            new Animal(AnimalKind.Horse, "Poulains"),
            new Animal(AnimalKind.Horse, "Juments poulinières de race légère"),
            new Animal(AnimalKind.Horse, "Juments de Race Lourde"),
            new Animal(AnimalKind.Horse, "Poney"),
            
            new Animal(AnimalKind.Pig, "Truies naisseurs 7 kg"),
            new Animal(AnimalKind.Pig, "Truies naisseurs 25 kg"),
            new Animal(AnimalKind.Pig, "Truies naisseurs engraisseurs"),
            new Animal(AnimalKind.Pig, "Porcelets post-sevrage 8-25 kg"),
            new Animal(AnimalKind.Pig, "Porc charcutier avec post-sevrage"),
            new Animal(AnimalKind.Pig, "Porc charcutier sans post-sevrage"),
            
            new Animal(AnimalKind.Rabbit, "Lapins naisseur engraisseur"),
            
            new Animal(AnimalKind.Poultry, "Poulets standards"),
            new Animal(AnimalKind.Poultry, "Poulets labellisés"),
            new Animal(AnimalKind.Poultry, "Dindes industrielles"),
            new Animal(AnimalKind.Poultry, "Pintades labellisées"),
            
        };
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IAnimal> GetAnimals()
    {
        return _animals;
    }
}