using System.Collections.Generic;

namespace VetSolutionRationLib.Helpers;

public static class LabelsHelper
{
    public static string JoinAsLabel(this IEnumerable<string>? str)
    {
        if (str == null)
        {
            return @"/";
        }
        return string.Join(" | ", str);
    }

}