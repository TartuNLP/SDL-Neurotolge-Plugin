using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TartuNLP
{
    class ListTranslationProvider : ITranslationProvider
    {
        public static readonly string ListTranslationProviderScheme = "openlistprovider";

        #region "ListTranslationOptions"
        public ListTranslationOptions Options
        {
            get;
            set;
        }

        public ListTranslationProvider(ListTranslationOptions options)
        {
            Options = options;
        }
        #endregion

        #region ITranslationProvider Members

        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            return new ListTranslationProviderLanguageDirection(this, languageDirection);
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void LoadState(string translationProviderState)
        {
        }

        public string Name
        {
            get { return PluginResources.Plugin_NiceName; }
        }

        public void RefreshStatusInfo()
        {
        }

        public string SerializeState()
        {
            return null;
        }

        public ProviderStatusInfo StatusInfo
        {
            get { return new ProviderStatusInfo(true, PluginResources.Plugin_NiceName); }
        }

        public bool SupportsConcordanceSearch
        {
            get { return false; }
        }

        public bool SupportsDocumentSearches
        {
            get { return false; }
        }

        public bool SupportsFilters
        {
            get { return false; }
        }

        public bool SupportsFuzzySearch
        {
            get { return false; }
        }

        public bool SupportsLanguageDirection(LanguagePair languageDirection)
        {
            return true;
        }

        public bool SupportsMultipleResults
        {
            get { return false; }
        }

        public bool SupportsPenalties
        {
            get { return false; }
        }

        public bool SupportsPlaceables
        {
            get { return false; }
        }

        public bool SupportsScoring
        {
            get { return false; }
        }

        public bool SupportsSearchForTranslationUnits
        {
            get { return true; }
        }

        public bool SupportsSourceConcordanceSearch
        {
            get { return false; }
        }

        public bool SupportsStructureContext
        {
            get { return false; }
        }

        public bool SupportsTaggedInput
        {
            get { return false; }
        }

        public bool SupportsTargetConcordanceSearch
        {
            get { return false; }
        }

        public bool SupportsTranslation
        {
            get { return true; }
        }

        public bool SupportsUpdate
        {
            get { return false; }
        }

        public bool SupportsWordCounts
        {
            get { return false; }
        }

        public TranslationMethod TranslationMethod
        {
            get { return ListTranslationOptions.ProviderTranslationMethod; }
        }

        public Uri Uri
        {
            get { return Options.Uri; }
        }

        #endregion
    }
}

