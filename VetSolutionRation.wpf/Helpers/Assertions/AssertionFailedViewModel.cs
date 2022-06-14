using System;
using System.Windows;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore;
using PRF.WPFCore.Commands;

namespace VetSolutionRation.wpf.Helpers.Assertions
{
    internal sealed class AssertionFailedViewModel : ViewModelBase
    {
        private AssertionResponse _assertionResponse = AssertionResponse.Ignore;
        public IDelegateCommandLight DebugCommand { get; }
        public IDelegateCommandLight ExportTraceCommand { get; }
        public IDelegateCommandLight KillApplicationCommand { get; }
        public string Message { get; }
        public string SourceMethod { get; }
        public string StackTrace { get; }

        public IDelegateCommandLight CopyToClipboardCommand { get; }

        public event Action? OnResponseSet;

        public AssertionFailedViewModel(AssertionFailedResult assertionFailedResult)
        {
            CopyToClipboardCommand = new DelegateCommandLight(ExecuteCopyToClipboardCommand);
            DebugCommand = new DelegateCommandLight(ExecuteDebugCommand);
            ExportTraceCommand = new DelegateCommandLight(ExecuteExportTraceCommand);
            KillApplicationCommand = new DelegateCommandLight(ExecuteKillApplicationCommand);
            Message = assertionFailedResult.Message;
            StackTrace = assertionFailedResult.StackTrace;
            SourceMethod = assertionFailedResult.SourceMethod;
        }

        private void ExecuteCopyToClipboardCommand()
        {
            Clipboard.SetText(StackTrace);
        }

        private void ExecuteExportTraceCommand()
        {
            // Export trace
        }

        private void ExecuteDebugCommand()
        {
            SetResponse(AssertionResponse.Debug);
        }

        private void ExecuteKillApplicationCommand()
        {
            SetResponse(AssertionResponse.TerminateProcess);
        }

        public AssertionResponse GetResponse()
        {
            return _assertionResponse;
        }

        private void SetResponse(AssertionResponse assertionResponse)
        {
            _assertionResponse = assertionResponse;
            RaiseOnResponseSet();
        }

        private void RaiseOnResponseSet()
        {
            OnResponseSet?.Invoke();
        }
    }
}
