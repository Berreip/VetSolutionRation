using PRF.Utils.CoreComponents.Diagnostic;

namespace VetSolutionRation.Common.Async;

public static class ErrorHandler
{
    /// <summary>
    /// Handle consistently error accross application
    /// </summary>
    public static void HandleError(string message)
    {
        DebugCore.Fail(message);
    }
}