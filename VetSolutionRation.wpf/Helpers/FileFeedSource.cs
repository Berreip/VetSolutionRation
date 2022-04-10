using PRF.Utils.CoreComponents.Extensions;

namespace VetSolutionRation.wpf.Helpers;

public enum FileFeedSource
{
    Forage, 
    Concentrate
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
                fileFeedParsed = FileFeedSource.Concentrate;
                return true;
            }
            else if (fileName.ContainsInsensitive(FR_FORAGES) || fileName.ContainsInsensitive(EN_FORAGES))
            {
                fileFeedParsed = FileFeedSource.Forage;
                return true;
            }
        }

        fileFeedParsed = default;
        return false;
    }
}