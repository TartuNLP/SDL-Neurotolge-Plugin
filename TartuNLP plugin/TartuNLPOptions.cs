using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using Newtonsoft.Json;

namespace TartuNLP
{
    public class TartuNLPOptions
    {
        #region "TranslationMethod"
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.MachineTranslation;
        #endregion

        TranslationProviderUriBuilder _uriBuilder;

        public TartuNLPOptions()
        {
            _uriBuilder = new TranslationProviderUriBuilder(TartuNLPProvider.ListTranslationProviderScheme);
        }

        public TartuNLPOptions(Uri uri)
        {
            _uriBuilder = new TranslationProviderUriBuilder(uri);
        }

        public Uri Uri => _uriBuilder.Uri;

        public string URL
        {
            get => GetStringParameter("url");
            set => SetStringParameter("url", value);
        }

        public string Auth
        {
            get => GetStringParameter("Auth");
            set => SetStringParameter("Auth", value);
        }

        public string SelectedDomainCode
        {
            get => GetStringParameter("SelectedDomainCode");
            set => SetStringParameter("SelectedDomainCode", value);
        }
        public string SelectedDomainName
        {
            get => GetStringParameter("SelectedDomainName");
            set => SetStringParameter("SelectedDomainName", value);
        }

        public (string, string)[] SupportedLanguages
        {
            get
            {
                var supportedLanguagesPairs = GetStringParameter("SupportedLanguagesPairs");
                if (supportedLanguagesPairs != null)
                {
                    return JsonConvert.DeserializeObject<(string, string)[]>(supportedLanguagesPairs);
                }
                return null;
            }
            set => SetStringParameter("SupportedLanguagesPairs", JsonConvert.SerializeObject(value));
        }

        public bool FormattingAndTagUsage
        {
            get => Convert.ToBoolean(GetStringParameter("FormattingAndTagUsage"));
            set => SetStringParameter("FormattingAndTagUsage", value.ToString());
        }

        public EngineConf EngineConf
        {
            get
            {
                var engineConf = GetStringParameter("EngineConf");
                if (engineConf != null)
                {
                    return JsonConvert.DeserializeObject<EngineConf>(engineConf);
                }

                return null;
            }
            set => SetStringParameter("EngineConf", JsonConvert.SerializeObject(value));
        }

        #region "SetStringParameter"
        private void SetStringParameter(string p, string value)
        {
            _uriBuilder[p] = value;
        }
        #endregion

        #region "GetStringParameter"
        private string GetStringParameter(string p)
        {
            var paramString = _uriBuilder[p];
            return paramString;
        }
        #endregion


    }
}
