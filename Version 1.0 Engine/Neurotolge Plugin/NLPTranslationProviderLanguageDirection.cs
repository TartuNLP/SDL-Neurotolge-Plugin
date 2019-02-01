using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.Web;
using System.Threading.Tasks;
using Neurotolge_Plugin.Model;

namespace Neurotolge_Plugin
{
    class NLPTranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
    {
        #region "PrivateMembers"
        private NLPTranslationProvider _provider;
        private LanguagePair _languageDirection;
        private NLPTranslationOptions _options;
        private TranslationUnit _inputTu;
        private NLPTranslationProviderElementVisitor _visitor;
        private NLPConnector _neurotolgeConnector;
        #endregion

        public NLPTranslationProviderLanguageDirection(NLPTranslationProvider provider, LanguagePair languages)
        {
            _provider = provider;
            _languageDirection = languages;
            _options = _provider.Options;
            _visitor = new NLPTranslationProviderElementVisitor(_options);
        }

        #region ITranslationProviderLanguageDirection Members

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

        public bool CanReverseLanguageDirection
        {
            get { throw new NotImplementedException(); }
        }

        private SearchResult CreateSearchResult(Segment segment, Segment translation)
        {
            TranslationUnit tu = new TranslationUnit();
            tu.SourceSegment = segment.Duplicate();
            tu.TargetSegment = translation;

            tu.ResourceId = new PersistentObjectToken(tu.GetHashCode(), Guid.Empty);
            tu.Origin = TranslationUnitOrigin.MachineTranslation;

            SearchResult searchResult = new SearchResult(tu);
            searchResult.ScoringResult = new ScoringResult();

            return searchResult;
        }

        public SearchResults SearchSegment(SearchSettings settings, Segment segment)
        {
            _visitor.Reset();
            foreach (var element in segment.Elements)
            {
                element.AcceptSegmentElementVisitor(_visitor);
            }

            SearchResults results = new SearchResults();
            results.SourceSegment = segment.Duplicate();

            string sourceText = _visitor.PlainText;

            //Get the translation from the server
            string translatedSentence = searchInServer(sourceText);
            //string translatedSentence = "test";

            if (String.IsNullOrEmpty(translatedSentence))
                return results;

            // Look up the currently selected segment in the collection (normal segment lookup).
            if (settings.Mode == SearchMode.FullSearch)
            {
                Segment translation = new Segment(_languageDirection.TargetCulture);
                translation.Add(translatedSentence);
                results.Add(CreateSearchResult(segment, translation));
            }

            if (settings.Mode == SearchMode.NormalSearch)
            {
                Segment translation = new Segment(_languageDirection.TargetCulture);
                translation.Add(translatedSentence);
                results.Add(CreateSearchResult(segment, translation));
            }

            return results;
        }

        private string searchInServer(String sourceString)
        {
            if (_neurotolgeConnector == null)
            {
                // Use basic connection settings
                string serverAddress = _options.serverAddress;
                int serverPort = int.Parse(_options.port);

                // Use features
                string client = _options.client;
                string subject = _options.subject;
                List<string> features = new List<string>();

                if (!String.IsNullOrEmpty(client))
                {
                    features.Add(client);
                }

                if (!String.IsNullOrEmpty(subject))
                {
                    features.Add(subject);
                }

                if (!String.IsNullOrEmpty(_options.otherFeatures))
                {
                    features.AddRange(_options.otherFeatures.Split(';'));
                }

                _neurotolgeConnector = new NLPConnector(serverAddress, serverPort, _languageDirection, features);
            }

            var translatedText = _neurotolgeConnector.getTranslation(_languageDirection, sourceString);

            return translatedText;
        }

        public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
        {
            throw new NotImplementedException();
        }

        public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
        {
            throw new NotImplementedException();
        }

        public SearchResults SearchText(SearchSettings settings, string segment)
        {
            throw new NotImplementedException();
        }

        public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
        {
            _inputTu = translationUnit;
            return SearchSegment(settings, translationUnit.SourceSegment);
        }

        public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
        {
            throw new NotImplementedException();
        }

        public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
        {
            var noOfResults = mask.Length;
            List<SearchResults> results = new List<SearchResults>();
            List<PreTranslateSegment> preTranslateList = new List<PreTranslateSegment>();

            // plugin is called from pre-translate batch task 
            // we receive the data in chunk of 10 segments
            if (translationUnits.Length > 2)
            {
                var i = 0;
                foreach (var tu in translationUnits)
                {
                    if (mask == null || mask[i])
                    {
                        var preTranslate = new PreTranslateSegment
                        {
                            SearchSettings = settings,
                            TranslationUnit = tu
                        };
                        preTranslateList.Add(preTranslate);
                    }
                    else
                    {
                        results.Add(null);
                    }
                    i++;
                }

                if (preTranslateList.Count > 0)
                {
                    // Create temp file with translations
                    var translatedSegments = PrepareTempData(preTranslateList).Result;
                    var preTranslateSearchResults = GetPreTranslationSearchResults(translatedSegments);
                    results.AddRange(preTranslateSearchResults);
                }
            }
            else
            {
                var i = 0;
                foreach (var tu in translationUnits)
                {
                    if (mask == null || mask[i])
                    {
                        var result = SearchTranslationUnit(settings, tu);
                        results.Add(result);
                    }
                    else
                    {
                        results.Add(null);
                    }
                    i++;
                }
            }

            return results.ToArray();
        }

        private async Task<List<PreTranslateSegment>> PrepareTempData(List<PreTranslateSegment> preTranslateSegments)
        {
            try
            {
                string sourceString = String.Empty;
                for (var i = 0; i < preTranslateSegments.Count; i++)
                {
                    var newSeg = preTranslateSegments[i].TranslationUnit.SourceSegment.Duplicate();
                    string sourceText = newSeg.ToPlain();

                    preTranslateSegments[i].SourceText = sourceText;

                    //sourceString += sourceText + "|";
                }

                // Use basic connection settings
                string serverAddress = _options.serverAddress;
                int serverPort = int.Parse(_options.port);

                // Use features
                string client = _options.client;
                string subject = _options.subject;
                List<string> features = new List<string>();

                if (!String.IsNullOrEmpty(client))
                {
                    features.Add(client);
                }

                if (!String.IsNullOrEmpty(subject))
                {
                    features.Add(subject);
                }

                if (!String.IsNullOrEmpty(_options.otherFeatures))
                {
                    features.AddRange(_options.otherFeatures.Split(';'));
                }
                var translator = new NLPConnector(serverAddress, serverPort, _languageDirection, features);

                await Task.Run(() => Parallel.ForEach(preTranslateSegments, segment =>
                {
                    var translation = HttpUtility.UrlDecode(translator.getTranslation(_languageDirection, segment.SourceText));
                    segment.PlainTranslation = HttpUtility.HtmlDecode(translation);
                })).ConfigureAwait(true);

                return preTranslateSegments;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return preTranslateSegments;
        }

        public List<SearchResults> GetPreTranslationSearchResults(List<PreTranslateSegment> preTranslateList)
        {
            var resultsList = new List<SearchResults>();
            foreach (var preTranslate in preTranslateList)
            {
                var translation = new Segment(_languageDirection.TargetCulture);
                var newSeg = preTranslate.TranslationUnit.SourceSegment.Duplicate();

                translation.Add(preTranslate.PlainTranslation);

                var searchResult = CreateSearchResult(newSeg, translation);
                var results = new SearchResults
                {
                    SourceSegment = newSeg
                };
                results.Add(searchResult);
                resultsList.Add(results);
            }

            return resultsList;
        }

        public System.Globalization.CultureInfo SourceLanguage
        {
            get { return _languageDirection.SourceCulture; }
        }

        public System.Globalization.CultureInfo TargetLanguage
        {
            get { return _languageDirection.TargetCulture; }
        }

        public ITranslationProvider TranslationProvider
        {
            get { return _provider; }
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
