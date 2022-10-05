using System.IO;

namespace VSR.WPF.Utils.Helpers
{
    public static class VetSolutionRatioConstants
    {
        public const string INRAE_SOURCE_FILE_EXTENSIONS = ".xlsx";
        public const string VET_SOLUTION_RATIO_FOLDER = "VetSolutionRatio";
        public const string CACHE_FOLDER = "Cache";
        public const string LOADED_DATA_CACHE_FOLDER = "LoadedData";
        public const string SAVED_DATA_REFERENCE_FILE_NAME = "VSR_Saved_Data.vsr";
        public const double DEFAULT_FEED_VALUE = 1d;

        /// <summary>
        /// Returns true if the file extensions could match with allowed format
        /// </summary>
        public static bool IsAllowedDiffExtension(this FileInfo file)
        {
            return file.Extension == INRAE_SOURCE_FILE_EXTENSIONS;
        }
    }
}