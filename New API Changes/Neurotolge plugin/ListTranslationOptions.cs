using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TartuNLP
{
    public class ListTranslationOptions
    {
        #region "TranslationMethod"
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.MachineTranslation;
        #endregion

        TranslationProviderUriBuilder _uriBuilder;

        public ListTranslationOptions()
        {
            _uriBuilder = new TranslationProviderUriBuilder(ListTranslationProvider.ListTranslationProviderScheme);
        }

        public ListTranslationOptions(Uri uri)
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

        public string languageDirection
        {
            get { return GetStringParameter("languageDirection"); }
            set { SetStringParameter("languageDirection", value); }
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

        public string languageDirectionSource
        {
            get { return GetStringParameter("languageDirectionSource"); }
            set { SetStringParameter("languageDirectionSource", value); }
        }

        public string languageDirectionTarget
        {
            get { return GetStringParameter("languageDirectionTarget"); }
            set { SetStringParameter("languageDirectionTarget", value); }
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
