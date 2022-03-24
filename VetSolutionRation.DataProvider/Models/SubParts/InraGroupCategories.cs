using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

namespace VetSolutionRation.DataProvider.Models.SubParts;

internal sealed class InraGroupCategories
{
    private readonly List<HeaderGroup> _orderedGroups = new List<HeaderGroup>();
    public IReadOnlyCollection<HeaderGroup> OrderedGroups => _orderedGroups;
    public InraSourceFileCulture GuessedCulture { get; }

    public InraGroupCategories(IExcelRowDto groupRow)
    {
        foreach (var rowCell in groupRow.Cells)
        {
            _orderedGroups.Add(new HeaderGroup(rowCell.Key, rowCell.Value));
        }

        GuessedCulture = _orderedGroups[0].GroupHeader.StartsWith("Valeurs de la table") ? InraSourceFileCulture.French : InraSourceFileCulture.English;
    }

    public HeaderGroup GetGroupByIndex(int columnIndex)
    {
        for (var i = 0; i < _orderedGroups.Count; i++)
        {
            if (_orderedGroups[i].StartingIndex > columnIndex)
            {
                return _orderedGroups[i - 1];
            }
        }

        return _orderedGroups.Last();
    }
}