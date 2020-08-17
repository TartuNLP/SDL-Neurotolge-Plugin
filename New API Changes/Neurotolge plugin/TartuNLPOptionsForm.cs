using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TartuNLP
{
   
    public partial class TartuNLPConfigForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32
           SendMessage(
                           IntPtr hWnd,
                           int msg,
                           int wParam,
                           [MarshalAs(UnmanagedType.LPWStr)]string lParam
                       );

        private const int EM_SETCUEBANNER = 0x1501;


        public ListTranslationOptions Options

        {

            get;

            set;

        }

        private class LanguageDomainSupport
        {
            public string URL;
            public string Auth;
            public bool UpdateSuccessful;
            public IDictionary<string, string[]> SupportedLanguages;
            public IDictionary<string, string> SupportedDomains;
            public IDictionary<string, string[]> JSON;
            public Exception Exception;
        }

        private LanguageDomainSupport languageDomainSupport;

        public TartuNLPConfigForm(ListTranslationOptions options, Sdl.LanguagePlatform.Core.LanguagePair[] languagePairs)
        {
            string sSourceCulture = languagePairs[0].SourceCultureName.ToLower();
            string sTargetCulture = languagePairs[0].TargetCultureName.ToLower();
            Options = options;
            InitializeComponent();
            SendMessage(tbURL.Handle, EM_SETCUEBANNER, 0, "http://193.40.33.51/v1.2/translate");
            SendMessage(tbAuth.Handle, EM_SETCUEBANNER, 0, "briskywombatflightluisa");
            tbURL.Text = "http://193.40.33.51/v1.2/translate";
            tbAuth.Text = "briskywombatflightluisa";
           
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Options.Auth != null)
            {
                tbURL.Text = Options.URL;
                tbAuth.Text = Options.Auth;
                lbLanguages.Items.Clear();
                this.btnOK.Enabled = !string.IsNullOrEmpty(Options.URL);
                cbDomains.Items.Clear();
                IDictionary<string, string> domains = new Dictionary<string, string>();
                if (Options.domains != null)
                {
                    string[] doms = Options.domains.Split(';');
                    foreach (string dom in doms)
                    {
                        string[] domain = dom.Split('|');
                        domains.Add(domain[1].Trim(), domain[0].Trim());
                        cbDomains.Items.Add(domain[0].Trim());
                    }
                }

                if (Options.selectedDomain != null)
                    cbDomains.SelectedItem = domains[Options.selectedDomain];

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
            lbLanguages.Items.Clear();

            // do the update in the background
            languageDomainSupport = updateConfig(tbURL.Text, tbAuth.Text);
            handleUpdateFinished();
        }

        private LanguageDomainSupport updateConfig(string url, string auth)
        {
            var languageDomainSupport = new LanguageDomainSupport()
            {
                URL = url,
                Auth = auth
            };
            try
            {
                // try to get Configuration
                // Do not call any blocking service in the user interface thread; it has to use background threads.
                languageDomainSupport.JSON = NeurotolgeConnector.getConfig(url, auth);

                if (languageDomainSupport.JSON == null)
                {
                    //invalid user name or password
                    languageDomainSupport.UpdateSuccessful = false;
                }
                else
                {
                    //successful login
                    languageDomainSupport.UpdateSuccessful = true;

                    //try to get the list of the Domain in the background
                    languageDomainSupport.SupportedDomains = new Dictionary<string, string>();
                    languageDomainSupport.SupportedLanguages = new Dictionary<string, string[]>();
                    foreach (string key in languageDomainSupport.JSON.Keys)
                    {
                        languageDomainSupport.SupportedDomains.Add(languageDomainSupport.JSON[key][0], key);
                        languageDomainSupport.SupportedLanguages.Add(languageDomainSupport.JSON[key][0], languageDomainSupport.JSON[key]);
                    }
                }
            }
            catch (Exception ex)
            {
                languageDomainSupport.Exception = ex;
            }

            return languageDomainSupport;
        }

        private void handleUpdateFinished()
        {
            // it is possible that the form has disposed during the background operation (e.g. the user clicked on the cancel button)
            if (!IsDisposed)
            {
                if (languageDomainSupport.Exception != null)
                {
                    // there was an error, display for the user
                    string caption = "CommunicationErrorCaption";
                    string text = "CommunicationErrorText";
                    MessageBox.Show(this, string.Format(text, languageDomainSupport.Exception.Message), caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!languageDomainSupport.UpdateSuccessful)
                {
                    // the URL or Auth is invalid, display for the user
                    string caption = "InvalidURLorAuthCaption";
                    string text = "InvalidURLorAuthText";
                    MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // we have managed to get the supported Domain, display them in the combo box
                    cbDomains.Items.Clear();
                    foreach (string dom in languageDomainSupport.SupportedDomains.Keys)
                    {
                        cbDomains.Items.Add(dom);
                    }
                    cbDomains.SelectedIndex = 0;

                }
            }
        }

        private void cbDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (languageDomainSupport != null)
            {

                lbLanguages.Items.Clear();
                foreach (string language in languageDomainSupport.SupportedLanguages[this.cbDomains.SelectedItem.ToString()])
                {
                    lbLanguages.Items.Add(language);
                }
                lbLanguages.Items.RemoveAt(0);

                btnOK.Enabled = languageDomainSupport.SupportedLanguages[this.cbDomains.SelectedItem.ToString()].Length > 0;
            }
            else
            {
                lbLanguages.Items.Clear();
                IDictionary<string, string[]> supportedLanguages = new Dictionary<string, string[]>();
                if (Options.supportedLanguages != null) {
                    string[] supportedLangs = Options.supportedLanguages.Split(';');
                    foreach (string languageList in supportedLangs)
                    {
                        string[] languages = languageList.Split('|');
                        supportedLanguages.Add(languages[0].Trim(), languages[1].Split(','));
                    }
                }

                foreach (string language in supportedLanguages[this.cbDomains.SelectedItem.ToString()])
                {
                    lbLanguages.Items.Add(language);
                }
                lbLanguages.Items.RemoveAt(0);

                btnOK.Enabled = supportedLanguages[this.cbDomains.SelectedItem.ToString()].Length > 0;
            }
        }


        private void TartuNLPOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // if there was a modification, we have to save the changes
                Options.URL = tbURL.Text;
                Options.Auth = tbAuth.Text;
                if (languageDomainSupport != null)
                {
                    int counter = 0;
                    string[] supportedLanguages = new string[languageDomainSupport.SupportedLanguages.Count];
                    foreach (KeyValuePair<string, string[]> langs in languageDomainSupport.SupportedLanguages)
                    {
                        supportedLanguages[counter] = langs.Key + "|" + String.Join(",", langs.Value);
                        counter++;
                    }
                    string supportedLangs = string.Join("; ", supportedLanguages);
                    
                    Options.supportedLanguages = supportedLangs;
                    string[] supportedDomains = new string[cbDomains.Items.Count];
                    int count = 0;
                    foreach (KeyValuePair<string, string> dom in languageDomainSupport.SupportedDomains)
                    {
                        supportedDomains[count] = dom.Key + "|" + dom.Value;
                        count++;
                    }
                    string supportedDoms = string.Join(";", supportedDomains);
                    Options.domains = supportedDoms;
                }
                IDictionary<string, string> domains = new Dictionary<string, string>();
                string[] doms = Options.domains.Split(';');
                foreach (string dom in doms)
                {
                    string[] domain = dom.Split('|');
                    domains.Add(domain[0], domain[1]);
                }
                Options.selectedDomain = domains[this.cbDomains.SelectedItem.ToString()];
                this.lbLanguages.Items.Clear();
            }
        }

       }
}
