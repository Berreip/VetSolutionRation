using System.Text.RegularExpressions;

namespace VetSolutionRation.DataProvider.Utils;

/// <summary>
/// Class that match label with a regex
/// </summary>
internal static class LabelRegexHolder
{
    private static readonly Regex _labelRegexFr = new Regex("^Libellé ([0-9]*)$", RegexOptions.Compiled);
    private static readonly Regex _labelRegexEn = new Regex("^Label([0-9]*)$", RegexOptions.Compiled);
    
    public static bool Match(string str, out int labelPosition)
    {
        var matchFr = _labelRegexFr.Match(str);
        if (matchFr.Success && matchFr.Groups.Count > 1 &&  int.TryParse(matchFr.Groups[1].Value, out labelPosition))
        {
            return true;
        }
        var matchEn = _labelRegexEn.Match(str);
        if (matchEn.Success && matchEn.Groups.Count > 1 &&  int.TryParse(matchEn.Groups[1].Value, out labelPosition))
        {
            return true;
        }
        labelPosition = -1;
        return false;
    }
}