using System.Diagnostics;
using VetSolutionRation.DataProvider.Models.SubParts;
using VSR.Enums;

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


    public IEnumerable<(InraHeader HeaderKind, string HeaderGroupLabel, int HeaderPosition)> GetDefinedHeaders()
    {
        var results = new List<(InraHeader HeaderKind, string HeaderGroupLabel, int HeaderPosition)>();
        foreach (var groups in _columnIndexByGroupAndHeader)
        {
            // pas de filtre des duplicats par groupe
            foreach (var data in groups.Value)
            {
                try
                {
                    results.Add((data.Key, groups.Key.GroupHeader, data.Value));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        return results;
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