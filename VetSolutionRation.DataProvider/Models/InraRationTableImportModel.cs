namespace VetSolutionRation.DataProvider.Models;

public interface IInraRationTableImportModel
{
    /// <summary>
    /// Returns all available label in the file
    /// </summary>
    IReadOnlyList<string> GetAllLabels();

    /// <summary>
    /// Returns all lines in the inraTable
    /// </summary>
    IReadOnlyCollection<IInraRationLineImportModel> GetAllLines();
}

public sealed class InraRationTableImportModel : IInraRationTableImportModel
{
    private readonly IEnumerable<IInraRationLineImportModel> _lines;

    public InraRationTableImportModel(IEnumerable<IInraRationLineImportModel> lines)
    {
        _lines = lines;
    }

    /// <inheritdoc />
    public IReadOnlyList<string> GetAllLabels()
    {
        return _lines.Select(o => o.JoinedLabel).ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IInraRationLineImportModel> GetAllLines()
    {
        return _lines.ToArray();
    }
}