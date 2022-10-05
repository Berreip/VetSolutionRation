using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.Utils.CoreComponents.Extensions;
using VSR.Core.Services;
using VSR.Dto;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Helpers;
using VSR.WPF.Utils.Services.Configuration;

namespace VSR.WPF.Utils.Services;

internal sealed class IngredientsFileLoader
{
    private readonly IIngredientsManager _ingredientsManager;
    private readonly DirectoryInfo _cacheFolder;
    private readonly object _key = new object();

    public IngredientsFileLoader(IConfigurationManager configurationManager, IIngredientsManager ingredientsManager)
    {
        _ingredientsManager = ingredientsManager;
        _cacheFolder = configurationManager.GetCacheDataFolder();
    }

    public void LoadInitialSavedFeeds()
    {
        lock (_key)
        {
            if (_cacheFolder.ExistsExplicit())
            {
                try
                {
                    // if we find the matching file
                    if (_cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, out var file))
                    {
                        var fileContent = DtoExporter.DeserializeFromJson(file.ReadAllText());
                        _ingredientsManager.AddIngredients(fileContent.ConvertFromDtoToIngredients());
                        _ingredientsManager.AddRecipes(fileContent.ConvertFromDtoToRecipeCandidates());
                    }
                }
                catch (Exception e)
                {
                    DebugCore.Fail($"Error while loading the file: {VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME}: {e}");
                }
            }

            // start listening changed after initial loading for updates
            _ingredientsManager.OnIngredientsChanged += OnIngredientsChanged;
            _ingredientsManager.OnRecipesChanged += OnRecipesChanged;
        }
    }

    private void OnIngredientsChanged(IIngredientsChangeMonitor monitor)
    {
        SaveShared(_ingredientsManager.GetAllIngredients(), _ingredientsManager.GetAllRecipes());
    }

    private void OnRecipesChanged(IRecipesChangeMonitor monitor)
    {
        SaveShared(_ingredientsManager.GetAllIngredients(), _ingredientsManager.GetAllRecipes());
    }

    private void SaveShared(IReadOnlyCollection<IIngredient> ingredients, IReadOnlyCollection<IRecipe> recipes)
    {
        lock (_key)
        {
            try
            {
                _cacheFolder.CreateIfNotExist();
                var jsonFeeds = DtoExporter
                    .ConvertFromModelsToDto(ingredients, recipes)
                    .SerializeReferenceToJson();

                File.WriteAllText(_cacheFolder.GetFile(VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME).FullName, jsonFeeds);
            }
            catch (Exception e)
            {
                Trace.TraceError($"error while trying to save new feeds: {e}");
                DebugCore.Fail($"error while trying to save new feeds: {e}");
            }
        }
    }
}