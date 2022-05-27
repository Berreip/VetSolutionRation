using System;
using System.Windows;
using System.Windows.Controls;
using VetSolutionRation.wpf.Views.Popups.Adapters;

namespace VetSolutionRation.wpf.Views.TemplateSelector;

public sealed class UnitTemplateSelector : DataTemplateSelector
{
    public DataTemplate? Percentage { get; set; }
    public DataTemplate? Specific { get; set; }
    public DataTemplate? NoUnit { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is FeedDetailInEditionAdapter adapter)
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