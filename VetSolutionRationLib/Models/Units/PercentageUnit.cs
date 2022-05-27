namespace VetSolutionRationLib.Models.Units;

public interface IPercentageUnit : IUnit
{
    /// <summary>
    /// The specific label of this unit
    /// </summary>
    string DisplayMention { get; }
}

public sealed class PercentageUnit : UnitBase, IPercentageUnit
{
    public string DisplayMention { get; }

    /// <inheritdoc />
    public PercentageUnit(string displayMention = "%") : base(UnitKind.Percentage)
    {
        DisplayMention = displayMention;
    }
}