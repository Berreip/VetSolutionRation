using System.Collections.Generic;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.wpf.Views.Popups.Adapters;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;

internal class FeedInEditionHelpers
{
    public static bool FilterDetailsParts(object item, string? search)
    {
        if (item is FeedDetailInEditionAdapter feedInEditionAdapter)
        {
            if (feedInEditionAdapter.HeaderName.ContainsInsensitive(search))
            {
                return true;
            }
        }

        return false;
    }
        public static FeedDetailInEditionAdapter[] GetDefaultHeaderForEdition()
    {
        return new []
        {
            new FeedDetailInEditionAdapter(InraHeader.MS),
            new FeedDetailInEditionAdapter(InraHeader.UFL),
            new FeedDetailInEditionAdapter(InraHeader.UFV),
            new FeedDetailInEditionAdapter(InraHeader.PDIA),
            new FeedDetailInEditionAdapter(InraHeader.PDI),
            new FeedDetailInEditionAdapter(InraHeader.BPR),
            new FeedDetailInEditionAdapter(InraHeader.LysDI),
            new FeedDetailInEditionAdapter(InraHeader.MetDI),
            new FeedDetailInEditionAdapter(InraHeader.HisDI),	
            new FeedDetailInEditionAdapter(InraHeader.bVEc),	
            new FeedDetailInEditionAdapter(InraHeader.MO),
            new FeedDetailInEditionAdapter(InraHeader.dMO),
            new FeedDetailInEditionAdapter(InraHeader.MAT),
            new FeedDetailInEditionAdapter(InraHeader.dMA),	
            new FeedDetailInEditionAdapter(InraHeader.CB),	
            new FeedDetailInEditionAdapter(InraHeader.NDF),	
            new FeedDetailInEditionAdapter(InraHeader.dNDF),
            new FeedDetailInEditionAdapter(InraHeader.ADF),
            new FeedDetailInEditionAdapter(InraHeader.ADL),
            new FeedDetailInEditionAdapter(InraHeader.Amidon),	
            new FeedDetailInEditionAdapter(InraHeader.AG),	
            new FeedDetailInEditionAdapter(InraHeader.EE),
            new FeedDetailInEditionAdapter(InraHeader.P),	
            new FeedDetailInEditionAdapter(InraHeader.Pabs),	
            new FeedDetailInEditionAdapter(InraHeader.Ca),
            new FeedDetailInEditionAdapter(InraHeader.Caabs),	
            new FeedDetailInEditionAdapter(InraHeader.Mg),
            new FeedDetailInEditionAdapter(InraHeader.BE),	
            new FeedDetailInEditionAdapter(InraHeader.EB),
            new FeedDetailInEditionAdapter(InraHeader.dE),
            new FeedDetailInEditionAdapter(InraHeader.EM),
            new FeedDetailInEditionAdapter(InraHeader.DT_N),
            new FeedDetailInEditionAdapter(InraHeader.DT6_N),
            new FeedDetailInEditionAdapter(InraHeader.dr_N),	
            new FeedDetailInEditionAdapter(InraHeader.DT_Ami),	
            new FeedDetailInEditionAdapter(InraHeader.DT6_Ami),
            new FeedDetailInEditionAdapter(InraHeader.DT_MS),
            new FeedDetailInEditionAdapter(InraHeader.DT6_MS)
        };
    }

        public static bool FilterAvailableHeaders(object item, HashSet<InraHeader> presentFeeds)
        {
            return item is HeaderAdapter headerAdapter && !presentFeeds.Contains(headerAdapter.HeaderKind);
        }
}