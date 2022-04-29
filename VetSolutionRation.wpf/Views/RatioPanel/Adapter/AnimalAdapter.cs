using VetSolutionRation.wpf.Searcheable;

namespace VetSolutionRation.wpf.Views.RatioPanel.Adapter
{
    internal sealed class AnimalAdapter : SearcheableBase
    {
        public string DisplayName { get; }

        public AnimalAdapter(string animaldescription) : base(animaldescription)
        {
            DisplayName = animaldescription;

        }

        /// <inheritdoc />
        public override string ToString() => DisplayName;
    }
}