using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Neurotolge_plugin.Model;
using System.Threading.Tasks;
using System.Web;

namespace Neurotolge_plugin
{
    class ListTranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
    {
        #region "PrivateMembers"
        private ListTranslationProvider _provider;
        private LanguagePair _languageDirection;
        private ListTranslationOptions _options;
        private TranslationUnit _inputTu;
        private ListTranslationProviderElementVisitor _visitor;
        private NeurotolgeConnector _neurotolgeConnector;
        #endregion

        public ListTranslationProviderLanguageDirection(ListTranslationProvider provider, LanguagePair languages)
        {
            _provider = provider;
            _languageDirection = languages;
            _options = _provider.Options;
            _visitor = new ListTranslationProviderElementVisitor(_options);
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

                _neurotolgeConnector = new NeurotolgeConnector(serverAddress, serverPort, features);
            }

            var translatedText = _neurotolgeConnector.getTranslation(_languageDirection, sourceString);

            return translatedText;
        }

        public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
        {
            SearchResults[] results = new SearchResults[segments.Length];
            for (int p = 0; p < segments.Length; ++p)
            {
                results[p] = SearchSegment(settings, segments[p]);
            }
            return results;
        }

        public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
        {
            if (segments == null)
            {
                throw new ArgumentNullException("segments in SearchSegmentsMasked");
            }
            if (mask == null || mask.Length != segments.Length)
            {
                throw new ArgumentException("mask in SearchSegmentsMasked");
            }

            SearchResults[] results = new SearchResults[segments.Length];
            for (int p = 0; p < segments.Length; ++p)
            {
                if (mask[p])
                {
                    results[p] = SearchSegment(settings, segments[p]);
                }
                else
                {
                    results[p] = null;
                }
            }
            return results;
        }

        public SearchResults SearchText(SearchSettings settings, string segment)
        {
            Segment s = new Segment(_languageDirection.SourceCulture);
            s.Add(segment);
            return SearchSegment(settings, s);
        }

        public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
        {
            _inputTu = translationUnit;
            return SearchSegment(settings, translationUnit.SourceSegment);
        }

        public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
        {
            SearchResults[] results = new SearchResults[translationUnits.Length];
            for (int p = 0; p < translationUnits.Length; ++p)
            {
                results[p] = SearchSegment(settings, translationUnits[p].SourceSegment);
            }
            return results;
        }

        public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
        {
            List<SearchResults> results = new List<SearchResults>();
            string sourceString = String.Empty;
            int stringLength = 0;

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
            
            if (_neurotolgeConnector == null)
            {
                var translator = new NeurotolgeConnector(serverAddress, serverPort, features);
                _neurotolgeConnector = translator;
            }

            var sourceStrings = new List<String>();
            for (var i = 0; i < translationUnits.Length; i++)
            {
                var tu = translationUnits[i];
                if (mask == null || mask[i])
                {
                    // There are problems if the text is too long
                    if (stringLength < 700)
                    {
                        if (i == translationUnits.Length - 1)
                        {
                            sourceString += tu.SourceSegment.ToPlain();
                            stringLength = sourceString.Length;
                            sourceStrings.Add(sourceString);
                        }
                        else
                        {
                            sourceString += tu.SourceSegment.ToPlain() + "|";
                            stringLength = sourceString.Length;
                        }
                    }
                    else
                    {
                        sourceString = sourceString.Remove(sourceString.Length - 1);
                        sourceStrings.Add(sourceString);
                        sourceString = String.Empty;

                        if (i == translationUnits.Length - 1)
                        {
                            sourceString += tu.SourceSegment.ToPlain();
                            stringLength = sourceString.Length;
                            sourceStrings.Add(sourceString);
                        }
                        else
                        {
                            sourceString += tu.SourceSegment.ToPlain() + "|";
                            stringLength = sourceString.Length;
                        }
                    }

                }
                else
                {
                    results.Add(null);
                }
            }

            foreach (var source in sourceStrings)
            {
                var translations = _neurotolgeConnector.getTranslation(_languageDirection, source);
                string[] translationsSplit = translations.Split('|');
                for(var j = 0; j < translationsSplit.Length; j++)
                {
                    var translation = new Segment(_languageDirection.TargetCulture);
                    var newSeg = translationUnits[j].SourceSegment.Duplicate();

                    translation.Add(translationsSplit[j]);

                    var searchResult = CreateSearchResult(newSeg, translation);
                    var results_small = new SearchResults
                    {
                        SourceSegment = newSeg
                    };
                    results_small.Add(searchResult);
                    results.Add(results_small);

                }
            }

            return results.ToArray();
        }

        private async Task<List<PreTranslateSegment>> PrepareTempData(List<PreTranslateSegment> preTranslateSegments)
        {
            try
            {
                string sourceString = String.Empty;
                for(var i = 0; i < preTranslateSegments.Count; i++)
                {
                    var newSeg = preTranslateSegments[i].TranslationUnit.SourceSegment.Duplicate();
                    string sourceText = newSeg.ToPlain();

                    preTranslateSegments[i].SourceText = sourceText;

                    if (i == preTranslateSegments.Count - 1)
                    {
                        sourceString += sourceText;
                    }
                    else
                    {
                        sourceString += sourceText + "|";
                    }
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
                var translator = new NeurotolgeConnector(serverAddress, serverPort, features);

                //await Task.Run(() => Parallel.ForEach(preTranslateSegments, segment =>
                //{
                //    var translation = HttpUtility.UrlDecode(translator.getTranslation(_languageDirection, segment.SourceText));
                //    segment.PlainTranslation = HttpUtility.HtmlDecode(translation);
                //})).ConfigureAwait(true);

                //await Task.Run(() =>
                //{
                //    var translations = translator.getTranslation(_languageDirection, sourceString);
                //    string[] substrings = translations.Split('|');

                //}).ConfigureAwait(true);
                var translations = translator.getTranslation(_languageDirection, sourceString);
                string[] translationsSplit = translations.Split('|');
                if (translationsSplit.Length == preTranslateSegments.Count)
                {
                    for (var i = 0; i < preTranslateSegments.Count; i++)
                    {
                        preTranslateSegments[i].PlainTranslation = translationsSplit[i];
                    }
                }
                else
                {
                    Console.Write("error in searchtranslationunits. The lengths are not the same.");
                }

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
