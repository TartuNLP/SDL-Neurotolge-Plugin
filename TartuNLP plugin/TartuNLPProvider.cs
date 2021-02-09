using System;
using System.Linq;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TartuNLP
{
    class TartuNLPProvider : ITranslationProvider
    {
        public static readonly string ListTranslationProviderScheme = "openlistprovider";

        #region "ListTranslationOptions"
        public TartuNLPOptions Options
        {
            get;
            set;
        }

        public TartuNLPProvider(TartuNLPOptions options)
        {
            Options = options;
        }
        #endregion

        #region ITranslationProvider Members

        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            return new TartuNLPProviderLanguageDirection(this, languageDirection);
        }

        public bool IsReadOnly => true;

        public void LoadState(string translationProviderState)
        {
        }

        public string Name => PluginResources.Plugin_NiceName;

        public void RefreshStatusInfo(){}

        public string SerializeState() => null;

        public ProviderStatusInfo StatusInfo => new ProviderStatusInfo(true, PluginResources.Plugin_NiceName);

        public bool SupportsConcordanceSearch => false;

        public bool SupportsDocumentSearches => false;

        public bool SupportsFilters => false;

        public bool SupportsFuzzySearch => false;

        public bool SupportsLanguageDirection(LanguagePair languageDirection)
        {
            var source = languageDirection.SourceCulture.ThreeLetterISOLanguageName;
            var target = languageDirection.TargetCulture.ThreeLetterISOLanguageName;
            // The API does not use standard ISO codes for German.
            source = source == "deu" ? "ger" : source;
            target = target == "deu" ? "ger" : target;
            return (Options.SupportedLanguages.Contains((source, target)));
        }

        public bool SupportsMultipleResults => false;

        public bool SupportsPenalties => false;

        public bool SupportsPlaceables => Options.FormattingAndTagUsage;

        public bool SupportsScoring => false;

        public bool SupportsSearchForTranslationUnits => true;

        public bool SupportsSourceConcordanceSearch => false;

        public bool SupportsStructureContext => false;

        public bool SupportsTaggedInput => Options.FormattingAndTagUsage;

        public bool SupportsTargetConcordanceSearch => false;

        public bool SupportsTranslation => true;

        public bool SupportsUpdate => false;

        public bool SupportsWordCounts => false;

        public TranslationMethod TranslationMethod => TartuNLPOptions.ProviderTranslationMethod;

        public Uri Uri => Options.Uri;

        #endregion
    }
}

