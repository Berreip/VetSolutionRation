using System;
using System.Collections.Generic;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.wpf.Views.Adapter;
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

    public static FeedDetailInEditionAdapter[] GetDefaultHeaderForEdition(IFeedWithValue feedWithValue, Action refreshValidity)
    {
        return new[]
        {
            CreateAdapter(feedWithValue, InraHeader.MS, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.UFL, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.UFV, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.PDIA, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.PDI, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.BPR, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.LysDI, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.MetDI, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.HisDI, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.bVEc, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.MO, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.dMO, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.MAT, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.dMA, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.CB, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.NDF, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.dNDF, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.ADF, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.ADL, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.Amidon, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.AG, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.EE, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.P, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.Pabs, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.Ca, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.Caabs, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.Mg, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.BE, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.EB, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.dE, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.EM, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.DT_N, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.DT6_N, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.dr_N, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.DT_Ami, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.DT6_Ami, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.DT_MS, refreshValidity),
            CreateAdapter(feedWithValue, InraHeader.DT6_MS, refreshValidity),
        };
    }

    private static FeedDetailInEditionAdapter CreateAdapter(IFeedWithValue feedWithValue, InraHeader inraHeader, Action refreshValidity)
    {
        return new FeedDetailInEditionAdapter(inraHeader, feedWithValue.GetInraValue(inraHeader), refreshValidity);
    }

    public static bool FilterAvailableHeaders(object item, HashSet<InraHeader> presentFeeds)
    {
        return item is HeaderAdapter headerAdapter && !presentFeeds.Contains(headerAdapter.HeaderKind);
    }
}