using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TartuNLP
{
    public class TartuNLPOptions // TODO revise how supported languages and domains are stored.
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

        public Uri Uri
        {
            get
            {
                return _uriBuilder.Uri;
            }
        }

        public string URL
        {
            get { return GetStringParameter("URL"); }
            set { SetStringParameter("URL", value); }
        }

        public string Auth
        {
            get { return GetStringParameter("auth"); }
            set { SetStringParameter("auth", value); }
        }

        public string selectedDomain
        {
            get { return GetStringParameter("selectdomain"); }
            set { SetStringParameter("selectdomain", value); }
        }

        public string supportedLanguages
        {
            get { return GetStringParameter("supportedLanguages"); }
            set { SetStringParameter("supportedLanguages", value); }
        }

        public string domains
        {
            get { return GetStringParameter("domains"); }
            set { SetStringParameter("domains", value); }
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
            string paramString = _uriBuilder[p];
            return paramString;
        }
        #endregion


    }
}
