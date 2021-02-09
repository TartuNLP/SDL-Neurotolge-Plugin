using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TartuNLP
{
   
    public partial class TartuNLPConfigForm : Form
    {
        public TartuNLPOptions Options { get; set; }

        private class LanguageDomainSupport
        { 
            public EngineConf EngineConf;
            public bool UpdateSuccessful;
            public Dictionary<string, string> SupportedDomains;
            public Dictionary<string, Dictionary<string, List<string>>> SupportedLanguages;
            public bool FormattingAndTagUsage;
            public Exception Exception;
        }

        private LanguageDomainSupport languageDomainSupport;
        public TartuNLPConfigForm(TartuNLPOptions options)
        {
            Options = options;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnUpdate.Enabled = true;
            btnOK.Enabled = false;
            cbDomain.Items.Clear();
            if (Options.EngineConf != null && Options.URL != null && Options.Auth != null)
            {
                tbURL.Text = Options.URL;
                tbAuth.Text = Options.Auth;
                languageDomainSupport = loadEngineConf(Options.EngineConf);
                cbDomain.Items.Clear();
                foreach (var domainName in languageDomainSupport.SupportedDomains.Keys)
                {
                    cbDomain.Items.Add(domainName);
                }
                cbDomain.SelectedItem = Options.SelectedDomainName;
                btnOK.Enabled = true;
            }
        }
        
        private void tbURLAuth_TextChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = true;
            btnOK.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            srcLanguages.Items.Clear();
            tgtLanguages.Items.Clear();
            
            languageDomainSupport = UpdateConfig(tbURL.Text, tbAuth.Text);
            HandleUpdateFinished();
        }

        private LanguageDomainSupport loadEngineConf(EngineConf engineConf)
        {
            var config = new LanguageDomainSupport
            {
                EngineConf = engineConf,
                UpdateSuccessful = true,
                FormattingAndTagUsage = engineConf.xml_support,
                SupportedDomains = new Dictionary<string, string>(),
                SupportedLanguages = new Dictionary<string, Dictionary<string, List<string>>>()
            };
            
            foreach (var domain in engineConf.domains)
            {
                config.SupportedDomains.Add(domain.name, domain.code);
                config.SupportedLanguages.Add(domain.code, new Dictionary<string, List<string>>());
                foreach (var language in domain.languages)
                {
                    var languagePair = language.Split('-');
                    if (!config.SupportedLanguages[domain.code].ContainsKey(languagePair[0]))
                    {
                        config.SupportedLanguages[domain.code].Add(languagePair[0], new List<string>());
                    }

                    config.SupportedLanguages[domain.code][languagePair[0]].Add(languagePair[1]);
                }
            }

            return config;
        }

        
        private LanguageDomainSupport UpdateConfig(string url, string auth)
        {
            var config = new LanguageDomainSupport();
            try
            {
                // try to get Configuration
                // Do not call any blocking service in the user interface thread; it has to use background threads.
                var engineConf = TartuNLPConnector.GetConfig(url, auth);
                
                if (engineConf == null)
                {
                    //invalid user name or password
                    config.UpdateSuccessful = false;
                }
                else
                {
                    config = loadEngineConf(engineConf);
                }
            }
            catch (Exception ex)
            {
                config.Exception = ex;
            }

            return config;
        }

        private void HandleUpdateFinished()
        {
            // it is possible that the form has disposed during the background operation (e.g. the user clicked on the cancel button)
            if (!IsDisposed)
            {
                if (languageDomainSupport.Exception != null)
                {
                    // there was an error, display for the user
                    var caption = "Communication Error";
                    var text = "There was an error during the communication with the service. Please check the URL and authentication token or try again.";
                    MessageBox.Show(this, string.Format(text, languageDomainSupport.Exception.Message), caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!languageDomainSupport.UpdateSuccessful)
                {
                    // the URL or Auth is invalid, display for the user
                    var caption = "Invalid URL or Auth";
                    var text = "Invalid URL or Authentication token";
                    MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // we have managed to get the supported Domain, display them in the combo box
                    cbDomain.Items.Clear();
                    foreach (var domain in languageDomainSupport.SupportedDomains.Keys)
                    {
                        cbDomain.Items.Add(domain);
                    }
                    cbDomain.SelectedIndex = 0;

                }
            }
        }

        private void cbDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            srcLanguages.Items.Clear();
            if (languageDomainSupport != null)
            {
                var domain = languageDomainSupport.SupportedDomains[cbDomain.SelectedItem.ToString()];
                srcLanguages.Items.Clear();
                foreach (var language in languageDomainSupport.SupportedLanguages[domain])
                {
                    srcLanguages.Items.Add(language.Key);
                }
                srcLanguages.SelectedIndex = 0;
                
                btnOK.Enabled = languageDomainSupport.SupportedLanguages[domain].Count > 0;
            }
        }

        private void srcLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            tgtLanguages.Items.Clear();
            if (languageDomainSupport != null)
            {
                var domain = languageDomainSupport.SupportedDomains[cbDomain.SelectedItem.ToString()];
                tgtLanguages.Items.Clear();
                foreach (var language in languageDomainSupport.SupportedLanguages[domain][srcLanguages.SelectedItem.ToString()])
                {
                    tgtLanguages.Items.Add(language);
                }
            }
        }

        private void TartuNLPOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                // if there was a modification, we have to save the changes
                Options.URL = tbURL.Text;
                Options.Auth = tbAuth.Text;
                if (languageDomainSupport != null)
                {
                    var domain = languageDomainSupport.SupportedDomains[cbDomain.SelectedItem.ToString()];
                    Options.EngineConf = languageDomainSupport.EngineConf;
                    var languagePairs = new List<(string, string)>();
                    foreach (var languagePair in languageDomainSupport.SupportedLanguages[domain])
                    {
                        foreach (var targetLang in languagePair.Value)
                        {
                            languagePairs.Add((languagePair.Key, targetLang));
                        }
                    }
                    Options.SupportedLanguages = languagePairs.ToArray();
                    Options.SelectedDomainCode = domain;
                    Options.SelectedDomainName = cbDomain.SelectedItem.ToString();
                    Options.FormattingAndTagUsage = languageDomainSupport.FormattingAndTagUsage;
                }
            }
            srcLanguages.Items.Clear();
            tgtLanguages.Items.Clear();
            cbDomain.Items.Clear();
        }
    }
}
