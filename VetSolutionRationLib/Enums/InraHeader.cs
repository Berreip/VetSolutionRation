using System;
using System.Collections.Generic;
using System.Linq;

namespace VetSolutionRationLib.Enums;

// ReSharper disable InconsistentNaming
public enum InraHeader
{
    No,
    Etat,
    CodeInra,
    MS,
    UFL,
    UFV,
    PDIA,
    PDI,
    BPR,
    bVEc,
    MO,
    dMO,
    MAT,
    dMA,
    CB,
    NDF,
    dNDF,
    ADF,
    ADL,
    Amidon,
    AG,
    EE,
    P,
    Pabs,
    Ca,
    Caabs,
    Mg,
    BE,
    EB,

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
    NIref,
    UEM,
    UEL,
    UEB,
    dCB,
    dADF,
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

public static class InraHeaderExtensions
{
    private static readonly Dictionary<InraHeader, string[]> _inraHeaderToDisplayName;
    private static readonly Dictionary<InraHeader, string> _inraHeaderToDetailledInfo;
    private static readonly Dictionary<string, InraHeader> _inraDisplayNameToHeaderFr;
    private static readonly Dictionary<string, InraHeader> _inraDisplayNameToHeaderEn;
    private static readonly Dictionary<string, InraHeader> _inraDtoToHeader;

    // inra header that should be converted to string
    private static readonly HashSet<InraHeader> _stringInraContent = new HashSet<InraHeader>
    {
        InraHeader.No,
        InraHeader.Etat,
        InraHeader.CodeInra
    };

    static InraHeaderExtensions()
    {
        _inraHeaderToDisplayName = new Dictionary<InraHeader, string[]>
        {
            { InraHeader.No, new[] { "No" } },
            { InraHeader.Etat, new[] { "Etat", "Status" } },
            { InraHeader.CodeInra, new[] { "Code INRA", "INRA Code" } },
            { InraHeader.MS, new[] { "MS", "DM" } },
            { InraHeader.UFL, new[] { "UFL" } },
            { InraHeader.UFV, new[] { "UFV" } },
            { InraHeader.PDIA, new[] { "PDIA" } },
            { InraHeader.PDI, new[] { "PDI" } },
            { InraHeader.BPR, new[] { "BPR", "RPB", "RBP" } },
            { InraHeader.bVEc, new[] { "bVEc", "bFVc" } },
            { InraHeader.MO, new[] { "MO", "OM" } },
            { InraHeader.dMO, new[] { "dMO", "OMd" } },
            { InraHeader.MAT, new[] { "MAT", "CP" } },
            { InraHeader.dMA, new[] { "dMA", "CPd" } },
            { InraHeader.CB, new[] { "CB", "CF" } },
            { InraHeader.NDF, new[] { "NDF" } },
            { InraHeader.dNDF, new[] { "dNDF", "NDFd" } },
            { InraHeader.ADF, new[] { "ADF" } },
            { InraHeader.ADL, new[] { "ADL" } },
            { InraHeader.Amidon, new[] { "Amidon", "Starch" } },
            { InraHeader.AG, new[] { "AG", "FA" } },
            { InraHeader.EE, new[] { "EE" } },
            { InraHeader.P, new[] { "P" } },
            { InraHeader.Pabs, new[] { "Pabs" } },
            { InraHeader.Ca, new[] { "Ca" } },
            { InraHeader.Caabs, new[] { "Caabs" } },
            { InraHeader.Mg, new[] { "Mg" } },
            { InraHeader.BE, new[] { "BE", "EB" } },
            { InraHeader.EB, new[] { "EB", "GE" } },
            { InraHeader.dE, new[] { "dE", "Ed" } },
            { InraHeader.EM, new[] { "EM", "ME" } },
            { InraHeader.DT_N, new[] { "DT_N", "ED N", "ED_N" } },
            { InraHeader.DT6_N, new[] { "DT6_N", "ED6 N", "ED6_N" } },
            { InraHeader.dr_N, new[] { "dr_N", "dr N", "dr_N" } },
            { InraHeader.DT_Ami, new[] { "DT_Ami", "ED Starch" } },
            { InraHeader.DT6_Ami, new[] { "DT6_Ami", "ED6 Starch" } },
            { InraHeader.DT_MS, new[] { "DT_MS", "ED DM" } },
            { InraHeader.DT6_MS, new[] { "DT6_MS", "ED6 DM" } },
            { InraHeader.S, new[] { "S" } },
            { InraHeader.Na, new[] { "Na" } },
            { InraHeader.K, new[] { "K" } },
            { InraHeader.Cl, new[] { "Cl" } },
            { InraHeader.BACA, new[] { "BACA", "DCAD" } },
            { InraHeader.Cu, new[] { "Cu" } },
            { InraHeader.Zn, new[] { "Zn" } },
            { InraHeader.Mn, new[] { "Mn" } },
            { InraHeader.Co, new[] { "Co" } },
            { InraHeader.Se, new[] { "Se" } },
            { InraHeader.I, new[] { "I" } },
            { InraHeader.VitA, new[] { "VitA" } },
            { InraHeader.VitD, new[] { "VitD" } },
            { InraHeader.VitE, new[] { "VitE" } },
            { InraHeader.LysBP, new[] { "LysBP" } },
            { InraHeader.HisBP, new[] { "HisBP" } },
            { InraHeader.ArgBP, new[] { "ArgBP" } },
            { InraHeader.ThrBP, new[] { "ThrBP" } },
            { InraHeader.ValBP, new[] { "ValBP" } },
            { InraHeader.MetBP, new[] { "MetBP" } },
            { InraHeader.IleBP, new[] { "IleBP" } },
            { InraHeader.LeuBP, new[] { "LeuBP" } },
            { InraHeader.PheBP, new[] { "PheBP" } },
            { InraHeader.AspBP, new[] { "AspBP" } },
            { InraHeader.SerBP, new[] { "SerBP" } },
            { InraHeader.GluBP, new[] { "GluBP" } },
            { InraHeader.ProBP, new[] { "ProBP" } },
            { InraHeader.GlyBP, new[] { "GlyBP" } },
            { InraHeader.AlaBP, new[] { "AlaBP" } },
            { InraHeader.TyrBP, new[] { "TyrBP" } },
            { InraHeader.CysTrpBP, new[] { "CysTrpBP" } },
            { InraHeader.LysDI, new[] { "LysDI" } },
            { InraHeader.HisDI, new[] { "HisDI" } },
            { InraHeader.ArgDI, new[] { "ArgDI" } },
            { InraHeader.ThrDI, new[] { "ThrDI" } },
            { InraHeader.ValDI, new[] { "ValDI" } },
            { InraHeader.MetDI, new[] { "MetDI" } },
            { InraHeader.IleDI, new[] { "IleDI" } },
            { InraHeader.LeuDI, new[] { "LeuDI" } },
            { InraHeader.PheDI, new[] { "PheDI" } },
            { InraHeader.AspDI, new[] { "AspDI" } },
            { InraHeader.SerDI, new[] { "SerDI" } },
            { InraHeader.GluDI, new[] { "GluDI" } },
            { InraHeader.ProDI, new[] { "ProDI" } },
            { InraHeader.GlyDI, new[] { "GlyDI" } },
            { InraHeader.AlaDI, new[] { "AlaDI" } },
            { InraHeader.TyrDI, new[] { "TyrDI" } },
            { InraHeader.NIref, new[] { "NIref", "FLref" } },
            { InraHeader.UEM, new[] { "UEM" } },
            { InraHeader.UEL, new[] { "UEL" } },
            { InraHeader.UEB, new[] { "UEB" } },
            { InraHeader.dCB, new[] { "dCB", "CFd" } },
            { InraHeader.dADF, new[] { "dADF", "ADFd" } },
            { InraHeader.C6_10, new[] { "C6-10" } },
            { InraHeader.C12_0, new[] { "C12:0" } },
            { InraHeader.C14_0, new[] { "C14:0" } },
            { InraHeader.C16_0, new[] { "C16:0" } },
            { InraHeader.C16_1, new[] { "C16:1" } },
            { InraHeader.C18_0, new[] { "C18:0" } },
            { InraHeader.C18_1, new[] { "C18:1" } },
            { InraHeader.C18_2, new[] { "C18:2" } },
            { InraHeader.C18_3, new[] { "C18:3" } },
            { InraHeader.C20_0, new[] { "C20:0" } },
            { InraHeader.C20_1, new[] { "C20:1" } },
            { InraHeader.C22_0, new[] { "C22:0" } },
            { InraHeader.C22_1, new[] { "C22:1" } },
            { InraHeader.C24_0, new[] { "C24:0" } },
        };

        _inraDtoToHeader = Enum.GetValues(typeof(InraHeader)).Cast<InraHeader>().ToDictionary(o => o.ToDtoKey());
        _inraDisplayNameToHeaderFr = new Dictionary<string, InraHeader>(_inraHeaderToDisplayName.Count, StringComparer.OrdinalIgnoreCase);
        _inraDisplayNameToHeaderEn = new Dictionary<string, InraHeader>(_inraHeaderToDisplayName.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var (inraHeader, headersLabels) in _inraHeaderToDisplayName)
        {
            _inraDisplayNameToHeaderFr.Add(headersLabels[0], inraHeader);
            if (headersLabels.Length == 1)
            {
                _inraDisplayNameToHeaderEn.Add(headersLabels[0], inraHeader);
            }
            else
            {
                for (var i = 1; i < headersLabels.Length; i++)
                {
                    // there is on case  where there is 2 traductions in EN (typo probably) ("BPR" => "RPB"/"RBP")
                    _inraDisplayNameToHeaderEn.Add(headersLabels[i], inraHeader);
                }
            }
        }

        _inraHeaderToDetailledInfo = new Dictionary<InraHeader, string>
        {
            { InraHeader.MS, @"Matière sèche (%)" },
            { InraHeader.UFL, @"Energie nette pour la lactation (UFL/kg MS) " },
            { InraHeader.UFV, @"Energie nette pour la production de viande (UFV/kg MS) " },
            { InraHeader.PDIA, @"Protéines digestibles dans l’intestin d’origine alimentaire (g/kg MS) " },
            { InraHeader.PDI, @"Protéines digestibles dans l’intestin d’origine alimentaire et microbienne (g/kg MS) " },
            { InraHeader.BPR, @"Balance protéique du rumen (g/kg MS) " },
            { InraHeader.LysDI, @"Lysine digestible (% des PDI) " },
            { InraHeader.MetDI, @"Méthionine digestible (% des PDI) " },
            { InraHeader.HisDI, @"Histidine digestible (% des PDI) " },
            { InraHeader.NIref, @"Niveau d’ingestion de référence (% du PV) " },
            { InraHeader.UEM, @"Valeur d’encombrement « mouton » (UEM/kg MS) " },
            { InraHeader.UEL, @"Valeur d’encombrement « vaches et chèvres laitières » (UEL/kg MS) " },
            { InraHeader.UEB, @"Valeur d’encombrement « bovins allaitants et en croissance » (UEB/kg MS) " },
            // { InraHeader.bFVc, @"Valeur basale d’encombrement de l’aliment concentré (UEL/kg MS) " },
            { InraHeader.MO, @"Matière organique (g/kg MS) " },
            { InraHeader.MAT, @"Matières azotées totales (N x 6.25) (g/kg MS) " },
            { InraHeader.CB, @"Cellulose brute (g/kg MS) " },
            { InraHeader.NDF, @"Fibre insoluble dans le détergent neutre (g/kg MS) " },
            { InraHeader.ADF, @"Fibre insoluble dans le détergent acide (g/kg MS) " },
            { InraHeader.ADL, @"Lignine insoluble dans l’acide sulfurique (g/kg MS) " },
            { InraHeader.AG, @"Acides gras (g/kg MS) " },
            { InraHeader.EE, @"Extrait éthéré (matière grasse brute) (g/kg MS) " },
            { InraHeader.Amidon, @"Amidon (g/kg MS) " },
            { InraHeader.dMO, @" Coefficients de digestibilité apparente de la MO (%)" },
            { InraHeader.dMA, @" Coefficients de digestibilité apparente de la MAT (%)" },
            { InraHeader.dCB, @" Coefficients de digestibilité apparente de la CB (%)" },
            { InraHeader.dNDF, @" Coefficients de digestibilité apparente de la NDF (%)" },
            { InraHeader.dADF, @" Coefficients de digestibilité apparente de l'ADF (%)" },
            { InraHeader.dE, @" Coefficients de digestibilité apparente de l’énergie (%)" },
            { InraHeader.P, @"Phosphore total (g/kg MS) " },
            { InraHeader.Pabs, @"Phosphore absorbable (g/kg MS) " },
            { InraHeader.Ca, @"Calcium total (g/kg MS) " },
            { InraHeader.Caabs, @"Calcium absorbable (g/kg MS) " },
            { InraHeader.Mg, @"Magnésium (g/kg MS) " },
            { InraHeader.BE, @"Bilan électrolytique (mEq/kg MS) " },
            { InraHeader.EB, @"Energie brute (kcal/kg MS) " },
            { InraHeader.EM, @"Energie métabolisable (kcal/kg MS) " },
            {
                InraHeader.DT_N, @"Coefficients de dégradabilité théorique calculés pour un taux de passage correspondant à NIref 
(DT_X) et pour un taux de passage de 6%/h (DT6_X) respectivement de l’azote dans le rumen (%)"
            },
            {
                InraHeader.DT6_N, @"Coefficients de dégradabilité théorique calculés pour un taux de passage correspondant à NIref 
(DT_X) et pour un taux de passage de 6%/h (DT6_X) respectivement de l’azote dans le rumen (%)"
            },
            {
                InraHeader.DT_MS, @"Coefficients de dégradabilité théorique calculés pour un taux de passage correspondant à NIref 
(DT_X) et pour un taux de passage de 6%/h (DT6_X) respectivement de la matière 
sèche dans le rumen (%)"
            },
            {
                InraHeader.DT6_MS, @"Coefficients de dégradabilité théorique calculés pour un taux de passage correspondant à NIref 
(DT_X) et pour un taux de passage de 6%/h (DT6_X) respectivement de la matière 
sèche dans le rumen (%)"
            },
            {
                InraHeader.DT_Ami, @"Coefficients de dégradabilité théorique calculés pour un taux de passage correspondant à NIref 
(DT_X) et pour un taux de passage de 6%/h (DT6_X) respectivement de l’amidon dans le rumen (%)"
            },
            {
                InraHeader.DT6_Ami, @"Coefficients de dégradabilité théorique calculés pour un taux de passage correspondant à NIref 
(DT_X) et pour un taux de passage de 6%/h (DT6_X) respectivement de l’amidon dans le rumen (%)"
            },
            { InraHeader.dr_N, @"igestibilité réelle de l’azote d’origine alimentaire dans l’intestin (%)" },
            { InraHeader.S, @"Soufre (g/kg MS)" },
            { InraHeader.Na, @"Sodium (g/kg MS)" },
            { InraHeader.K, @"Potassium (g/kg MS)" },
            { InraHeader.Cl, @"Chlore (g/kg MS)" },
            { InraHeader.BACA, @"Bilan cations anions (mEq/kg MS)" },
            { InraHeader.Cu, @"Cuivre (mg/kg MS" },
            { InraHeader.Zn, @"Zinc (mg/kg MS)" },
            { InraHeader.Mn, @"Manganèse (mg/kg MS)" },
            { InraHeader.Co, @"Cobalt (mg/kg MS)" },
            { InraHeader.Se, @"Sélénium (mg/kg MS)" },
            { InraHeader.I, @"Iode (mg/kg MS)" },
            { InraHeader.VitA, @"Vitamine A en UI " },
            { InraHeader.VitD, @"Vitamine D en UI " },
            { InraHeader.VitE, @"Vitamine E en UI " },
            // { InraHeader.AABP, @"Concentration en Acides Aminés By-Pass au duodénum en g/100 g des 16 AA pour les 16 AABP (de la Lys à la Tyr), et en g/100 g des 18 AA pour la somme de la Cys et du Trp (CysTrpBP). Ces valeurs sont notées [AAni] dans les chapitres de Inra 2018" },
            // { InraHeader.AADI, @"Acides Aminés Digestibles dans l’Intestin (% PDI)" },
            { InraHeader.C6_10, @"en % de la teneur en Acide Gras" },
            { InraHeader.C12_0, @"en % de la teneur en Acide Gras" },
            { InraHeader.C14_0, @"en % de la teneur en Acide Gras" },
            { InraHeader.C16_0, @"en % de la teneur en Acide Gras" },
            { InraHeader.C16_1, @"en % de la teneur en Acide Gras" },
            { InraHeader.C18_0, @"en % de la teneur en Acide Gras" },
            { InraHeader.C18_1, @"en % de la teneur en Acide Gras" },
            { InraHeader.C18_2, @"en % de la teneur en Acide Gras" },
            { InraHeader.C18_3, @"en % de la teneur en Acide Gras" },
            { InraHeader.C20_0, @"en % de la teneur en Acide Gras" },
            { InraHeader.C20_1, @"en % de la teneur en Acide Gras" },
            { InraHeader.C22_0, @"en % de la teneur en Acide Gras" },
            { InraHeader.C22_1, @"en % de la teneur en Acide Gras" },
            { InraHeader.C24_0, @"en % de la teneur en Acide Gras" },
        };
    }

    /// <summary>
    /// Return the detailled information about an INRA header
    /// </summary>
    public static string GetDetailledInfo(this InraHeader inraHeader)
    {
        return _inraHeaderToDetailledInfo.TryGetValue(inraHeader, out var detailledInfo) 
            ? detailledInfo
            : inraHeader.GetInraHeaderLabel();
    }
    
    public static bool IsStringContent(this InraHeader inraHeader)
    {
        return _stringInraContent.Contains(inraHeader);
    }

    public static string ToDtoKey(this InraHeader inraHeader)
    {
        // just a to string for now
        return inraHeader.ToString();
    }

    public static string GetInraHeaderLabel(this InraHeader inraHeader, InraSourceFileCulture culture = InraSourceFileCulture.French)
    {
        if (_inraHeaderToDisplayName.TryGetValue(inraHeader, out var label))
        {
            if (label.Length == 1)
            {
                // same FR/EN
                return label[0];
            }

            return label[culture == InraSourceFileCulture.French ? 0 : 1];
        }

        throw new ArgumentOutOfRangeException(nameof(inraHeader), inraHeader, null);
    }

    public static bool TryParseInraHeader(string label, InraSourceFileCulture culture, out InraHeader header)
    {
        return culture == InraSourceFileCulture.French
            ? _inraDisplayNameToHeaderFr.TryGetValue(label, out header)
            : _inraDisplayNameToHeaderEn.TryGetValue(label, out header);
    }

    public static bool TryParseDtoInraHeader(string label, out InraHeader header)
    {
        return _inraDtoToHeader.TryGetValue(label, out header);
    }
}

public enum InraSourceFileCulture
{
    French,
    English
}