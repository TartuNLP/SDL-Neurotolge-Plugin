using System;
using System.Collections.Generic;
using System.Linq;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TartuNLP
{
    class TartuNLPProviderLanguageDirection : ITranslationProviderLanguageDirection
    {
        #region "PrivateMembers"
        private readonly TartuNLPProvider _provider;
        private readonly LanguagePair _languageDirection;
        private readonly TartuNLPOptions _options;
        private TartuNLPConnector _tartuNLPConnector;
        #endregion

        public TartuNLPProviderLanguageDirection(TartuNLPProvider provider, LanguagePair languages)
        {
            _provider = provider;
            _languageDirection = languages;
            _options = _provider.Options;
        }

        #region ITranslationProviderLanguageDirection Members
        
        private SearchResult CreateSearchResult(Segment segment, TartuNLPTagPlacer tagPlacer, string translation)
        {
            var targetSegment = tagPlacer.GetTaggedSegment(translation);
            
            var translationUnit = new TranslationUnit
                {
                    SourceSegment = segment.Duplicate(),
                    TargetSegment = targetSegment
                };

            translationUnit.ResourceId = new PersistentObjectToken(translationUnit.GetHashCode(), Guid.Empty);
            translationUnit.Origin = TranslationUnitOrigin.MachineTranslation;

            var searchResult = new SearchResult(translationUnit) {ScoringResult = new ScoringResult()};

            return searchResult;
        }
        
        private List<string> SearchInServer(List<string> sourceStrings)
        {
            if (_tartuNLPConnector == null)
            {
                // Use basic connection settings
                var url = _options.URL;
                var auth = _options.Auth;
                var domain = _options.SelectedDomainCode;
                _tartuNLPConnector = new TartuNLPConnector(url, auth, domain);
            }
            return _tartuNLPConnector.GetTranslation(_languageDirection, sourceStrings);
        }

        public SearchResults SearchSegment(SearchSettings settings, Segment segment)
        {
            Segment[] segments = {segment};
            return SearchSegments(settings, segments)[0];
        }

        public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
        {
            return SearchSegmentsMasked(settings, segments, null);
        }

        public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
        {
            if (segments == null)
            {
                throw new ArgumentNullException(nameof(segments), PluginResources.TartuNLPProviderLanguageDirection_SearchSegmentsMasked_segments_in_SearchSegmentsMasked);
            }
            if (mask != null && mask.Length != segments.Length)
            {
                throw new ArgumentException("mask in SearchSegmentsMasked");
            }

            var tagPlacers = segments
                .Where((segment, i) => mask == null || mask[i])
                .Select(segment => new TartuNLPTagPlacer(segment, _options.FormattingAndTagUsage)).ToList();
            
            var results = new SearchResults[segments.Length];
            
            var sourceStrings = tagPlacers
                .Select(tagPlacer => tagPlacer.PreparedSourceText).ToList();
            
            var translations = SearchInServer(sourceStrings);
            var j = 0;
            for (var i = 0; i < segments.Length; i++)
            {
                if (mask == null || mask[i])
                {
                    var source = segments[i].Duplicate();

                    var searchResults = new SearchResults {SourceSegment = source};
                    searchResults.Add(CreateSearchResult(source, tagPlacers[j], translations[j]));
                    
                    results[i]=(searchResults);
                    j++;
                }
                else
                {
                    results[i]=(null);
                }
            }
            
            return results;
        }

        public SearchResults SearchText(SearchSettings settings, string segment)
        {
            var s = new Segment(_languageDirection.SourceCulture);
            s.Add(segment);
            return SearchSegment(settings, s);
        }

        public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
        {
            return SearchSegment(settings, translationUnit.SourceSegment);
        }

        public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
        {
            return SearchTranslationUnitsMasked(settings, translationUnits, null);
        }

        public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
        {
            var segments = translationUnits.Select(tu => tu.SourceSegment).ToArray();
            return SearchSegmentsMasked(settings, segments, mask);
        }
        
        public bool CanReverseLanguageDirection
        {
            get
            {
                var source = _languageDirection.SourceCulture.ThreeLetterISOLanguageName;
                var target = _languageDirection.SourceCulture.ThreeLetterISOLanguageName;
                // The API does not use standard ISO codes for German.
                source = source == "deu" ? "ger" : source;
                target = target == "deu" ? "ger" : target;
                return (_provider.Options.SupportedLanguages.Contains((target, source)));
            }
        }

        public System.Globalization.CultureInfo SourceLanguage => _languageDirection.SourceCulture;

        public System.Globalization.CultureInfo TargetLanguage => _languageDirection.TargetCulture;

        public ITranslationProvider TranslationProvider => _provider;

        public ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)
        {
            throw new NotImplementedException();
        }

        public ImportResult[] AddOrUpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, bool[] mask)
        {
            throw new NotImplementedException();
        }

        public ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)
        {
            throw new NotImplementedException();
        }

        public ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)
        {
            throw new NotImplementedException();
        }

        public ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask)
        {
            throw new NotImplementedException();
        }

        public ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)
        {
            throw new NotImplementedException();
        }

        public ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
