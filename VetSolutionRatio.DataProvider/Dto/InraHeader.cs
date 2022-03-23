using System.Diagnostics.CodeAnalysis;

namespace VetSolutionRatio.DataProvider.Dto;

// ReSharper disable InconsistentNaming
internal enum InraHeader
{
    No,
    Etat,
    CodeInra,
    dE,
    EM,
    DT_N,
    DT6_N,
    dr_N,
    DT_Ami,
    DT6_Ami,
    DT_MS,
    DT6_MS,

    S,
    Na,
    K,
    Cl,
    BACA,
    Cu,
    Zn,
    Mn,
    Co,
    Se,
    I,
    VitA,
    VitD,
    VitE,
    LysBP,
    HisBP,
    ArgBP,
    ThrBP,
    ValBP,
    MetBP,
    IleBP,
    LeuBP,
    PheBP,
    AspBP,
    SerBP,
    GluBP,
    ProBP,
    GlyBP,
    AlaBP,
    TyrBP,
    CysTrpBP,
    LysDI,
    HisDI,
    ArgDI,
    ThrDI,
    ValDI,
    MetDI,
    IleDI,
    LeuDI,
    PheDI,
    AspDI,
    SerDI,
    GluDI,
    ProDI,
    GlyDI,
    AlaDI,
    TyrDI,
    C6_10,
    C12_0,
    C14_0,
    C16_0,
    C16_1,
    C18_0,
    C18_1,
    C18_2,
    C18_3,
    C20_0,
    C20_1,
    C22_0,
    C22_1,
    C24_0,
}
// ReSharper restore InconsistentNaming

internal static class InraHeaderExtensions
{
    private static readonly Dictionary<InraHeader, string> _inraHeaderToDisplayName;
    private static readonly Dictionary<string, InraHeader> _inraDisplayNameToHeader;

    static InraHeaderExtensions()
    {
        _inraHeaderToDisplayName = new Dictionary<InraHeader, string>
        {
            { InraHeader.No, "No" },
            { InraHeader.Etat, "Etat" },
            { InraHeader.CodeInra, "Code INRA" },
            { InraHeader.dE, "dE" },
            { InraHeader.EM, "EM" },
            { InraHeader.DT_N, "DT_N" },
            { InraHeader.DT6_N, "DT6_N" },
            { InraHeader.dr_N, "dr_N" },
            { InraHeader.DT_Ami, "DT_Ami" },
            { InraHeader.DT6_Ami, "DT6_Ami" },
            { InraHeader.DT_MS, "DT_MS" },
            { InraHeader.DT6_MS, "DT6_MS" },
            { InraHeader.S , "S"},
            { InraHeader.Na , "Na"},
            { InraHeader.K , "K"},
            { InraHeader.Cl , "Cl"},
            { InraHeader.BACA , "BACA"},
            { InraHeader.Cu , "Cu"},
            { InraHeader.Zn , "Zn"},
            { InraHeader.Mn , "Mn"},
            { InraHeader.Co , "Co"},
            { InraHeader.Se , "Se"},
            { InraHeader.I , "I"},
            { InraHeader.VitA , "VitA"},
            { InraHeader.VitD , "VitD"},
            { InraHeader.VitE , "VitE"},
            { InraHeader.LysBP , "LysBP"},
            { InraHeader.HisBP , "HisBP"},
            { InraHeader.ArgBP , "ArgBP"},
            { InraHeader.ThrBP , "ThrBP"},
            { InraHeader.ValBP , "ValBP"},
            { InraHeader.MetBP , "MetBP"},
            { InraHeader.IleBP , "IleBP"},
            { InraHeader.LeuBP , "LeuBP"},
            { InraHeader.PheBP , "PheBP"},
            { InraHeader.AspBP , "AspBP"},
            { InraHeader.SerBP , "SerBP"},
            { InraHeader.GluBP , "GluBP"},
            { InraHeader.ProBP , "ProBP"},
            { InraHeader.GlyBP , "GlyBP"},
            { InraHeader.AlaBP , "AlaBP"},
            { InraHeader.TyrBP , "TyrBP"},
            { InraHeader.CysTrpBP , "CysTrpBP"},
            { InraHeader.LysDI , "LysDI"},
            { InraHeader.HisDI , "HisDI"},
            { InraHeader.ArgDI , "ArgDI"},
            { InraHeader.ThrDI , "ThrDI"},
            { InraHeader.ValDI , "ValDI"},
            { InraHeader.MetDI , "MetDI"},
            { InraHeader.IleDI , "IleDI"},
            { InraHeader.LeuDI , "LeuDI"},
            { InraHeader.PheDI , "PheDI"},
            { InraHeader.AspDI , "AspDI"},
            { InraHeader.SerDI , "SerDI"},
            { InraHeader.GluDI , "GluDI"},
            { InraHeader.ProDI , "ProDI"},
            { InraHeader.GlyDI , "GlyDI"},
            { InraHeader.AlaDI , "AlaDI"},
            { InraHeader.TyrDI , "TyrDI"},
            { InraHeader.C6_10 , "C6-10"},
            { InraHeader.C12_0 , "C12:0"},
            { InraHeader.C14_0 , "C14:0"},
            { InraHeader.C16_0 , "C16:0"},
            { InraHeader.C16_1 , "C16:1"},
            { InraHeader.C18_0 , "C18:0"},
            { InraHeader.C18_1 , "C18:1"},
            { InraHeader.C18_2 , "C18:2"},
            { InraHeader.C18_3 , "C18:3"},
            { InraHeader.C20_0 , "C20:0"},
            { InraHeader.C20_1 , "C20:1"},
            { InraHeader.C22_0 , "C22:0"},
            { InraHeader.C22_1 , "C22:1"},
            { InraHeader.C24_0 , "C24:0"},
        };
        _inraDisplayNameToHeader = _inraHeaderToDisplayName.ToDictionary(o => o.Value, o => o.Key, StringComparer.OrdinalIgnoreCase);
    }

    public static string GetInraHeaderLabel(this InraHeader inraHeader)
    {
        if (_inraHeaderToDisplayName.TryGetValue(inraHeader, out var label))
        {
            return label;
        }

        throw new ArgumentOutOfRangeException(nameof(inraHeader), inraHeader, null);
    }

    public static bool TryParseInraHeader(string label, out InraHeader header)
    {
        return _inraDisplayNameToHeader.TryGetValue(label, out header);
    }
}