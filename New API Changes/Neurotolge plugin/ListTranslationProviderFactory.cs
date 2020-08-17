using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;


namespace TartuNLP
{
    [TranslationProviderFactory(Id = "Neurotolge_Translation_Provider_Factory",
                                Name = "Neurotolge_Translation_Provider_Factory",
                                Description = "Neurotolge Translation Provider Factory")]
    class ListTranslationProviderFactory : ITranslationProviderFactory
    {
        #region ITranslationProviderFactory Members

        public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            if (!SupportsTranslationProviderUri(translationProviderUri))
            {
                throw new Exception("Cannot handle URI.");
            }

            ListTranslationProvider tp = new ListTranslationProvider(new ListTranslationOptions(translationProviderUri));

            return tp;           
        }

        public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderInfo info = new TranslationProviderInfo();

            #region "TranslationMethod"
            info.TranslationMethod = ListTranslationOptions.ProviderTranslationMethod;
            #endregion

            #region "Name"
            info.Name = PluginResources.Plugin_NiceName;
            #endregion

            return info;
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException("Translation provider URI not supported.");
            }

            return String.Equals(translationProviderUri.Scheme, ListTranslationProvider.ListTranslationProviderScheme, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
