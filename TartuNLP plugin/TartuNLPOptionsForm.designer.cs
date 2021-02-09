namespace TartuNLP
{
    partial class TartuNLPConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblURL = new System.Windows.Forms.Label();
            this.lblAuth = new System.Windows.Forms.Label();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.tbAuth = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.srcLanguages = new System.Windows.Forms.ListBox();
            this.lblSupportedLanguages = new System.Windows.Forms.Label();
            this.cbDomain = new System.Windows.Forms.ComboBox();
            this.lblDomains = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tgtLanguages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblURL
            // 
            this.lblURL.Location = new System.Drawing.Point(8, 12);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(152, 16);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL";
            // 
            // lblAuth
            // 
            this.lblAuth.Location = new System.Drawing.Point(8, 40);
            this.lblAuth.Name = "lblAuth";
            this.lblAuth.Size = new System.Drawing.Size(152, 16);
            this.lblAuth.TabIndex = 2;
            this.lblAuth.Text = "API Key\r\n";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(168, 8);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(286, 20);
            this.tbURL.TabIndex = 1;
            this.tbURL.Text = "https://api.tartunlp.ai/v2.0/translate";
            this.tbURL.TextChanged += new System.EventHandler(this.tbURLAuth_TextChanged);
            // 
            // tbAuth
            // 
            this.tbAuth.Location = new System.Drawing.Point(168, 36);
            this.tbAuth.Name = "tbAuth";
            this.tbAuth.Size = new System.Drawing.Size(286, 20);
            this.tbAuth.TabIndex = 3;
            this.tbAuth.Text = "public";
            this.tbAuth.TextChanged += new System.EventHandler(this.tbURLAuth_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(296, 244);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(377, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // srcLanguages
            // 
            this.srcLanguages.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.srcLanguages.FormattingEnabled = true;
            this.srcLanguages.Location = new System.Drawing.Point(8, 130);
            this.srcLanguages.Name = "srcLanguages";
            this.srcLanguages.Size = new System.Drawing.Size(218, 108);
            this.srcLanguages.TabIndex = 6;
            this.srcLanguages.SelectedIndexChanged += new System.EventHandler(this.srcLanguages_SelectedIndexChanged);
            // 
            // lblSupportedLanguages
            // 
            this.lblSupportedLanguages.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSupportedLanguages.Location = new System.Drawing.Point(8, 113);
            this.lblSupportedLanguages.Name = "lblSupportedLanguages";
            this.lblSupportedLanguages.Size = new System.Drawing.Size(376, 16);
            this.lblSupportedLanguages.TabIndex = 5;
            this.lblSupportedLanguages.Text = "Supported languages";
            // 
            // cbDomain
            // 
            this.cbDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDomain.FormattingEnabled = true;
            this.cbDomain.Location = new System.Drawing.Point(168, 88);
            this.cbDomain.Name = "cbDomain";
            this.cbDomain.Size = new System.Drawing.Size(286, 21);
            this.cbDomain.TabIndex = 11;
            this.cbDomain.SelectedIndexChanged += new System.EventHandler(this.cbDomain_SelectedIndexChanged);
            // 
            // lblDomains
            // 
            this.lblDomains.Location = new System.Drawing.Point(8, 92);
            this.lblDomains.Name = "lblDomains";
            this.lblDomains.Size = new System.Drawing.Size(152, 16);
            this.lblDomains.TabIndex = 12;
            this.lblDomains.Text = "Domains";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(389, 59);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(65, 23);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // tgtLanguages
            // 
            this.tgtLanguages.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tgtLanguages.FormattingEnabled = true;
            this.tgtLanguages.Location = new System.Drawing.Point(236, 130);
            this.tgtLanguages.Name = "tgtLanguages";
            this.tgtLanguages.Size = new System.Drawing.Size(218, 108);
            this.tgtLanguages.TabIndex = 14;
            // 
            // TartuNLPConfigForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(461, 278);
            this.Controls.Add(this.tgtLanguages);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblDomains);
            this.Controls.Add(this.cbDomain);
            this.Controls.Add(this.lblSupportedLanguages);
            this.Controls.Add(this.srcLanguages);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbAuth);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.lblAuth);
            this.Controls.Add(this.lblURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TartuNLPConfigForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TartuNLP Plugin Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TartuNLPOptionsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ListBox tgtLanguages;

        #endregion

        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblAuth;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.TextBox tbAuth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox srcLanguages;
        private System.Windows.Forms.Label lblSupportedLanguages;
        private System.Windows.Forms.ComboBox cbDomain;
        private System.Windows.Forms.Label lblDomains;
        private System.Windows.Forms.Button btnUpdate;
    }
}