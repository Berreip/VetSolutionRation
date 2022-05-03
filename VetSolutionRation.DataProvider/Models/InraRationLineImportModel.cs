using System.Diagnostics;
using PRF.Utils.CoreComponents.Diagnostic;
using VetSolutionRation.DataProvider.Utils;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.DataProvider.Models;

public interface IInraRationLineImportModel
{
    string JoinedLabel { get; }
    IReadOnlyDictionary<InraHeader, FeedCellModel> GetAllCells();
    IReadOnlyCollection<string> GetLabels();
}

/// <summary>
/// Rpresent a line in a feed table reference
/// </summary>
public sealed class InraRationLineImportModel : IInraRationLineImportModel
{
    private readonly IReadOnlyCollection<string> _labels;
    private readonly Dictionary<InraHeader, FeedCellModel> _feedCellModels = new Dictionary<InraHeader, FeedCellModel>();
    public string JoinedLabel { get; }

    public InraRationLineImportModel(IEnumerable<FeedCellModel> feedCellModels, IReadOnlyCollection<string> labels)
    {
        _labels = labels;
        DebugCore.Assert(labels.Count != 0, "should have at least one label");
        JoinedLabel = labels.JoinAsLabel();
        foreach (var cell in feedCellModels)
        {
            if(_feedCellModels.TryGetValue(cell.HeaderKind, out var value))
            {
                if (value.IsContentIgnorableWhenDuplicates())
                {
                    // update if first input is ignorable
                    _feedCellModels[cell.HeaderKind] = cell;
                }
                else if(cell.IsContentIgnorableWhenDuplicates())
                {
                    // ignore
                    continue;
                }
                
                if (!value.Match(cell))
                {
                    Trace.TraceError($"row [{JoinedLabel}] : the field {cell.HeaderKind} is duplicated and it value differs: [{value.Content}] VS [{cell.Content}]");
                    DebugCore.Fail($"row [{JoinedLabel}] : the field {cell.HeaderKind} is duplicated and it value differs: [{value.Content}] VS [{cell.Content}]");
                }
                // else ignore the duplicates if same values
            }
            else
            {
                _feedCellModels.Add(cell.HeaderKind, cell);
            }
        }
    }

    public IReadOnlyDictionary<InraHeader, FeedCellModel> GetAllCells()
    {
        return new Dictionary<InraHeader, FeedCellModel>(_feedCellModels);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string> GetLabels()
    {
        return _labels.ToArray();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{_feedCellModels.Count}] - {JoinedLabel}";
    }
}