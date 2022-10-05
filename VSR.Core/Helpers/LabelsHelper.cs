using System.Collections.Generic;

namespace VSR.Core.Helpers;

public static class LabelsHelper
{
// TODO PBO => virer
    public static string JoinAsLabel(this IEnumerable<string>? str)
    {
        if (str == null)
        {
            return @"/";
        }
        return string.Join(" | ", str);
    }

}