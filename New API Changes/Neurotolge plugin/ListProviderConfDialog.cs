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
    public partial class NeurotolgeConfDialog : Form
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

        public NeurotolgeConfDialog(ListTranslationOptions options, Sdl.LanguagePlatform.Core.LanguagePair[] languagePairs)
        {
            string sSourceCulture = languagePairs[0].SourceCultureName.ToLower();
            string sTargetCulture = languagePairs[0].TargetCultureName.ToLower();
            Options = options;
            InitializeComponent();
            SendMessage(address_txtbox.Handle, EM_SETCUEBANNER, 0, "api.neurotolge.ee/v1.1");
            SendMessage(port_txtbox.Handle, EM_SETCUEBANNER, 0, "80");
            address_txtbox.Text = "api.neurotolge.ee/v1.1";
            port_txtbox.Text = "80";
            Console.WriteLine(options);

            //Options.serverAddress = address_txtbox.Text.Trim();
            //Options.port = port_txtbox.Text.Trim();
            //Options.client = textBoxClient.Text.Trim();
            //Options.subject = textBoxSubject.Text.Trim();
            //Options.otherFeatures = textBoxOtherFeatures.Text.Trim();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Save_btn_Click(object sender, EventArgs e)
        {
            //Options.serverAddress = this.address_txtbox.Text.Trim();
            //Options.port = this.port_txtbox.Text.Trim();
            //Options.client = this.textBoxClient.Text.Trim();
            //Options.subject = this.textBoxSubject.Text.Trim();
            //Options.otherFeatures = this.textBoxOtherFeatures.Text.Trim();
            //this.DialogResult = DialogResult.OK;
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void NeurotolgeConfDialog_Load(object sender, EventArgs e)
        {
            //this.address_txtbox.Text = Options.serverAddress;
            //this.port_txtbox.Text = Options.port;
            //this.textBoxClient.Text = Options.client;
            //this.textBoxSubject.Text = Options.subject;
            //this.textBoxOtherFeatures.Text = Options.otherFeatures;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
