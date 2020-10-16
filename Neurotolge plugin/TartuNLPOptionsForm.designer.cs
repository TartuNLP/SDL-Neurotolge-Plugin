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
            this.lbLanguages = new System.Windows.Forms.ListBox();
            this.lblSupportedLanguages = new System.Windows.Forms.Label();
            this.cbDomains = new System.Windows.Forms.ComboBox();
            this.lblDomains = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblURL
            // 
            this.lblURL.Location = new System.Drawing.Point(12, 19);
            this.lblURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(228, 25);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL";
            // 
            // lblAuth
            // 
            this.lblAuth.Location = new System.Drawing.Point(12, 61);
            this.lblAuth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAuth.Name = "lblAuth";
            this.lblAuth.Size = new System.Drawing.Size(228, 25);
            this.lblAuth.TabIndex = 2;
            this.lblAuth.Text = "Auth";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(252, 12);
            this.tbURL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(427, 26);
            this.tbURL.TabIndex = 1;
            this.tbURL.Text = "193.40.33.51/v1.2/translate";
            this.tbURL.TextChanged += new System.EventHandler(this.tbURLAuth_TextChanged);
            // 
            // tbAuth
            // 
            this.tbAuth.Location = new System.Drawing.Point(252, 55);
            this.tbAuth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbAuth.Name = "tbAuth";
            this.tbAuth.Size = new System.Drawing.Size(427, 26);
            this.tbAuth.TabIndex = 3;
            this.tbAuth.Text = "public";
            this.tbAuth.TextChanged += new System.EventHandler(this.tbURLAuth_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(444, 376);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 35);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(566, 376);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbLanguages
            // 
            this.lbLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbLanguages.FormattingEnabled = true;
            this.lbLanguages.ItemHeight = 20;
            this.lbLanguages.Location = new System.Drawing.Point(18, 205);
            this.lbLanguages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbLanguages.Name = "lbLanguages";
            this.lbLanguages.Size = new System.Drawing.Size(660, 164);
            this.lbLanguages.TabIndex = 6;
            // 
            // lblSupportedLanguages
            // 
            this.lblSupportedLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSupportedLanguages.Location = new System.Drawing.Point(12, 174);
            this.lblSupportedLanguages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSupportedLanguages.Name = "lblSupportedLanguages";
            this.lblSupportedLanguages.Size = new System.Drawing.Size(564, 25);
            this.lblSupportedLanguages.TabIndex = 5;
            this.lblSupportedLanguages.Text = "Supported languages";
            // 
            // cbDomains
            // 
            this.cbDomains.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDomains.FormattingEnabled = true;
            this.cbDomains.Location = new System.Drawing.Point(252, 135);
            this.cbDomains.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbDomains.Name = "cbDomains";
            this.cbDomains.Size = new System.Drawing.Size(427, 28);
            this.cbDomains.TabIndex = 11;
            this.cbDomains.SelectedIndexChanged += new System.EventHandler(this.cbDomain_SelectedIndexChanged);
            // 
            // lblDomains
            // 
            this.lblDomains.Location = new System.Drawing.Point(12, 141);
            this.lblDomains.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDomains.Name = "lblDomains";
            this.lblDomains.Size = new System.Drawing.Size(228, 25);
            this.lblDomains.TabIndex = 12;
            this.lblDomains.Text = "Domains";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(580, 91);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(98, 35);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // TartuNLPConfigForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(692, 428);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblDomains);
            this.Controls.Add(this.cbDomains);
            this.Controls.Add(this.lblSupportedLanguages);
            this.Controls.Add(this.lbLanguages);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbAuth);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.lblAuth);
            this.Controls.Add(this.lblURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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

        #endregion

        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblAuth;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.TextBox tbAuth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbLanguages;
        private System.Windows.Forms.Label lblSupportedLanguages;
        private System.Windows.Forms.ComboBox cbDomains;
        private System.Windows.Forms.Label lblDomains;
        private System.Windows.Forms.Button btnUpdate;
    }
}