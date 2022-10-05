using System;
using System.Collections.Generic;
using System.Linq;
using VSR.Enums;

namespace VSR.Dto.Utils;

internal static class InraHeaderDtoConverter
{
    private static readonly Dictionary<string, InraHeader> _inraDtoToHeader;
    
    static InraHeaderDtoConverter()
    {
        _inraDtoToHeader = Enum.GetValues(typeof(InraHeader)).Cast<InraHeader>().ToDictionary(o => o.ToDtoKey());
    }
    
    public static string ToDtoKey(this InraHeader inraHeader)
    {
        // just a to string for now
        return inraHeader.ToString();
    }

    public static bool TryParseDtoInraHeader(string? label, out InraHeader header)
    {
        if (label == null)
        {
            header = default;
            return false;
        }
        return _inraDtoToHeader.TryGetValue(label, out header);
    }

}