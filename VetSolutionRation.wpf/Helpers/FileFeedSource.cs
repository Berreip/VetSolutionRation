using PRF.Utils.CoreComponents.Extensions;

namespace VetSolutionRation.wpf.Helpers;

public enum FileFeedSource
{
    /// <summary>
    /// Official reference (INRAE) source file (not deleteable)
    /// </summary>
    Reference, 
    
    /// <summary>
    /// Custom reference
    /// </summary>
    Custom
}

public static class FileFeedSourceExtensions
{
    private const string FR_CONCENTRATES = "Concentre";
    private const string EN_CONCENTRATES = "Concentrate";
    private const string FR_FORAGES = "Fourrage";
    private const string EN_FORAGES = "Forage";

    public static bool TryParseFromFileName(string fileName, out FileFeedSource fileFeedParsed)
    {
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (fileName.ContainsInsensitive(FR_CONCENTRATES) || fileName.ContainsInsensitive(EN_CONCENTRATES))
            {
                fileFeedParsed = FileFeedSource.Reference;
                return true;
            }

            if (fileName.ContainsInsensitive(FR_FORAGES) || fileName.ContainsInsensitive(EN_FORAGES))
            {
                fileFeedParsed = FileFeedSource.Reference;
                return true;
            }
        }

        fileFeedParsed = FileFeedSource.Custom;
        return false;
    }
}