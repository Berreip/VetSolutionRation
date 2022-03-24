using System.Diagnostics;
using VetSolutionRation.DataProvider.Models.SubParts;

namespace VetSolutionRation.DataProvider.Models;

internal sealed class InraHeaderModel
{
    private readonly InraGroupCategories _inraGroupCategories;
    private readonly Dictionary<HeaderGroup, Dictionary<InraHeader, int>> _columnIndexByGroupAndHeader = new Dictionary<HeaderGroup,Dictionary<InraHeader, int>>();
    private readonly Dictionary<int, string> _labelParts = new Dictionary<int, string>();

    public InraHeaderModel(InraGroupCategories inraGroupCategories)
    {
        _inraGroupCategories = inraGroupCategories;
        foreach (var groups in inraGroupCategories.OrderedGroups)
        {
            _columnIndexByGroupAndHeader.Add(groups, new Dictionary<InraHeader, int>());
        }
    }

    public void AddHeader(int columnIndex, InraHeader inraHeader)
    {
        try
        {
            _columnIndexByGroupAndHeader[_inraGroupCategories.GetGroupByIndex(columnIndex)].Add(inraHeader, columnIndex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void AddLabelPart(int labelPosition, string value)
    {
        if (!_labelParts.ContainsKey(labelPosition))
        {
            _labelParts.Add(labelPosition, value);
        }
        else
        {
            Debug.Fail($"duplicate label position for {labelPosition} [{value}]");
        }
    }

    public IEnumerable<int> GetLabelPositions()
    {
        return _labelParts.Keys;
    }
}