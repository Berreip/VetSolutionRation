
namespace VetSolutionRation.wpf.Views.Calculation;

internal sealed partial class CalculationView 
{
    public ICalculationViewModel ViewModel { get; }
    
    public CalculationView(ICalculationViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        ViewModel = vm;
    }
}