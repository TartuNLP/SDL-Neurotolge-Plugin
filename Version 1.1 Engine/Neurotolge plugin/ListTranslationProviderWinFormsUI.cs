using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace Neurotolge_plugin
{
    [TranslationProviderWinFormsUi(Id = "Neurotolge_Translation_Provider_WinFormsUI",
                                   Name = "Neurotolge_Translation_Provider_WinFormsUI",
                                   Description = "Neurotolge Translation Provider WinFormsUI")]
    class ListTranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        #region ITranslationProviderWinFormsUI Members

        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            NeurotolgeConfDialog dialog = new NeurotolgeConfDialog(new ListTranslationOptions(), languagePairs);
            if (dialog.ShowDialog(owner) == DialogResult.OK)
            {
                ListTranslationProvider testProvider = new ListTranslationProvider(dialog.Options);
                return new ITranslationProvider[] { testProvider };
            }
            return null;
        }

        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            ListTranslationProvider editProvider = translationProvider as ListTranslationProvider;
            if (editProvider == null)
            {
                return false;
            }

            NeurotolgeConfDialog dialog = new NeurotolgeConfDialog(editProvider.Options, languagePairs);
            if (dialog.ShowDialog(owner) == DialogResult.OK)
            {
                editProvider.Options = dialog.Options;
                return true;
            }
            return false;
        }

        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            return true;
        }

        public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderDisplayInfo info = new TranslationProviderDisplayInfo();
            info.Name = PluginResources.Plugin_NiceName;
            info.TranslationProviderIcon = PluginResources.band_aid_icon;
            info.SearchResultImage = PluginResources.band_aid_symbol;

            return info;
        }

        public bool SupportsEditing
        {
            get { return true; }
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException("URI not supported by the plug-in.");
            }
            return String.Equals(translationProviderUri.Scheme, ListTranslationProvider.ListTranslationProviderScheme, StringComparison.CurrentCultureIgnoreCase);
        }

        public string TypeDescription
        {
            get { return PluginResources.Plugin_Description; }
        }

        public string TypeName
        {
            get { return PluginResources.Plugin_NiceName; }
        }

        #endregion
    }
}
