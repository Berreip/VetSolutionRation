using System.Diagnostics;
using PRF.Utils.CoreComponents.Diagnostic;
using VSR.Enums;

namespace VetSolutionRation.DataProvider.Models;

/// <summary>
/// Rpresent a line in a feed table reference
/// </summary>
public interface IInraRationLineImportModel
{
    string Label { get; }
    IReadOnlyDictionary<InraHeader, FeedCellModel> GetAllCells();
}

/// <summary>
/// Rpresent a line in a feed table reference
/// </summary>
public sealed class InraRationLineImportModel : IInraRationLineImportModel
{
    private readonly Dictionary<InraHeader, FeedCellModel> _feedCellModels = new Dictionary<InraHeader, FeedCellModel>();
    public string Label { get; }

    public InraRationLineImportModel(IEnumerable<FeedCellModel> feedCellModels, IReadOnlyCollection<string> labels)
    {
        Label = string.Join(" | ", labels);
        DebugCore.Assert(labels.Count != 0, "should have at least one label");
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
                    Trace.TraceError($"row [{Label}] : the field {cell.HeaderKind} is duplicated and it value differs: [{value.Content}] VS [{cell.Content}]");
                    DebugCore.Fail($"row [{Label}] : the field {cell.HeaderKind} is duplicated and it value differs: [{value.Content}] VS [{cell.Content}]");
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
    public override string ToString()
    {
        return $"[{_feedCellModels.Count}] - {Label}";
    }
}