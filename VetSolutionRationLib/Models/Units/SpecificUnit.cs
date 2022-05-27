namespace VetSolutionRationLib.Models.Units;

public interface ISpecificUnit : IUnit
{
    string DisplayMention { get; }
}

public sealed class SpecificUnit : UnitBase, ISpecificUnit
{
    /// <inheritdoc />
    public SpecificUnit(string displayMention) : base(UnitKind.Specific)
    {
        DisplayMention = displayMention;
    }

    /// <inheritdoc />
    public string DisplayMention { get; }
}