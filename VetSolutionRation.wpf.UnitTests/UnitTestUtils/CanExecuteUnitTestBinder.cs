using System.Windows.Input;

namespace VetSolutionRation.wpf.UnitTests.UnitTestUtils;

public static class CanExecuteUnitTestBinder
{
    // bind the can execute handler to the aexecute to reproduce a WPF behaviour in unit tests
    public static void BindCanExecuteChangedForUnitTests(this ICommand cmd)
    {
        cmd.CanExecuteChanged += (sender, _) => cmd.CanExecute(sender);
    }
    
}