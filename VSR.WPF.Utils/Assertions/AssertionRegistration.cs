using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore.UiWorkerThread;

namespace VSR.WPF.Utils.Assertions;

public static class AssertionRegistration
{
    public static void RegisterAssertionHandler()
    {
        DebugCore.SetAssertionFailedCallback(OnAssertionFailed);
    }

    private static AssertionResponse OnAssertionFailed(AssertionFailedResult assertionfailedResult)
    {
        return UiThreadDispatcher.ExecuteOnUI(() =>
        {
            var assertionVm = new AssertionFailedViewModel(assertionfailedResult);
            var view = new AssertionFailedView(assertionVm);
            assertionVm.OnResponseSet += () => view.Close();
            view.ShowDialog();
            return assertionVm.GetResponse();
        });
    }
}