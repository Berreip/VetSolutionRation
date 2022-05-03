namespace VetSolutionRation.DataProvider.Models;

public interface IInraRationTableImportModel
{

    /// <summary>
    /// Returns all lines in the inraTable
    /// </summary>
    IReadOnlyList<IInraRationLineImportModel> GetAllLines();
}

public sealed class InraRationTableImportModel : IInraRationTableImportModel
{
    private readonly IEnumerable<IInraRationLineImportModel> _lines;

    public InraRationTableImportModel(IEnumerable<IInraRationLineImportModel> lines)
    {
        _lines = lines;
    }

    /// <inheritdoc />
    public IReadOnlyList<IInraRationLineImportModel> GetAllLines()
    {
        return _lines.ToArray();
    }
}