using System;
using System.Collections.Generic;
using PRF.Utils.CoreComponents.Extensions;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters;
using VSR.WPF.Utils.Adapters.EditionIngredients;

namespace VetSolutionRation.wpf.Views.PopupDuplicatesAndEditFeed;

internal class FeedInEditionHelpers
{
    public static bool FilterDetailsParts(object item, string? search)
    {
        if (item is NutritionalDetailsAdapter feedInEditionAdapter)
        {
            if (feedInEditionAdapter.HeaderName.ContainsInsensitive(search))
            {
                return true;
            }
        }

        return false;
    }

    public static NutritionalDetailsAdapter[] GetDefaultHeaderForEdition(IIngredient ingredient, Action refreshValidity)
    {
        return new[]
        {
            CreateAdapter(ingredient, InraHeader.MS, refreshValidity),
            CreateAdapter(ingredient, InraHeader.UFL, refreshValidity),
            CreateAdapter(ingredient, InraHeader.UFV, refreshValidity),
            CreateAdapter(ingredient, InraHeader.PDIA, refreshValidity),
            CreateAdapter(ingredient, InraHeader.PDI, refreshValidity),
            CreateAdapter(ingredient, InraHeader.BPR, refreshValidity),
            CreateAdapter(ingredient, InraHeader.LysDI, refreshValidity),
            CreateAdapter(ingredient, InraHeader.MetDI, refreshValidity),
            CreateAdapter(ingredient, InraHeader.HisDI, refreshValidity),
            CreateAdapter(ingredient, InraHeader.bVEc, refreshValidity),
            CreateAdapter(ingredient, InraHeader.MO, refreshValidity),
            CreateAdapter(ingredient, InraHeader.dMO, refreshValidity),
            CreateAdapter(ingredient, InraHeader.MAT, refreshValidity),
            CreateAdapter(ingredient, InraHeader.dMA, refreshValidity),
            CreateAdapter(ingredient, InraHeader.CB, refreshValidity),
            CreateAdapter(ingredient, InraHeader.NDF, refreshValidity),
            CreateAdapter(ingredient, InraHeader.dNDF, refreshValidity),
            CreateAdapter(ingredient, InraHeader.ADF, refreshValidity),
            CreateAdapter(ingredient, InraHeader.ADL, refreshValidity),
            CreateAdapter(ingredient, InraHeader.Amidon, refreshValidity),
            CreateAdapter(ingredient, InraHeader.AG, refreshValidity),
            CreateAdapter(ingredient, InraHeader.EE, refreshValidity),
            CreateAdapter(ingredient, InraHeader.P, refreshValidity),
            CreateAdapter(ingredient, InraHeader.Pabs, refreshValidity),
            CreateAdapter(ingredient, InraHeader.Ca, refreshValidity),
            CreateAdapter(ingredient, InraHeader.Caabs, refreshValidity),
            CreateAdapter(ingredient, InraHeader.Mg, refreshValidity),
            CreateAdapter(ingredient, InraHeader.BE, refreshValidity),
            CreateAdapter(ingredient, InraHeader.EB, refreshValidity),
            CreateAdapter(ingredient, InraHeader.dE, refreshValidity),
            CreateAdapter(ingredient, InraHeader.EM, refreshValidity),
            CreateAdapter(ingredient, InraHeader.DT_N, refreshValidity),
            CreateAdapter(ingredient, InraHeader.DT6_N, refreshValidity),
            CreateAdapter(ingredient, InraHeader.dr_N, refreshValidity),
            CreateAdapter(ingredient, InraHeader.DT_Ami, refreshValidity),
            CreateAdapter(ingredient, InraHeader.DT6_Ami, refreshValidity),
            CreateAdapter(ingredient, InraHeader.DT_MS, refreshValidity),
            CreateAdapter(ingredient, InraHeader.DT6_MS, refreshValidity),
        };
    }

    private static NutritionalDetailsAdapter CreateAdapter(IIngredient feedWithValue, InraHeader inraHeader, Action refreshValidity)
    {
        var detailsValue = feedWithValue.TryGetNutritionDetail(inraHeader, out var nutritionalDetails) ? nutritionalDetails.Value : 0;
        return new NutritionalDetailsAdapter(inraHeader, detailsValue, refreshValidity);
    }

    public static bool FilterAvailableHeaders(object item, HashSet<InraHeader> presentFeeds)
    {
        return item is InraHeaderAdapter headerAdapter && !presentFeeds.Contains(headerAdapter.HeaderKind);
    }
}