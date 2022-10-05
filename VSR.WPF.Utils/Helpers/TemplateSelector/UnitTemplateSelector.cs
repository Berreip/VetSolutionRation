using System.Windows;
using System.Windows.Controls;
using VSR.WPF.Utils.Adapters.EditionIngredients;

namespace VSR.WPF.Utils.Helpers.TemplateSelector;

public sealed class UnitTemplateSelector : DataTemplateSelector
{
    public DataTemplate? Percentage { get; set; }
    public DataTemplate? Specific { get; set; }
    public DataTemplate? NoUnit { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is NutritionalDetailsAdapter adapter)
        {
            switch (adapter.Unit)
            {
                case PercentageUnitFeedAdapter percentageUnitFeedAdapter:
                    return Percentage;
                case SpecificUnitFeedAdapter specificUnitFeedAdapter:
                    return Specific;
                default:
                    return NoUnit;
            }
        }

        return null;
    }
}